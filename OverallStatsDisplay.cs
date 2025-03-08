#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class OverallStatsDisplay : MonoBehaviour
{
	[SerializeField]
	public GameStatData[] stats;

	[SerializeField]
	public int statsPerGroup = 20;

	[SerializeField]
	public TMP_Text[] nameGroups;

	[SerializeField]
	public TMP_Text[] valueGroups;

	[SerializeField]
	public float lineHeight = 0.34f;

	public static readonly Dictionary<string, bool> localesCentred = new Dictionary<string, bool>
	{
		{ "en", false },
		{ "zh-Hans", true },
		{ "zh-Hant", true },
		{ "ko", true },
		{ "ja", true }
	};

	public static bool centred;

	public void OnEnable()
	{
		localesCentred.TryGetValue(LocalizationSettings.SelectedLocale.Identifier.Code, out centred);
		Clear();
		Populate();
	}

	public void Populate()
	{
		StopWatch.Start();
		CampaignStats campaignStats = OverallStatsSystem.Get().Clone();
		List<string> list = new List<string> { "GameModeNormal", "GameModeDaily", "GameModeTutorial" };
		GameMode gameMode = null;
		if (Campaign.Data != null && (bool)References.Campaign && References.Campaign.result == Campaign.Result.None)
		{
			list.Remove(Campaign.Data.GameMode.name);
			OverallStatsSystem.Combine(campaignStats, StatsSystem.Get());
			gameMode = Campaign.Data.GameMode;
		}

		foreach (string item in list)
		{
			if (gameMode != null && item == gameMode.name)
			{
				continue;
			}

			GameMode gameMode2 = AddressableLoader.Get<GameMode>("GameMode", item);
			if (SaveSystem.CampaignExists(gameMode2) && Campaign.CheckContinue(gameMode2))
			{
				CampaignStats campaignStats2 = SaveSystem.LoadCampaignData<CampaignStats>(gameMode2, "stats");
				if (campaignStats2 != null)
				{
					OverallStatsSystem.Combine(campaignStats, campaignStats2);
				}
			}
		}

		int num = 0;
		int num2 = 0;
		TMP_Text tMP_Text = nameGroups[num2];
		TMP_Text tMP_Text2 = valueGroups[num2];
		GameStatData[] array = stats;
		foreach (GameStatData gameStatData in array)
		{
			if (!gameStatData)
			{
				tMP_Text.text += "<br>";
				if (!centred)
				{
					tMP_Text2.text += "<br>";
				}
			}
			else
			{
				string localizedString = gameStatData.stringKey.GetLocalizedString();
				localizedString = StringExt.Format(localizedString, centred ? ("<#fff>" + gameStatData.GetStringValue(campaignStats, gameStatData.GetValue(campaignStats)) + "</color><br>") : "<br>");
				tMP_Text.text += localizedString;
				if (!centred)
				{
					TMP_Text tMP_Text3 = tMP_Text2;
					tMP_Text3.text = tMP_Text3.text + gameStatData.GetStringValue(campaignStats, gameStatData.GetValue(campaignStats)) + "<br>";
				}
			}

			if (++num > statsPerGroup)
			{
				num = 0;
				if (++num2 >= nameGroups.Length)
				{
					break;
				}

				tMP_Text = nameGroups[num2];
				tMP_Text2 = valueGroups[num2];
			}
		}

		Debug.Log($"OverallStatsDisplay → Populated ({StopWatch.Stop()}ms)");
	}

	public void Clear()
	{
		TMP_Text[] array = nameGroups;
		foreach (TMP_Text tMP_Text in array)
		{
			tMP_Text.text = $"<line-height={lineHeight}>";
			if (centred)
			{
				tMP_Text.text += "<align=center>";
			}
		}

		array = valueGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = $"<line-height={lineHeight}>";
		}
	}
}
