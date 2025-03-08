#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class CheckAchievements : MonoBehaviour
{
	public static readonly Dictionary<string, string> AchievementChallenge = new Dictionary<string, string>
	{
		{ "ACHIEVEMENT_0", "Challenge Charm 1" },
		{ "ACHIEVEMENT_1", "Challenge Charm 2" },
		{ "ACHIEVEMENT_2", "Challenge Charm 3" },
		{ "ACHIEVEMENT_3", "Challenge Charm 4" },
		{ "ACHIEVEMENT_4", "Challenge Charm 5" },
		{ "ACHIEVEMENT_5", "Challenge Charm 6" },
		{ "ACHIEVEMENT_6", "Challenge Charm 7" },
		{ "ACHIEVEMENT_7", "Challenge Charm 8" },
		{ "ACHIEVEMENT_8", "Challenge Charm 9" },
		{ "ACHIEVEMENT_9", "Challenge Charm 10" },
		{ "ACHIEVEMENT_10", "Challenge Charm 11" },
		{ "ACHIEVEMENT_11", "Challenge Charm 12" },
		{ "ACHIEVEMENT_12", "Challenge Charm 13" },
		{ "ACHIEVEMENT_13", "Challenge Charm 14" },
		{ "ACHIEVEMENT_14", "Challenge Charm 15" },
		{ "ACHIEVEMENT_15", "Challenge Charm 16" },
		{ "ACHIEVEMENT_16", "Challenge Charm 17" },
		{ "ACHIEVEMENT_17", "Challenge Charm 18" },
		{ "ACHIEVEMENT_18", "Challenge Charm 19" },
		{ "ACHIEVEMENT_19", "Challenge Charm 20" },
		{ "ACHIEVEMENT_20", "Challenge Charm 21" },
		{ "ACHIEVEMENT_21", "Challenge Charm 22" },
		{ "ACHIEVEMENT_22", "Challenge Charm 23" },
		{ "ACHIEVEMENT_23", "Challenge Charm 24" },
		{ "ACHIEVEMENT_24", "Challenge Charm 25" },
		{ "ACHIEVEMENT_25", "Challenge Charm 26" },
		{ "ACHIEVEMENT_26", "Challenge Charm 27" }
	};

	public void Start()
	{
		if (!SteamManager.init)
		{
			return;
		}

		List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges");
		Debug.Log("Steam Achievements:");
		foreach (Achievement achievement in SteamUserStats.Achievements)
		{
			Debug.Log($"→ {achievement.Name} ({achievement.State})");
			if (!achievement.State)
			{
				string text = AchievementChallenge[achievement.Identifier];
				Debug.Log("-→ Requires [" + text + "]");
				if (list != null && list.Contains(text))
				{
					AchievementsSystem.Achieve(achievement.Identifier);
				}
			}
		}
	}
}
