#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Is Specific Card", menuName = "Target Constraints/Is Specific Card")]
public class TargetConstraintIsSpecificCard : TargetConstraint
{
	[SerializeField]
	public CardData[] allowedCards;

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		if (!allowedCards.Any((CardData a) => a.name == targetData.name))
		{
			return not;
		}

		return !not;
	}
}
