#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Threading.Tasks;
using Steamworks.Data;
using UnityEngine;

public class ScoreSubmitSystem : GameSystem
{
	public enum Status
	{
		Submitting,
		Failed,
		Success
	}

	public static ScoreSubmitSystem instance;

	public const int StartScore = -100;

	public static Status status { get; set; }

	public static int? playerRank { get; set; }

	public static int SubmittedTime { get; set; }

	public static int SubmittedGold { get; set; }

	public static int SubmittedBattlesWon { get; set; }

	public static int SubmittedScore { get; set; }

	public static LeaderboardUpdate? result { get; set; }

	public void OnEnable()
	{
		instance = this;
		Events.OnCampaignEnd += CampaignEnd;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
	}

	public static void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		if (Campaign.Data.GameMode.submitScore)
		{
			bool win = result == Campaign.Result.Win;
			instance.StartCoroutine(SubmitScore(win, stats));
		}
	}

	public static IEnumerator SubmitScore(bool win, CampaignStats stats)
	{
		yield return null;
		playerRank = null;
		status = Status.Submitting;
		DateTime date = DateTime.ParseExact(SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "startDate", ""), "dd/MM/yyyy", GameManager.CultureInfo);
		int num = (win ? 1 : 0);
		SubmittedTime = Mathf.RoundToInt((float)(stats.hours * 3600) + stats.time);
		SubmittedGold = stats.Count("goldGained");
		SubmittedBattlesWon = stats.Count("battlesWon");
		SubmittedScore = GetScore(win, SubmittedTime, SubmittedGold, SubmittedBattlesWon);
		if (SteamManager.init)
		{
			Task<LeaderboardUpdate?> task = Scores.Submit(Campaign.Data.GameMode.leaderboardType, date, SubmittedScore, SubmittedTime, num);
			yield return new WaitUntil(() => task.IsCompleted);
			result = task.Result;
		}
		else
		{
			result = null;
		}

		if (result.HasValue)
		{
			playerRank = result.Value.NewGlobalRank;
			Debug.Log($"Score Changed? {result.Value.Changed}");
			if (result.Value.Changed)
			{
				Debug.Log($"Global Rank: {result.Value.OldGlobalRank} → {result.Value.NewGlobalRank}");
			}

			status = Status.Success;
		}
		else
		{
			Debug.LogWarning("Submitting score failed!");
			status = Status.Failed;
		}
	}

	public static int GetScore(bool win, int seconds, int gold, int battlesWon)
	{
		int scoreFromTime = GetScoreFromTime(win, seconds);
		int scoreFromGold = GetScoreFromGold(gold);
		int scoreFromBattlesWon = GetScoreFromBattlesWon(battlesWon);
		return scoreFromTime + scoreFromGold + scoreFromBattlesWon;
	}

	public static int GetScoreFromTime(bool win, int seconds)
	{
		if (!win)
		{
			return 0;
		}

		return Mathf.Max(0, 3600 - seconds);
	}

	public static int GetScoreFromGold(int goldRemaining)
	{
		return goldRemaining;
	}

	public static int GetScoreFromBattlesWon(int battlesWon)
	{
		return -100 + battlesWon * 100;
	}
}
