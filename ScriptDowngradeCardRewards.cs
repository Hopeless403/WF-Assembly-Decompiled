#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Downgrade Card Rewards", menuName = "Scripts/Downgrade Card Rewards")]
public class ScriptDowngradeCardRewards : Script
{
	[Serializable]
	public struct FixedCharmSet
	{
		public string[] cards;

		public CardUpgradeData[] charmOptions;
	}

	[SerializeField]
	public CardUpgradeData[] charms;

	[SerializeField]
	public int downgradesPerTier = 2;

	[SerializeField]
	public FixedCharmSet[] charmSets;

	public override IEnumerator Run()
	{
		List<CardUpgradeData> pool = PopulatePool();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i <= 8; i++)
		{
			dictionary[i] = downgradesPerTier;
		}

		foreach (CampaignNode item in References.Campaign.nodes.Where((CampaignNode a) => a.type.interactable && a.dataLinkedTo == -1 && a.tier >= 0 && a.data != null).InRandomOrder())
		{
			if (dictionary[item.tier] > 0 && TryAddDowngrade(item, pool))
			{
				dictionary[item.tier]--;
			}
		}

		yield break;
	}

	public bool TryAddDowngrade(CampaignNode node, List<CardUpgradeData> pool)
	{
		bool result = false;
		CampaignNodeType type = node.type;
		if (type is CampaignNodeTypeItem || type is CampaignNodeTypeCompanion || type is CampaignNodeTypeCurseItems)
		{
			if (TryAddDowngrade(node.data.GetSaveCollection<string>("cards"), pool, out var appliedToIndex, out var downgradeApplied))
			{
				result = true;
				AddUpgradeToCardInNodeData(node, appliedToIndex, downgradeApplied.name);
			}
		}
		else if (node.type is CampaignNodeTypeShop)
		{
			ShopRoutine.Data data = node.data.Get<ShopRoutine.Data>("shopData");
			if (TryAddDowngrade(data.items.Select((ShopRoutine.Item a) => a.cardDataName).ToArray(), pool, out var appliedToIndex2, out var downgradeApplied2))
			{
				result = true;
				for (int i = 0; i <= appliedToIndex2; i++)
				{
					if (data.upgrades.Count <= i)
					{
						string[] item = ((i != appliedToIndex2) ? null : new string[1] { downgradeApplied2.name });
						data.upgrades.Add(item);
					}
					else if (i == appliedToIndex2)
					{
						string[] array = data.upgrades[i];
						if (array == null)
						{
							array = new string[1] { downgradeApplied2.name };
						}
						else
						{
							List<string> list = array.ToList();
							list.Add(downgradeApplied2.name);
							array = list.ToArray();
						}

						data.upgrades[i] = array;
					}
				}
			}
		}

		else if (node.type is CampaignNodeTypeCharmShop)
		{
			EventRoutineCharmShop.Data data2 = node.data.Get<EventRoutineCharmShop.Data>("data");
			if (TryAddDowngrade(data2.cards.Select((EventRoutineCharmShop.UpgradedCard a) => a.cardDataName).ToArray(), pool, out var appliedToIndex3, out var downgradeApplied3))
			{
				result = true;
				EventRoutineCharmShop.UpgradedCard upgradedCard = data2.cards[appliedToIndex3];
				List<string> list2 = ((upgradedCard.upgradeNames == null) ? new List<string>() : upgradedCard.upgradeNames.ToList());
				list2.Add(downgradeApplied3.name);
				upgradedCard.upgradeNames = list2.ToArray();
			}
		}

		return result;
	}

	public static void AddUpgradeToCardInNodeData(CampaignNode node, int cardIndex, string upgradeName)
	{
		string key = $"upgrades{cardIndex}";
		if (node.data.ContainsKey(key))
		{
			node.data.Get<SaveCollection<string>>(key).Add(upgradeName);
		}
		else
		{
			node.data[key] = new SaveCollection<string>(upgradeName);
		}

		if (node.linkedToThis == null)
		{
			return;
		}

		foreach (int linkedToThi in node.linkedToThis)
		{
			AddUpgradeToCardInNodeData(Campaign.GetNode(linkedToThi), cardIndex, upgradeName);
		}
	}

	public bool TryAddDowngrade(string[] cardNames, List<CardUpgradeData> pool, out int appliedToIndex, out CardUpgradeData downgradeApplied)
	{
		appliedToIndex = -1;
		downgradeApplied = null;
		bool result = false;
		foreach (int item in cardNames.GetIndices().InRandomOrder())
		{
			string assetName = cardNames[item];
			CardData cardData = AddressableLoader.Get<CardData>("CardData", assetName);
			downgradeApplied = FindDowngrade(cardData, pool);
			if ((bool)downgradeApplied)
			{
				pool.Remove(downgradeApplied);
				result = true;
				appliedToIndex = item;
				break;
			}
		}

		return result;
	}

	public CardUpgradeData FindDowngrade(CardData cardData, List<CardUpgradeData> pool)
	{
		FixedCharmSet[] array = charmSets;
		for (int i = 0; i < array.Length; i++)
		{
			FixedCharmSet fixedCharmSet = array[i];
			if (fixedCharmSet.cards.Contains(cardData.name))
			{
				return fixedCharmSet.charmOptions.RandomItem();
			}
		}

		for (int j = 0; j < pool.Count; j++)
		{
			if (pool[j].CanAssign(cardData))
			{
				return pool[j];
			}
		}

		return null;
	}

	public List<CardUpgradeData> PopulatePool()
	{
		List<CardUpgradeData> list = new List<CardUpgradeData>();
		list.AddRange(charms.InRandomOrder());
		list.AddRange(charms.InRandomOrder());
		return list;
	}
}
