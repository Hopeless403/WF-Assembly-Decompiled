#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public static class NexFrequencyChecker
{
	public enum Function
	{
		PutScore,
		GetCommonData,
		PutCommonData,
		DeleteCommonData,
		GetRanking,
		GetCategorySetting,
		GetRankingChart,
		GetEstimatedScoreRank
	}

	public readonly struct Profile
	{
		public readonly int allowedRequests;

		public readonly float cooldownSeconds;

		public readonly List<float> requests;

		public Profile(int allowedRequests, float cooldownSeconds)
		{
			this.allowedRequests = allowedRequests;
			this.cooldownSeconds = cooldownSeconds;
			requests = new List<float>();
		}

		public bool Check()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RemoveInactive(realtimeSinceStartup);
			return requests.Count < allowedRequests;
		}

		public void RemoveInactive(float time)
		{
			float num = time - cooldownSeconds;
			for (int num2 = requests.Count - 1; num2 >= 0; num2--)
			{
				if (requests[num2] < num)
				{
					requests.RemoveAt(num2);
				}
			}
		}

		public void LogRequest()
		{
			requests.Add(Time.realtimeSinceStartup);
		}
	}

	public static readonly Dictionary<Function, Profile> lookup = new Dictionary<Function, Profile>
	{
		{
			Function.PutScore,
			new Profile(10, 60f)
		},
		{
			Function.GetCommonData,
			new Profile(10, 60f)
		},
		{
			Function.PutCommonData,
			new Profile(10, 60f)
		},
		{
			Function.DeleteCommonData,
			new Profile(10, 60f)
		},
		{
			Function.GetRanking,
			new Profile(20, 60f)
		},
		{
			Function.GetCategorySetting,
			new Profile(20, 60f)
		},
		{
			Function.GetRankingChart,
			new Profile(20, 60f)
		},
		{
			Function.GetEstimatedScoreRank,
			new Profile(20, 60f)
		}
	};

	public static bool Check(Function functionName)
	{
		if (lookup.TryGetValue(functionName, out var value) && value.Check())
		{
			value.LogRequest();
			return true;
		}

		return false;
	}
}
