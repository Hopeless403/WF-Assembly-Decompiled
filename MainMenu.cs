#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class MainMenu : Menu
{
	public GameObject playButton;

	public GameObject continueButton;

	public GameObject modsButton;

	public bool skipTutorial = true;

	public bool tutorial
	{
		get
		{
			if (!skipTutorial)
			{
				return SaveSystem.LoadProgressData("tutorialProgress", 0) == 0;
			}

			return false;
		}
	}

	public void Start()
	{
		modsButton.SetActive(value: true);
		skipTutorial = false;
	}

	public void Play()
	{
		if (tutorial)
		{
			StartGame("GameModeTutorial");
		}
		else
		{
			GoToTown();
		}
	}

	public void Credits()
	{
		if (active)
		{
			StartCoroutine(CreditsRoutine());
		}
	}

	public IEnumerator CreditsRoutine()
	{
		active = false;
		yield return SceneManager.Load("Credits", SceneType.Temporary);
		yield return SceneManager.WaitUntilUnloaded("Credits");
		active = true;
	}

	public void Settings()
	{
		Object.FindObjectOfType<PauseMenu>(includeInactive: true)?.Settings();
	}
}
