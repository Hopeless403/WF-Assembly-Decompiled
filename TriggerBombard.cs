#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;

public class TriggerBombard : Trigger
{
	public CardContainer[] slots;

	public TriggerBombard(Entity entity, Entity triggeredBy, string type, Entity[] targets, CardContainer[] slots)
		: base(entity, triggeredBy, type, targets)
	{
		this.slots = slots;
	}

	public override IEnumerator PreProcess()
	{
		List<Entity> list = new List<Entity>();
		CardContainer[] array = slots;
		for (int i = 0; i < array.Length; i++)
		{
			Entity top = array[i].GetTop();
			list.Add(top);
		}

		targets = list.ToArray();
		yield return StatusEffectSystem.PreCardPlayedEvent(entity, targets);
		TriggerBombard triggerBombard = this;
		Entity[] array2 = targets;
		triggerBombard.hits = new Hit[(array2 != null) ? array2.Length : 0];
		if (targets != null)
		{
			for (int j = 0; j < targets.Length; j++)
			{
				Hit hit = new Hit(entity, targets[j]);
				hit.AddAttackerStatuses();
				hit.trigger = this;
				hits[j] = hit;
			}
		}

		Hit[] array3 = hits;
		foreach (Hit hit2 in array3)
		{
			if ((bool)hit2.target)
			{
				yield return StatusEffectSystem.PreAttackEvent(hit2);
			}
		}
	}

	public override IEnumerator Animate()
	{
		new Routine(AssetLoader.Lookup<CardAnimation>("CardAnimations", "BombardRocketShoot").Routine(entity));
		yield return Sequences.Wait(0.2f);
	}

	public override IEnumerator ProcessHits()
	{
		yield return RainRockets();
	}

	public IEnumerator RainRockets()
	{
		CardAnimation rocketAnimation = AssetLoader.Lookup<CardAnimation>("CardAnimations", "BombardRocket");
		Routine.Clump clump = new Routine.Clump();
		for (int i = 0; i < hits.Length; i++)
		{
			Hit hit = hits[i];
			CardContainer slot = slots[i];
			clump.Add(Fire(rocketAnimation, hit, slot));
			yield return Sequences.Wait(0.3f);
		}

		yield return clump.WaitForEnd();
	}

	public static IEnumerator Fire(CardAnimation rocketAnimation, Hit hit, CardContainer slot)
	{
		yield return rocketAnimation.Routine(slot.transform.position);
		if ((bool)hit.target)
		{
			yield return Trigger.ProcessHit(hit);
		}
	}
}
