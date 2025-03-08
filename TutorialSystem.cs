#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class TutorialSystem : GameSystem
{
	[Serializable]
	public struct Data
	{
		public int battleNumber;

		public int eventNumber;
	}

	public UnityEngine.Localization.LocalizedString redrawBellHelpKey;

	public Prompt.Emote.Type redrawBellHelpEmote = Prompt.Emote.Type.Explain;

	public UnityEngine.Localization.LocalizedString redrawBellHelpButtonKey;

	public Sprite redrawBellHelpSprite;

	public Data data;

	public void OnEnable()
	{
		Events.OnSceneChanged += SceneChanged;
		Events.OnCampaignSaved += CampaignSaved;
		Events.OnCampaignLoaded += CampaignLoaded;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
		Events.OnCampaignSaved -= CampaignSaved;
		Events.OnCampaignLoaded -= CampaignLoaded;
	}

	public void SceneChanged(Scene scene)
	{
		string text = scene.name;
		if (!(text == "Battle"))
		{
			if (text == "Event")
			{
				switch (data.eventNumber)
				{
					case 0:
						base.gameObject.AddComponent<TutorialCompanionSystem>();
						break;
					case 2:
						base.gameObject.AddComponent<TutorialItemSystem2>();
						break;
				}

				data.eventNumber++;
			}

			return;
		}

		switch (data.battleNumber)
		{
			case 0:
				base.gameObject.AddComponent<TutorialBattleSystem1>();
				SaveSystem.SaveProgressData("tutorialProgress", 1);
				Events.InvokeTutorialProgress(1);
				break;
			case 1:
				base.gameObject.AddComponent<TutorialBattleSystem2>();
				break;
			case 2:
			{
				base.gameObject.AddComponent<TutorialBattleSystem3>();
				DynamicTutorialSystem dynamicTutorialSystem = UnityEngine.Object.FindObjectOfType<DynamicTutorialSystem>();
			if ((object)dynamicTutorialSystem != null)
			{
				dynamicTutorialSystem.enabled = true;
				}
	
				SaveSystem.SaveProgressData("tutorialProgress", 2);
				Events.InvokeTutorialProgress(2);
				break;
			}
		}

		data.battleNumber++;
	}

	public void CampaignSaved()
	{
		SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "tutorialData", data);
	}

	public void CampaignLoaded()
	{
		data = SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "tutorialData", default(Data));
	}
}
