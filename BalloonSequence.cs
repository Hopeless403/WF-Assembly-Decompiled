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
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class BalloonSequence : BuildingSequence
{
	[SerializeField]
	public RectTransform panel;

	[SerializeField]
	public TMP_Text title;

	[SerializeField]
	public TMP_Text date;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public DeckDisplayGroup deckDisplayGroup;

	[SerializeField]
	public DailyGenerator dailyGenerator;

	[SerializeField]
	public GameMode gameMode;

	[SerializeField]
	public GameObject playButton;

	[SerializeField]
	public GameObject scoresButton;

	[SerializeField]
	public GameObject timer;

	[SerializeField]
	public HelpPanelShower noConnection;

	[SerializeField]
	public ModifierDisplay modifierDisplay;

	[SerializeField]
	public Scroller scroller;

	[SerializeField]
	public GameObject loadingIcon;

	[SerializeField]
	public HelpPanelShower firstTimeHelp;

	public int seed;

	public bool loading;

	public DateTime dateTime;

	public void OnDisable()
	{
		DailyFetcher.CancelFetch();
	}

	public override IEnumerator Sequence()
	{
		panel.anchoredPosition = new Vector2(-10f, 0f);
		playButton.SetActive(value: false);
		scoresButton.SetActive(value: false);
		loadingIcon.SetActive(value: true);
		yield return DailyFetcher.FetchDateTime();
		if (!DailyFetcher.fetched)
		{
			noConnection.Show();
			noConnection.AddButton(0, HelpPanelSystem.ButtonType.Positive, Close);
			yield break;
		}

		loadingIcon.SetActive(value: false);
		panel.LeanMove(new Vector3(-0.7f, 0f), 1.2f).setFrom(new Vector3(-2.5f, 0f)).setEaseOutElastic();
		bool flag = DailyFetcher.CanPlay();
		playButton.SetActive(flag);
		timer.SetActive(!flag);
		scoresButton.SetActive(value: true);
		seed = DailyFetcher.GetSeed();
		UpdateTitleText();
		dateTime = DailyFetcher.GetDateTime();
		Debug.Log($"DailyFetcher → Daily Time: {dateTime}");
		UpdateDateText();
		yield return dailyGenerator.Run(seed, gameMode);
		List<GameModifierData> modifiers = Campaign.Data.Modifiers;
		if (modifiers != null && modifiers.Count > 0)
		{
			foreach (GameModifierData modifier in Campaign.Data.Modifiers)
			{
				modifierDisplay.CreateIcon(modifier);
			}
		}

		yield return CreateCards(References.PlayerData.inventory.deck);
		if (!SaveSystem.LoadProgressData("dailyRunHelpSeen", defaultValue: false))
		{
			firstTimeHelp.Show();
			SaveSystem.SaveProgressData("dailyRunHelpSeen", value: true);
		}
	}

	public void UpdateTitleText()
	{
		string localizedString = titleKey.GetLocalizedString();
		title.text = localizedString;
	}

	public void UpdateDateText()
	{
		CultureInfo cultureInfo = LocalizationSettings.SelectedLocale.Identifier.CultureInfo;
		date.text = dateTime.ToString("D", cultureInfo);
	}

	public IEnumerator CreateCards(IEnumerable<CardData> cardsToCreate)
	{
		List<Card> list = new List<Card>();
		Routine.Clump clump = new Routine.Clump();
		foreach (CardData item in cardsToCreate)
		{
			Card card = CardManager.Get(item, cardController, null, inPlay: false, isPlayerCard: true);
			list.Add(card);
			card.entity.flipper.FlipDownInstant();
			clump.Add(UpdateCardData(card));
		}

		foreach (Card item2 in list)
		{
			deckDisplayGroup.AddCard(item2);
		}

		yield return clump.WaitForEnd();
		yield return null;
		deckDisplayGroup.UpdatePositions();
		if ((bool)scroller)
		{
			scroller.ScrollImmediate(10f);
		}
	}

	public static IEnumerator UpdateCardData(Card card)
	{
		yield return card.UpdateData();
		card.entity.flipper.FlipUp(force: true);
	}

	public void Continue()
	{
		if (base.enabled)
		{
			base.enabled = false;
			SaveSystem.SaveProgressData("dailyPlayed", DailyFetcher.GetDateTime().ToString());
			Campaign.Begin();
			new Routine(Transition.To("Campaign"));
		}
	}

	public new void Close()
	{
		UnityEngine.Object.FindObjectOfType<BuildingDisplay>()?.End();
	}
}
