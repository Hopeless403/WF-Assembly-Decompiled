#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Status Effects/Instant/Split", fileName = "Split")]
public class StatusEffectInstantSplit : StatusEffectInstant
{
	[Serializable]
	public struct Profile
	{
		public string cardName;

		public string changeToCardName;
	}

	[SerializeField]
	public Profile[] profiles;

	public readonly List<Entity> newCards = new List<Entity>();

	public bool CanSplit()
	{
		if (target.height > 1)
		{
			return true;
		}

		foreach (CardContainer item in References.Battle.rows[target.owner])
		{
			if (item.Count < item.max)
			{
				return true;
			}
		}

		return false;
	}

	public override IEnumerator Process()
	{
		if (CanSplit())
		{
			yield return (target.height > 1) ? SplitDoubleHeight() : Split();
		}
		else if (NoTargetTextSystem.Exists())
		{
			yield return NoTargetTextSystem.Run(target, NoTargetType.CantSplit);
		}

		yield return base.Process();
	}

	public IEnumerator Split()
	{
		target.hp.current = Mathf.CeilToInt((float)target.hp.current * 0.5f);
		target.hp.max = target.hp.current;
		target.data.value = Mathf.CeilToInt((float)target.data.value * 0.5f);
		target.ResetWhenHealthLostEffects();
		List<StatusEffectApply> statusesToApply = (from e in target.statusEffects
			where e.count > e.temporary && e.isStatus
			select new StatusEffectApply(e.applier, null, e.GetOriginal(), Mathf.CeilToInt((float)(e.count - e.temporary) * 0.5f))).ToList();
		Routine.Clump clump = new Routine.Clump();
		foreach (StatusEffectData item in target.statusEffects.Where((StatusEffectData e) => e.isStatus && e.count > e.temporary))
		{
			int num = item.count - item.temporary;
			if (num > 0)
			{
				clump.Add(item.RemoveStacks(num, removeTemporary: false));
			}
		}

		yield return clump.WaitForEnd();
		yield return CreateNewCards(1);
		Entity newCard = newCards[0];
		foreach (StatusEffectApply item2 in statusesToApply)
		{
			clump.Add(StatusEffectSystem.Apply(newCard, item2.applier, item2.effectData, item2.count, temporary: false, null, fireEvents: false));
			clump.Add(StatusEffectSystem.Apply(target, item2.applier, item2.effectData, item2.count, temporary: false, null, fireEvents: false));
		}

		yield return clump.WaitForEnd();
		if (ShoveSystem.CanShove(target, target.owner.entity, out var shoveData))
		{
			CardContainer toContainer = target.actualContainers[0];
			yield return ShoveSystem.DoShove(shoveData, updatePositions: true);
			toContainer.Add(newCard);
			Transform transform = newCard.transform;
			Vector3 containerWorldPosition = newCard.GetContainerWorldPosition();
			transform.position = containerWorldPosition;
			newCard.wobbler.WobbleRandom();
			target.wobbler.WobbleRandom();
		}

		yield return EnableCards(newCards);
	}

	public IEnumerator SplitDoubleHeight()
	{
		target.hp.current = Mathf.CeilToInt((float)target.hp.current * 0.5f);
		target.hp.max = target.hp.current;
		target.data.value = Mathf.CeilToInt((float)target.data.value * 0.5f);
		yield return CreateNewCards(2);
		List<StatusEffectData> list = target.statusEffects.Where((StatusEffectData e) => e.count > e.temporary && e.isStatus).ToList();
		Routine.Clump clump = new Routine.Clump();
		foreach (StatusEffectData item in list)
		{
			int num = Mathf.CeilToInt((float)(item.count - item.temporary) * 0.5f);
			foreach (Entity newCard in newCards)
			{
				clump.Add(StatusEffectSystem.Apply(newCard, item.applier, item, num, temporary: false, null, fireEvents: false));
			}
		}

		yield return clump.WaitForEnd();
		Vector3 position = target.transform.position;
		for (int i = 0; i < newCards.Count; i++)
		{
			Entity entity = newCards[i];
			target.actualContainers[i % target.actualContainers.Count].Add(entity);
			Transform transform = entity.transform;
			Vector3 containerWorldPosition = entity.GetContainerWorldPosition();
			transform.position = Vector3.Lerp(position, containerWorldPosition, 0.1f);
			LeanTween.move(entity.gameObject, containerWorldPosition, PettyRandom.Range(0.8f, 1.2f)).setEaseOutElastic();
			entity.wobbler.WobbleRandom();
		}

		PlayAction[] actions = ActionQueue.GetActions();
		foreach (PlayAction obj in actions)
		{
			if (obj is ActionTriggerAgainst actionTriggerAgainst)
			{
				int[] rows = References.Battle.GetRowIndices(actionTriggerAgainst.entity);
				Entity[] array = newCards.Where((Entity a) => References.Battle.GetRowIndices(a).ContainsAny(rows)).ToArray();
				actionTriggerAgainst.target = ((array.Length != 0) ? array.RandomItem() : newCards.RandomItem());
			}
		}

		target.RemoveFromContainers();
		CardManager.ReturnToPool(target);
		yield return EnableCards(newCards);
	}

	public IEnumerator EnableCards(IEnumerable<Entity> cards)
	{
		MinibossIntroSystem minibossIntroSystem = UnityEngine.Object.FindObjectOfType<MinibossIntroSystem>();
		foreach (Entity card in cards)
		{
			if ((bool)minibossIntroSystem)
			{
				minibossIntroSystem.Ignore(card);
			}

			CardContainer[] toContainers = card.actualContainers.ToArray();
			card.enabled = true;
			card.RemoveFromContainers();
			card.owner.reserveContainer.Add(card);
			yield return new ActionMove(card, toContainers).Run();
			ActionQueue.Stack(new ActionRunEnableEvent(card)
			{
				priority = eventPriority
			}, fixedPosition: true);
		}
	}

	public IEnumerator CreateNewCards(int count)
	{
		CardController controller = target.display.hover.controller;
		Character owner = target.owner;
		Routine.Clump clump = new Routine.Clump();
		for (int i = 0; i < count; i++)
		{
			CardData cardData = target.data;
			Profile[] array = profiles;
			for (int j = 0; j < array.Length; j++)
			{
				Profile profile = array[j];
				if (profile.cardName == cardData.name)
				{
					cardData = AddressableLoader.Get<CardData>("CardData", profile.changeToCardName);
					break;
				}
			}

			clump.Add(CreateCard(target.data, cardData, controller, owner, delegate(Card c)
			{
				newCards.Add(c.entity);
			}));
		}

		yield return clump.WaitForEnd();
		foreach (Entity newCard in newCards)
		{
			clump.Add(CopyEntity(newCard, target));
		}

		yield return clump.WaitForEnd();
	}

	public static IEnumerator CreateCard(CardData original, CardData cardData, CardController controller, Character owner, UnityAction<Card> onComplete)
	{
		CardData cardData2 = cardData.Clone();
		original.TryGetCustomData("splitOriginalId", out var value, original.id);
		cardData2.SetCustomData("splitOriginalId", value);
		Card card = CardManager.Get(cardData2, controller, owner, inPlay: true, owner.team == References.Player.team);
		card.entity.startingEffectsApplied = true;
		onComplete?.Invoke(card);
		yield return card.UpdateData();
	}

	public IEnumerator CopyEntity(Entity entity, Entity toCopy)
	{
		List<StatusEffectData> list = toCopy.statusEffects.Where((StatusEffectData e) => e.count > e.temporary && e.HasDescOrIsKeyword && e != this).ToList();
		foreach (StatusEffectData item in list)
		{
			yield return StatusEffectSystem.Apply(entity, item.applier, item, item.count - item.temporary, temporary: false, null, fireEvents: false);
		}

		foreach (Entity.TraitStacks trait in toCopy.traits)
		{
			int num = trait.count - trait.tempCount;
			if (num > 0)
			{
				entity.GainTrait(trait.data, num);
			}
		}

		entity.attackEffects = (from a in CardData.StatusEffectStacks.Stack(entity.attackEffects, toCopy.attackEffects)
			select a.Clone()).ToList();
		entity.hp = toCopy.hp;
		entity.damage = toCopy.damage;
		entity.counter = toCopy.counter;
		if (entity.counter.current <= 0)
		{
			entity.counter.Max();
		}

		entity.uses = toCopy.uses;
		entity.display.promptUpdateDescription = true;
		entity.ResetWhenHealthLostEffects();
		entity.PromptUpdate();
		yield return entity.UpdateTraits();
	}
}
