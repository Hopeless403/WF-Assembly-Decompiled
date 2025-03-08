#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Play Type", menuName = "Target Constraints/Play Type")]
public class TargetConstraintPlayType : TargetConstraint
{
	[SerializeField]
	public Card.PlayType targetPlayType;

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		if (targetData.playType != targetPlayType)
		{
			return not;
		}

		return !not;
	}
}
