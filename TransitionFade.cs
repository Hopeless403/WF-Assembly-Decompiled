#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class TransitionFade : TransitionType
{
	public CanvasGroup fade;

	[Header("Tweens")]
	public LeanTweenType easeIn = LeanTweenType.easeInQuint;

	public float easeInDur = 0.5f;

	public LeanTweenType easeOut = LeanTweenType.easeOutQuint;

	public float easeOutDur = 0.5f;

	public override IEnumerator In()
	{
		base.IsRunning = true;
		fade.blocksRaycasts = true;
		fade.LeanAlpha(1f, easeInDur).setEase(easeIn);
		yield return new WaitForSeconds(easeInDur);
		base.IsRunning = false;
	}

	public override IEnumerator Out()
	{
		base.IsRunning = true;
		fade.blocksRaycasts = false;
		fade.LeanAlpha(0f, easeOutDur).setEase(easeOut);
		yield return new WaitForSeconds(easeOutDur);
		base.IsRunning = false;
		base.gameObject.Destroy();
	}
}
