#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class TransitionSnow : TransitionType
{
	[SerializeField]
	public ParticleSystem inParticles;

	[SerializeField]
	public ParticleSystem outParticles;

	[Header("Fade Image")]
	[SerializeField]
	public CanvasGroup fade;

	[Header("Tweens")]
	[SerializeField]
	public LeanTweenType easeIn = LeanTweenType.easeInCubic;

	[SerializeField]
	public float easeInDur = 0.6f;

	[SerializeField]
	public LeanTweenType easeOut = LeanTweenType.easeOutCubic;

	[SerializeField]
	public float easeOutDur = 0.1f;

	[Button("In", EButtonEnableMode.Always)]
	public void SnowIn()
	{
		if (!base.IsRunning)
		{
			StopAllCoroutines();
			StartCoroutine(In());
		}
	}

	[Button("Out", EButtonEnableMode.Always)]
	public void SnowOut()
	{
		if (!base.IsRunning)
		{
			StopAllCoroutines();
			StartCoroutine(Out());
		}
	}

	public override IEnumerator In()
	{
		inParticles.Play();
		base.IsRunning = true;
		fade.blocksRaycasts = true;
		fade.LeanAlpha(1f, easeInDur).setEase(easeIn);
		yield return new WaitForSeconds(easeInDur);
		base.IsRunning = false;
	}

	public override IEnumerator Out()
	{
		outParticles.Play();
		base.IsRunning = true;
		fade.blocksRaycasts = false;
		fade.LeanAlpha(0f, easeOutDur).setEase(easeOut);
		yield return new WaitForSeconds(easeOutDur);
		base.IsRunning = false;
		yield return new WaitUntil(() => !outParticles.isPlaying);
		base.gameObject.Destroy();
	}
}
