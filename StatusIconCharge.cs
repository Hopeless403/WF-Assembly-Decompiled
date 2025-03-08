#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class StatusIconCharge : StatusIcon
{
	public Sprite chargedSprite;

	public Material chargedMaterial;

	public Sprite snowSprite;

	public Material snowMaterial;

	public Image _image;

	public Sprite baseSprite;

	public Material baseMaterial;

	public Image image
	{
		get
		{
			if (_image == null)
			{
				_image = GetComponent<Image>();
				baseSprite = _image.sprite;
			}

			return _image;
		}
	}

	public void Awake()
	{
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
			textElement.gameObject.SetActive(value: true);
		}
		else if (stat.current <= 0)
		{
			sprite = chargedSprite ?? baseSprite;
			material = chargedMaterial ?? baseMaterial;
			textElement.gameObject.SetActive(value: false);
		}
		else
		{
			textElement.gameObject.SetActive(value: true);
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
