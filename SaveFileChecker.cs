#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveFileChecker : MonoBehaviour
{
	public static bool saveRequired;

	public void Start()
	{
		Debug.Log("~SAVE FILE CHECKER~");
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		GainUnlocksFromCompletedChallenges(unlockedList);
		CheckChallengeUnlocks(unlockedList);
		CheckUnlockRequirements(unlockedList);
		saveRequired = unlockedList.RemoveDuplicates() || saveRequired;
		if (saveRequired)
		{
			SaveSystem.SaveProgressData("unlocked", unlockedList);
		}
	}

	public static void GainUnlocksFromCompletedChallenges(List<string> unlocksList)
	{
		IEnumerable<ChallengeData> allChallenges = ChallengeSystem.GetAllChallenges();
		List<string> list = SaveSystem.LoadProgressData("completedChallenges", new List<string>());
		HashSet<string> hashSet = new HashSet<string>();
		foreach (string challengeName in list)
		{
			ChallengeData challengeData = allChallenges.FirstOrDefault((ChallengeData a) => a.name == challengeName);
			if (challengeData != null)
			{
				UnlockData reward = challengeData.reward;
				if ((object)reward != null)
				{
					hashSet.AddRange(GetUnlockAndRequirements(reward));
				}
			}
		}

		if (hashSet.Count <= 0)
		{
			return;
		}

		foreach (string item in hashSet.Where((string add) => !unlocksList.Contains(add)))
		{
			unlocksList.Add(item);
		}
	}

	public static HashSet<string> GetUnlockAndRequirements(UnlockData unlockData)
	{
		HashSet<string> hashSet = new HashSet<string> { unlockData.name };
		UnlockData[] requires = unlockData.requires;
		foreach (UnlockData unlockData2 in requires)
		{
			hashSet.AddRange(GetUnlockAndRequirements(unlockData2));
		}

		saveRequired = true;
		return hashSet;
	}

	public static void CheckChallengeUnlocks(List<string> unlocksList)
	{
		List<string> completedChallenges = SaveSystem.LoadProgressData("completedChallenges", new List<string>());
		ChallengeData[] array = (from a in ChallengeSystem.GetAllChallenges()
			where !completedChallenges.Contains(a.name)
			select a).ToArray();
		List<string> removed = new List<string>();
		ChallengeData[] array2 = array;
		foreach (ChallengeData challengeData in array2)
		{
			if (unlocksList.Remove(challengeData.reward.name))
			{
				removed.Add(challengeData.reward.name);
				Debug.Log("[" + challengeData.reward.name + "] removed from unlocks since [" + challengeData.name + "] is not completed");
			}
		}

		if (removed.Count > 0)
		{
			saveRequired = true;
			List<string> source = SaveSystem.LoadProgressData("inventorHutUnlocks", new List<string>());
			SaveSystem.SaveProgressData("inventorHutUnlocks", source.Where((string a) => !removed.Contains(a)).ToList());
			List<string> source2 = SaveSystem.LoadProgressData("petHutUnlocks", new List<string>());
			SaveSystem.SaveProgressData("petHutUnlocks", source2.Where((string a) => !removed.Contains(a)).ToList());
		}
	}

	public static void CheckUnlockRequirements(List<string> unlocksList)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int num = unlocksList.Count - 1; num >= 0; num--)
			{
				string text = unlocksList[num];
				UnlockData unlockData = AddressableLoader.Get<UnlockData>("UnlockData", text);
				if (unlockData == null)
				{
					Debug.Log("[" + text + "] no longer exists. removing from save data");
					unlocksList.RemoveAt(num);
					flag = true;
					saveRequired = true;
				}
				else if (unlockData.requires != null && unlockData.requires.Length != 0)
				{
					UnlockData[] requires = unlockData.requires;
					foreach (UnlockData unlockData2 in requires)
					{
						if (!unlocksList.Contains(unlockData2.name))
						{
							Debug.Log("[" + text + "] requirements are not met (requires [" + unlockData2.name + "]). removing from save data");
							unlocksList.RemoveAt(num);
							flag = true;
							saveRequired = true;
							break;
						}
					}
				}
			}
		}
	}
}
