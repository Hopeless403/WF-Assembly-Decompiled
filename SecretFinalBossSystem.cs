#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class SecretFinalBossSystem : GameSystem
{
	[SerializeField]
	public string[] requireInDeck = new string[1] { "LuminVase" };

	[SerializeField]
	public string targetNodeName = "CampaignNodeFinalBoss";

	public void OnEnable()
	{
		Events.OnBattleEnd += BattleEnd;
	}

	public void OnDisable()
	{
		Events.OnBattleEnd -= BattleEnd;
	}

	public void BattleEnd()
	{
		CampaignNode node = Campaign.FindCharacterNode(References.Player);
		CheckContinuePastFinalBoss(node);
	}

	public void CheckContinuePastFinalBoss(CampaignNode node)
	{
		if (node.finalNode && node.type.name == targetNodeName && PlayerHasRequiredCards())
		{
			SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "trueWin", value: true);
			node.finalNode = false;
		}
	}

	public bool PlayerHasRequiredCards()
	{
		List<string> list = requireInDeck.ToList();
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			int num = list.IndexOf(item.name);
			if (num >= 0)
			{
				list.RemoveAt(num);
				if (list.Count <= 0)
				{
					break;
				}
			}
		}

		return list.Count <= 0;
	}
}
