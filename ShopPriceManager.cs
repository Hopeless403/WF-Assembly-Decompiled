#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopPriceManager : MonoBehaviour
{
	[Serializable]
	public struct PositionProfile
	{
		public ShopPrice.Position type;

		public Vector3 offset;
	}

	[SerializeField]
	public ShopPrice pricePrefab;

	[SerializeField]
	public List<ShopPrice> targets = new List<ShopPrice>();

	[SerializeField]
	public PositionProfile[] positionProfiles;

	public void LateUpdate()
	{
		for (int num = targets.Count - 1; num >= 0; num--)
		{
			ShopPrice shopPrice = targets[num];
			if (!shopPrice)
			{
				targets.RemoveAt(num);
			}
			else
			{
				shopPrice.UpdatePosition();
			}
		}
	}

	public ShopPrice Add(ShopItem target, ShopPrice.Position position)
	{
		PositionProfile positionProfile = positionProfiles.FirstOrDefault((PositionProfile a) => a.type == position);
		ShopPrice shopPrice = UnityEngine.Object.Instantiate(pricePrefab, base.transform, worldPositionStays: false);
		shopPrice.Set(target, positionProfile.offset);
		shopPrice.gameObject.SetActive(value: true);
		targets.Add(shopPrice);
		return shopPrice;
	}

	public ShopPrice Get(ShopItem target)
	{
		return targets.Find((ShopPrice a) => a.target == target);
	}

	public void Remove(ShopItem target)
	{
		ShopPrice shopPrice = Get(target);
		shopPrice.gameObject.Destroy();
		targets.Remove(shopPrice);
	}

	public void Clear()
	{
		foreach (ShopPrice target in targets)
		{
			target.Destroy();
		}

		targets.Clear();
	}
}
