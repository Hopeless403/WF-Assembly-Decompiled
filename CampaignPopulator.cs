#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignPopulator", menuName = "Campaign/Populator")]
public class CampaignPopulator : ScriptableObject
{
	public class Tier
	{
		public int number;

		public List<CampaignNode> nodes = new List<CampaignNode>();

		public List<BattleData> battles = new List<BattleData>();

		public List<CampaignNodeType> rewards = new List<CampaignNodeType>();

		public CampaignTier campaignTier;

		public Tier(int number, CampaignTier campaignTier)
		{
			this.number = number;
			this.campaignTier = campaignTier;
		}

		public BattleData PullBattle(CampaignPopulator campaignPopulator)
		{
			if (battles.Count <= 0)
			{
				BattleData[] battlePool = campaignTier.battlePool;
				foreach (BattleData battleData in battlePool)
				{
					if (!campaignPopulator.BattleIsLocked(battleData.name))
					{
						battles.Add(battleData);
					}
				}
			}

			BattleData battleData2 = battles.RandomItem();
			battles.Remove(battleData2);
			return battleData2;
		}

		public int GetBattlePoints()
		{
			return campaignTier.pointRange.Random();
		}

		public CampaignNodeType PullReward()
		{
			if (rewards.Count <= 0)
			{
				rewards.AddRange(campaignTier.rewardPool);
			}

			CampaignNodeType campaignNodeType = rewards.RandomItem();
			rewards.Remove(campaignNodeType);
			return campaignNodeType;
		}
	}

	[SerializeField]
	public bool removeLockedCards = true;

	[SerializeField]
	public CampaignTier[] tiers;

	[SerializeField]
	public int playerStartNodeId;

	[SerializeField]
	public BattleLockData[] battleLockers;

	public void LoadCharacters(Campaign campaign, CharacterSaveData[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			Character character = data[i].Load();
			character.transform.SetParent(campaign.characterContainer);
			campaign.characters.Add(character);
		}
	}

	public IEnumerator Populate(Campaign campaign)
	{
		Debug.Log($"[{this}] POPULATING");
		StopWatch.Start();
		campaign.characterContainer.DestroyAllChildren();
		Character character = Object.Instantiate(References.PlayerData.classData.characterPrefab, campaign.characterContainer);
		character.name = "Player (" + character.title + ")";
		campaign.characters.Add(character);
		campaign.nodes[playerStartNodeId].characters.Add(campaign.characters.Count - 1);
		for (int i = 0; i < playerStartNodeId; i++)
		{
			CampaignNode campaignNode = campaign.nodes[i];
			if (campaignNode.type.interactable)
			{
				campaignNode.revealed = true;
				campaignNode.SetCleared();
			}
		}

		References.Player = character;
		character.Assign(References.PlayerData);
		CharacterRewards component = character.GetComponent<CharacterRewards>();
		if ((object)component != null)
		{
			component.Populate(character.data.classData);
			if (removeLockedCards)
			{
				component.RemoveLockedCards();
			}

			component.RemoveCardsFromStartingDeck();
			if (Campaign.Data.GameMode.mainGameMode)
			{
				component.RemoveCompanionsInFinalBossBattle();
			}
		}

		CharacterDisplay.FindAndAssign(character);
		List<int> list = new List<int>();
		string battleTiers = campaign.battleTiers;
		for (int j = 0; j < battleTiers.Length; j++)
		{
			int item = int.Parse(battleTiers[j].ToString());
			list.Add(item);
		}

		List<Tier> list2 = new List<Tier>();
		foreach (CampaignNode node in campaign.nodes)
		{
			int tierNumber = list[node.positionIndex];
			Tier tier = list2.Find((Tier a) => a.number == tierNumber);
			if (tier == null)
			{
				tier = new Tier(tierNumber, tiers[tierNumber]);
				tier.nodes.Add(node);
				list2.Add(tier);
			}
			else
			{
				tier.nodes.Add(node);
			}
		}

		foreach (Tier item2 in list2)
		{
			foreach (CampaignNode node2 in item2.nodes)
			{
				if (node2.type.isBattle)
				{
					BattleData battleData = item2.PullBattle(this);
					int battlePoints = item2.GetBattlePoints();
					node2.data = new Dictionary<string, object>
					{
						["battle"] = battleData.name,
						["waves"] = battleData.generationScript.Run(battleData, battlePoints)
					};
				}
				else if (node2.type.name == "CampaignNodeReward")
				{
					node2.SetType(item2.PullReward());
				}
			}
		}

		Dictionary<CampaignNode, CampaignNode> links = LinkNodes(list2);
		Routine.Clump clump = new Routine.Clump();
		foreach (CampaignNode node3 in campaign.nodes)
		{
			if (!links.ContainsKey(node3))
			{
				clump.Add(node3.type.SetUp(node3));
			}
		}

		yield return clump.WaitForEnd();
		foreach (KeyValuePair<CampaignNode, CampaignNode> item3 in links)
		{
			item3.Key.CopyData(item3.Value);
		}

		Debug.Log($"DONE ({StopWatch.Stop()}ms)");
		yield return null;
	}

	public bool BattleIsLocked(string battleName)
	{
		return battleLockers.FirstOrDefault((BattleLockData a) => a.battleName == battleName)?.IsLocked() ?? false;
	}

	public static Dictionary<CampaignNode, CampaignNode> LinkNodes(List<Tier> currentTiers)
	{
		Dictionary<CampaignNode, CampaignNode> links = new Dictionary<CampaignNode, CampaignNode>();
		foreach (Tier currentTier in currentTiers)
		{
			foreach (CampaignNode node in currentTier.nodes)
			{
				if (!node.type.canLink || links.ContainsKey(node) || links.ContainsValue(node) || node.pathId < 0)
				{
					continue;
				}

				CampaignNode campaignNode = (from a in currentTier.nodes
					where !links.ContainsKey(a) && !links.ContainsValue(a) && a.type.canLink && a.id != node.id && a.pathId != node.pathId && a.pathId >= 0 && a.type.name == node.type.name
					orderby a.id
					select a).FirstOrDefault();
				if (campaignNode != null)
				{
					node.dataLinkedTo = campaignNode.id;
					CampaignNode campaignNode2 = campaignNode;
					if (campaignNode2.linkedToThis == null)
					{
						campaignNode2.linkedToThis = new List<int>();
					}

					campaignNode.linkedToThis.Add(node.id);
					links.Add(node, campaignNode);
				}
			}
		}

		return links;
	}
}
