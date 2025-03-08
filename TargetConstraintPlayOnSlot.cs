#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Plays On", menuName = "Target Constraints/Plays On")]
public class TargetConstraintPlayOnSlot : TargetConstraint
{
	[SerializeField]
	public bool slot;

	[SerializeField]
	public bool board;

	[SerializeField]
	public bool hand;

	[SerializeField]
	public bool enemy;

	[SerializeField]
	public bool friendly;

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		if ((slot && !targetData.playOnSlot) || (board && !targetData.canPlayOnBoard) || (hand && !targetData.canPlayOnHand) || (enemy && !targetData.canPlayOnEnemy) || (friendly && !targetData.canPlayOnFriendly))
		{
			return not;
		}

		return !not;
	}
}
