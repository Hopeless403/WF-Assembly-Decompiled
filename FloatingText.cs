#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textAsset;

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public CanvasGroup canvasGroup;

	public static FloatingText Create(Vector3 position)
	{
		FloatingText fromPool = FloatingTextManager.GetFromPool();
		fromPool.transform.position = position;
		return fromPool;
	}

	public static FloatingText Create(Vector3 position, string text)
	{
		FloatingText fromPool = FloatingTextManager.GetFromPool();
		fromPool.transform.position = position;
		fromPool.SetText(text);
		return fromPool;
	}

	public FloatingText SetText(string text)
	{
		textAsset.text = text;
		return this;
	}

	public FloatingText Reset()
	{
		canvas.overrideSorting = false;
		canvasGroup.alpha = 1f;
		return this;
	}

	public FloatingText SetSortingLayer(string sortingLayerName, int orderInLayer)
	{
		canvas.overrideSorting = true;
		canvas.sortingLayerName = sortingLayerName;
		canvas.sortingOrder = orderInLayer;
		return this;
	}

	public FloatingText Animate(string animationName, float strength = 1f)
	{
		FloatingTextManager.Animation animation = FloatingTextManager.GetAnimation(animationName);
		if (animation.tweens != null && animation.tweens.Length != 0)
		{
			StartCoroutine(AnimateRoutine(animation, strength));
		}

		return this;
	}

	public FloatingText Fade(string fadeCurveName, float duration = 1f, float delay = 0f)
	{
		StartCoroutine(FadeRoutine(FloatingTextManager.GetFadeCurve(fadeCurveName).curve, duration, delay));
		return this;
	}

	public FloatingText Fade(AnimationCurve curve, float duration = 1f, float delay = 0f)
	{
		StartCoroutine(FadeRoutine(curve, duration, delay));
		return this;
	}

	public FloatingText DestroyAfterSeconds(float seconds)
	{
		StartCoroutine(DestroyAfterSecondsRoutine(seconds));
		return this;
	}

	public IEnumerator AnimateRoutine(FloatingTextManager.Animation animation, float strength)
	{
		animation.Fire(base.gameObject, strength);
		yield return DestroyAfterSecondsRoutine(animation.GetDuration());
	}

	public IEnumerator DestroyAfterSecondsRoutine(float seconds)
	{
		yield return Sequences.Wait(seconds);
		FloatingTextManager.ReturnToPool(this);
	}

	public IEnumerator FadeRoutine(AnimationCurve curve, float duration = 1f, float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return Sequences.Wait(delay);
		}

		if (curve.length > 1)
		{
			float t1 = curve[0].time;
			float num = curve[curve.length - 1].time - t1;
			float currentTime = 0f;
			float scale = num / duration;
			while (currentTime <= duration)
			{
				canvasGroup.alpha = curve.Evaluate(t1 + currentTime * scale);
				currentTime += Time.deltaTime;
				yield return null;
			}
		}

		FloatingTextManager.ReturnToPool(this);
	}
}
