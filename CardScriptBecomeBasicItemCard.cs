#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Become Basic Item Card", menuName = "Card Scripts/Become Basic Item Card")]
public class CardScriptBecomeBasicItemCard : CardScript
{
	public override void Run(CardData target)
	{
		target.playType = Card.PlayType.Play;
		target.needsTarget = true;
		target.canPlayOnBoard = true;
		target.canPlayOnHand = false;
		target.canPlayOnFriendly = true;
		target.canPlayOnEnemy = true;
		target.playOnSlot = false;
		target.defaultPlayPosition = CardData.PlayPosition.None;
	}
}
