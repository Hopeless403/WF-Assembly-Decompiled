#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class EventRoutineCopyItem : EventRoutine
{
	public CardContainer cardContainer;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public CardType[] canCopyCardTypes;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString promptKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString confirmPromptKey;

	[SerializeField]
	public TweenUI moveToSide;

	[SerializeField]
	public TweenUI moveToCentre;

	[SerializeField]
	public Transform toCopyAnchor;

	[SerializeField]
	public Talker talker;

	public Button backButton;

	public bool cardsCreated;

	public bool promptOpenGrid;

	public bool promptCloseGrid;

	public bool promptCopy;

	public Entity toCopy;

	public int toCopyOriginalIndex;

	public Entity copyEntity;

	public bool promptEnd;

	public bool gridOpen;

	public override IEnumerator Run()
	{
		int num = base.data.Get<int>("enterCount") + 1;
		base.data["enterCount"] = num;
		cardController.owner = base.player;
		cardSelector.character = base.player;
		cardContainer.owner = base.player;
		CinemaBarSystem.Top.SetPrompt(promptKey.GetLocalizedString(), "Select");
		promptOpenGrid = true;
		while (!promptEnd || promptCopy)
		{
			if (promptOpenGrid)
			{
				promptOpenGrid = false;
				if (!gridOpen)
				{
					yield return OpenGrid();
				}
			}
			else if (promptCloseGrid)
			{
				promptCloseGrid = false;
				if (gridOpen)
				{
					yield return CloseGrid();
				}
			}

			else if (promptCopy)
			{
				yield return CopyRoutine();
				promptCopy = false;
			}

			yield return null;
		}

		CinemaBarSystem.Clear();
		if (base.data.Get<int>("canCopy") <= 0)
		{
			node.SetCleared();
		}
	}

	public IEnumerator OpenGrid()
	{
		gridOpen = true;
		cardController.Enable();
		if (!cardsCreated)
		{
			cardsCreated = true;
			Routine.Clump clump = new Routine.Clump();
			clump.Add(CreateCards());
			clump.Add(Sequences.Wait(0.2f));
			yield return clump.WaitForEnd();
		}

		cardContainer.gameObject.SetActive(value: true);
		cardContainer.transform.localScale = Vector3.one * 0.5f;
		LeanTween.scale(cardContainer.gameObject, Vector3.one, 1.25f).setEase(LeanTweenType.easeOutElastic);
	}

	public IEnumerator CreateCards()
	{
		Routine.Clump clump = new Routine.Clump();
		List<CardData> list = new List<CardData>();
		foreach (CardData item in base.player.data.inventory.deck)
		{
			if (canCopyCardTypes.Contains(item.cardType))
			{
				list.Add(item);
			}
		}

		foreach (CardData item2 in base.player.data.inventory.reserve)
		{
			if (canCopyCardTypes.Contains(item2.cardType))
			{
				list.Add(item2);
			}
		}

		foreach (CardData item3 in list)
		{
			Card card = CardManager.Get(item3, cardController, base.player, inPlay: false, isPlayerCard: true);
			cardContainer.Add(card.entity);
			clump.Add(card.UpdateData());
		}

		yield return clump.WaitForEnd();
		cardContainer.SetChildPositions();
	}

	public IEnumerator CloseGrid()
	{
		gridOpen = false;
		float num = 0.3f;
		LeanTween.scale(cardContainer.gameObject, Vector3.zero, num).setEase(LeanTweenType.easeInBack).setOnComplete((Action)delegate
		{
			cardContainer.gameObject.SetActive(value: false);
		});
		yield return new WaitForSeconds(num);
	}

	public void Copy(Entity entity)
	{
		if (toCopy == entity)
		{
			promptCopy = true;
			copyEntity = entity;
			End();
		}
		else if (!toCopy)
		{
			moveToCentre.Fire();
			toCopy = entity;
			toCopyOriginalIndex = cardContainer.IndexOf(entity);
			cardContainer.Remove(entity);
			entity.transform.SetParent(toCopyAnchor);
			LeanTween.moveLocal(entity.gameObject, Vector3.zero, 0.33f).setEaseOutQuart();
			promptCloseGrid = true;
			CinemaBarSystem.Top.SetPrompt(confirmPromptKey.GetLocalizedString(), "Select");
		}
	}

	public IEnumerator CopyRoutine()
	{
		backButton.interactable = false;
		int num = base.data.Get<int>("canCopy") - 1;
		base.data["canCopy"] = num;
		if (num <= 0)
		{
			PromptCloseGrid();
		}

		CardData cardData = copyEntity.data.Clone(runCreateScripts: false);
		cardData.upgrades.RemoveAll((CardUpgradeData a) => a.type == CardUpgradeData.Type.Crown);
		Card card = CardManager.Get(cardData, cardController, base.player, inPlay: false, isPlayerCard: true);
		yield return card.UpdateData();
		card.transform.position = copyEntity.transform.position;
		cardSelector.TakeCard(card.entity);
		promptCloseGrid = true;
		yield return new WaitForSeconds(0.5f);
	}

	public void Back()
	{
		if ((bool)toCopy)
		{
			promptOpenGrid = true;
			cardContainer.Insert(toCopyOriginalIndex, toCopy);
			toCopy = null;
			cardContainer.SetChildPositions();
			moveToSide.Fire();
			CinemaBarSystem.Top.SetPrompt(promptKey.GetLocalizedString(), "Select");
		}
		else
		{
			End();
		}
	}

	public void PromptCloseGrid()
	{
		promptCloseGrid = true;
	}

	public void End()
	{
		promptEnd = true;
		backButton.interactable = false;
		cardController.Disable();
	}
}
