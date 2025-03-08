#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Does Kill", menuName = "Target Constraints/Does Kill")]
public class TargetConstraintDoesKill : TargetConstraint
{
	public override bool Check(Entity target)
	{
		if ((!target.data || !target.HasAttackIcon()) && !target.statusEffects.Any((StatusEffectData a) => a.doesDamage) && !target.attackEffects.Any((CardData.StatusEffectStacks a) => a.data.doesDamage))
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if ((!targetData || !targetData.hasAttack) && !targetData.startWithEffects.Any((CardData.StatusEffectStacks a) => a.data.doesDamage) && !targetData.attackEffects.Any((CardData.StatusEffectStacks a) => a.data.doesDamage))
		{
			return not;
		}

		return !not;
	}
}
