#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeClunkShop", menuName = "Campaign/Node Type/Clunk Shop")]
public class CampaignNodeTypeClunkShop : CampaignNodeTypeEvent
{
	[Header("Stock")]
	[SerializeField]
	public int itemCount = 3;

	[SerializeField]
	public int charmCount = 3;

	[SerializeField]
	public CardUpgradeData[] charmPool;

	[Header("Prices")]
	[SerializeField]
	[MinMaxSlider(0f, 2f)]
	public Vector2 priceFactorRange = new Vector2(0.8f, 1.2f);

	[SerializeField]
	public int discounts = 1;

	[SerializeField]
	public float discountFactor = 0.5f;

	[SerializeField]
	public Vector2Int charmPriceRange = new Vector2Int(60, 80);

	public override IEnumerator SetUp(CampaignNode node)
	{
		yield return null;
		CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
		EventRoutineClunkShop.Data data = new EventRoutineClunkShop.Data();
		CardData[] obj = component.Pull<CardData>(match: (DataFile a) => a is CardData cardData2 && cardData2.cardType.name == "Clunker", pulledBy: node, poolName: "Items", itemCount: itemCount, allowDuplicates: false);
		data.cards = new List<EventRoutineClunkShop.CardItem>();
		CardData[] array = obj;
		foreach (CardData cardData in array)
		{
			data.cards.Add(new EventRoutineClunkShop.CardItem(cardData));
		}

		List<CardUpgradeData> list = new List<CardUpgradeData>(charmPool);
		list.Shuffle();
		data.charms = new List<EventRoutineClunkShop.CharmItem>();
		for (int j = 0; j < charmCount; j++)
		{
			CardUpgradeData cardUpgradeData = ((list.Count > 0) ? list[0] : null);
			if (cardUpgradeData == null)
			{
				break;
			}

			list.RemoveAt(0);
			data.charms.Add(new EventRoutineClunkShop.CharmItem(cardUpgradeData.name, charmPriceRange.Random()));
		}

		if (discounts > 0)
		{
			List<EventRoutineClunkShop.Item> list2 = new List<EventRoutineClunkShop.Item>();
			list2.AddRange(data.cards);
			list2.AddRange(data.charms);
			list2.Shuffle();
			for (int k = 0; k < discounts; k++)
			{
				EventRoutineClunkShop.Item item = ((list2.Count > 0) ? list2[0] : null);
				if (item == null)
				{
					break;
				}

				list2.RemoveAt(0);
				item.priceFactor = discountFactor;
			}
		}

		node.data = new Dictionary<string, object> { { "shopData", data } };
	}

	public override bool HasMissingData(CampaignNode node)
	{
		foreach (EventRoutineClunkShop.CardItem card in node.data.Get<EventRoutineClunkShop.Data>("shopData").cards)
		{
			if (MissingCardSystem.IsMissing(card.dataName))
			{
				return true;
			}
		}

		return false;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineClunkShop eventRoutineClunkShop = Object.FindObjectOfType<EventRoutineClunkShop>();
		eventRoutineClunkShop.node = node;
		yield return eventRoutineClunkShop.Populate();
	}
}
