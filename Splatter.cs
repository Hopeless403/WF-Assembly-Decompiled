#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Dead;
using UnityEngine;
using UnityEngine.UI;

public class Splatter : MonoBehaviour
{
	[SerializeField]
	public Sprite[] spriteOptions;

	[SerializeField]
	public Vector2 sizeRange = new Vector2(1.5f, 2f);

	[SerializeField]
	public Vector2 angleRange = new Vector2(0f, 360f);

	[SerializeField]
	public Image image;

	public bool fading { get; set; }

	public Sprite sprite
	{
		get
		{
			return image.sprite;
		}
		set
		{
			image.sprite = value;
		}
	}

	public Color color
	{
		get
		{
			return image.color;
		}
		set
		{
			image.color = value;
		}
	}

	public void Awake()
	{
		image.sprite = spriteOptions[PettyRandom.Range(0, spriteOptions.Length - 1)];
		base.transform.localEulerAngles = new Vector3(0f, 0f, angleRange.PettyRandom());
		base.transform.localScale = Vector3.one * sizeRange.PettyRandom();
	}

	public void FadeOut(float reduceTime = 0f)
	{
		if (fading)
		{
			return;
		}

		float time = Mathf.Max(0.1f, PettyRandom.Range(10f, 15f) - reduceTime);
		if (SplatterSystem.CheckStartTween(time))
		{
			LeanTween.cancel(base.gameObject);
			LeanTween.value(base.gameObject, image.color.a, 0f, time).setOnUpdate(delegate(float a)
			{
				image.color = image.color.With(-1f, -1f, -1f, a);
			}).setEase(LeanTweenType.easeInOutQuint)
				.setOnComplete((Action)delegate
				{
					base.gameObject.Destroy();
				});
			fading = true;
		}
		else
		{
			base.gameObject.Destroy();
		}
	}
}
