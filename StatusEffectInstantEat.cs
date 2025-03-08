#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Eat", fileName = "Eat")]
public class StatusEffectInstantEat : StatusEffectInstant
{
	[SerializeField]
	public bool gainHealth = true;

	[SerializeField]
	public bool gainAttack = true;

	[SerializeField]
	public bool gainEffects = true;

	[SerializeField]
	[ShowIf("gainEffects")]
	public TraitData[] illegalTraits;

	[SerializeField]
	[ShowIf("gainEffects")]
	public StatusEffectData[] illegalEffects;

	public override IEnumerator Process()
	{
		if ((bool)applier && applier.alive && (bool)target && target.alive && (gainHealth || gainAttack || gainEffects))
		{
			yield return Eat();
		}

		target.forceKill = DeathType.Eaten;
		yield return base.Process();
	}

	public IEnumerator Eat()
	{
		if (gainHealth)
		{
			GainHealth();
		}

		if (gainAttack)
		{
			GainAttack();
		}

		if (gainEffects)
		{
			yield return GainEffects();
			applier.PromptUpdate();
		}
	}

	public void GainHealth()
	{
		applier.hp.current += target.hp.current;
		applier.hp.max += target.hp.max;
	}

	public void GainAttack()
	{
		applier.damage.current += target.damage.current;
		applier.damage.max += target.damage.max;
	}

	public IEnumerator GainEffects()
	{
		applier.attackEffects = CardData.StatusEffectStacks.Stack(applier.attackEffects, target.attackEffects).ToList();
		List<StatusEffectData> list = target.statusEffects.Where((StatusEffectData e) => e != this && !illegalEffects.Select((StatusEffectData e) => e.name).Contains(e.name)).ToList();
		foreach (Entity.TraitStacks trait in target.traits)
		{
			foreach (StatusEffectData passiveEffect in trait.passiveEffects)
			{
				list.Remove(passiveEffect);
			}

			int num = trait.count - trait.tempCount;
			if (num > 0 && !illegalTraits.Select((TraitData t) => t.name).Contains(trait.data.name))
			{
				applier.GainTrait(trait.data, num);
			}
		}

		foreach (StatusEffectData item in list)
		{
			yield return StatusEffectSystem.Apply(applier, target, item, item.count);
		}

		yield return applier.UpdateTraits();
		applier.display.promptUpdateDescription = true;
	}
}
