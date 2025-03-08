#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public static class Scores
{
	public enum Type
	{
		TopScores,
		Daily
	}

	public static string GetLeaderboardName(Type boardType, DateTime date)
	{
		switch (boardType)
		{
			case Type.TopScores:
				return "Scores";
			case Type.Daily:
				return DailyFetcher.GetLeaderboardName(date);
			default:
				return "Unknown";
		}
	}

	public static async Task<LeaderboardUpdate?> Submit(Type type, DateTime date, int score, params int[] details)
	{
		string leaderboardName = GetLeaderboardName(type, date);
		Debug.Log($"> Submitting Score [{score}] to Leaderboard [{leaderboardName}]");
		Leaderboard? leaderboard = await GetLeaderboard(leaderboardName);
		if (leaderboard.HasValue)
		{
			return await leaderboard.Value.SubmitScoreAsync(score, (details.Length != 0) ? details : null);
		}

		return null;
	}

	public static async Task<Leaderboard?> GetLeaderboard(Type type, DateTime date)
	{
		if (!SteamManager.init)
		{
			return null;
		}

		return await GetLeaderboard(GetLeaderboardName(type, date));
	}

	public static async Task<Leaderboard?> GetLeaderboard(string leaderboardName)
	{
		if (!SteamManager.init)
		{
			return null;
		}

		return await SteamUserStats.FindOrCreateLeaderboardAsync(leaderboardName, LeaderboardSort.Descending, LeaderboardDisplay.Numeric);
	}

	public static async Task<LeaderboardEntry[]> GetGlobal(Leaderboard board, int entries, int offset)
	{
		if (!SteamManager.init)
		{
			return null;
		}

		return await board.GetScoresAsync(entries, offset);
	}

	public static async Task<LeaderboardEntry[]> GetFriends(Leaderboard board, int entries)
	{
		if (!SteamManager.init)
		{
			return null;
		}

		return await board.GetScoresFromFriendsAsync();
	}

	public static async Task<LeaderboardEntry[]> GetAround(Leaderboard board, int entries)
	{
		if (!SteamManager.init)
		{
			return null;
		}

		int num = entries / 2;
		return await board.GetScoresAroundUserAsync(-num, num);
	}
}
