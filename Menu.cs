#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField]
	public HelpPanelShower retryTutorialHelp;

	public bool active = true;

	public void GoTo(string sceneName)
	{
		if (active)
		{
			new Routine(Transition.To(sceneName));
			active = false;
		}
	}

	public void StartGameOrContinue()
	{
		StartGameOrContinue("GameModeNormal");
	}

	public void StartGameOrContinue(string gameModeName, bool skipTutorial = false)
	{
		GameMode gameMode = AddressableLoader.Get<GameMode>("GameMode", "GameModeTutorial");
		if (Campaign.CheckContinue(gameMode))
		{
			Campaign.Data = CampaignData.Load(gameMode);
			new Routine(SceneManager.Load("ContinueRun", SceneType.Temporary));
		}
		else if (!skipTutorial && SaveSystem.LoadProgressData("tutorialProgress", 0) <= 1 && (bool)retryTutorialHelp)
		{
			retryTutorialHelp.Show();
			retryTutorialHelp.AddButton(0, HelpPanelSystem.ButtonType.Positive, delegate
			{
				StartGame("GameModeTutorial");
			});
			retryTutorialHelp.AddButton(1, HelpPanelSystem.ButtonType.Negative, delegate
			{
				SaveSystem.SaveProgressData("tutorialProgress", 2);
				Events.InvokeTutorialSkip();
				StartGameOrContinue(gameModeName, skipTutorial: true);
			});
		}
		else
		{
			GameMode gameMode2 = AddressableLoader.Get<GameMode>("GameMode", gameModeName);
			if (Campaign.CheckContinue(gameMode2))
			{
				Campaign.Data = CampaignData.Load(gameMode2);
				new Routine(SceneManager.Load("ContinueRun", SceneType.Temporary));
			}
			else
			{
				StartGame(gameMode2);
			}
		}
	}

	public void StartGame()
	{
		StartGame("GameModeNormal");
	}

	public void StartGame(string gameModeName)
	{
		GameMode gameMode = AddressableLoader.Get<GameMode>("GameMode", gameModeName);
		StartGame(gameMode);
	}

	public void StartGame(GameMode gameMode)
	{
		SaveSystem.DeleteCampaign(gameMode);
		Events.InvokeCampaignDeleted();
		Campaign.Data = new CampaignData(gameMode);
		GoTo(gameMode.startScene);
	}

	public void GoToTown()
	{
		GoTo("Town");
	}

	public void ExitGame()
	{
		if (active)
		{
			GameManager.Quit();
		}
	}

	public virtual void Open()
	{
		base.gameObject.SetActive(value: true);
		active = true;
	}

	public void Close()
	{
		base.gameObject.SetActive(value: false);
		active = false;
	}

	public void Toggle()
	{
		if (base.gameObject.activeSelf)
		{
			Close();
		}
		else
		{
			Open();
		}
	}
}
