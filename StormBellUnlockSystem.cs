#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class StormBellUnlockSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnCampaignEnd += CampaignEnd;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
	}

	public void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		if (result != Campaign.Result.Win || !Campaign.Data.GameMode.mainGameMode)
		{
			return;
		}

		if (!SaveSystem.LoadProgressData("stormBellsUnlocked", defaultValue: false))
		{
			SaveSystem.SaveProgressData("stormBellsUnlocked", value: true);
		}

		List<string> list = SaveSystem.LoadProgressData<List<string>>("activeHardModeModifiers");
		if (list == null)
		{
			list = new List<string>();
		}

		int currentStormPoints = StormBellManager.GetCurrentStormPoints(list);
		int num = SaveSystem.LoadProgressData("maxStormPoints", 5);
		if (currentStormPoints == num && num + 1 <= 10)
		{
			SaveSystem.SaveProgressData("maxStormPoints", num + 1);
			SaveSystem.SaveProgressData("maxStormPointIncrease", 1);
		}

		if (!SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false))
		{
			return;
		}

		List<string> list2 = SaveSystem.LoadProgressData<List<string>>("goldHardModeModifiers") ?? new List<string>();
		List<string> list3 = SaveSystem.LoadProgressData<List<string>>("goldHardModeModifiersNew") ?? new List<string>();
		bool flag = false;
		foreach (string item in list)
		{
			if (!list2.Contains(item))
			{
				list2.Add(item);
				list3.Add(item);
				flag = true;
			}
		}

		if (flag)
		{
			SaveSystem.SaveProgressData("goldHardModeModifiers", list2);
			SaveSystem.SaveProgressData("goldHardModeModifiersNew", list3);
		}
	}
}
