#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Max Counter More Than", menuName = "Target Constraints/Max Counter More Than")]
public class TargetConstraintMaxCounterMoreThan : TargetConstraint
{
	[SerializeField]
	public int moreThan;

	public override bool Check(Entity target)
	{
		if (target.counter.max <= moreThan)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		if (targetData.counter <= moreThan)
		{
			return not;
		}

		return !not;
	}
}
