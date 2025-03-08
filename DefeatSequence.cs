#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using FMODUnity;
using UnityEngine;

public class DefeatSequence : MonoBehaviour
{
	[SerializeField]
	public EventReference music;

	[SerializeField]
	public float startDelay;

	[SerializeField]
	public GameObject restartButton;

	[SerializeField]
	public GameObject scoresButton;

	[SerializeField]
	public GameObject defeatLayout;

	[SerializeField]
	public GameObject winLayout;

	[SerializeField]
	public GameObject vanquishedLayout;

	[SerializeField]
	public GameObject statsLayout;

	[SerializeField]
	public GameObject leaderDisplayLayout;

	[SerializeField]
	public GameObject challengeLayout;

	[SerializeField]
	public GameObject progressLayout;

	[SerializeField]
	public GameObject scoresLayout;

	[SerializeField]
	public GameObject buttonsLayout;

	[SerializeField]
	public LayoutLink[] titlePanels;

	[SerializeField]
	public ChallengeProgressSequence challengeSequence;

	[SerializeField]
	public MetaprogressSequence progressSequence;

	[SerializeField]
	public ScoreSequence scoreSequence;

	[SerializeField]
	public string endCreditsScene = "CreditsEnd";

	[SerializeField]
	public WinMusic winMusic;

	[Header("Temporary win help panel")]
	[SerializeField]
	public HelpPanelShower gameWinHelp;

	[SerializeField]
	public ParticleSystem gameWinPS;

	public bool active;

	public IEnumerator Start()
	{
		bool flag = References.Battle.winner == References.Battle.player;
		if (!flag)
		{
			MusicSystem.StartMusic(music);
		}

		active = true;
		restartButton.SetActive(!flag && Campaign.Data.GameMode.canRestart);
		scoresButton.SetActive(Campaign.Data.GameMode.submitScore);
		yield return Routine();
	}

	public IEnumerator Routine()
	{
		yield return new WaitForSeconds(startDelay);
		LayoutLink[] array = titlePanels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].offset = new Vector3(0f, 0f, -4f);
		}

		if (References.Battle.winner == References.Battle.player)
		{
			if (SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false))
			{
				yield return SceneManager.Load(endCreditsScene, SceneType.Temporary);
				yield return SceneManager.WaitUntilUnloaded(endCreditsScene);
			}

			winMusic.Play();
			gameWinPS.Play();
			if (SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "trueWin", defaultValue: false))
			{
				vanquishedLayout.SetActive(value: true);
			}
			else
			{
				winLayout.SetActive(value: true);
			}
		}
		else
		{
			defeatLayout.SetActive(value: true);
		}

		yield return new WaitForSeconds(1f);
		array = titlePanels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].offset = Vector3.zero;
		}

		if (Campaign.Data.GameMode.showStats)
		{
			statsLayout.SetActive(value: true);
			leaderDisplayLayout.SetActive(value: true);
		}

		ChallengeProgressSystem challengeProgressSystem = Object.FindObjectOfType<ChallengeProgressSystem>();
		if ((object)challengeProgressSystem != null && challengeProgressSystem.progress != null && challengeProgressSystem.progress.Count((ChallengeProgress a) => a.currentValue > a.originalValue) > 0)
		{
			challengeLayout.SetActive(value: true);
			yield return new WaitForSeconds(1f);
			yield return new WaitUntil(() => !challengeSequence.running);
		}

		Routine.Clump clump = new Routine.Clump();
		if (Campaign.Data.GameMode.submitScore)
		{
			scoresLayout.SetActive(value: true);
			yield return new WaitForSeconds(1f);
			clump.Add(new WaitUntil(() => !scoreSequence.running));
		}

		yield return clump.WaitForEnd();
		buttonsLayout.SetActive(value: true);
	}

	public void GoTo(string sceneName)
	{
		if (active)
		{
			active = false;
			new Routine(Sequences.EndCampaign(sceneName));
		}
	}

	public void QuickRestart()
	{
		if (active)
		{
			Campaign.Data = new CampaignData(Campaign.Data.GameMode);
			new Routine(Sequences.EndCampaign("CharacterSelect"));
			active = false;
		}
	}

	public void Exit()
	{
		if (active)
		{
			active = false;
			if (References.Campaign != null)
			{
				References.Campaign.Final();
			}

			GameManager.Quit();
		}
	}
}
