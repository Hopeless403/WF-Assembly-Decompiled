#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AddressableSpriteLoader : AddressableAssetLoader<Sprite>
{
	public enum Type
	{
		SpriteRenderer,
		Image,
		ImageSprite
	}

	[SerializeField]
	public bool atlas = true;

	[ShowIf("atlas")]
	public AssetReferenceAtlasedSprite atlasedSpriteRef;

	[HideIf("atlas")]
	public AssetReferenceSprite spriteRef;

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
		if (loaded)
		{
			return;
		}

		operation = (atlas ? atlasedSpriteRef.LoadAssetAsync() : spriteRef.LoadAssetAsync());
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
