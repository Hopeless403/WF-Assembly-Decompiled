#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

public class DrainLeaderModifierSystem : GameSystem
{
	public const int addHealth = -1;

	public void OnEnable()
	{
		Events.PostBattle += PostBattle;
	}

	public void OnDisable()
	{
		Events.PostBattle -= PostBattle;
	}

	public void PostBattle(CampaignNode campaignNode)
	{
		CardData cardData = References.PlayerData.inventory.deck.FirstOrDefault((CardData a) => a.cardType.miniboss);
		if ((bool)cardData && cardData.hp > 1)
		{
			int hp = cardData.hp;
			cardData.hp = Mathf.Max(1, cardData.hp + -1);
			Debug.Log($"[{this}] draining leader health ({hp} → {cardData.hp})");
		}
	}
}
