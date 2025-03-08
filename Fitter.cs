#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class Fitter : MonoBehaviourRect
{
	[SerializeField]
	public Fitter[] linkedFitters;

	public Vector2 padding;

	public bool widthFixed = true;

	[ShowIf("widthFixed")]
	public float fixedWidth = 2.4f;

	[HideIf("widthFixed")]
	public float minWidth = 2.4f;

	public bool heightFixed;

	[ShowIf("heightFixed")]
	public float fixedHeight = 0.5f;

	[HideIf("heightFixed")]
	public float minHeight = 0.5f;

	public void Fit(Action onComplete)
	{
		StartCoroutine(FitRoutine(onComplete));
	}

	public void SetSize(float w, float h)
	{
		base.rectTransform.sizeDelta = new Vector2(w, h) + padding;
	}

	public virtual IEnumerator FitRoutine(Action onComplete)
	{
		float w = fixedWidth;
		float h = fixedHeight;
		if (!widthFixed && !heightFixed)
		{
			float num;
			float num2 = (num = base.rectTransform.localPosition.x);
			float num3;
			float num4 = (num3 = base.rectTransform.localPosition.y);
			foreach (RectTransform item in base.rectTransform)
			{
				Vector2 vector = item.sizeDelta * 0.5f;
				Vector3 localPosition = item.localPosition;
				num2 = Mathf.Min(num2, localPosition.x - vector.x);
				num4 = Mathf.Min(num4, localPosition.y - vector.y);
				num = Mathf.Max(num, localPosition.x + vector.x);
				num3 = Mathf.Max(num3, localPosition.y + vector.y);
			}

			w = num - num2;
			h = num3 - num4;
		}
		else
		{
			if (!widthFixed)
			{
				float num5;
				float num6 = (num5 = 0f);
				foreach (RectTransform item2 in base.rectTransform)
				{
					Vector2 vector2 = item2.sizeDelta * 0.5f;
					Vector3 localPosition2 = item2.localPosition;
					num6 = Mathf.Min(num6, localPosition2.x - vector2.x);
					num5 = Mathf.Max(num5, localPosition2.x + vector2.x);
				}

				w = Mathf.Max(minWidth, num5 - num6);
			}

			if (!heightFixed)
			{
				float num7;
				float num8 = (num7 = 0f);
				foreach (RectTransform item3 in base.rectTransform)
				{
					Vector2 vector3 = item3.sizeDelta * 0.5f;
					Vector3 localPosition3 = item3.localPosition;
					num8 = Mathf.Min(num8, localPosition3.y - vector3.y);
					num7 = Mathf.Max(num7, localPosition3.y + vector3.y);
				}

				h = Mathf.Max(minHeight, num7 - num8);
			}
		}

		SetSize(w, h);
		yield return UpdateLinkedFitters();
		onComplete?.Invoke();
	}

	public IEnumerator UpdateLinkedFitters()
	{
		Fitter[] array = linkedFitters;
		foreach (Fitter fitter in array)
		{
			yield return fitter.FitRoutine(null);
		}
	}
}
