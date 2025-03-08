#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;

public class ChallengeSystem : GameSystem
{
	public List<ChallengeData> activeChallenges;

	public List<ChallengeData> saveRequired;

	public void OnEnable()
	{
		List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>();
		activeChallenges = new List<ChallengeData>();
		foreach (ChallengeData allChallenge in GetAllChallenges())
		{
			if (list.Contains(allChallenge.name))
			{
				continue;
			}

			bool flag = true;
			ChallengeData[] requires = allChallenge.requires;
			foreach (ChallengeData challengeData in requires)
			{
				if (!list.Contains(challengeData.name))
				{
					flag = false;
					break;
				}
			}

			if (flag)
			{
				activeChallenges.Add(allChallenge);
			}
		}

		foreach (ChallengeData item in activeChallenges.Where((ChallengeData a) => a.listener.checkType == ChallengeListener.CheckType.CustomSystem))
		{
			item.listener.AddCustomSystem(item, this);
		}

		Events.OnStatChanged += StatChanged;
		Events.OnOverallStatsSaved += OverallStatsChanged;
		Events.OnCampaignSaved += CheckSave;
	}

	public void OnDisable()
	{
		Events.OnStatChanged -= StatChanged;
		Events.OnOverallStatsSaved -= OverallStatsChanged;
		Events.OnCampaignSaved -= CheckSave;
	}

	public void StatChanged(string stat, string key, int oldValue, int newValue)
	{
		for (int num = activeChallenges.Count - 1; num >= 0; num--)
		{
			ChallengeData challengeData = activeChallenges[num];
			ChallengeListener listener = challengeData.listener;
			if (listener.checkType == ChallengeListener.CheckType.MidRun && listener.Check(stat, key))
			{
				listener.Set(challengeData.name, oldValue, newValue);
				if (ChallengeProgressSystem.GetProgress(challengeData.name) >= challengeData.goal)
				{
					activeChallenges.RemoveAt(num);
					saveRequired.Add(challengeData);
				}
			}
		}
	}

	public void OverallStatsChanged(CampaignStats stats)
	{
		bool flag = false;
		for (int num = activeChallenges.Count - 1; num >= 0; num--)
		{
			ChallengeData challengeData = activeChallenges[num];
			ChallengeListener listener = challengeData.listener;
			if (listener.checkType == ChallengeListener.CheckType.EndOfRun && listener.CheckComplete(stats))
			{
				ChallengeProgressSystem.AddProgress(challengeData.name, 1);
				if (ChallengeProgressSystem.GetProgress(challengeData.name) >= challengeData.goal)
				{
					activeChallenges.RemoveAt(num);
					saveRequired.Add(challengeData);
					flag = true;
				}
			}
		}

		if (flag)
		{
			CheckSave();
		}
	}

	public void SetAsComplete(ChallengeData challengeData)
	{
		activeChallenges.Remove(challengeData);
		saveRequired.Add(challengeData);
		CheckSave();
	}

	public void CheckSave()
	{
		if (saveRequired.Count <= 0)
		{
			return;
		}

		List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>();
		List<string> list2 = SaveSystem.LoadProgressData<List<string>>("townNew", null) ?? new List<string>();
		List<string> list3 = SaveSystem.LoadProgressData<List<string>>("unlocked", null) ?? new List<string>();
		foreach (ChallengeData item in saveRequired)
		{
			list.Add(item.name);
			list2.Add(item.reward.name);
			list3.Add(item.reward.name);
			Events.InvokeChallengeCompletedSaved(item);
		}

		SaveSystem.SaveProgressData("completedChallenges", list);
		SaveSystem.SaveProgressData("townNew", list2);
		SaveSystem.SaveProgressData("unlocked", list3);
		saveRequired.Clear();
	}

	public static IEnumerable<ChallengeData> GetAllChallenges()
	{
		return from a in AddressableLoader.GetGroup<ChallengeData>("ChallengeData")
			where a.reward.IsActive
			select a;
	}
}
