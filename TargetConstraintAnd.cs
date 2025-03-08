#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "And", menuName = "Target Constraints/And")]
public class TargetConstraintAnd : TargetConstraint
{
	[SerializeField]
	public TargetConstraint[] constraints;

	public override bool Check(Entity target)
	{
		TargetConstraint[] array = constraints;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Check(target))
			{
				return not;
			}
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		TargetConstraint[] array = constraints;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Check(targetData))
			{
				return not;
			}
		}

		return !not;
	}
}
