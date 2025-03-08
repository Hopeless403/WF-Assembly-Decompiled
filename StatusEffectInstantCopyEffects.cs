#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Copy Effects", fileName = "Copy Effects")]
public class StatusEffectInstantCopyEffects : StatusEffectInstant
{
	[SerializeField]
	public bool replaceAllEffects = true;

	[SerializeField]
	[HideIf("replaceAllEffects")]
	public int replaceEffectNumber;

	public override IEnumerator Process()
	{
		yield return replaceAllEffects ? RemoveAllEffects() : RemoveEffect(replaceEffectNumber);
		yield return AddEffectCopies();
		if (applier.display is Card card)
		{
			card.promptUpdateDescription = true;
		}

		applier.PromptUpdate();
		yield return base.Process();
	}

	public IEnumerator RemoveAllEffects()
	{
		foreach (Entity.TraitStacks trait in applier.traits)
		{
			trait.count = 0;
		}

		yield return applier.UpdateTraits();
		Routine.Clump clump = new Routine.Clump();
		foreach (StatusEffectData item in applier.statusEffects.Where((StatusEffectData e) => !e.visible))
		{
			clump.Add(item.Remove());
		}

		yield return clump.WaitForEnd();
	}

	public IEnumerator RemoveEffect(int effectNumber)
	{
		StatusEffectData statusEffectData = applier.statusEffects[effectNumber];
		yield return statusEffectData.Remove();
	}

	public IEnumerator AddEffectCopies()
	{
		List<StatusEffectData> list = target.statusEffects.Where((StatusEffectData e) => e.count > e.temporary && !e.isStatus && e != this && e.HasDescOrIsKeyword).ToList();
		foreach (Entity.TraitStacks trait in target.traits)
		{
			foreach (StatusEffectData passiveEffect in trait.passiveEffects)
			{
				list.Remove(passiveEffect);
			}

			int num = trait.count - trait.tempCount;
			if (num > 0)
			{
				applier.GainTrait(trait.data, num);
			}
		}

		foreach (StatusEffectData item in list)
		{
			yield return StatusEffectSystem.Apply(applier, item.applier, item, item.count - item.temporary);
		}

		applier.attackEffects = (from a in CardData.StatusEffectStacks.Stack(applier.attackEffects, target.attackEffects)
			select a.Clone()).ToList();
		yield return applier.UpdateTraits();
	}
}
