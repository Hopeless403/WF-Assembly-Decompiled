#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;
using UnityEngine.UI;

public class Glow : MonoBehaviourRect
{
	[SerializeField]
	public Image image;

	[SerializeField]
	public AnimationCurve fadeCurve;

	[SerializeField]
	public float fadeDuration = 0.5f;

	public float delay;

	public float t;

	public Glow SetSize(Vector2 size)
	{
		base.rectTransform.sizeDelta = size;
		return this;
	}

	public Glow SetPosition(Vector2 position)
	{
		base.rectTransform.anchoredPosition = position;
		return this;
	}

	public Glow SetColor(Color color)
	{
		image.color = color;
		return this;
	}

	public Glow RandomColor(float saturation = 1f, float brightness = 1f)
	{
		image.color = Color.HSVToRGB(PettyRandom.Range(0f, 1f), saturation, brightness);
		return this;
	}

	public Glow Fade(AnimationCurve curve, float duration, float delay = 0f)
	{
		image.color = image.color.WithAlpha(0f);
		fadeCurve = curve;
		fadeDuration = duration;
		t = 0f;
		this.delay = delay;
		return this;
	}

	public void Update()
	{
		if (delay > 0f)
		{
			delay -= Time.deltaTime;
			return;
		}

		t += Time.deltaTime / fadeDuration;
		image.color = image.color.WithAlpha(fadeCurve.Evaluate(t));
		if (t > 1f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
