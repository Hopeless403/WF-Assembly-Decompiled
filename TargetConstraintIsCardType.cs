#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Is Card Type", menuName = "Target Constraints/Is Card Type")]
public class TargetConstraintIsCardType : TargetConstraint
{
	[SerializeField]
	public CardType[] allowedTypes;

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		if (!allowedTypes.Contains(targetData.cardType))
		{
			return not;
		}

		return !not;
	}
}
