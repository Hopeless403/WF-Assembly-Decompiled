#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;

public static class DailyFetcher
{
	public static int DayOffset;

	public static DateTime dateTime;

	public static bool fetched { get; set; }

	public static IEnumerator FetchDateTime()
	{
		dateTime = DateTime.Now.AddDays(DayOffset);
		fetched = true;
		yield break;
	}

	public static void CancelFetch()
	{
		fetched = false;
	}

	public static void SetContinueDateTime()
	{
		dateTime = DateTime.ParseExact(SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "startDate", ""), "dd/MM/yyyy", GameManager.CultureInfo);
	}

	public static DateTime GetDateTime()
	{
		return dateTime;
	}

	public static DateTime GetNextDateTime()
	{
		return dateTime.Date.AddDays(1.0);
	}

	public static int GetSeed()
	{
		DateTime dateTime = GetDateTime();
		int.TryParse($"{dateTime:yyMMdd}{dateTime.DayOfYear:D3}", out var result);
		return result;
	}

	public static string GetDate()
	{
		return GetDateTime().ToString("dd/MM/yyyy");
	}

	public static string GetLeaderboardName()
	{
		return GetLeaderboardName(GetDateTime());
	}

	public static string GetLeaderboardName(DateTime dateTime)
	{
		return GetLeaderboardName(dateTime.ToString("dd/MM/yyyy"));
	}

	public static string GetLeaderboardName(string dateString)
	{
		return "Daily-" + dateString;
	}

	public static bool CanPlay()
	{
		string text = SaveSystem.LoadProgressData<string>("dailyPlayed", null);
		if (!text.IsNullOrWhitespace() && DateTime.TryParse(text, out var result))
		{
			return !SameDay(GetDateTime(), result);
		}

		return true;
	}

	public static bool SameDay(DateTime a, DateTime b)
	{
		if (a.Year == b.Year)
		{
			return a.DayOfYear == b.DayOfYear;
		}

		return false;
	}
}
