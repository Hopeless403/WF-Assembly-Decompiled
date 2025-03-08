#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeShop", menuName = "Campaign/Node Type/Shop")]
public class CampaignNodeTypeShop : CampaignNodeTypeEvent
{
	[Serializable]
	public struct Stock
	{
		public AnimationCurve companions;

		public bool companionsCanDiscount;

		public AnimationCurve items;

		public bool itemsCanDiscount;

		public AnimationCurve consumables;

		public bool consumablesCanDiscount;

		public AnimationCurve charms;

		public int companionCount => Mathf.RoundToInt(companions.Evaluate(UnityEngine.Random.value));

		public int itemCount => Mathf.RoundToInt(items.Evaluate(UnityEngine.Random.value));

		public int consumableCount => Mathf.RoundToInt(consumables.Evaluate(UnityEngine.Random.value));

		public int charmCount => Mathf.RoundToInt(charms.Evaluate(UnityEngine.Random.value));
	}

	[SerializeField]
	public Stock stock;

	[Header("Prices")]
	[SerializeField]
	[MinMaxSlider(0f, 2f)]
	public Vector2 priceFactorRange = new Vector2(0.8f, 1.2f);

	[SerializeField]
	public int discounts = 1;

	[SerializeField]
	public float discountFactor = 0.5f;

	[SerializeField]
	public int charmPrice = 50;

	[SerializeField]
	public int charmPriceAdd = 20;

	[SerializeField]
	public int crownPrice = 80;

	[SerializeField]
	public int crownPriceAdd = 30;

	[SerializeField]
	public int priceOffset = -5;

	public override IEnumerator SetUp(CampaignNode node)
	{
		yield return null;
		CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
		Dictionary<string, DataFile[]> obj = new Dictionary<string, DataFile[]>
		{
			["Companions"] = component.Pull<DataFile>(node, "Units", stock.companionCount),
			["Items"] = component.Pull<DataFile>(match: (DataFile a) => a is CardData cardData3 && cardData3.cardType.name == "Item" && !cardData3.traits.Exists((CardData.TraitStacks b) => b.data.name == "Consume"), pulledBy: node, poolName: "Items", itemCount: stock.itemCount, allowDuplicates: false),
			["Consumables"] = component.Pull<DataFile>(match: (DataFile a) => a is CardData cardData2 && cardData2.cardType.name == "Item" && cardData2.traits.Exists((CardData.TraitStacks b) => b.data.name == "Consume"), pulledBy: node, poolName: "Items", itemCount: stock.consumableCount, allowDuplicates: false)
		};
		ShopRoutine.Data data = new ShopRoutine.Data
		{
			charmPrice = charmPrice,
			charmPriceAdd = charmPriceAdd,
			crownPrice = crownPrice,
			crownPriceAdd = crownPriceAdd
		};
		foreach (KeyValuePair<string, DataFile[]> item2 in obj)
		{
			item2.Deconstruct(out var key, out var value);
			string category = key;
			DataFile[] array = value;
			if (array == null)
			{
				continue;
			}

			value = array;
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] is CardData cardData)
				{
					ShopRoutine.Item item = new ShopRoutine.Item(category, cardData, priceOffset, priceFactorRange.Random());
					data.items.Add(item);
				}
			}
		}

		CardUpgradeData[] array2 = component.Pull<CardUpgradeData>(node, "Charms", stock.charmCount);
		foreach (CardUpgradeData cardUpgradeData in array2)
		{
			data.charms.Add(cardUpgradeData.name);
		}

		List<ShopRoutine.Item> list = new List<ShopRoutine.Item>();
		foreach (ShopRoutine.Item item3 in data.items)
		{
			if (item3.category == "Companions" && stock.companionsCanDiscount)
			{
				list.Add(item3);
			}
			else if (item3.category == "Items" && stock.itemsCanDiscount)
			{
				list.Add(item3);
			}

			else if (item3.category == "Consumables" && stock.consumablesCanDiscount)
			{
				list.Add(item3);
			}
		}

		for (int j = 0; j < discounts; j++)
		{
			if (list.Count > 0)
			{
				int index = list.RandomIndex();
				list[index].priceFactor = discountFactor;
				list.RemoveAt(index);
			}
		}

		node.data = new Dictionary<string, object> { { "shopData", data } };
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		ShopRoutine shopRoutine = UnityEngine.Object.FindObjectOfType<ShopRoutine>();
		shopRoutine.node = node;
		yield return shopRoutine.Populate();
	}

	public override bool HasMissingData(CampaignNode node)
	{
		foreach (ShopRoutine.Item item in node.data.Get<ShopRoutine.Data>("shopData").items)
		{
			if (MissingCardSystem.IsMissing(item.cardDataName))
			{
				return true;
			}
		}

		return false;
	}
}
