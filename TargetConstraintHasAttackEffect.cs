#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Has Attack Effect", menuName = "Target Constraints/Has Attack Effect")]
public class TargetConstraintHasAttackEffect : TargetConstraint
{
	[SerializeField]
	public StatusEffectData effect;

	public override bool Check(Entity target)
	{
		if (!target.attackEffects.Any((CardData.StatusEffectStacks a) => a.data == effect))
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if (!targetData.attackEffects.Any((CardData.StatusEffectStacks a) => a.data == effect))
		{
			return not;
		}

		return !not;
	}
}
