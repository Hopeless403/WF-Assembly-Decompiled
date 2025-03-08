#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class UISequence : MonoBehaviour
{
	public bool promptEnd;

	public float startDelay = 0.25f;

	public float delayBetween = 0.2f;

	[Header("Tween In")]
	public float tweenInDur = 0.75f;

	public LeanTweenType tweenInEase = LeanTweenType.easeOutBack;

	[Header("Tween Out")]
	public float tweenOutDur = 0.25f;

	public LeanTweenType tweenOutEase = LeanTweenType.easeInBack;

	public Routine routine;

	public bool IsRunning => routine?.IsRunning ?? false;

	public void Begin()
	{
		if (IsRunning)
		{
			routine.Stop();
		}

		routine = new Routine(Run());
	}

	public virtual void End()
	{
		promptEnd = true;
	}

	public void EndIfRunning()
	{
		if (IsRunning)
		{
			End();
		}
	}

	public virtual IEnumerator Run()
	{
		return null;
	}
}
