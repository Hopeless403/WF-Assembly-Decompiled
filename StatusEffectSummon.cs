#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(menuName = "Status Effects/Summon", fileName = "Summon")]
public class StatusEffectSummon : StatusEffectData
{
	public CardData summonCard;

	public StatusEffectData gainTrait;

	public CardType setCardType;

	[SerializeField]
	public AssetReference effectPrefabRef;

	public bool unsubRequired;

	public CardSlot[] toSummon;

	public override void Init()
	{
		if (target.data.playOnSlot)
		{
			Events.OnActionPerform += ActionPerform;
			unsubRequired = true;
		}

		base.OnCardPlayed += CardPlayed;
	}

	public void OnDestroy()
	{
		if (unsubRequired)
		{
			Events.OnActionPerform -= ActionPerform;
		}
	}

	public void ActionPerform(PlayAction action)
	{
		if (target.silenced || !(action is ActionTriggerAgainst actionTriggerAgainst) || !actionTriggerAgainst.targetContainer || !(actionTriggerAgainst.entity == target))
		{
			return;
		}

		CardContainer targetContainer = actionTriggerAgainst.targetContainer;
		if (!(targetContainer is CardSlot cardSlot))
		{
			if (targetContainer is CardSlotLane row)
			{
				toSummon = target.targetMode.GetTargetSlots(row);
			}
		}
		else
		{
			toSummon = new CardSlot[1] { cardSlot };
		}
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target)
		{
			return !target.silenced;
		}

		return false;
	}

	public IEnumerator CardPlayed(Entity entity, Entity[] targets)
	{
		if (toSummon == null)
		{
			HashSet<CardContainer> hashSet = new HashSet<CardContainer>();
			hashSet.AddRange(entity.actualContainers);
			if (hashSet.Count > 0 && hashSet.ToArray().RandomItem() is CardSlot cardSlot)
			{
				toSummon = new CardSlot[1] { cardSlot };
			}
		}

		if (toSummon != null && toSummon.Length != 0)
		{
			CardController controller = target.display.hover.controller;
			target.curveAnimator.Ping();
			CardSlot[] array = toSummon;
			foreach (CardSlot container in array)
			{
				yield return TrySummon(container, controller, target);
			}
		}

		toSummon = null;
		yield return null;
	}

	public IEnumerator Summon(CardContainer container, CardController controller, Entity summonedBy, StatusEffectData[] withEffects = null, int withEffectsAmount = 0, UnityAction<Entity> onComplete = null)
	{
		if (!container)
		{
			yield break;
		}

		Entity entity = null;
		yield return CreateCard(summonCard, container, controller, delegate(Entity e)
		{
			entity = e;
		});
		if (withEffectsAmount > 0 && withEffects != null)
		{
			foreach (StatusEffectData effectData in withEffects)
			{
				yield return StatusEffectSystem.Apply(entity, null, effectData, withEffectsAmount);
			}
		}

		if ((bool)gainTrait)
		{
			ActionQueue.Stack(new ActionSequence(Animate(entity, new CardData.StatusEffectStacks(gainTrait, 1))), fixedPosition: true);
		}
		else
		{
			ActionQueue.Stack(new ActionSequence(Animate(entity)), fixedPosition: true);
		}

		ActionQueue.Stack(new ActionSequence(ShoveIfNecessary(entity, container))
		{
			note = "Shove If Necessary"
		}, fixedPosition: true);
		ActionQueue.Stack(new ActionRunEnableEvent(entity), fixedPosition: true);
		ActionQueue.Stack(new ActionMove(entity, container), fixedPosition: true);
		Events.InvokeEntitySummoned(entity, summonedBy);
		onComplete?.Invoke(entity);
	}

	public IEnumerator SummonCopy(Entity toCopy, CardContainer container, CardController controller, Entity summonedBy, StatusEffectData[] withEffects = null, int withEffectsAmount = 0, UnityAction<Entity> onComplete = null)
	{
		Entity entity = null;
		yield return CreateCard(toCopy.data, container, controller, delegate(Entity e)
		{
			entity = e;
			e.startingEffectsApplied = true;
		});
		yield return CopyStatsAndEffects(entity, toCopy);
		yield return SummonPreMade(entity, container, controller, summonedBy, withEffects, withEffectsAmount, onComplete);
	}

	public IEnumerator SummonPreMade(Entity preMade, CardContainer container, CardController controller, Entity summonedBy, StatusEffectData[] withEffects = null, int withEffectsAmount = 0, UnityAction<Entity> onComplete = null)
	{
		if (withEffectsAmount > 0 && withEffects != null)
		{
			foreach (StatusEffectData effectData in withEffects)
			{
				yield return StatusEffectSystem.Apply(preMade, null, effectData, withEffectsAmount);
			}
		}

		if ((bool)gainTrait)
		{
			yield return Animate(preMade, new CardData.StatusEffectStacks(gainTrait, 1));
		}
		else
		{
			yield return Animate(preMade);
		}

		Events.InvokeEntitySummoned(preMade, summonedBy);
		onComplete?.Invoke(preMade);
		yield return ShoveIfNecessary(preMade, container);
		yield return new ActionRunEnableEvent(preMade).Run();
		yield return new ActionMove(preMade, container).Run();
	}

	public static IEnumerator ShoveIfNecessary(Entity entity, CardContainer container)
	{
		if (container is CardSlot cardSlot && !cardSlot.Empty && ShoveSystem.CanShove(cardSlot.GetTop(), entity, out var shoveData) && shoveData != null)
		{
			yield return ShoveSystem.DoShove(shoveData, updatePositions: true);
		}
	}

	public IEnumerator CopyStatsAndEffects(Entity entity, Entity toCopy)
	{
		toCopy.data.TryGetCustomData("splitOriginalId", out var value, toCopy.data.id);
		entity.data.SetCustomData("splitOriginalId", value);
		List<StatusEffectData> list = toCopy.statusEffects.Where((StatusEffectData e) => e.count > e.temporary && !e.IsNegativeStatusEffect() && (e.HasDescOrIsKeyword || e.isStatus)).ToList();
		foreach (Entity.TraitStacks trait in toCopy.traits)
		{
			foreach (StatusEffectData passiveEffect in trait.passiveEffects)
			{
				list.Remove(passiveEffect);
			}

			int num = trait.count - trait.tempCount;
			if (num > 0)
			{
				entity.GainTrait(trait.data, num);
			}
		}

		foreach (StatusEffectData item in list)
		{
			yield return StatusEffectSystem.Apply(entity, item.applier, item, item.count - item.temporary);
		}

		entity.attackEffects = (from a in CardData.StatusEffectStacks.Stack(entity.attackEffects, toCopy.attackEffects)
			select a.Clone()).ToList();
		entity.damage = toCopy.damage;
		entity.hp = toCopy.hp;
		entity.counter = toCopy.counter;
		entity.counter.current = entity.counter.max;
		entity.uses = toCopy.uses;
		entity.display.promptUpdateDescription = true;
		entity.PromptUpdate();
		yield return entity.UpdateTraits();
	}

	public IEnumerator Animate(Entity entity, params CardData.StatusEffectStacks[] withEffects)
	{
		AsyncOperationHandle<GameObject> handle = effectPrefabRef.InstantiateAsync(entity.transform);
		yield return handle;
		CreateCardAnimation component = handle.Result.GetComponent<CreateCardAnimation>();
		if ((object)component != null)
		{
			yield return component.Run(entity, withEffects);
		}
	}

	public Card CreateCardCopy(CardData cardData, CardContainer container, CardController controller)
	{
		CardData cardData2 = cardData.Clone(runCreateScripts: false);
		if ((bool)setCardType)
		{
			cardData2.cardType = setCardType;
		}

		cardData2.upgrades.RemoveAll((CardUpgradeData a) => a.type == CardUpgradeData.Type.Crown);
		Card card = CardManager.Get(cardData2, controller, container.owner, inPlay: true, container.owner.team == References.Player.team);
		card.entity.flipper.FlipUpInstant();
		card.canvasGroup.alpha = 0f;
		container.Add(card.entity);
		Transform transform = card.transform;
		transform.localPosition = card.entity.GetContainerLocalPosition();
		transform.localEulerAngles = card.entity.GetContainerLocalRotation();
		transform.localScale = card.entity.GetContainerScale();
		container.Remove(card.entity);
		card.entity.owner.reserveContainer.Add(card.entity);
		return card;
	}

	public IEnumerator CreateCard(CardData cardData, CardContainer container, CardController controller, UnityAction<Entity> onComplete = null)
	{
		Card card = CreateCardCopy(cardData, container, controller);
		onComplete?.Invoke(card.entity);
		yield return card.UpdateData();
	}

	public IEnumerator TrySummon(CardContainer container, CardController controller, Entity summonedBy)
	{
		Dictionary<Entity, List<CardSlot>> shoveData;
		if (container.Count < container.max)
		{
			yield return Summon(container, controller, summonedBy);
		}
		else if (ShoveSystem.CanShove(container.GetTop(), target, out shoveData))
		{
			yield return ShoveSystem.DoShove(shoveData, updatePositions: true);
			yield return Summon(container, controller, summonedBy);
		}

		else if (NoTargetTextSystem.Exists())
		{
			yield return NoTargetTextSystem.Run(target, NoTargetType.NoSpaceToSummon);
		}
	}
}
