#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class SummonCreateCardAnimation : CreateCardAnimation
{
	[SerializeField]
	public ParticleSystem inParticleSystem;

	[SerializeField]
	public ParticleSystem outParticleSystem;

	[SerializeField]
	public float chargeDelay = 0.54f;

	public override IEnumerator Run(Entity entity, params CardData.StatusEffectStacks[] withEffects)
	{
		Routine.Clump clump = new Routine.Clump();
		SfxSystem.OneShot("event:/sfx/card/summon");
		clump.Add(In());
		clump.Add(CreateCardAnimation.GainEffects(entity, withEffects));
		yield return clump.WaitForEnd();
		Out();
		yield return new WaitForSeconds(chargeDelay);
		if (entity.display is Card card)
		{
			LeanTween.alphaCanvas(card.canvasGroup, 1f, 0.1f);
		}

		entity.curveAnimator.Ping();
		entity.wobbler.WobbleRandom();
		clump.Add(DestroyOnEnd(outParticleSystem));
	}

	public IEnumerator In()
	{
		inParticleSystem.Play();
		yield return new WaitForSeconds(inParticleSystem.main.duration);
	}

	public void Out()
	{
		inParticleSystem.gameObject.Destroy();
		outParticleSystem.Play();
	}
}
