#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour
{
	public CanvasGroup _canvasGroup;

	[SerializeField]
	public float fadeOutTime = 0.5f;

	[SerializeField]
	public LeanTweenType fadeOutEase = LeanTweenType.easeInOutQuad;

	[SerializeField]
	public bool removeInteractable = true;

	[SerializeField]
	public bool removeBlocksRaycast = true;

	[SerializeField]
	public bool disableAfter;

	[Header("Fade Out After Delay")]
	[SerializeField]
	public bool fadeOutAfter;

	[SerializeField]
	[ShowIf("fadeOutAfter")]
	public bool afterEnable;

	[SerializeField]
	[ShowIf("fadeOutAfter")]
	public float delay;

	public CanvasGroup canvasGroup => _canvasGroup ?? (_canvasGroup = GetComponent<CanvasGroup>());

	public void OnEnable()
	{
		if (fadeOutAfter && afterEnable)
		{
			StartCoroutine(FadeOutAfter(delay));
		}
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public IEnumerator FadeOutAfter(float delay)
	{
		yield return new WaitForSeconds(delay);
		FadeOut();
	}

	public void FadeOut()
	{
		LeanTween.cancel(base.gameObject);
		LeanTween.alphaCanvas(canvasGroup, 0f, fadeOutTime).setEase(fadeOutEase);
		if (removeInteractable)
		{
			canvasGroup.interactable = false;
		}

		if (removeBlocksRaycast)
		{
			canvasGroup.blocksRaycasts = false;
		}

		if (disableAfter)
		{
			StartCoroutine(DisableAfter(fadeOutTime));
		}
	}

	public IEnumerator DisableAfter(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.gameObject.SetActive(value: false);
	}
}
