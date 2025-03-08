#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Does Damage", menuName = "Target Constraints/Does Damage")]
public class TargetConstraintDoesDamage : TargetConstraint
{
	public override bool Check(Entity target)
	{
		if (!target.data || !target.HasAttackIcon())
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if (!targetData || !targetData.hasAttack)
		{
			return not;
		}

		return !not;
	}
}
