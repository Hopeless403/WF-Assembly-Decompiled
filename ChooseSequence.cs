#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using UnityEngine;

public class ChooseSequence : UISequence
{
	public RectTransform background;

	public override IEnumerator Run()
	{
		UIAnchors anchors = GetComponent<UIAnchors>();
		UIAnchors.AnchorPoint[] list = anchors.list;
		foreach (UIAnchors.AnchorPoint obj in list)
		{
			obj.Deactivate();
			obj.SetUp();
		}

		background.gameObject.SetActive(value: false);
		yield return Sequences.Wait(startDelay);
		base.gameObject.SetActive(value: true);
		background.gameObject.SetActive(value: true);
		background.localScale = Vector3.zero;
		yield return null;
		background.LeanScale(Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic);
		yield return Sequences.Wait(0.25f);
		int c = anchors.Count;
		for (int i = 0; i < c; i++)
		{
			anchors.Activate(i);
			yield return null;
			StartCoroutine(anchors.Reveal(i));
		}

		StartCoroutine(anchors.UpdatePositions());
		while (!promptEnd)
		{
			yield return null;
		}

		background.LeanScale(Vector3.zero, tweenOutDur).setEase(LeanTweenType.easeInBack);
		for (int k = 0; k < c; k++)
		{
			float delay = PettyRandom.Range(0f, delayBetween);
			StartCoroutine(anchors.UnReveal(k, delay));
		}

		yield return Sequences.Wait(tweenInDur + delayBetween);
		base.gameObject.SetActive(value: false);
	}
}
