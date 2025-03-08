#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Can Be Hit", menuName = "Target Constraints/Can Be Hit")]
public class TargetConstraintCanBeHit : TargetConstraint
{
	public override bool Check(Entity target)
	{
		if (!target.canBeHit)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if (!targetData.canBeHit)
		{
			return not;
		}

		return !not;
	}
}
