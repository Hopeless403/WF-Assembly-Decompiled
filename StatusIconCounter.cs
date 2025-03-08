#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class StatusIconCounter : StatusIcon
{
	[SerializeField]
	public Image image;

	[SerializeField]
	public Sprite customSprite;

	[SerializeField]
	public Material customMaterial;

	[SerializeField]
	public Sprite snowSprite;

	[SerializeField]
	public Material snowMaterial;

	public CardIdleAnimation imminentAnimation;

	public Sprite baseSprite;

	public Material baseMaterial;

	public void Awake()
	{
		baseSprite = image.sprite;
		baseMaterial = textElement.fontSharedMaterial;
	}

	public void CheckSetSprite()
	{
		Stat stat = GetValue();
		Sprite sprite = baseSprite;
		Material material = baseMaterial;
		if (base.target != null && base.target.IsSnowed)
		{
			sprite = snowSprite ?? baseSprite;
			material = snowMaterial ?? baseMaterial;
		}
		else if (stat.current <= 1)
		{
			sprite = customSprite ?? baseSprite;
			material = customMaterial ?? baseMaterial;
		}

		if (sprite != null)
		{
			image.sprite = sprite;
		}

		if (material != null)
		{
			textElement.fontSharedMaterial = material;
		}
	}
}
