#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Is Unit", menuName = "Target Constraints/Is Unit")]
public class TargetConstraintIsUnit : TargetConstraint
{
	[SerializeField]
	public bool mustBeMiniboss;

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		if (!targetData)
		{
			return not;
		}

		CardType cardType = targetData.cardType;
		if (!cardType || !cardType.unit || !CheckMiniboss(cardType))
		{
			return not;
		}

		return !not;
	}

	public bool CheckMiniboss(CardType cardType)
	{
		if (mustBeMiniboss)
		{
			return cardType.miniboss;
		}

		return true;
	}
}
