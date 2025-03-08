#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmoothScrollRect : ScrollRect
{
	[SerializeField]
	public float speed = 1f;

	public const float smoothScrollTime = 0.1f;

	public const LeanTweenType smoothScrollEase = LeanTweenType.easeOutQuart;

	public LTDescr tween;

	public override void OnScroll(PointerEventData data)
	{
		if (base.isActiveAndEnabled)
		{
			Vector2 from = base.normalizedPosition;
			if (tween != null)
			{
				LeanTween.cancel(tween.uniqueId);
			}

			base.OnScroll(data);
			Vector2 to = base.normalizedPosition.Clamp(Vector2.zero, Vector2.one);
			base.normalizedPosition = from;
			tween = LeanTween.value(base.gameObject, from, to, base.scrollSensitivity * 0.1f).setEase(LeanTweenType.easeOutQuart).setOnUpdateVector2(delegate(Vector2 a)
			{
				base.normalizedPosition = a;
			});
			tween.setIgnoreTimeScale(useUnScaledTime: true);
		}
	}

	public void ScrollTo(Vector2 targetPosition)
	{
		Vector2 from = base.normalizedPosition;
		if (tween != null)
		{
			LeanTween.cancel(tween.uniqueId);
		}

		base.content.anchoredPosition = targetPosition;
		Vector2 to = base.normalizedPosition.Clamp(Vector2.zero, Vector2.one);
		base.normalizedPosition = from;
		tween = LeanTween.value(base.gameObject, from, to, base.scrollSensitivity * 0.1f / speed).setEase(LeanTweenType.easeOutQuart).setOnUpdateVector2(delegate(Vector2 a)
		{
			base.normalizedPosition = a;
		});
		tween.setIgnoreTimeScale(useUnScaledTime: true);
	}

	public void SetContent(GameObject comp)
	{
		RectTransform component = comp.GetComponent<RectTransform>();
		if ((object)component != null)
		{
			base.content = component;
		}
	}

	public void SetContent(RectTransform rectTransform)
	{
		base.content = rectTransform;
	}

	public void ScrollToTop()
	{
		base.verticalNormalizedPosition = 1f;
	}

	public void ScrollToTopAfterFrame()
	{
		StartCoroutine(ScrollToTopAfterFrameRoutine());
	}

	public IEnumerator ScrollToTopAfterFrameRoutine()
	{
		yield return new WaitForEndOfFrame();
		ScrollToTop();
	}
}
