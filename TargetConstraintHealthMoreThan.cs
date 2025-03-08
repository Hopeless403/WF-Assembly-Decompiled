#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Health More Than", menuName = "Target Constraints/Health More Than")]
public class TargetConstraintHealthMoreThan : TargetConstraint
{
	[SerializeField]
	public int value;

	public override bool Check(Entity target)
	{
		if (target.hp.current <= value)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if (targetData.hp <= value)
		{
			return not;
		}

		return !not;
	}
}
