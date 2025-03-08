#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class ScrollerScrollTo : MonoBehaviour
{
	[SerializeField]
	public Scroller scroller;

	[SerializeField]
	[Range(0f, 1f)]
	public float scrollTo = 1f;

	[SerializeField]
	public bool onAwake;

	[SerializeField]
	public bool onEnable = true;

	[SerializeField]
	public float delay;

	[SerializeField]
	public bool instant;

	public void Awake()
	{
		if (onAwake)
		{
			StartCoroutine(Sequence());
		}
	}

	public void OnEnable()
	{
		if (onEnable)
		{
			StartCoroutine(Sequence());
		}
	}

	public void Run()
	{
		StartCoroutine(Sequence());
	}

	public IEnumerator Sequence()
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}

		scroller.ScrollTo(scrollTo);
		if (instant)
		{
			scroller.rectTransform.anchoredPosition = scroller.targetPos;
		}
	}
}
