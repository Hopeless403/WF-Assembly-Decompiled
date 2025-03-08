#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Attack More Than", menuName = "Target Constraints/Attack More Than")]
public class TargetConstraintAttackMoreThan : TargetConstraint
{
	[SerializeField]
	public int value;

	public override bool Check(Entity target)
	{
		if (target.damage.current + target.tempDamage.Value <= value)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if (targetData.damage <= value)
		{
			return not;
		}

		return !not;
	}
}
