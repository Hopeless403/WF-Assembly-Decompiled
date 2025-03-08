#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Has Any Effect", menuName = "Target Constraints/Has Any Effect")]
public class TargetConstraintHasAnyEffect : TargetConstraint
{
	public override bool Check(CardData targetData)
	{
		CardData.StatusEffectStacks[] attackEffects = targetData.attackEffects;
		if ((attackEffects == null || attackEffects.Length <= 0) && (targetData.startWithEffects == null || !targetData.startWithEffects.Any((CardData.StatusEffectStacks a) => !a.data.isStatus)))
		{
			List<CardData.TraitStacks> traits = targetData.traits;
			if (traits != null)
			{
				return traits.Count > 0;
			}

			return false;
		}

		return true;
	}

	public override bool Check(Entity target)
	{
		List<CardData.StatusEffectStacks> attackEffects = target.attackEffects;
		if (attackEffects == null || attackEffects.Count <= 0)
		{
			if (target.statusEffects != null)
			{
				return target.statusEffects.Any((StatusEffectData a) => !a.isStatus);
			}

			return false;
		}

		return true;
	}
}
