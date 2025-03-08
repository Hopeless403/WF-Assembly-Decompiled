#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PauseMenu : Menu
{
	[SerializeField]
	public ButtonAnimator backToMenuButton;

	[SerializeField]
	public ButtonAnimator quickRestartButton;

	[SerializeField]
	public bool freezeTimeScale = true;

	[SerializeField]
	public JournalTab battleLogTab;

	[SerializeField]
	public JournalTab settingsTab;

	[SerializeField]
	public JournalTab lorePageTab;

	public static int blocked;

	[SerializeField]
	public bool doButtonLogic = true;

	public void OnEnable()
	{
		if (doButtonLogic)
		{
			switch (SceneManager.GetActive().name)
			{
				case "MainMenu":
					backToMenuButton.interactable = false;
					quickRestartButton.interactable = false;
					break;
				case "CharacterSelect":
				case "ContinueRun":
				case "Town":
				case "Cards":
					backToMenuButton.interactable = true;
					quickRestartButton.interactable = false;
					break;
				default:
					backToMenuButton.interactable = true;
					quickRestartButton.interactable = Campaign.Data != null && Campaign.Data.GameMode.canRestart;
					break;
			}
		}

		GameManager.paused = true;
		if (freezeTimeScale)
		{
			Time.timeScale = 0f;
		}
	}

	public void OnDisable()
	{
		GameManager.paused = false;
		if (freezeTimeScale)
		{
			Time.timeScale = 1f;
		}
	}

	public void GoToMainMenu()
	{
		if (active)
		{
			MonoBehaviourSingleton<Transition>.instance.StartCoroutine(SceneManager.Unload("Mods"));
			MonoBehaviourSingleton<Transition>.instance.StartCoroutine(SceneManager.Load("MainMenu", SceneType.Active));
		}
	}

	public void BackToMainMenu()
	{
		if (active)
		{
			if ((bool)References.Campaign && !Campaign.Data.GameMode.doSave)
			{
				References.Campaign.End(Campaign.Result.None);
				JournalNameHistory.MostRecentNameKilled();
			}

			new Routine(Sequences.EndCampaign("MainMenu"));
			active = false;
			Events.InvokeBackToMainMenu();
		}
	}

	public void QuickRestart()
	{
		if (active)
		{
			if ((bool)References.Campaign)
			{
				References.Campaign.End(Campaign.Result.Restart);
				JournalNameHistory.MostRecentNameKilled();
			}

			Campaign.Data = new CampaignData(Campaign.Data.GameMode);
			new Routine(Sequences.EndCampaign("CharacterSelect"));
			active = false;
		}
	}

	public void BattleLog()
	{
		if (!base.gameObject.activeSelf)
		{
			Open();
			battleLogTab.Select();
		}
	}

	public void Settings()
	{
		if (!base.gameObject.activeSelf)
		{
			Open();
			settingsTab.Select();
		}
	}

	public void OpenLorePages()
	{
		if (!base.gameObject.activeSelf)
		{
			Open();
			lorePageTab.Select();
		}
	}

	public override void Open()
	{
		if (blocked <= 0)
		{
			base.Open();
		}
	}

	public static void Block()
	{
		blocked++;
		PauseMenu[] array = Object.FindObjectsOfType<PauseMenu>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
		}
	}

	public static void Unblock()
	{
		blocked--;
	}
}
