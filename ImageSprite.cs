#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class ImageSprite : Image
{
	[SerializeField]
	public bool copySpriteSize = true;

	[SerializeField]
	public bool copySpritePivot = true;

	public void SetSprite(Sprite sprite)
	{
		base.sprite = sprite;
		if ((bool)sprite)
		{
			Vector2 size = sprite.rect.size;
			if (copySpritePivot)
			{
				Vector3 anchoredPosition3D = base.rectTransform.anchoredPosition3D;
				base.rectTransform.pivot = sprite.pivot / size;
				base.rectTransform.anchoredPosition3D = anchoredPosition3D;
			}

			if (copySpriteSize)
			{
				base.rectTransform.sizeDelta = size / sprite.pixelsPerUnit;
			}
		}
	}
}
