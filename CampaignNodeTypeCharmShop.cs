#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeCharmShop", menuName = "Campaign/Node Type/Charm Shop")]
public class CampaignNodeTypeCharmShop : CampaignNodeTypeEvent
{
	[Serializable]
	public class UpgradedCard
	{
		public CardData cardData;

		public CardUpgradeData[] upgrades;

		public int price;

		public float priceFactor;
	}

	[SerializeField]
	public int cardChoices = 1;

	[SerializeField]
	public UpgradedCard[] forceCards;

	[SerializeField]
	public int choices = 3;

	[SerializeField]
	public CardUpgradeData[] force;

	[SerializeField]
	public Vector2Int priceRange = new Vector2Int(20, 80);

	[SerializeField]
	public int priceOffset = -5;

	public override IEnumerator SetUp(CampaignNode node)
	{
		yield return null;
		CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
		List<EventRoutineCharmShop.UpgradedCard> list = new List<EventRoutineCharmShop.UpgradedCard>();
		UpgradedCard[] array = forceCards;
		if (array != null && array.Length > 0)
		{
			array = forceCards;
			foreach (UpgradedCard upgradedCard in array)
			{
				list.Add(new EventRoutineCharmShop.UpgradedCard
				{
					cardDataName = upgradedCard.cardData.name,
					upgradeNames = upgradedCard.upgrades.Select((CardUpgradeData a) => a.name).ToArray(),
					price = upgradedCard.price,
					priceFactor = upgradedCard.priceFactor
				});
				component.PullOut("Items", upgradedCard.cardData);
			}
		}

		int num = cardChoices - list.Count;
		for (int j = 0; j < num; j++)
		{
			CardData card = component.Pull<CardData>(node, "Items");
			int itemCount = 1;
			if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
			{
				itemCount = 2;
			}

			CardUpgradeData[] array2 = component.Pull<CardUpgradeData>(node, "Charms", itemCount, allowDuplicates: false, (DataFile a) => a is CardUpgradeData cardUpgradeData3 && cardUpgradeData3.CanAssign(card));
			float f = (float)card.value * UnityEngine.Random.Range(0.8f, 1.2f) + (float)(array2.Length * UnityEngine.Random.Range(10, 20)) + (float)priceOffset;
			EventRoutineCharmShop.UpgradedCard upgradedCard2 = new EventRoutineCharmShop.UpgradedCard
			{
				cardDataName = card.name,
				price = Mathf.RoundToInt(f),
				priceFactor = 1f
			};
			if (array2.Length != 0)
			{
				upgradedCard2.upgradeNames = array2.Select((CardUpgradeData a) => a.name).ToArray();
			}

			list.Add(upgradedCard2);
		}

		List<EventRoutineCharmShop.CharmShopItemData> list2 = new List<EventRoutineCharmShop.CharmShopItemData>();
		CardUpgradeData[] array3 = force;
		if (array3 != null && array3.Length > 0)
		{
			array3 = force;
			foreach (CardUpgradeData cardUpgradeData in array3)
			{
				list2.Add(new EventRoutineCharmShop.CharmShopItemData(cardUpgradeData.name, GetPrice(cardUpgradeData)));
			}

			component.PullOut("Charms", force);
		}

		int num2 = choices - list2.Count;
		for (int k = 0; k < num2; k++)
		{
			CardUpgradeData cardUpgradeData2 = component.Pull<CardUpgradeData>(node, "Charms");
			list2.Add(new EventRoutineCharmShop.CharmShopItemData(cardUpgradeData2.name, GetPrice(cardUpgradeData2)));
		}

		node.data = new Dictionary<string, object> { 
		{
			"data",
			new EventRoutineCharmShop.Data
			{
				cards = list,
				items = list2
			}
		} };
	}

	public override bool HasMissingData(CampaignNode node)
	{
		EventRoutineCharmShop.Data data = node.data.Get<EventRoutineCharmShop.Data>("data");
		foreach (EventRoutineCharmShop.UpgradedCard card in data.cards)
		{
			if (MissingCardSystem.IsMissing(card.cardDataName))
			{
				return true;
			}
		}

		foreach (EventRoutineCharmShop.CharmShopItemData item in data.items)
		{
			if (AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", item.upgradeDataName) == null)
			{
				return true;
			}
		}

		return false;
	}

	public int GetPrice(CardUpgradeData upgradeData)
	{
		return priceRange.Random() + upgradeData.tier * 10 + priceOffset;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineCharmShop eventRoutineCharmShop = UnityEngine.Object.FindObjectOfType<EventRoutineCharmShop>();
		eventRoutineCharmShop.node = node;
		yield return eventRoutineCharmShop.Populate();
	}
}
