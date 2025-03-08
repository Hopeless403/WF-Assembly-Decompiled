#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Threading.Tasks;
using Steamworks.Data;
using UnityEngine;

public class LeaderboardsFetcher : MonoBehaviour
{
	public enum Type
	{
		Global,
		Friends,
		Around
	}

	public enum Result
	{
		Success,
		NoConnection,
		TooManyRequests,
		Cancel
	}

	[SerializeField]
	public Scores.Type boardType = Scores.Type.Daily;

	public bool playerRankFetched;

	public uint fetchCount;

	public Leaderboard? leaderboard;

	public int fetchBoardTaskId;

	public int fetchScoresTaskId;

	public int fetchPlayerRankTaskId;

	public int dayOffset = -1;

	public Result result { get; set; }

	public bool playerHasRank { get; set; }

	public uint playerRank { get; set; }

	public uint totalScores { get; set; }

	public LeaderboardEntry[] info { get; set; }

	public void ResetPlayerRank()
	{
		playerRankFetched = false;
		playerHasRank = false;
		playerRank = 0u;
	}

	public async Task Fetch(Type type, int dayOffset, int entriesPerPage, int page = -1)
	{
		DateTime date = DailyFetcher.GetDateTime().AddDays(-dayOffset);
		if (this.dayOffset != dayOffset || !leaderboard.HasValue)
		{
			this.dayOffset = dayOffset;
			Task<Leaderboard?> fetchBoardTask = Scores.GetLeaderboard(boardType, date);
			fetchBoardTaskId = fetchBoardTask.Id;
			await fetchBoardTask;
			if (fetchBoardTaskId == fetchBoardTask.Id)
			{
				leaderboard = fetchBoardTask.Result;
			}
		}

		if (!leaderboard.HasValue)
		{
			return;
		}

		totalScores = (uint)leaderboard.Value.EntryCount;
		if (!playerRankFetched)
		{
			await FetchPlayerRankIfNecessary(leaderboard.Value);
		}

		if (page < 0)
		{
			page = (playerHasRank ? Mathf.FloorToInt((float)(playerRank - 1) / (float)entriesPerPage) : 0);
		}

		switch (type)
		{
			case Type.Global:
			{
				Task<LeaderboardEntry[]> fetchGlobalScoresTask = Scores.GetGlobal(leaderboard.Value, entriesPerPage, 1 + page * entriesPerPage);
				fetchScoresTaskId = fetchGlobalScoresTask.Id;
				await fetchGlobalScoresTask;
			if (fetchScoresTaskId == fetchGlobalScoresTask.Id)
			{
				info = fetchGlobalScoresTask.Result;
				}
	
				break;
			}
			case Type.Friends:
			{
				Task<LeaderboardEntry[]> fetchFriendsScoresTask = Scores.GetFriends(leaderboard.Value, entriesPerPage);
				fetchScoresTaskId = fetchFriendsScoresTask.Id;
				await fetchFriendsScoresTask;
			if (fetchScoresTaskId == fetchFriendsScoresTask.Id)
			{
				info = fetchFriendsScoresTask.Result;
				}
	
				break;
			}
		}
	}

	public async Task FetchPlayerRankIfNecessary(Leaderboard board)
	{
		Task<LeaderboardEntry[]> fetchPlayerRankTask = Scores.GetAround(board, 0);
		fetchPlayerRankTaskId = fetchPlayerRankTask.Id;
		await fetchPlayerRankTask;
		if (fetchPlayerRankTask.Id == fetchPlayerRankTaskId)
		{
			LeaderboardEntry[] array = fetchPlayerRankTask.Result;
			if (array != null && array.Length > 0)
			{
				playerHasRank = true;
				playerRank = (uint)array[0].GlobalRank;
			}

			playerRankFetched = true;
		}
	}
}
