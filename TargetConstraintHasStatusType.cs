#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Has Status Type", menuName = "Target Constraints/Has Status Type")]
public class TargetConstraintHasStatusType : TargetConstraint
{
	[SerializeField]
	public string statusType;

	public override bool Check(Entity target)
	{
		if (!target.FindStatus(statusType))
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		CardData.StatusEffectStacks[] startWithEffects = targetData.startWithEffects;
		for (int i = 0; i < startWithEffects.Length; i++)
		{
			if (startWithEffects[i].data.type == statusType)
			{
				return !not;
			}
		}

		return not;
	}
}
