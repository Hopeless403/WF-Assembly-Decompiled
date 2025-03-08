#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardFramesSystem : GameSystem
{
	public static CardFramesSystem instance;

	public Dictionary<string, int> frameLevels = new Dictionary<string, int>();

	public Dictionary<string, int> newFrameLevels = new Dictionary<string, int>();

	public bool show;

	public void OnEnable()
	{
		frameLevels = SaveSystem.LoadProgressData<Dictionary<string, int>>("frameLevels") ?? new Dictionary<string, int>();
		newFrameLevels = SaveSystem.LoadProgressData<Dictionary<string, int>>("newFrameLevels") ?? new Dictionary<string, int>();
		instance = this;
		Events.OnCampaignEnd += CampaignEnd;
		Events.OnSettingChanged += SettingChanged;
		show = Settings.Load("SpecialCardFrames", 1) == 1;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
		Events.OnSettingChanged -= SettingChanged;
	}

	public void SettingChanged(string key, object value)
	{
		if (!(key != "SpecialCardFrames") && value is int)
		{
			int num = (int)value;
			show = num == 1;
		}
	}

	public static int GetFrameLevel(string cardDataName)
	{
		if (!instance.show)
		{
			return 0;
		}

		instance.frameLevels.TryGetValue(cardDataName, out var value);
		return value;
	}

	public void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		if (result == Campaign.Result.Win)
		{
			bool anyChange = false;
			if (CheckTrueWin())
			{
				SetFrameLevel(2, out anyChange);
			}
			else
			{
				SetFrameLevel(1, out anyChange);
			}

			if (anyChange)
			{
				SaveSystem.SaveProgressData("frameLevels", frameLevels);
				SaveSystem.SaveProgressData("newFrameLevels", newFrameLevels);
			}
		}
	}

	public static bool CheckTrueWin()
	{
		return SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false);
	}

	public void SetFrameLevel(int level, out bool anyChange)
	{
		anyChange = false;
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			if (TrySetFrameLevel(item, level))
			{
				anyChange = true;
			}
		}
	}

	public bool TrySetFrameLevel(CardData cardData, int level)
	{
		if (!cardData.cardType.miniboss && (!frameLevels.TryGetValue(cardData.name, out var value) || value < level))
		{
			frameLevels[cardData.name] = level;
			newFrameLevels[cardData.name] = level;
			return true;
		}

		return false;
	}

	public bool AnyNewFrames()
	{
		return newFrameLevels.Count > 0;
	}

	public IEnumerator DisplayNewFrames()
	{
		yield return DisplayNewFrames(2);
		yield return DisplayNewFrames(1);
		newFrameLevels.Clear();
		SaveSystem.SaveProgressData("newFrameLevels", newFrameLevels);
	}

	public IEnumerator DisplayNewFrames(int level)
	{
		string[] cards = GetNewCards(level);
		if (cards.Length != 0)
		{
			InputSystem.Disable();
			yield return SceneManager.Load("CardFramesUnlocked", SceneType.Temporary);
			InputSystem.Enable();
			CardFramesUnlockedSequence cardFramesUnlockedSequence = Object.FindObjectOfType<CardFramesUnlockedSequence>();
			yield return cardFramesUnlockedSequence.Run(level, cards);
			yield return SceneManager.WaitUntilUnloaded("CardFramesUnlocked");
		}
	}

	public string[] GetNewCards(int level)
	{
		return (from a in newFrameLevels
			where a.Value == level
			select a.Key).ToArray();
	}
}
