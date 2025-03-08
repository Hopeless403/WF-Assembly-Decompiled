#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Effects More Than", menuName = "Target Constraints/Effects More Than")]
public class TargetConstraintEffectsMoreThan : TargetConstraint
{
	[SerializeField]
	public int amount;

	public override bool Check(Entity target)
	{
		if (CheckAttackEffects(target))
		{
			return !not;
		}

		if (CheckPassiveEffects(target))
		{
			return !not;
		}

		return not;
	}

	public override bool Check(CardData targetData)
	{
		if (CheckAttackEffects(targetData))
		{
			return !not;
		}

		if (CheckPassiveEffects(targetData))
		{
			return !not;
		}

		if (CheckTraits(targetData))
		{
			return !not;
		}

		return not;
	}

	public bool CheckAttackEffects(Entity target)
	{
		return CheckAttackEffects(target.attackEffects);
	}

	public bool CheckAttackEffects(CardData targetData)
	{
		return CheckAttackEffects(targetData.attackEffects);
	}

	public bool CheckAttackEffects(IEnumerable<CardData.StatusEffectStacks> effects)
	{
		return effects?.Any((CardData.StatusEffectStacks e) => e.data.stackable && e.count > amount) ?? false;
	}

	public bool CheckPassiveEffects(Entity target)
	{
		return CheckPassiveEffects(target.statusEffects);
	}

	public bool CheckPassiveEffects(CardData targetData)
	{
		if (targetData.startWithEffects == null)
		{
			return false;
		}

		CardData.StatusEffectStacks[] array = targetData.startWithEffects.Where((CardData.StatusEffectStacks e) => e.data.canBeBoosted).ToArray();
		if (array.Length != 0)
		{
			return array.All((CardData.StatusEffectStacks e) => e.count > amount);
		}

		return false;
	}

	public bool CheckPassiveEffects(IEnumerable<StatusEffectData> effects)
	{
		if (effects == null)
		{
			return false;
		}

		StatusEffectData[] array = effects.Where((StatusEffectData e) => e.canBeBoosted).ToArray();
		if (array.Length != 0)
		{
			return array.All((StatusEffectData e) => e.count > amount);
		}

		return false;
	}

	public bool CheckTraits(Entity target)
	{
		if (target.traits == null)
		{
			return false;
		}

		foreach (Entity.TraitStacks trait in target.traits)
		{
			if (CheckPassiveEffects(trait.data.effects))
			{
				return true;
			}
		}

		return false;
	}

	public bool CheckTraits(CardData targetData)
	{
		if (targetData.traits == null)
		{
			return false;
		}

		foreach (CardData.TraitStacks trait in targetData.traits)
		{
			if (CheckPassiveEffects(trait.data.effects))
			{
				return true;
			}
		}

		return false;
	}
}
