#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardSlot : CardContainer
{
	[SerializeField]
	public SpriteRenderer icon;

	public Color originalIconColor;

	public void Awake()
	{
		originalIconColor = icon.color;
	}

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		holder.sizeDelta = GameManager.CARD_SIZE * cardScale;
	}

	public override void CardAdded(Entity entity)
	{
		FadeOutIcon();
	}

	public override void CardRemoved(Entity entity)
	{
		if (base.Empty)
		{
			FadeInIcon();
		}
	}

	public void ForceHover()
	{
		base.cc?.HoverSlot(this);
	}

	public void ForceUnHover()
	{
		if (base.cc != null && base.cc.hoverSlot == this)
		{
			base.cc.UnHoverSlot();
		}
	}

	public override void Hover()
	{
		if (canHover && base.cc != null)
		{
			base.cc.HoverSlot(this);
		}
	}

	public override void UnHover()
	{
		if (canHover && base.cc != null && base.cc.hoverSlot == this)
		{
			base.cc.UnHoverSlot();
		}
	}

	public void FadeOutIcon()
	{
		LeanTween.cancel(icon.gameObject);
		LeanTween.color(icon.gameObject, originalIconColor.With(-1f, -1f, -1f, 0f), 0.33f);
	}

	public void FadeInIcon()
	{
		LeanTween.cancel(icon.gameObject);
		LeanTween.color(icon.gameObject, originalIconColor, 0.33f);
	}
}
