#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Has Reaction", menuName = "Target Constraints/Has Reaction")]
public class TargetConstraintHasReaction : TargetConstraint
{
	public override bool Check(Entity target)
	{
		if (target.statusEffects.Any((StatusEffectData effect) => effect.isReaction))
		{
			return !not;
		}

		return not;
	}

	public override bool Check(CardData targetData)
	{
		if (targetData.startWithEffects.Any((CardData.StatusEffectStacks effectStacks) => effectStacks.data.isReaction))
		{
			return !not;
		}

		return not;
	}
}
