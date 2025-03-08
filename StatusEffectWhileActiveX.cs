#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/While Active X", fileName = "While Active X")]
public class StatusEffectWhileActiveX : StatusEffectApplyX
{
	[SerializeField]
	public bool ifCloneAffectOriginal = true;

	[SerializeField]
	public bool affectOthersWithSameEffect = true;

	public bool active;

	public int currentAmount;

	public readonly List<Entity> affected = new List<Entity>();

	public bool pingDone;

	public readonly List<CardContainer> containersToAffect = new List<CardContainer>();

	public bool affectsSelf;

	public bool targetIsClone;

	public ulong cloneOriginalId;

	public bool AffectsRow()
	{
		if (!applyToFlags.HasFlag(ApplyToFlags.AlliesInRow) && !applyToFlags.HasFlag(ApplyToFlags.EnemiesInRow) && !applyToFlags.HasFlag(ApplyToFlags.FrontAlly) && !applyToFlags.HasFlag(ApplyToFlags.BackAlly))
		{
			return applyToFlags.HasFlag(ApplyToFlags.FrontEnemy);
		}

		return true;
	}

	public bool AffectsSlot()
	{
		if (!applyToFlags.HasFlag(ApplyToFlags.AllyInFrontOf) && !applyToFlags.HasFlag(ApplyToFlags.AllyBehind) && !applyToFlags.HasFlag(ApplyToFlags.FrontAlly))
		{
			return applyToFlags.HasFlag(ApplyToFlags.BackAlly);
		}

		return true;
	}

	public override void Init()
	{
		base.OnBegin += Begin;
		base.OnEnable += Enable;
		base.OnDisable += Disable;
		base.OnCardMove += CardMove;
		base.OnEffectBonusChanged += EffectBonusChanged;
		Events.OnEntityDataUpdated += EntityDataUpdated;
		Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
		pingDone = !doPing;
		targetIsClone = target.data.TryGetCustomData("splitOriginalId", out cloneOriginalId, 0uL);
	}

	public virtual void OnDestroy()
	{
		Events.OnEntityDataUpdated -= EntityDataUpdated;
		Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		int num = affected.IndexOf(entity);
		if (num >= 0)
		{
			affected.RemoveAt(num);
		}

		return false;
	}

	public void EntityDataUpdated(Entity entity)
	{
		if (active && affected.Contains(entity))
		{
			ActionQueue.Add(new ActionSequence(ReAffect(entity)));
		}
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		if (active && entity == target && GetAmount(target) != currentAmount)
		{
			ActionQueue.Add(new ActionRefreshWhileActiveEffect(this));
		}
	}

	public virtual bool CanActivate()
	{
		return Battle.IsOnBoard(target);
	}

	public virtual bool CheckActivateOnMove(CardContainer[] fromContainers, CardContainer[] toContainers)
	{
		if (Battle.IsOnBoard(toContainers))
		{
			return !Battle.IsOnBoard(fromContainers);
		}

		return false;
	}

	public virtual bool CheckDeactivateOnMove(CardContainer[] fromContainers, CardContainer[] toContainers)
	{
		if (!Battle.IsOnBoard(toContainers))
		{
			return Battle.IsOnBoard(fromContainers);
		}

		return false;
	}

	public static bool CompareContainerArrays(IReadOnlyList<CardContainer> a, IReadOnlyList<CardContainer> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}

		for (int i = 0; i < a.Count; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}

		return true;
	}

	public override bool RunBeginEvent()
	{
		if (target.enabled)
		{
			return CanActivate();
		}

		return false;
	}

	public IEnumerator Begin()
	{
		return Activate();
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (!active)
		{
			if (entity == target)
			{
				return CanActivate();
			}

			return false;
		}

		return false;
	}

	public IEnumerator Enable(Entity entity)
	{
		return Activate();
	}

	public override bool RunDisableEvent(Entity entity)
	{
		return entity == target;
	}

	public virtual IEnumerator Disable(Entity entity)
	{
		return Deactivate();
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (target.enabled)
		{
			if (!(target == entity))
			{
				return active;
			}

			return true;
		}

		return false;
	}

	public virtual IEnumerator CardMove(Entity entity)
	{
		if (target == entity)
		{
			CardContainer[] preContainers = entity.preContainers;
			if (active)
			{
				if (CheckDeactivateOnMove(preContainers, entity.containers))
				{
					yield return Deactivate();
				}
				else if (AffectsRow())
				{
					if (!CompareContainerArrays(preContainers, entity.containers))
					{
						yield return Deactivate();
						yield return Activate();
					}
				}

				else if (AffectsSlot())
				{
					yield return Deactivate();
					yield return Activate();
				}
			}
			else if (CheckActivateOnMove(preContainers, entity.containers))
			{
				yield return Activate();
			}
		}
		else
		{
			if (!active)
			{
				yield break;
			}

			if (AffectsSlot())
			{
				CardContainer[] other = containersToAffect.Select((CardContainer a) => a.Group).ToArray();
				if (entity.containers.ContainsAny(other) || entity.preContainers.ContainsAny(other))
				{
					yield return Deactivate();
					yield return Activate();
				}
			}
			else if (affected.Contains(entity))
			{
				if (!containersToAffect.ContainsAny(entity.containers))
				{
					yield return UnAffect(entity);
				}
			}

			else if (containersToAffect.ContainsAny(entity.containers))
			{
				yield return Affect(entity);
			}
		}
	}

	public override bool RunEffectBonusChangedEvent()
	{
		if (target.enabled)
		{
			return active;
		}

		return false;
	}

	public IEnumerator EffectBonusChanged()
	{
		ActionQueue.Add(new ActionRefreshWhileActiveEffect(this));
		yield break;
	}

	public IEnumerator Activate()
	{
		active = true;
		if (!pingDone)
		{
			target.curveAnimator?.Ping();
			pingDone = true;
		}

		currentAmount = GetAmount(target);
		FindContainers();
		List<Entity> list = new List<Entity>();
		if (affectsSelf)
		{
			list.Add(target);
		}

		foreach (CardContainer item in containersToAffect)
		{
			foreach (Entity item2 in item)
			{
				if (!list.Contains(item2) && item2 != target)
				{
					list.Add(item2);
				}
			}
		}

		Routine.Clump clump = new Routine.Clump();
		foreach (Entity item3 in list)
		{
			clump.Add(Affect(item3));
		}

		yield return clump.WaitForEnd();
	}

	public IEnumerator Deactivate()
	{
		active = false;
		Routine.Clump clump = new Routine.Clump();
		foreach (Entity item in affected)
		{
			clump.Add(UnAffect(item));
		}

		yield return clump.WaitForEnd();
		affected.Clear();
	}

	public void FindContainers()
	{
		containersToAffect.Clear();
		Character opponent = Battle.GetOpponent(target.owner);
		int[] rowIndices = Battle.instance.GetRowIndices(target);
		affectsSelf = AppliesTo(ApplyToFlags.Self);
		if (AppliesTo(ApplyToFlags.Allies))
		{
			containersToAffect.AddRange(Battle.instance.GetRows(target.owner));
		}
		else if (AppliesTo(ApplyToFlags.AlliesInRow))
		{
			int[] array = rowIndices;
			foreach (int rowIndex in array)
			{
				containersToAffect.Add(Battle.instance.GetRow(target.owner, rowIndex));
			}
		}
		else
		{
			if (AppliesTo(ApplyToFlags.FrontAlly))
			{
				int[] array = rowIndices;
				foreach (int rowIndex2 in array)
				{
					if (Battle.instance.GetRow(target.owner, rowIndex2) is CardSlotLane cardSlotLane)
					{
						CardSlot value = cardSlotLane.slots.FirstOrDefault((CardSlot a) => !a.Empty);
						containersToAffect.AddIfNotNull(value);
					}
				}
			}

			if (AppliesTo(ApplyToFlags.BackAlly))
			{
				int[] array = rowIndices;
				foreach (int rowIndex3 in array)
				{
					if (Battle.instance.GetRow(target.owner, rowIndex3) is CardSlotLane cardSlotLane2)
					{
						CardSlot value2 = cardSlotLane2.slots.LastOrDefault((CardSlot a) => !a.Empty);
						containersToAffect.AddIfNotNull(value2);
					}
				}
			}

			if (AppliesTo(ApplyToFlags.AllyInFrontOf))
			{
				int[] array = rowIndices;
				foreach (int rowIndex4 in array)
				{
					if (!(Battle.instance.GetRow(target.owner, rowIndex4) is CardSlotLane cardSlotLane3))
					{
						continue;
					}

					for (int num = cardSlotLane3.IndexOf(target) - 1; num >= 0; num--)
					{
						CardSlot cardSlot = cardSlotLane3.slots[num];
						if (!cardSlot.Empty)
						{
							containersToAffect.Add(cardSlot);
							break;
						}
					}
				}
			}

			if (AppliesTo(ApplyToFlags.AllyBehind))
			{
				int[] array = rowIndices;
				foreach (int rowIndex5 in array)
				{
					if (!(Battle.instance.GetRow(target.owner, rowIndex5) is CardSlotLane cardSlotLane4))
					{
						continue;
					}

					for (int j = cardSlotLane4.IndexOf(target) + 1; j < cardSlotLane4.slots.Count; j++)
					{
						CardSlot cardSlot2 = cardSlotLane4.slots[j];
						if (!cardSlot2.Empty)
						{
							containersToAffect.Add(cardSlot2);
							break;
						}
					}
				}
			}
		}

		if (AppliesTo(ApplyToFlags.Enemies))
		{
			containersToAffect.AddRange(Battle.instance.GetRows(opponent));
		}
		else if (AppliesTo(ApplyToFlags.EnemiesInRow))
		{
			int[] array = rowIndices;
			foreach (int rowIndex6 in array)
			{
				containersToAffect.Add(Battle.instance.GetRow(opponent, rowIndex6));
			}
		}

		if (AppliesTo(ApplyToFlags.Hand) && (bool)References.Player)
		{
			containersToAffect.AddIfNotNull(References.Player.handContainer);
		}

		if (AppliesTo(ApplyToFlags.EnemyHand) && (bool)opponent)
		{
			containersToAffect.AddIfNotNull(opponent.handContainer);
		}
	}

	public IEnumerator Affect(Entity entity)
	{
		if (affected.Contains(entity) || !effectToApply.CanPlayOn(entity))
		{
			yield break;
		}

		bool flag = false;
		if (targetIsClone && !ifCloneAffectOriginal)
		{
			if (entity.data.TryGetCustomData("splitOriginalId", out var value, 0uL) && cloneOriginalId == value)
			{
				flag = true;
			}
			else if (entity.data.id == cloneOriginalId)
			{
				flag = true;
			}
		}

		if (!flag && !affectOthersWithSameEffect)
		{
			foreach (StatusEffectData statusEffect in entity.statusEffects)
			{
				if (statusEffect.name == base.name)
				{
					flag = true;
					break;
				}
			}
		}

		if (!flag && applyConstraints.All((TargetConstraint c) => c.Check(entity)))
		{
			affected.Add(entity);
			if (currentAmount > 0)
			{
				yield return StatusEffectSystem.Apply(entity, target, effectToApply, currentAmount, temporary: true);
				entity.PromptUpdate();
			}
		}
	}

	public IEnumerator UnAffect(Entity entity)
	{
		if (!affected.Contains(entity))
		{
			yield break;
		}

		for (int num = entity.statusEffects.Count - 1; num >= 0; num--)
		{
			StatusEffectData statusEffectData = entity.statusEffects[num];
			if ((bool)statusEffectData && statusEffectData.name == effectToApply.name)
			{
				yield return statusEffectData.RemoveStacks(currentAmount, removeTemporary: true);
				break;
			}
		}

		affected.Remove(entity);
	}

	public IEnumerator ReAffect(Entity entity)
	{
		if (affected.Contains(entity))
		{
			yield return UnAffect(entity);
			yield return Affect(entity);
		}
	}
}
