#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapStamp : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Stamp(Vector3 position)
	{
		base.gameObject.SetActive(value: true);
		base.transform.position = position;
		base.transform.localEulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(-20, 20));
		LeanTween.cancel(base.gameObject);
		base.transform.localScale = Vector3.one * 2f;
		LeanTween.scale(base.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBounce);
		spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		LeanTween.color(base.gameObject, Color.white, 0.33f).setEase(LeanTweenType.easeOutQuad);
	}

	public void FadeOut()
	{
		LeanTween.cancel(base.gameObject);
		LeanTween.color(base.gameObject, new Color(1f, 1f, 1f, 0f), 0.33f).setEase(LeanTweenType.easeOutQuad).setOnComplete((Action)delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}
}
