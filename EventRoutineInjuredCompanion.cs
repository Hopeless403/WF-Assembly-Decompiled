#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoutineInjuredCompanion : EventRoutine
{
	[SerializeField]
	public ChooseNewCardSequence sequence;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public InspectNewUnitSequence inspectSequence;

	[SerializeField]
	public StatusEffectData injuryEffect;

	[SerializeField]
	public SpriteRenderer[] bloodSplats;

	[SerializeField]
	public PromptShower tutorialPrompt;

	[SerializeField]
	public GameObject missingDataDisplay;

	public override IEnumerator Populate()
	{
		CardSaveData cardSaveData = base.data.Get<CardSaveData>("cardSaveData");
		if (!MissingCardSystem.IsMissing(cardSaveData))
		{
			CardData cardData = cardSaveData.Load(keepId: false);
			AddInjuryIfNecessary(cardData);
			cardData.upgrades.RemoveWhere((CardUpgradeData a) => a.type == CardUpgradeData.Type.Crown);
			yield return CreateCard(cardData);
		}
		else
		{
			missingDataDisplay.SetActive(value: true);
			End();
		}
	}

	public void AddInjuryIfNecessary(CardData cardData)
	{
		if (cardData.injuries == null)
		{
			cardData.injuries = new List<CardData.StatusEffectStacks>();
		}

		if (cardData.injuries.Count <= 0)
		{
			cardData.injuries.Add(new CardData.StatusEffectStacks(injuryEffect, 1));
		}
	}

	public override IEnumerator Run()
	{
		CinemaBarSystem.In();
		if (cardContainer.Count > 0)
		{
			tutorialPrompt.Show(cardContainer[0].data.title);
		}

		yield return Sequences.Wait(0.1f);
		yield return sequence.Run();
		CinemaBarSystem.Clear();
		node.SetCleared();
	}

	public IEnumerator CreateCard(CardData cardDataClone)
	{
		cardSelector.character = base.player;
		cardController.owner = base.player;
		cardContainer.SetSize(1, 0.8f);
		cardContainer.owner = base.player;
		Card card = CardManager.Get(cardDataClone, cardController, base.player, inPlay: false, isPlayerCard: true);
		card.entity.flipper.FlipDownInstant();
		cardContainer.Add(card.entity);
		SetBloodSplatColours(card.entity);
		yield return card.UpdateData();
		foreach (Entity item in cardContainer)
		{
			Transform obj = item.transform;
			obj.localPosition = cardContainer.GetChildPosition(item);
			obj.localScale = cardContainer.GetChildScale(item);
			obj.localEulerAngles = cardContainer.GetChildRotation(item);
		}
	}

	public void SetBloodSplatColours(Entity entity)
	{
		Color bloodColour = Object.FindObjectOfType<SplatterSystem>().GetBloodColour(entity);
		SpriteRenderer[] array = bloodSplats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = bloodColour;
		}
	}

	public void TrySelect(Entity entity)
	{
		ActionSelect action = new ActionSelect(entity, delegate
		{
			inspectSequence.SetUnit(entity);
			inspectSequence.Begin();
			cardController.enabled = false;
			cardController.UnHover(entity);
		});
		if (Events.CheckAction(action))
		{
			ActionQueue.Add(action);
		}
	}

	public void CardSelected(Entity entity)
	{
		Events.InvokeEntityChosen(entity);
		End();
	}

	public void End()
	{
		sequence.End();
		cardController.enabled = false;
		tutorialPrompt.Hide();
	}
}
