#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Is In Deck", menuName = "Target Constraints/Is In Deck")]
public class TargetConstraintIsInDeck : TargetConstraint
{
	[SerializeField]
	public bool includeReserve = true;

	public override bool Check(CardData targetData)
	{
		if (References.PlayerData == null)
		{
			return false;
		}

		if (!IsInDeck(targetData))
		{
			if (includeReserve)
			{
				return IsInReserve(targetData);
			}

			return false;
		}

		return true;
	}

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public static bool IsInDeck(CardData cardData)
	{
		return References.PlayerData.inventory.deck.Contains(cardData);
	}

	public static bool IsInReserve(CardData cardData)
	{
		return References.PlayerData.inventory.reserve.Contains(cardData);
	}
}
