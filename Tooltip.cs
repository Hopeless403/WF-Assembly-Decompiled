#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tooltip : MonoBehaviour
{
	public bool animate = true;

	public bool ignoreTimeScale;

	public readonly HashSet<Tooltip> children = new HashSet<Tooltip>();

	[SerializeField]
	public CanvasGroup canvasGroup;

	[Header("Panel")]
	[SerializeField]
	public Image panel;

	[SerializeField]
	public Sprite defaultPanelSprite;

	[SerializeField]
	public Color defaultPanelColor;

	[Header("Animation")]
	[SerializeField]
	public Vector3 scaleFrom = Vector3.zero;

	[SerializeField]
	public AnimationCurve scaleCurve;

	[SerializeField]
	public float scaleDur = 0.5f;

	[SerializeField]
	public float fadeFrom;

	[SerializeField]
	public AnimationCurve fadeCurve;

	[SerializeField]
	public float fadeDur = 0.2f;

	public void Ping()
	{
		if (animate)
		{
			LeanTween.cancel(base.gameObject);
			base.transform.localScale = scaleFrom;
			LeanTween.scale(base.gameObject, Vector3.one, scaleDur).setEase(scaleCurve).setIgnoreTimeScale(ignoreTimeScale);
			canvasGroup.alpha = fadeFrom;
			canvasGroup.LeanAlpha(1f, fadeDur).setEase(fadeCurve).setIgnoreTimeScale(ignoreTimeScale);
		}
	}

	public Tooltip()
	{
	}
}
