#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;

public class ChallengeProgressSystem : GameSystem
{
	public static ChallengeProgressSystem instance;

	public List<ChallengeProgress> progress;

	public bool saveRequired;

	public void Awake()
	{
		instance = this;
	}

	public void OnEnable()
	{
		Events.OnCampaignSaved += CheckSave;
		Events.OnCampaignLoaded += Load;
		instance.progress = LoadProgress();
	}

	public void OnDisable()
	{
		Events.OnCampaignSaved -= CheckSave;
		Events.OnCampaignLoaded -= Load;
	}

	public static int GetProgress(string challengeName)
	{
		return (instance.progress?.FirstOrDefault((ChallengeProgress a) => a.challengeName == challengeName))?.currentValue ?? 0;
	}

	public static void AddProgress(string challengeName, int add)
	{
		ChallengeProgress challengeProgress = instance.progress.FirstOrDefault((ChallengeProgress a) => a.challengeName == challengeName);
		if (challengeProgress == null)
		{
			ChallengeProgress challengeProgress2 = SaveSystem.LoadProgressData<List<ChallengeProgress>>("challengeProgress", null)?.FirstOrDefault((ChallengeProgress a) => a.challengeName == challengeName);
			challengeProgress = ((challengeProgress2 == null) ? new ChallengeProgress(challengeName, 0) : new ChallengeProgress(challengeName, challengeProgress2.currentValue));
			instance.progress.Add(challengeProgress);
		}

		instance.saveRequired = true;
		challengeProgress.currentValue += add;
	}

	public static List<ChallengeProgress> LoadProgress()
	{
		List<ChallengeProgress> list = SaveSystem.LoadProgressData("challengeProgress", new List<ChallengeProgress>());
		foreach (ChallengeProgress item in list)
		{
			item.originalValue = item.currentValue;
		}

		return list;
	}

	public void CheckSave()
	{
		if (saveRequired)
		{
			if (Campaign.Data.GameMode.doSave)
			{
				SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "challengeProgress", progress);
			}

			SaveSystem.SaveProgressData("challengeProgress", progress);
			saveRequired = false;
		}
	}

	public void Load()
	{
		progress = LoadProgress();
		List<ChallengeProgress> list = SaveSystem.LoadCampaignData<List<ChallengeProgress>>(Campaign.Data.GameMode, "challengeProgress", null);
		if (list == null)
		{
			return;
		}

		foreach (ChallengeProgress inCampaignData in list)
		{
			ChallengeProgress challengeProgress = progress.FirstOrDefault((ChallengeProgress a) => a.challengeName == inCampaignData.challengeName);
			if (challengeProgress != null && inCampaignData.currentValue >= challengeProgress.currentValue)
			{
				challengeProgress.originalValue = inCampaignData.originalValue;
			}
		}
	}
}
