#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class UIAnchor : MonoBehaviour
{
	public float revealDur = 0.75f;

	public float hideDur = 0.25f;

	[Header("Rotation")]
	public Vector3 angleStart = new Vector3(45f, 90f, 0f);

	public bool angleRandomSign = true;

	public LeanTweenType angleTweenEase = LeanTweenType.easeOutBack;

	[Header("Rotation Hide")]
	public Vector3 angleHide = new Vector3(45f, 90f, 0f);

	public bool angleHideRandomSign = true;

	public LeanTweenType angleHideTweenEase = LeanTweenType.easeInBack;

	[Header("Scale")]
	public Vector3 scaleStart = Vector3.one;

	public LeanTweenType scaleTweenEase = LeanTweenType.easeInQuart;

	[Header("Scale Hide")]
	public Vector3 scaleHide = Vector3.zero;

	public LeanTweenType scaleHideTweenEase = LeanTweenType.easeInBack;

	[Header("Wobble?")]
	public float wobbleAmount = 2f;

	[Header("Fade In?")]
	public bool doFadeIn = true;

	public float fadeInDur = 0.25f;

	public bool doFadeOut = true;

	public float fadeOutDur = 0.25f;

	public void SetUp()
	{
		if (angleRandomSign)
		{
			base.transform.localEulerAngles = new Vector3(angleStart.x.WithRandomSign(), angleStart.y.WithRandomSign(), angleStart.z.WithRandomSign());
		}
		else
		{
			base.transform.localEulerAngles = angleStart;
		}

		base.transform.localScale = scaleStart;
		if (doFadeIn)
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.alpha = 0f;
			}
		}
	}

	public void Reveal()
	{
		LeanTween.cancel(base.gameObject);
		Vector3 zero = Vector3.zero;
		if (base.transform.localEulerAngles != zero)
		{
			LeanTween.rotateLocal(base.gameObject, zero, revealDur).setEase(angleTweenEase);
		}

		Vector3 one = Vector3.one;
		if (base.transform.localScale != one)
		{
			LeanTween.scale(base.gameObject, one, revealDur).setEase(scaleTweenEase);
		}

		if (doFadeIn && fadeInDur > 0f)
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.LeanAlpha(1f, fadeInDur).setEase(LeanTweenType.linear);
			}
		}

		if (wobbleAmount > 0f)
		{
			Vector3 vector = -(base.transform.localEulerAngles / 90f) * wobbleAmount;
			base.transform.GetComponentInChildren<Wobbler>()?.Wobble(new Vector3(vector.y, vector.x, 0f));
		}
	}

	public void UnReveal(float delay = 0f)
	{
		LeanTween.cancel(base.gameObject);
		Vector3 vector = angleHide;
		if (base.transform.localEulerAngles != vector)
		{
			LeanTween.rotateLocal(base.gameObject, vector, hideDur).setDelay(delay).setEase(angleHideTweenEase);
		}

		Vector3 vector2 = scaleHide;
		if (base.transform.localScale != vector2)
		{
			LeanTween.scale(base.gameObject, vector2, hideDur).setDelay(delay).setEase(scaleHideTweenEase);
		}

		if (doFadeOut && fadeOutDur > 0f)
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.LeanAlpha(0f, fadeOutDur).setDelay(delay).setEase(LeanTweenType.linear);
			}
		}
	}
}
