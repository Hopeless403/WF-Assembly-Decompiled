#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeCurseItems", menuName = "Campaign/Node Type/Curse Items")]
public class CampaignNodeTypeCurseItems : CampaignNodeTypeEvent
{
	[SerializeField]
	public int choices = 3;

	[SerializeField]
	public int curseCards = 2;

	[SerializeField]
	public List<CardData> force;

	[SerializeField]
	public CardData[] cursePool;

	[SerializeField]
	public CardData[] extraCards;

	[SerializeField]
	public CardData[] illegalCards;

	public override IEnumerator SetUp(CampaignNode node)
	{
		yield return null;
		Campaign campaign = Object.FindObjectOfType<Campaign>();
		CharacterRewards characterRewards = campaign.GetComponent<CharacterRewards>();
		if (!characterRewards)
		{
			characterRewards = campaign.gameObject.AddComponent<CharacterRewards>();
			List<ClassData> group = AddressableLoader.GetGroup<ClassData>("ClassData");
			HashSet<RewardPool> hashSet = new HashSet<RewardPool>();
			foreach (ClassData item2 in group)
			{
				RewardPool[] rewardPools = item2.rewardPools;
				foreach (RewardPool rewardPool in rewardPools)
				{
					if (rewardPool.type == "Items")
					{
						hashSet.Add(rewardPool);
					}
				}
			}

			foreach (RewardPool item3 in hashSet)
			{
				characterRewards.Add(item3);
			}

			characterRewards.PullOut("Items", illegalCards);
			characterRewards.Add("Items", extraCards);
			characterRewards.RemoveLockedCards();
		}

		List<CardData> list = force.Clone();
		if (list.Count > 0)
		{
			characterRewards.PullOut("Items", list);
		}

		int itemCount = choices - list.Count;
		list.AddRange(characterRewards.Pull<CardData>(node, "Items", itemCount));
		List<CardData> list2 = new List<CardData>();
		for (int j = 0; j < choices; j++)
		{
			CardData item = ((j < curseCards) ? cursePool.RandomItem() : null);
			list2.Insert(list2.RandomIndex(), item);
		}

		node.data = new Dictionary<string, object>
		{
			{
				"cards",
				ToSaveCollectionOfNames(list)
			},
			{
				"curses",
				ToSaveCollectionOfNames(list2)
			},
			{ "analyticsEventSent", false }
		};
	}

	public override bool HasMissingData(CampaignNode node)
	{
		string[] saveCollection = node.data.GetSaveCollection<string>("cards");
		string[] saveCollection2 = node.data.GetSaveCollection<string>("curses");
		if (!MissingCardSystem.HasMissingData(saveCollection))
		{
			return MissingCardSystem.HasMissingData(saveCollection2.Where((string a) => a != null));
		}

		return true;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineCurseItems eventRoutineCurseItems = Object.FindObjectOfType<EventRoutineCurseItems>();
		eventRoutineCurseItems.node = node;
		yield return eventRoutineCurseItems.Populate();
	}

	public static SaveCollection<string> ToSaveCollectionOfNames(IEnumerable<Object> list)
	{
		string[] collection = list.Select((Object a) => (!a) ? null : a.name).ToArray();
		return new SaveCollection<string>(collection);
	}
}
