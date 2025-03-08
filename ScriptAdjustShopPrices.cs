#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Adjust Shop Prices", menuName = "Scripts/Adjust Shop Prices")]
public class ScriptAdjustShopPrices : Script
{
	[SerializeField]
	public Vector2Int cardPrices = new Vector2Int(5, 5);

	[SerializeField]
	public Vector2Int charmPrices = new Vector2Int(5, 5);

	[SerializeField]
	public Vector2Int charmMachinePrice = new Vector2Int(5, 5);

	[SerializeField]
	public Vector2Int charmMachineAddPrice = new Vector2Int(5, 5);

	[SerializeField]
	public Vector2Int crownPrice = new Vector2Int(5, 5);

	[SerializeField]
	public Vector2Int crownAddPrice = new Vector2Int(5, 5);

	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			CampaignNodeType type = node.type;
			if (!(type is CampaignNodeTypeShop))
			{
				if (!(type is CampaignNodeTypeCharmShop))
				{
					continue;
				}

				EventRoutineCharmShop.Data data = node.data.Get<EventRoutineCharmShop.Data>("data");
				foreach (EventRoutineCharmShop.UpgradedCard card in data.cards)
				{
					card.price += cardPrices.Random();
				}

				foreach (EventRoutineCharmShop.CharmShopItemData item in data.items)
				{
					item.price += charmPrices.Random();
				}

				continue;
			}

			ShopRoutine.Data data2 = node.data.Get<ShopRoutine.Data>("shopData");
			data2.charmPrice += charmMachinePrice.Random();
			data2.charmPriceAdd += charmMachineAddPrice.Random();
			data2.crownPrice += crownPrice.Random();
			data2.crownPriceAdd += crownAddPrice.Random();
			foreach (ShopRoutine.Item item2 in data2.items)
			{
				item2.price += cardPrices.Random();
			}
		}

		yield break;
	}
}
