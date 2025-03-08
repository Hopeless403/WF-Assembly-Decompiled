#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

public class FinalBossDeckGenerationSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnCampaignEnd += CampaignEnd;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
	}

	public static void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		if (Campaign.Data.GameMode.mainGameMode)
		{
			if (CheckTrueWin(result))
			{
				RevertToDefaultBoss();
			}
			else if (CheckResult(result))
			{
				SetNewBoss(playerData);
			}
		}
	}

	public static bool CheckResult(Campaign.Result result)
	{
		bool flag = SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false);
		if (result != Campaign.Result.Win || flag)
		{
			return result != Campaign.Result.Win && flag;
		}

		return true;
	}

	public static bool CheckTrueWin(Campaign.Result result)
	{
		bool flag = SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false);
		return result == Campaign.Result.Win && flag;
	}

	public static void SetNewBoss(PlayerData playerData)
	{
		SaveSystem.SaveProgressData("finalBossDeck", playerData.inventory.deck.SaveArray<CardData, CardSaveData>());
		SaveSystem.SaveProgressData("newFinalBoss", value: true);
		Debug.Log("~ Player's Deck Saved! [" + string.Join(", ", playerData.inventory.deck.Select((CardData a) => a.name)) + "]");
	}

	public static void RevertToDefaultBoss()
	{
		SaveSystem.DeleteProgressData("finalBossDeck");
		SaveSystem.DeleteProgressData("newFinalBoss");
		Debug.Log("~ TRUE VICTORY! Reverting Final Boss To Default");
	}
}
