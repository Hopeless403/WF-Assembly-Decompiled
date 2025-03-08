#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CharacterSelectScreen : MonoBehaviour
{
	[SerializeField]
	public int options = 3;

	[SerializeField]
	public int differentTribes = 3;

	public List<Character> characters;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer leaderCardContainer;

	[SerializeField]
	public GameObject backButton;

	[SerializeField]
	public SelectTribe tribeSelection;

	[SerializeField]
	public SelectLeader leaderSelection;

	[SerializeField]
	public SelectStartingPet petSelection;

	[SerializeField]
	public InspectNewUnitSequence selectionSequence;

	[Header("Title")]
	[SerializeField]
	public GameObject title;

	[SerializeField]
	public LocalizeStringEvent titleText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleTribeKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleLeaderKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titlePetKey;

	[SerializeField]
	public Image titleUnderline;

	[SerializeField]
	public Sprite underlineTribeSprite;

	[SerializeField]
	public Sprite underlineLeaderSprite;

	[SerializeField]
	public Sprite underlinePetSprite;

	[Header("Hard Mode Modifiers")]
	[SerializeField]
	public HardModeModifierDisplay modifierDisplay;

	[SerializeField]
	public UnlockModifierSequence modifierUnlockSequence;

	[Header("Tribe Flags")]
	[SerializeField]
	public Transform flagGroup;

	[SerializeField]
	public TribeFlagDisplay flagBase;

	[SerializeField]
	public Vector3 flagOffset = new Vector3(0f, -4f);

	[SerializeField]
	public TribeDisplaySequence tribeDisplay;

	public readonly List<GameObject> flags = new List<GameObject>();

	public int seed;

	public bool loadingToCampaign;

	public List<ClassData> unlockedClassesForThisGameMode;

	public const bool selectTribe = true;

	public void OnEnable()
	{
		loadingToCampaign = false;
	}

	public IEnumerator Start()
	{
		leaderSelection.SetSeed(Campaign.Data.Seed);
		List<ClassData> lockedClasses = MetaprogressionSystem.GetLockedClasses();
		Debug.Log("Locked Classes: [" + string.Join(", ", lockedClasses) + "]");
		unlockedClassesForThisGameMode = new List<ClassData>(Campaign.Data.GameMode.classes);
		unlockedClassesForThisGameMode.RemoveMany(lockedClasses);
		Debug.Log("Available Classes For [" + Campaign.Data.GameMode.name + "]: [" + string.Join(", ", unlockedClassesForThisGameMode) + "]");
		if (unlockedClassesForThisGameMode.Count > 1)
		{
			tribeSelection.SetAvailableTribes(unlockedClassesForThisGameMode);
			tribeSelection.Run();
			title.SetActive(value: true);
			titleText.StringReference = titleTribeKey;
			titleUnderline.sprite = underlineTribeSprite;
		}
		else
		{
			leaderSelection.Run(unlockedClassesForThisGameMode);
			yield return leaderSelection.GenerateLeaders(useSeed: true);
			leaderSelection.FlipUpLeadersInstant();
			title.SetActive(value: true);
			titleText.StringReference = titleLeaderKey;
			titleUnderline.sprite = underlineLeaderSprite;
		}

		yield return Sequences.Wait(0.1f);
		yield return petSelection.SetUp();
		if (modifierDisplay.gameObject.activeSelf)
		{
			modifierDisplay.Populate();
		}

		backButton.gameObject.SetActive(Campaign.Data.GameMode.canGoBack);
		Transition.End();
		if (NewFinalBossChecker.Check())
		{
			cardController.Disable();
			yield return NewFinalBossChecker.Run();
			cardController.Enable();
		}
	}

	public void Continue()
	{
		if (leaderSelection.current == null || loadingToCampaign)
		{
			return;
		}

		if (petSelection.running)
		{
			petSelection.Stop();
		}

		if (petSelection.selectedPetIndex < 0 && petSelection.CanRun)
		{
			if (selectionSequence.IsRunning)
			{
				selectionSequence.UnsetUnit();
				selectionSequence.End();
				cardController.Enable();
			}

			petSelection.Run(leaderSelection.current.entity);
			SetTitlePet();
		}
		else
		{
			StartCoroutine(ContinueRoutine(leaderSelection.current));
		}
	}

	public IEnumerator ContinueRoutine(SelectLeader.Character selected)
	{
		loadingToCampaign = true;
		petSelection.Gain(leaderSelection.current.data);
		leaderSelection.current.AddLeaderToInventory();
		References.PlayerData = selected.data;
		yield return JournalAddNameSequence.LoadAndRun(leaderSelection.current.entity.data, unloadAfter: false);
		yield return Events.InvokeCampaignInit();
		Campaign.Begin();
		new Routine(Transition.To(Campaign.Data.GameMode.sceneAfterSelection));
	}

	public void SetTitleTribe()
	{
		title.SetActive(value: true);
		titleText.StringReference = titleTribeKey;
		titleUnderline.sprite = underlineTribeSprite;
	}

	public void SetTitleLeader()
	{
		title.SetActive(value: true);
		titleText.StringReference = titleLeaderKey;
		titleUnderline.sprite = underlineLeaderSprite;
	}

	public void SetTitlePet()
	{
		title.SetActive(value: false);
	}

	public void Back()
	{
		if (leaderSelection.running)
		{
			if (unlockedClassesForThisGameMode.Count > 1)
			{
				leaderSelection.Cancel();
				tribeSelection.SetAvailableTribes(unlockedClassesForThisGameMode);
				tribeSelection.Run();
				tribeSelection.RevealAnimation();
				SetTitleTribe();
			}
			else
			{
				ReturnToMenu();
			}
		}
		else if (petSelection.running)
		{
			petSelection.Cancel();
			leaderSelection.Return();
			SetTitleLeader();
		}
		else
		{
			ReturnToMenu();
		}
	}

	public void ReturnToMenu()
	{
		new Routine(Transition.To((SaveSystem.LoadProgressData("tutorialProgress", 0) == 0) ? "MainMenu" : "Town"));
	}
}
