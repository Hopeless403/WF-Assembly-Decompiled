#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverallStatsSystem : GameSystem
{
	public static OverallStatsSystem instance;

	[SerializeField]
	public CampaignStats stats;

	public void Awake()
	{
		instance = this;
	}

	public void OnEnable()
	{
		Events.OnCampaignEnd += CampaignEnd;
		Events.OnGameStart += GameStart;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
		Events.OnGameStart -= GameStart;
	}

	public static CampaignStats Get()
	{
		return instance.stats;
	}

	public void GameStart()
	{
		if (SaveSystem.Enabled && SaveSystem.StatsExists() && SaveSystem.StatsDataExists("stats"))
		{
			stats = SaveSystem.LoadStatsData<CampaignStats>("stats");
		}
		else
		{
			stats = new CampaignStats();
		}
	}

	public void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		Combine(this.stats, stats);
		bool mainGameMode = Campaign.Data.GameMode.mainGameMode;
		bool dailyRun = Campaign.Data.GameMode.dailyRun;
		switch (result)
		{
			case Campaign.Result.Win:
			if (SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false))
			{
				this.stats.Add("results", "trueWin", 1);
				if (mainGameMode)
				{
					this.stats.Add("currentTrueWinStreak", 1);
					this.stats.Max("bestTrueWinStreak", this.stats.Get("currentTrueWinStreak", 1));
				}
				}
			else
			{
				this.stats.Add("results", "win", 1);
				}
	
			if (mainGameMode)
			{
				this.stats.Add("currentWinStreak", 1);
				this.stats.Max("bestWinStreak", this.stats.Get("currentWinStreak", 1));
				this.stats.Min("bestWinTime", stats.hours * 3600 + Mathf.FloorToInt(stats.time));
				}
	
			if (dailyRun)
			{
				this.stats.Add("dailyRunResults", "win", 1);
				}
	
				this.stats.Add("winsWithTribe", playerData.classData.id, 1);
			foreach (string item in playerData.inventory.deck.Select((CardData a) => a.name).ToHashSet())
			{
				this.stats.Add("winsWithCardInDeck", item, 1);
				}
	
				break;
			case Campaign.Result.Lose:
				this.stats.Add("results", "lose", 1);
			if (mainGameMode)
			{
				ResetWinStreak();
				}
	
			if (dailyRun)
			{
				this.stats.Add("dailyRunResults", "lose", 1);
				}
	
				this.stats.Add("loseWithTribe", playerData.classData.id, 1);
				break;
			case Campaign.Result.Restart:
				this.stats.Add("results", "restart", 1);
			if (mainGameMode)
			{
				ResetWinStreak();
				}
	
			if (dailyRun)
			{
				this.stats.Add("dailyRunResults", "restart", 1);
				}
	
				this.stats.Add("loseWithTribe", playerData.classData.id, 1);
				break;
		}

		Save();
	}

	public void ResetWinStreak()
	{
		stats.Set("currentWinStreak", 0);
		stats.Set("currentTrueWinStreak", 0);
	}

	public void Save()
	{
		SaveSystem.SaveStatsData("stats", stats);
		Events.InvokeOverallStatsSaved(stats);
	}

	public static void Combine(CampaignStats stats, CampaignStats other)
	{
		stats.time += other.time;
		stats.hours += other.hours;
		if (other.add != null)
		{
			foreach (KeyValuePair<string, Dictionary<string, int>> item in other.add)
			{
				foreach (KeyValuePair<string, int> item2 in item.Value)
				{
					Change(item.Key, item2.Key, item2.Value, ref stats.add, Add);
				}
			}
		}

		if (other.max == null)
		{
			return;
		}

		foreach (KeyValuePair<string, Dictionary<string, int>> item3 in other.max)
		{
			foreach (KeyValuePair<string, int> item4 in item3.Value)
			{
				Change(item3.Key, item4.Key, item4.Value, ref stats.max, Max);
			}
		}
	}

	public static void Change(string stat, string key, int value, ref Dictionary<string, Dictionary<string, int>> values, Func<int, int, int> action)
	{
		int num = 0;
		if (values == null)
		{
			values = new Dictionary<string, Dictionary<string, int>>();
		}

		Dictionary<string, int> dictionary;
		if (!values.ContainsKey(stat))
		{
			dictionary = new Dictionary<string, int>();
			values[stat] = dictionary;
		}
		else
		{
			Dictionary<string, int> dictionary2 = values[stat];
			if (dictionary2 != null)
			{
				dictionary = dictionary2;
			}
			else
			{
				dictionary = new Dictionary<string, int>();
				values[stat] = dictionary;
			}
		}

		if (dictionary.ContainsKey(key))
		{
			int arg = dictionary[key];
			num = action(arg, value);
		}
		else
		{
			num = value;
		}

		dictionary[key] = num;
	}

	public static int Add(int value, int add)
	{
		return value + add;
	}

	public static int Max(int value, int max)
	{
		return Mathf.Max(value, max);
	}
}
