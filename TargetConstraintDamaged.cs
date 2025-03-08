#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Is Damaged", menuName = "Target Constraints/Is Damaged")]
public class TargetConstraintDamaged : TargetConstraint
{
	public override bool Check(Entity target)
	{
		if (target.hp.current >= target.hp.max)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		return not;
	}
}
