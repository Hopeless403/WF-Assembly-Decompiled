#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Shop Prices", menuName = "Scripts/Change Shop Prices")]
public class ScriptChangeShopPrices : Script
{
	[SerializeField]
	public Vector2Int itemsToAffect = new Vector2Int(2, 3);

	[SerializeField]
	public Vector2 affectPriceRange = new Vector2(1f, 1.5f);

	[SerializeField]
	public Vector2 charmPriceRange = new Vector2(1f, 1.5f);

	[SerializeField]
	public Vector2 crownPriceRange = new Vector2(1f, 1.5f);

	[SerializeField]
	public float removeDiscountChance = 0.5f;

	public override IEnumerator Run()
	{
		foreach (CampaignNode item in Campaign.instance.nodes.Where((CampaignNode node) => node.type is CampaignNodeTypeShop))
		{
			if (!item.data.TryGetValue("shopData", out var value) || !(value is ShopRoutine.Data data))
			{
				continue;
			}

			int num = itemsToAffect.Random();
			foreach (ShopRoutine.Item item2 in data.items.OrderBy((ShopRoutine.Item a) => Random.Range(0f, 1f)))
			{
				float num2 = affectPriceRange.Random();
				item2.price = Mathf.RoundToInt((float)item2.price * num2);
				if (!(num2 < 1f) || Random.Range(0f, 1f) < removeDiscountChance)
				{
				}

				num--;
				if (num <= 0)
				{
					break;
				}
			}

			data.charmPrice = Mathf.RoundToInt((float)data.charmPrice * charmPriceRange.Random());
			data.crownPrice = Mathf.RoundToInt((float)data.crownPrice * crownPriceRange.Random());
		}

		yield break;
	}
}
