#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ShopItem : MonoBehaviour, IRemoveWhenPooled
{
	public int price;

	public float priceFactor;

	public int GetPrice()
	{
		return Mathf.RoundToInt((float)price * priceFactor);
	}
}
