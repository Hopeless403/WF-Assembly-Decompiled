#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AddressableTieredSpriteLoader : AddressableAssetLoader<Sprite>
{
	public enum Type
	{
		SpriteRenderer,
		Image,
		ImageSprite
	}

	[Serializable]
	public struct Tier
	{
		public bool atlas;

		public AssetReferenceAtlasedSprite atlasedSpriteRef;

		public AssetReferenceSprite spriteRef;
	}

	[SerializeField]
	public Tier[] tiers;

	[SerializeField]
	public Type type;

	[SerializeField]
	[ShowIf("IsSpriteRenderer")]
	public SpriteRenderer spriteRenderer;

	[SerializeField]
	[ShowIf("IsImage")]
	public Image image;

	[SerializeField]
	[ShowIf("IsImageSprite")]
	public ImageSprite imageSprite;

	public bool IsSpriteRenderer => type == Type.SpriteRenderer;

	public bool IsImage => type == Type.Image;

	public bool IsImageSprite => type == Type.ImageSprite;

	public override void Load()
	{
		Load(0);
	}

	public void Load(int tier)
	{
		tier = Mathf.Clamp(tier, 0, tiers.Length - 1);
		Load(tiers[tier]);
	}

	public void Load(Tier tier)
	{
		if (loaded)
		{
			return;
		}

		operation = (tier.atlas ? tier.atlasedSpriteRef.LoadAssetAsync() : tier.spriteRef.LoadAssetAsync());
		if (instant)
		{
			operation.WaitForCompletion();
			SetSprite();
		}
		else
		{
			operation.Completed += delegate
			{
				SetSprite();
			};
		}

		loaded = true;
	}

	public void SetSprite()
	{
		switch (type)
		{
			case Type.SpriteRenderer:
				spriteRenderer.sprite = operation.Result;
				break;
			case Type.Image:
				image.sprite = operation.Result;
				break;
			case Type.ImageSprite:
				imageSprite.SetSprite(operation.Result);
				break;
		}
	}
}
