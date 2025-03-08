#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Can Take Crown", menuName = "Target Constraints/Can Take Crown")]
public class TargetConstraintCanTakeCrown : TargetConstraint
{
	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		if (!targetData.cardType.canTakeCrown)
		{
			return not;
		}

		return !not;
	}
}
