#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using Dead;
using UnityEngine;

public class ActionChangePhase : PlayAction
{
	public readonly Entity entity;

	public readonly CardData newPhase;

	public readonly CardData[] newPhases;

	public readonly CardAnimation animation;

	public List<Entity> newCards;

	public bool loadingNewCards;

	public ActionChangePhase(Entity entity, CardData newPhase, CardAnimation animation)
	{
		this.entity = entity;
		this.newPhase = newPhase;
		this.animation = animation;
	}

	public ActionChangePhase(Entity entity, CardData[] newPhases, CardAnimation animation)
	{
		this.entity = entity;
		this.newPhases = newPhases;
		this.animation = animation;
	}

	public override IEnumerator Run()
	{
		if (!entity.IsAliveAndExists())
		{
			yield break;
		}

		Events.InvokeEntityChangePhase(entity);
		CardData[] array = newPhases;
		bool multipleNewPhases = array != null && array.Length > 0;
		if (multipleNewPhases)
		{
			new Routine(CreateNewCards());
		}

		PauseMenu.Block();
		DeckpackBlocker.Block();
		if (Deckpack.IsOpen && References.Player.entity.display is CharacterDisplay characterDisplay)
		{
			characterDisplay.CloseInventory();
		}

		ChangePhaseAnimationSystem animationSystem = Object.FindObjectOfType<ChangePhaseAnimationSystem>();
		if ((bool)animationSystem)
		{
			yield return animationSystem.Focus(entity);
		}

		if ((bool)animation)
		{
			yield return animation.Routine(entity);
		}

		PlayAction[] actions = ActionQueue.GetActions();
		foreach (PlayAction playAction in actions)
		{
			if (!(playAction is ActionTrigger actionTrigger))
			{
				if (playAction is ActionEffectApply actionEffectApply)
				{
					actionEffectApply.TryRemoveEntity(entity);
				}
			}
			else if (actionTrigger.entity == entity)
			{
				ActionQueue.Remove(playAction);
			}
		}

		if (multipleNewPhases)
		{
			ActionQueue.Stack(new ActionSequence(Split(entity, newPhases))
			{
				note = "Split boss",
				priority = 10
			}, fixedPosition: true);
		}
		else
		{
			ActionQueue.Stack(new ActionSequence(Change(entity, newPhase))
			{
				note = "Change boss phase",
				priority = 10
			}, fixedPosition: true);
		}

		if ((bool)animationSystem)
		{
			ActionQueue.Stack(new ActionSequence(animationSystem.UnFocus())
			{
				note = "Unfocus boss",
				priority = 10
			}, fixedPosition: true);
		}
	}

	public static IEnumerator Change(Entity entity, CardData newData)
	{
		entity.alive = false;
		yield return entity.ClearStatuses();
		entity.data = newData;
		yield return entity.display.UpdateData(doPing: true);
		entity.alive = true;
		yield return StatusEffectSystem.EntityEnableEvent(entity);
	}

	public IEnumerator Split(Entity entity, IEnumerable<CardData> split)
	{
		entity.alive = false;
		while (loadingNewCards)
		{
			yield return null;
		}

		int num = 0;
		int count = entity.actualContainers.Count;
		Dictionary<CardContainer, List<Entity>> dictionary = new Dictionary<CardContainer, List<Entity>>();
		foreach (Entity newCard in newCards)
		{
			int num2 = num % count;
			CardContainer key = entity.actualContainers[num2];
			if (dictionary.ContainsKey(key))
			{
				CardContainer key2 = entity.containers[num2];
				if (dictionary.ContainsKey(key2))
				{
					dictionary[key2].Add(newCard);
				}
				else
				{
					dictionary[key2] = new List<Entity> { newCard };
				}
			}
			else
			{
				dictionary[key] = new List<Entity> { newCard };
			}

			num++;
		}

		Vector3 position = entity.transform.position;
		entity.RemoveFromContainers();
		CardManager.ReturnToPool(entity);
		foreach (var (cardContainer2, list2) in dictionary)
		{
			if (list2 == null)
			{
				continue;
			}

			foreach (Entity item in list2)
			{
				cardContainer2.Add(item);
				Transform transform = item.transform;
				transform.localScale = item.GetContainerScale();
				Vector3 containerWorldPosition = item.GetContainerWorldPosition();
				transform.position = Vector3.Lerp(position, containerWorldPosition, 0.1f);
				LeanTween.move(item.gameObject, containerWorldPosition, PettyRandom.Range(0.8f, 1.2f)).setEaseOutElastic();
				item.wobbler.WobbleRandom();
			}
		}

		ChangePhaseAnimationSystem changePhaseAnimationSystem = Object.FindObjectOfType<ChangePhaseAnimationSystem>();
		MinibossIntroSystem minibossIntroSystem = Object.FindObjectOfType<MinibossIntroSystem>();
		foreach (Entity newCard2 in newCards)
		{
			if ((bool)changePhaseAnimationSystem)
			{
				changePhaseAnimationSystem.RemoveTarget(entity);
				changePhaseAnimationSystem.Assign(newCard2);
			}

			if ((bool)minibossIntroSystem)
			{
				minibossIntroSystem.Ignore(newCard2);
			}
		}

		ActionQueue.Stack(new ActionSequence(FinalSplit(dictionary))
		{
			note = "Final boss split",
			priority = 10
		}, fixedPosition: true);
	}

	public static IEnumerator FinalSplit(Dictionary<CardContainer, List<Entity>> toMove)
	{
		foreach (KeyValuePair<CardContainer, List<Entity>> item in toMove)
		{
			foreach (Entity item2 in item.Value)
			{
				item2.enabled = true;
				item2.RemoveFromContainers();
				item2.owner.reserveContainer.Add(item2);
				ActionQueue.Stack(new ActionMove(item2, item.Key)
				{
					priority = 10
				}, fixedPosition: true);
				ActionQueue.Stack(new ActionRunEnableEvent(item2)
				{
					priority = 10
				}, fixedPosition: true);
			}
		}

		yield return null;
	}

	public static IEnumerator EnableBehaviour(Behaviour system)
	{
		system.enabled = true;
		yield return null;
	}

	public IEnumerator CreateNewCards()
	{
		loadingNewCards = true;
		newCards = new List<Entity>();
		CardController controller = entity.display.hover.controller;
		Character owner = entity.owner;
		Routine.Clump clump = new Routine.Clump();
		CardData[] array = newPhases;
		for (int i = 0; i < array.Length; i++)
		{
			Card card = CardManager.Get(array[i], controller, owner, true, owner.team == References.Player.team);
			newCards.Add(card.entity);
			clump.Add(card.UpdateData());
		}

		yield return clump.WaitForEnd();
		loadingNewCards = false;
	}
}
