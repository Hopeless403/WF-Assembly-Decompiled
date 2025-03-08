#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class OpenCharmBlockSequence : UISequence
{
	[Header("Custom Values")]
	[SerializeField]
	public CardUpgradeData charmData;

	[SerializeField]
	public Transform charmBlockHolder;

	[SerializeField]
	public GainCharmSequence gainCharmSequence;

	[SerializeField]
	public Character character;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public float animationDuration = 2f;

	[SerializeField]
	public ParticleSystem buildupParticleSystem;

	[SerializeField]
	public ParticleSystem explodeParticleSystem;

	public void SetCharm(CardUpgradeData charmData, Transform charmBlock)
	{
		this.charmData = charmData;
		charmBlockHolder.SetPositionAndRotation(charmBlock.position, charmBlock.rotation);
	}

	public void SetCharacter(Character character)
	{
		this.character = character;
	}

	public override IEnumerator Run()
	{
		yield return Sequences.Wait(startDelay);
		base.gameObject.SetActive(value: true);
		gainCharmSequence.SetCharm(charmData);
		gainCharmSequence.SetCharacter(character);
		animator.SetBool("BackgroundFade", value: true);
		Events.InvokeScreenRumble(0.1f, 0f, 0f, 0.1f, 0f, 0f);
		LeanTween.moveLocal(charmBlockHolder.gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutBack);
		LeanTween.rotateLocal(charmBlockHolder.gameObject, Vector3.zero, 1.5f).setEase(LeanTweenType.easeOutElastic);
		yield return Sequences.Wait(0.4f);
		animator.SetTrigger("Open");
		SfxSystem.OneShot("event:/sfx/inventory/charm_claim");
		Events.InvokeScreenRumble(0f, 2f, 0.25f, 0.75f, 0.5f, 0.25f);
		yield return Sequences.Wait(animationDuration);
		Events.InvokeScreenShake(5f, 0f);
		Routine.Clump clump = new Routine.Clump();
		clump.Add(gainCharmSequence.Run());
		clump.Add(FadeOutBackground(0.25f));
		yield return clump.WaitForEnd();
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator FadeOutBackground(float delay = 0f)
	{
		yield return Sequences.Wait(delay);
		animator.SetBool("BackgroundFade", value: false);
		yield return Sequences.Wait(0.5f);
	}

	public void PlayBuildUpParticleSystem()
	{
		buildupParticleSystem.Play();
	}

	public void PlayExplodeParticleSystem()
	{
		explodeParticleSystem.Play();
	}
}
