#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class InjuredCompanionEventSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnCampaignLoadPreset += CampaignLoadPreset;
	}

	public void OnDisable()
	{
		Events.OnCampaignLoadPreset -= CampaignLoadPreset;
	}

	public static int GetCampaignInsertPosition(RunHistory mostRecentRun)
	{
		int num = mostRecentRun.stats?.Count("battlesWon") ?? 0;
		int num2 = ((num >= 6) ? 23 : ((num >= 3) ? 11 : 2));
		Debug.Log($"InjuredCompanionEventSystem - Battles Won: {num} - Insert Pos: {num2}");
		return num2;
	}

	public static void CampaignLoadPreset(ref string[] lines)
	{
		if (Campaign.Data.GameMode.mainGameMode && !Campaign.Data.GameMode.tutorialRun)
		{
			RunHistory mostRecentRun = GetMostRecentRun();
			if (mostRecentRun != null && mostRecentRun.result == Campaign.Result.Lose && HasEligibleCompanion(mostRecentRun))
			{
				int campaignInsertPosition = GetCampaignInsertPosition(mostRecentRun);
				lines[0] = lines[0].Insert(campaignInsertPosition, "#");
				lines[1] = lines[1].Insert(campaignInsertPosition, " ");
				lines[2] = lines[2].Insert(campaignInsertPosition, lines[2][campaignInsertPosition - 1].ToString());
				lines[3] = lines[3].Insert(campaignInsertPosition, lines[3][campaignInsertPosition - 1].ToString());
			}
		}
	}

	public static RunHistory GetMostRecentRun()
	{
		List<RunHistory> list = SaveSystem.LoadHistoryData<List<RunHistory>>("list");
		if (list != null && list.Count > 0)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (list[num] != null)
				{
					string gameModeName = list[num].gameModeName;
					if (gameModeName != null)
					{
						GameMode gameMode = AddressableLoader.Get<GameMode>("GameMode", gameModeName);
						if ((bool)gameMode && gameMode.mainGameMode)
						{
							return list[num];
						}
					}
				}
			}
		}

		return null;
	}

	public static bool HasEligibleCompanion(RunHistory run)
	{
		string[] unlockedPets = MetaprogressionSystem.GetUnlockedPets();
		CardSaveData[] deck = run.inventory.deck;
		for (int i = 0; i < deck.Length; i++)
		{
			if (IsEligible(deck[i], unlockedPets))
			{
				return true;
			}
		}

		deck = run.inventory.reserve;
		for (int i = 0; i < deck.Length; i++)
		{
			if (IsEligible(deck[i], unlockedPets))
			{
				return true;
			}
		}

		return false;
	}

	public static bool IsEligible(CardSaveData card, string[] illegal)
	{
		if (illegal.Contains(card.name))
		{
			return false;
		}

		CardData cardData = card.Peek();
		if ((bool)cardData)
		{
			return cardData.cardType.name == "Friendly";
		}

		return false;
	}

	public static List<CardSaveData> GetEligibleCompanions(RunHistory run)
	{
		string[] unlockedPets = MetaprogressionSystem.GetUnlockedPets();
		List<CardSaveData> list = new List<CardSaveData>();
		CardSaveData[] deck = run.inventory.deck;
		foreach (CardSaveData cardSaveData in deck)
		{
			if (IsEligible(cardSaveData, unlockedPets))
			{
				list.Add(cardSaveData);
			}
		}

		deck = run.inventory.reserve;
		foreach (CardSaveData cardSaveData2 in deck)
		{
			if (IsEligible(cardSaveData2, unlockedPets))
			{
				list.Add(cardSaveData2);
			}
		}

		return list;
	}
}
