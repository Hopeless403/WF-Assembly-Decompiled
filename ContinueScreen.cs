#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ContinueScreen : MonoBehaviour
{
	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public ModifierDisplay modifierDisplay;

	[SerializeField]
	public TMP_Text titleText;

	[SerializeField]
	public TMP_Text dateText;

	[SerializeField]
	public TMP_Text progressText;

	[SerializeField]
	public Menu menu;

	[SerializeField]
	public GameObject backButton;

	[SerializeField]
	public GameObject continueButton;

	[SerializeField]
	public HelpPanelShower giveUpHelpShower;

	[SerializeField]
	public GameObject missingDataDisplay;

	public CampaignSaveData data;

	public bool closing;

	public IEnumerator Start()
	{
		bool flag = true;
		List<CardData> list = null;
		try
		{
			if (SaveSystem.CampaignExists(Campaign.Data.GameMode) && SaveSystem.CampaignDataExists(Campaign.Data.GameMode, "data"))
			{
				data = SaveSystem.LoadCampaignData<CampaignSaveData>(Campaign.Data.GameMode, "data");
				Events.InvokeCampaignLoaded();
				string text = SaveSystem.LoadCampaignData<string>(Campaign.Data.GameMode, "startDate");
				if (text != null)
				{
					dateText.gameObject.SetActive(value: true);
					DateTime dateTime = DateTime.ParseExact(text, "dd/MM/yyyy", GameManager.CultureInfo);
					CultureInfo cultureInfo = LocalizationSettings.SelectedLocale.Identifier.CultureInfo;
					dateText.text = dateTime.ToString("D", cultureInfo);
				}

				bool mainGameMode = Campaign.Data.GameMode.mainGameMode;
				string[] modifiers = data.modifiers;
				if (modifiers == null || modifiers.Length <= 0)
				{
					modifierDisplay.gameObject.SetActive(value: false);
				}
				else
				{
					modifiers = data.modifiers;
					foreach (string assetName in modifiers)
					{
						GameModifierData gameModifierData = AddressableLoader.Get<GameModifierData>("GameModifierData", assetName);
						if (gameModifierData != null)
						{
							modifierDisplay.CreateIcon(gameModifierData, mainGameMode);
						}
					}
				}

				list = data.characters[data.playerId].inventoryData.deck.LoadList<CardData, CardSaveData>();
				continueButton.SetActive(value: true);
			}
		}
		catch (NullReferenceException message)
		{
			flag = false;
			Debug.LogWarning(message);
		}

		if (flag)
		{
			CheckMissingData(data);
			Routine.Clump clump = new Routine.Clump();
			List<Entity> cards = new List<Entity>();
			foreach (CardData item in list)
			{
				Card card = CardManager.Get(item, cardController, null, inPlay: false, isPlayerCard: true);
				card.hover.enabled = false;
				cards.Add(card.entity);
				clump.Add(card.UpdateData());
			}

			yield return clump.WaitForEnd();
			foreach (Entity item2 in cards)
			{
				item2.display.hover.enabled = true;
			}

			cards.Sort((Entity a, Entity b) => a.data.cardType.sortPriority.CompareTo(b.data.cardType.sortPriority));
			cardContainer.max = cards.Count;
			for (int num = cards.Count - 1; num >= 0; num--)
			{
				Entity entity = cards[num];
				cardContainer.Add(entity);
			}

			cardContainer.SetChildPositions();
			backButton.gameObject.SetActive(Campaign.Data.GameMode.canGoBack);
		}
		else
		{
			Debug.LogWarning("Failed to load campaign save...");
			SaveSystem.DeleteCampaign(Campaign.Data.GameMode);
			Events.InvokeCampaignDeleted();
			Campaign.Data = new CampaignData(Campaign.Data.GameMode);
			new Routine(Sequences.SceneChange(Campaign.Data.GameMode.startScene));
		}
	}

	public void Continue()
	{
		if (!closing)
		{
			closing = true;
			menu.GoTo("Campaign");
		}
	}

	public void PromptGiveUp()
	{
		if (!closing)
		{
			giveUpHelpShower.Show();
			giveUpHelpShower.AddButton(0, HelpPanelSystem.ButtonType.Positive, GiveUp);
			giveUpHelpShower.AddButton(1, HelpPanelSystem.ButtonType.Negative, null);
		}
	}

	public void GiveUp()
	{
		if (!closing)
		{
			closing = true;
			CampaignSaveData campaignSaveData = SaveSystem.LoadCampaignData<CampaignSaveData>(Campaign.Data.GameMode, "data");
			CharacterSaveData characterSaveData = campaignSaveData.characters[campaignSaveData.playerId];
			if (characterSaveData != null)
			{
				CampaignStats stats = SaveSystem.LoadCampaignData<CampaignStats>(Campaign.Data.GameMode, "stats");
				Events.InvokeCampaignEnd(Campaign.Result.Restart, stats, characterSaveData.LoadPlayerData());
			}

			SaveSystem.DeleteCampaign(Campaign.Data.GameMode);
			Events.InvokeCampaignDeleted();
			StartCoroutine(GiveUpSequence());
		}
	}

	public IEnumerator GiveUpSequence()
	{
		if (Settings.Load("showJournalNameOnEnd", defaultValue: false))
		{
			yield return JournalVoidNameSequence.LoadAndRun(unloadAfter: false);
		}
		else
		{
			JournalNameHistory.MostRecentNameKilled();
		}

		Campaign.Data = new CampaignData(Campaign.Data.GameMode);
		ForceClose();
	}

	public void Close()
	{
		if (!closing)
		{
			ForceClose();
		}
	}

	public void ForceClose()
	{
		closing = true;
		new Routine(SceneManager.Unload("ContinueRun"));
	}

	public void CheckMissingData(CampaignSaveData data)
	{
		if (HasMissingData(data))
		{
			continueButton.SetActive(value: false);
			missingDataDisplay.SetActive(value: true);
		}
	}

	public bool HasMissingData(CampaignSaveData data)
	{
		CharacterSaveData characterSaveData = data.characters[data.playerId];
		if (MissingCardSystem.HasMissingData(characterSaveData.inventoryData.deck) || MissingCardSystem.HasMissingData(characterSaveData.inventoryData.reserve))
		{
			return true;
		}

		CampaignNodeSaveData[] nodes = data.nodes;
		foreach (CampaignNodeSaveData campaignNodeSaveData in nodes)
		{
			if (!campaignNodeSaveData.cleared)
			{
				CampaignNode campaignNode = campaignNodeSaveData.Load();
				if (campaignNode.type.HasMissingData(campaignNode))
				{
					return true;
				}
			}
		}

		return false;
	}
}
