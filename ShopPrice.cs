#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class ShopPrice : MonoBehaviour
{
	public enum Position
	{
		Top,
		Bottom
	}

	public ShopItem target;

	public Transform follow;

	[SerializeField]
	public Vector3 offset;

	[SerializeField]
	public TMP_Text textAsset;

	[SerializeField]
	public string format = "{0}<sprite name=bling>";

	[SerializeField]
	public string discountedFormat = "<color=red><s>{1}</s></color> {0}<sprite name=bling>";

	public float scaleWithTarget = 0.5f;

	public float scaleOffsetWithTarget = 0.5f;

	public void Set(ShopItem target, Vector3 offset)
	{
		this.target = target;
		follow = target.transform;
		this.offset = offset;
	}

	public void SetOffset(Vector3 offset)
	{
		this.offset = offset;
	}

	public void SetPrice(int price, float priceFactor = 1f)
	{
		target.price = price;
		target.priceFactor = priceFactor;
		if (priceFactor != 1f)
		{
			SetText(string.Format(discountedFormat, target.GetPrice(), price));
		}
		else
		{
			SetText(string.Format(format, price));
		}
	}

	public void SetText(string text)
	{
		textAsset.text = text;
	}

	public bool Check()
	{
		return follow != null;
	}

	public void UpdatePosition()
	{
		if (follow == null)
		{
			base.gameObject.Destroy();
			return;
		}

		Vector3 vector = offset;
		if (scaleOffsetWithTarget > 0f)
		{
			vector = Vector3.Scale(vector, Vector3.Lerp(Vector3.one, follow.localScale, scaleOffsetWithTarget));
		}

		base.transform.position = follow.position + vector;
		if (scaleWithTarget > 0f)
		{
			base.transform.localScale = Vector3.Lerp(Vector3.one, follow.localScale, scaleWithTarget);
		}
	}
}
