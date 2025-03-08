#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CampaignStats
{
	public float time;

	public int hours;

	public Dictionary<string, Dictionary<string, int>> add;

	public Dictionary<string, Dictionary<string, int>> max;

	public void Add(string stat, int value)
	{
		Change(stat, value, ref add, Add);
	}

	public void Add(string stat, string key, int value)
	{
		Change(stat, key, value, ref add, Add);
	}

	public void Max(string stat, int value)
	{
		Change(stat, value, ref max, Max);
	}

	public void Max(string stat, string key, int value)
	{
		Change(stat, key, value, ref max, Max);
	}

	public void Min(string stat, int value)
	{
		Change(stat, value, ref max, Min);
	}

	public void Min(string stat, string key, int value)
	{
		Change(stat, key, value, ref max, Min);
	}

	public void Change(string stat, int value, ref Dictionary<string, Dictionary<string, int>> values, Func<int, int, int> action)
	{
		Change(stat, "", value, ref values, action);
	}

	public void Change(string stat, string key, int value, ref Dictionary<string, Dictionary<string, int>> values, Func<int, int, int> action)
	{
		int num = 0;
		int num2 = 0;
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
			num = dictionary[key];
			num2 = action(num, value);
		}
		else
		{
			num2 = value;
		}

		dictionary[key] = num2;
		if (num != num2)
		{
			Events.InvokeStatChanged(stat, key, num, num2);
		}
	}

	public int Add(int value, int add)
	{
		return value + add;
	}

	public int Max(int value, int max)
	{
		return Mathf.Max(value, max);
	}

	public int Min(int value, int min)
	{
		return Mathf.Min(value, min);
	}

	public Dictionary<string, int> Get(string stat, Dictionary<string, Dictionary<string, int>> source)
	{
		if (source != null && source.ContainsKey(stat))
		{
			Dictionary<string, int> dictionary = source[stat];
			if (dictionary != null)
			{
				return dictionary;
			}
		}

		return null;
	}

	public Dictionary<string, int> Get(string stat)
	{
		return Get(stat, add);
	}

	public int Get(string stat, int defaultValue)
	{
		Dictionary<string, int> dictionary = Get(stat, add);
		if (dictionary == null || !dictionary.ContainsKey(""))
		{
			return defaultValue;
		}

		return dictionary[""];
	}

	public int Get(string stat, string key, int defaultValue)
	{
		Dictionary<string, int> dictionary = Get(stat, add);
		if (dictionary == null || !dictionary.ContainsKey(key))
		{
			return defaultValue;
		}

		return dictionary[key];
	}

	public int Best(string stat, int defaultValue)
	{
		return Get(stat, max)?.Values.Prepend(0).Max() ?? defaultValue;
	}

	public int Best(string stat, string key, int defaultValue)
	{
		Dictionary<string, int> dictionary = Get(stat, max);
		if (dictionary == null || !dictionary.ContainsKey(key))
		{
			return defaultValue;
		}

		return dictionary[key];
	}

	public int Count(string stat)
	{
		return Get(stat, add)?.Values.Sum() ?? 0;
	}

	public void Set(string stat, int value)
	{
		Set(add, stat, "", value);
	}

	public void Set(string stat, string key, int value)
	{
		Set(add, stat, key, value);
	}

	public void SetBest(string stat, int value)
	{
		Set(max, stat, "", value);
	}

	public void SetBest(string stat, string key, int value)
	{
		Set(max, stat, key, value);
	}

	public void Set(Dictionary<string, Dictionary<string, int>> dict, string stat, string key, int value)
	{
		if (!dict.ContainsKey(stat))
		{
			dict[stat] = new Dictionary<string, int> { { key, value } };
		}
		else
		{
			dict[stat][key] = value;
		}
	}

	public void Delete(string stat)
	{
		add.Remove(stat);
	}

	public void DeleteBest(string stat)
	{
		max.Remove(stat);
	}

	public CampaignStats Clone()
	{
		CampaignStats campaignStats = new CampaignStats
		{
			time = time,
			hours = hours
		};
		if (add != null)
		{
			campaignStats.add = add.ToDictionary((KeyValuePair<string, Dictionary<string, int>> a) => a.Key, (KeyValuePair<string, Dictionary<string, int>> a) => a.Value.ToDictionary((KeyValuePair<string, int> b) => b.Key, (KeyValuePair<string, int> b) => b.Value));
		}

		if (max != null)
		{
			campaignStats.max = max.ToDictionary((KeyValuePair<string, Dictionary<string, int>> a) => a.Key, (KeyValuePair<string, Dictionary<string, int>> a) => a.Value.ToDictionary((KeyValuePair<string, int> b) => b.Key, (KeyValuePair<string, int> b) => b.Value));
		}

		return campaignStats;
	}
}
