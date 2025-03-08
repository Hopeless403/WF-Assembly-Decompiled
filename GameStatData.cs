#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Game Stat", fileName = "Stat")]
public class GameStatData : ScriptableObject
{
	public enum Type
	{
		Normal,
		Best,
		BestAny,
		Count,
		Time,
		RandomKey,
		Rate,
		BestTime
	}

	public UnityEngine.Localization.LocalizedString stringKey;

	[SerializeField]
	public Type type;

	[SerializeField]
	[ShowIf("NeedsStatName")]
	public string statName;

	[SerializeField]
	[ShowIf("NeedsKey")]
	public string statKey;

	[SerializeField]
	[ShowIf("NeedsRate")]
	public string statKeyOver;

	[SerializeField]
	[ShowIf("NeedsDefault")]
	public int defaultValue;

	public float displayOrder;

	public float priority;

	public float par = 1f;

	public float priorityAddOverPar;

	public float prioritySubUnderPar;

	public GameStatData[] overwrites;

	public bool NeedsStatName => type != Type.Time;

	public bool NeedsKey
	{
		get
		{
			Type type = this.type;
			return type == Type.Normal || type == Type.Best || type == Type.Rate;
		}
	}

	public bool NeedsDefault
	{
		get
		{
			if (type != Type.Count && type != Type.Time)
			{
				return type != Type.RandomKey;
			}

			return false;
		}
	}

	public bool NeedsRate => type == Type.Rate;

	public float GetPriority(float value)
	{
		float num = value - par;
		float num2 = priority;
		if (num > 0f)
		{
			return num2 + priorityAddOverPar * num;
		}

		return num2 + prioritySubUnderPar * num;
	}

	public float GetValue(CampaignStats stats)
	{
		switch (type)
		{
			case Type.Best:
				return GetBestValue(stats, statKey);
			case Type.BestAny:
				return stats.Best(statName, defaultValue);
			case Type.Count:
				return stats.Count(statName);
			case Type.Time:
				return Mathf.RoundToInt(stats.time + (float)(stats.hours * 3600));
			case Type.RandomKey:
				return 1f;
			case Type.Rate:
				return GetRateValue(stats);
			case Type.BestTime:
				return GetBestValue(stats, statKey);
			default:
				return GetNormalValue(stats, statKey);
		}
	}

	public int GetBestValue(CampaignStats stats, string statKey)
	{
		if (HasMultipleStatKeys(statKey))
		{
			IEnumerable<string> statKeys = GetStatKeys(statKey);
			List<int> list = new List<int>();
			foreach (string item in statKeys)
			{
				list.Add(stats.Best(statName, item, defaultValue));
			}

			return list.Max();
		}

		return stats.Best(statName, statKey, defaultValue);
	}

	public int GetNormalValue(CampaignStats stats, string statKey)
	{
		if (HasMultipleStatKeys(statKey))
		{
			IEnumerable<string> statKeys = GetStatKeys(statKey);
			int num = 0;
			{
				foreach (string item in statKeys)
				{
					num += stats.Get(statName, item, defaultValue);
				}

				return num;
			}
		}

		return stats.Get(statName, statKey, defaultValue);
	}

	public float GetRateValue(CampaignStats stats)
	{
		float num = GetNormalValue(stats, statKey);
		float num2 = (float)GetNormalValue(stats, statKeyOver) + num;
		if (!(num2 > 0f))
		{
			return (num > 0f) ? 1 : 0;
		}

		return num / num2;
	}

	public static bool HasMultipleStatKeys(string @in)
	{
		return @in.Contains('|');
	}

	public static IEnumerable<string> GetStatKeys(string @in)
	{
		return @in.Split('|');
	}

	public string GetStringValue(CampaignStats stats, float value)
	{
		if (value <= 0f)
		{
			return "-";
		}

		switch (type)
		{
			case Type.RandomKey:
				return GetRandomStringValue(stats);
			case Type.Time:
				return FromSeconds(value);
			case Type.BestTime:
				return FromSeconds(value);
			case Type.Rate:
				return (value * 100f).ToString("0") + "%";
			default:
				return Mathf.RoundToInt(value).ToString("N0");
		}
	}

	public string GetRandomStringValue(CampaignStats stats)
	{
		Dictionary<string, int> dictionary = stats.Get(statName);
		if (dictionary != null)
		{
			string result = "-";
			int num = 0;
			{
				foreach (KeyValuePair<string, int> item in dictionary)
				{
					if (item.Value >= num)
					{
						num = item.Value;
						result = item.Key;
					}
				}

				return result;
			}
		}

		return "-";
	}

	public static string FromSeconds(float seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		return $"{(int)timeSpan.TotalHours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
	}
}
