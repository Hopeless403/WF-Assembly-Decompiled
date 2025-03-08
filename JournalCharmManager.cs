#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class JournalCharmManager : MonoBehaviour
{
	[SerializeField]
	public JournalCharm charmPrefab;

	[SerializeField]
	public Transform content;

	[SerializeField]
	public SmoothScrollRect scrollRect;

	public List<string> discovered;

	public readonly List<JournalCharm> charmIcons = new List<JournalCharm>();

	public Locale locale;

	public void OnEnable()
	{
		discovered = SaveSystem.LoadProgressData<List<string>>("charmsDiscovered");
		if (discovered != null)
		{
			foreach (JournalCharm charmIcon in charmIcons)
			{
				charmIcon.CheckDiscovered(discovered);
			}
		}
		else
		{
			discovered = new List<string>();
		}

		CardPopUp.SetCanvasLayer("PauseMenu", 1);
		CardPopUp.SetIgnoreTimeScale(ignore: true);
		if (locale == null || locale != LocalizationSettings.SelectedLocale)
		{
			locale = LocalizationSettings.SelectedLocale;
			CreateCharms();
		}
	}

	public void OnDisable()
	{
		CardPopUp.Reset();
	}

	public void CreateCharms()
	{
		content.DestroyAllChildren();
		charmIcons.Clear();
		foreach (KeyValuePair<string, CardUpgradeData> item in LoadCharmData())
		{
			JournalCharm journalCharm = Object.Instantiate(charmPrefab, content);
			charmIcons.Add(journalCharm);
			journalCharm.Assign(item.Value);
			if (discovered.Contains(item.Value.name))
			{
				journalCharm.SetDiscovered();
			}
		}

		scrollRect.ScrollToTop();
	}

	public static List<KeyValuePair<string, CardUpgradeData>> LoadCharmData()
	{
		return (from a in (from a in AddressableLoader.GetGroup<CardUpgradeData>("CardUpgradeData")
				where a.type == CardUpgradeData.Type.Charm && a.tier >= -2
				select a).ToDictionary((CardUpgradeData a) => a.title, (CardUpgradeData a) => a)
			orderby a.Value.tier >= 0 descending, a.Key
			select a).ToList();
	}
}
