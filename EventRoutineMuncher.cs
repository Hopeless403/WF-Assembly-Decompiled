#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class EventRoutineMuncher : EventRoutine
{
	[SerializeField]
	public UnityEngine.Animator muncherAnimator;

	[SerializeField]
	public TweenUI muncherMoveToSide;

	[SerializeField]
	public TweenUI muncherMoveToMid;

	public CardContainer cardContainer;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardType[] canEatCardTypes;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString initialPromptKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString feedPromptKey;

	[SerializeField]
	public ParticleSystem munchParticles;

	public Button backButton;

	[Header("Speech Bubs")]
	[SerializeField]
	public Talker talker;

	[SerializeField]
	public Vector2 sayDelay = new Vector2(0.8f, 1f);

	[SerializeField]
	public float greetingChance = 1f;

	[SerializeField]
	public float explainChance = 1f;

	public bool hoveringMuncher;

	public bool promptOpenGrid;

	public bool promptCloseGrid;

	public bool promptEnd;

	public bool gridOpen;

	public override IEnumerator Run()
	{
		int num = base.data.Get<int>("enterCount") + 1;
		base.data["enterCount"] = num;
		cardController.owner = base.player;
		cardContainer.owner = base.player;
		UpdatePrompt();
		SfxSystem.OneShot("event:/sfx/location/muncher/enter");
		if (UnityEngine.Random.value <= greetingChance && num == 1)
		{
			StartCoroutine(SayGreeting());
		}

		while (!promptEnd)
		{
			if (promptOpenGrid)
			{
				promptOpenGrid = false;
				if (!gridOpen && base.data.Get<int>("canEat") > 0)
				{
					base.data["openCount"] = base.data.Get<int>("openCount") + 1;
					gridOpen = true;
					if (UnityEngine.Random.value <= greetingChance)
					{
						StartCoroutine(SayExplain());
					}

					yield return OpenGrid();
				}
			}
			else if (promptCloseGrid)
			{
				promptCloseGrid = false;
				if (gridOpen)
				{
					gridOpen = false;
					yield return CloseGrid();
				}
			}

			yield return null;
		}

		CinemaBarSystem.Clear();
		if (base.data.Get<int>("canEat") <= 0)
		{
			node.SetCleared();
		}
	}

	public IEnumerator SayGreeting()
	{
		float seconds = sayDelay.Random();
		yield return new WaitForSeconds(seconds);
		if (base.data.Get<int>("openCount") <= 0)
		{
			talker.Say("greet", 0f);
		}
	}

	public IEnumerator SayExplain()
	{
		float seconds = sayDelay.Random();
		yield return new WaitForSeconds(seconds);
		if (base.data.Get<int>("openCount") == 1)
		{
			talker.Say("explain", 0f);
		}
	}

	public IEnumerator OpenGrid()
	{
		SfxSystem.OneShot("event:/sfx/location/muncher/slide_right");
		CinemaBarSystem.Clear();
		muncherMoveToSide.Fire();
		cardController.Enable();
		Routine.Clump clump = new Routine.Clump();
		clump.Add(CreateCards());
		clump.Add(Sequences.Wait(0.2f));
		yield return clump.WaitForEnd();
		cardContainer.gameObject.SetActive(value: true);
		cardContainer.transform.localScale = Vector3.one * 0.5f;
		LeanTween.scale(cardContainer.gameObject, Vector3.one, 1.25f).setEase(LeanTweenType.easeOutElastic);
		UpdatePrompt();
	}

	public IEnumerator CreateCards()
	{
		Routine.Clump clump = new Routine.Clump();
		List<CardData> list = new List<CardData>();
		foreach (CardData item in base.player.data.inventory.deck)
		{
			if (canEatCardTypes.Contains(item.cardType))
			{
				list.Add(item);
			}
		}

		foreach (CardData item2 in base.player.data.inventory.reserve)
		{
			if (canEatCardTypes.Contains(item2.cardType))
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
		UpdatePrompt();
		cardController.Disable();
		float num = 0.5f;
		LeanTween.scale(cardContainer.gameObject, Vector3.zero, num).setEase(LeanTweenType.easeInBack).setOnComplete((Action)delegate
		{
			foreach (Entity item in cardContainer)
			{
				CardManager.ReturnToPool(item);
			}

			cardContainer.Clear();
			cardContainer.gameObject.SetActive(value: false);
		});
		yield return new WaitForSeconds(num);
		yield return null;
		SfxSystem.OneShot("event:/sfx/location/muncher/slide_left");
		muncherMoveToMid.Fire();
	}

	public void UpdatePrompt()
	{
		if (base.data.Get<int>("canEat") > 0)
		{
			if (gridOpen)
			{
				CinemaBarSystem.Top.SetPrompt(feedPromptKey.GetLocalizedString(), "Select");
			}
			else
			{
				CinemaBarSystem.Top.SetPrompt(initialPromptKey.GetLocalizedString(), "Select");
			}
		}
		else
		{
			CinemaBarSystem.Clear();
		}
	}

	public void PromptOpenGrid(BaseEventData eventData)
	{
		if (!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left)
		{
			promptOpenGrid = true;
		}
	}

	public void PromptCloseGrid()
	{
		promptCloseGrid = true;
	}

	public void End()
	{
		promptEnd = true;
	}

	public void HoverMuncher()
	{
		hoveringMuncher = true;
	}

	public void UnHoverMuncher()
	{
		hoveringMuncher = false;
	}

	public void DragCardStart(Entity entity)
	{
		muncherAnimator.SetBool("DraggingCard", value: true);
		Events.InvokeMuncherDrag();
		NavigationState.Start(new NavigationStateMuncher(this));
	}

	public void DragCardEnd(Entity entity)
	{
		muncherAnimator.SetBool("DraggingCard", value: false);
		NavigationState.BackToPreviousState();
		Events.InvokeMuncherDragCancel();
		cardController.hoverEntity = null;
	}

	public void TryEatIfHovering(Entity entity)
	{
		if (hoveringMuncher && TryEat(entity))
		{
			Events.InvokeMuncherFeed(entity);
		}
	}

	public bool TryEat(Entity entity)
	{
		if (base.data.Get<int>("canEat") > 0 && canEatCardTypes.Contains(entity.data.cardType))
		{
			Eat(entity);
			return true;
		}

		return false;
	}

	public void Eat(Entity entity)
	{
		if (entity.owner.data.inventory.deck.RemoveWhere((CardData a) => entity.data.id == a.id))
		{
			Debug.Log("[" + entity.data.name + "] Removed From [" + base.player.name + "] deck");
		}
		else if (entity.owner.data.inventory.reserve.RemoveWhere((CardData a) => entity.data.id == a.id))
		{
			Debug.Log("[" + entity.data.name + "] Removed From [" + base.player.name + "] reserve");
		}

		int num = base.data.Get<int>("canEat") - 1;
		base.data["canEat"] = num;
		if (num <= 0)
		{
			PromptCloseGrid();
			talker.Say("full", 0f);
		}
		else
		{
			int num2 = base.data.Get<int>("thankCount") + 1;
			base.data["thankCount"] = num2;
			if (num2 < 1)
			{
				talker.Say("thanks", 0f);
			}
		}

		entity.RemoveFromContainers();
		if (num > 0)
		{
			cardContainer.TweenChildPositions();
		}

		CardManager.ReturnToPool(entity);
		muncherAnimator.SetTrigger("Munch");
		Events.InvokeScreenShake(0.5f, 0f);
		munchParticles.Play();
	}
}
