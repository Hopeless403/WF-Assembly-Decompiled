#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Can Be Boosted", menuName = "Target Constraints/Can Be Boosted")]
public class TargetConstraintCanBeBoosted : TargetConstraint
{
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

	public static bool CheckAttackEffects(Entity target)
	{
		return CheckAttackEffects(target.attackEffects.Select((CardData.StatusEffectStacks a) => a.data));
	}

	public static bool CheckAttackEffects(CardData targetData)
	{
		return CheckAttackEffects(targetData.attackEffects.Select((CardData.StatusEffectStacks a) => a.data));
	}

	public static bool CheckAttackEffects(IEnumerable<StatusEffectData> effects)
	{
		return effects?.Any((StatusEffectData e) => e.stackable) ?? false;
	}

	public static bool CheckPassiveEffects(Entity target)
	{
		return CheckPassiveEffects(target.statusEffects);
	}

	public static bool CheckPassiveEffects(CardData targetData)
	{
		return CheckPassiveEffects(targetData.startWithEffects.Select((CardData.StatusEffectStacks a) => a.data));
	}

	public static bool CheckPassiveEffects(IEnumerable<StatusEffectData> effects)
	{
		return effects?.Any((StatusEffectData e) => e.canBeBoosted) ?? false;
	}

	public static bool CheckTraits(Entity target)
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

	public static bool CheckTraits(CardData targetData)
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
