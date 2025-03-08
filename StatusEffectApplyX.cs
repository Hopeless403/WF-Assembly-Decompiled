#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public abstract class StatusEffectApplyX : StatusEffectData
{
	[Flags]
	public enum ApplyToFlags
	{
		None = 0,
		Self = 1,
		Hand = 2,
		EnemyHand = 4,
		Allies = 8,
		AlliesInRow = 0x10,
		FrontAlly = 0x20,
		BackAlly = 0x40,
		AllyInFrontOf = 0x80,
		AllyBehind = 0x100,
		Enemies = 0x200,
		EnemiesInRow = 0x400,
		FrontEnemy = 0x800,
		Attacker = 0x1000,
		Target = 0x2000,
		RandomAlly = 0x4000,
		RandomEnemy = 0x8000,
		RandomUnit = 0x10000,
		Applier = 0x20000,
		RightCardInHand = 0x40000,
		RandomCardInHand = 0x80000,
		RandomAllyInRow = 0x100000,
		RandomEnemyInRow = 0x200000
	}

	[Header("Status Effect To Apply")]
	public bool dealDamage;

	[HideIf("dealDamage")]
	public StatusEffectData effectToApply;

	[SerializeField]
	public SelectScript<Entity> selectScript;

	public TargetConstraint[] applyConstraints;

	public bool applyEqualAmount;

	[ShowIf("applyEqualAmount")]
	[SerializeField]
	public ScriptableAmount contextEqualAmount;

	[ShowIf("applyEqualAmount")]
	[SerializeField]
	public float equalAmountBonusMult;

	[HideIf("applyEqualAmount")]
	[SerializeField]
	public ScriptableAmount scriptableAmount;

	[Header("Who/What To Apply To")]
	public ApplyToFlags applyToFlags;

	public bool applyInReverseOrder;

	public bool canRetaliate;

	public bool countsAsHit;

	public bool waitForAnimationEnd;

	public bool waitForApplyToAnimationEnd;

	public bool queue;

	public bool separateActions;

	public bool doPing = true;

	public bool targetMustBeAlive = true;

	public float pauseAfter;

	[SerializeField]
	public NoTargetType noTargetType;

	[SerializeField]
	public string[] noTargetTypeArgs;

	public ActionEffectApply action;

	public override bool CanTrigger()
	{
		if (!base.CanTrigger() || !dealDamage)
		{
			return effectToApply;
		}

		return true;
	}

	public virtual int GetAmount(Entity entity, bool equalAmount = false, int equalTo = 0)
	{
		if ((bool)target && !TargetSilenced())
		{
			if (!scriptableAmount)
			{
				if (!equalAmount)
				{
					return GetAmount();
				}

				return equalTo;
			}

			return scriptableAmount.Get(entity);
		}

		return 0;
	}

	public virtual bool TargetSilenced()
	{
		return target.silenced;
	}

	public bool AppliesTo(ApplyToFlags applyTo)
	{
		return (applyToFlags & applyTo) != 0;
	}

	public IEnumerator Run(List<Entity> targets, int amount = 0)
	{
		if (!target || !CanTrigger())
		{
			yield break;
		}

		for (int num = targets.Count - 1; num >= 0; num--)
		{
			Entity entity = targets[num];
			if (!CanAffect(entity))
			{
				targets.RemoveAt(num);
			}
		}

		TargetConstraint[] array = applyConstraints;
		foreach (TargetConstraint targetConstraint in array)
		{
			for (int num2 = targets.Count - 1; num2 >= 0; num2--)
			{
				Entity entity2 = targets[num2];
				if (!targetConstraint.Check(entity2))
				{
					targets.RemoveAt(num2);
				}
			}
		}

		if (targets.Count > 0)
		{
			int amount2 = GetAmount(target, applyEqualAmount, Mathf.RoundToInt((float)amount * (1f + equalAmountBonusMult)));
			if (TargetSilenced() || (amount2 <= 0 && !CheckMultipleEffectsForUnboostable()))
			{
				yield break;
			}

			if (queue && target.IsAliveAndExists())
			{
				bool flag = false;
				if (effectToApply.CanStackActions)
				{
					ActionEffectApply actionEffectApply = action;
					if (actionEffectApply != null && !actionEffectApply.running)
					{
						action.Stack(targets, amount2);
						flag = true;
					}
				}

				if (!flag)
				{
					action = new ActionEffectApply(this, targets, amount2)
					{
						note = base.name
					};
					ActionQueue.Stack(action, fixedPosition: true);
				}
			}
			else
			{
				yield return Sequence(targets, amount2);
			}
		}
		else if (noTargetType != 0 && NoTargetTextSystem.Exists())
		{
			Entity entity3 = target;
			NoTargetType num3 = noTargetType;
			object[] args = noTargetTypeArgs;
			yield return NoTargetTextSystem.Run(entity3, num3, args);
		}
	}

	public bool CheckMultipleEffectsForUnboostable()
	{
		if (effectToApply is StatusEffectInstantMultiple statusEffectInstantMultiple)
		{
			StatusEffectInstant[] effects = statusEffectInstantMultiple.effects;
			for (int i = 0; i < effects.Length; i++)
			{
				if (!effects[i].canBeBoosted)
				{
					return true;
				}
			}
		}

		return false;
	}

	public IEnumerator Sequence(List<Entity> targets, int amount)
	{
		if (!target || (!target.alive && targetMustBeAlive))
		{
			yield break;
		}

		if (waitForAnimationEnd)
		{
			yield return Sequences.WaitForAnimationEnd(target);
		}

		if (waitForApplyToAnimationEnd)
		{
			foreach (Entity target in targets)
			{
				yield return Sequences.WaitForAnimationEnd(target);
			}
		}

		if ((bool)selectScript)
		{
			targets = selectScript.Run(targets);
		}

		if (separateActions)
		{
			if (queue)
			{
				foreach (Entity item in targets.AsEnumerable().Reverse())
				{
					ActionQueue.Stack(new ActionSequence(SequenceSingle(item, amount))
					{
						note = base.name + " - " + item.name
					});
				}
			}
			else
			{
				foreach (Entity item2 in targets.AsEnumerable())
				{
					yield return SequenceSingle(item2, amount);
				}
			}
		}
		else
		{
			for (int num = targets.Count - 1; num >= 0; num--)
			{
				if (!targets[num] || !CheckConstraints(targets[num]))
				{
					targets.RemoveAt(num);
				}
			}

			if (targets.Count > 0)
			{
				Routine.Clump clump = new Routine.Clump();
				if (doPing && (bool)base.target && (bool)base.target.curveAnimator)
				{
					base.target.curveAnimator.Ping();
					clump.Add(Sequences.Wait(0.13f));
				}

				foreach (Entity item3 in targets.Where((Entity t) => t))
				{
					int damage = (dealDamage ? amount : 0);
					Hit hit = new Hit(base.target, item3, damage)
					{
						canRetaliate = canRetaliate,
						countsAsHit = countsAsHit
					};
					if (!dealDamage)
					{
						hit.AddStatusEffect(effectToApply, amount);
					}

					clump.Add(hit.Process());
				}

				yield return clump.WaitForEnd();
			}
		}

		if (pauseAfter > 0f)
		{
			yield return Sequences.Wait(pauseAfter);
		}
	}

	public IEnumerator SequenceSingle(Entity t, int amount)
	{
		if ((bool)t && CheckConstraints(t))
		{
			Routine.Clump clump = new Routine.Clump();
			if (doPing && (bool)target && (bool)target.curveAnimator)
			{
				target.curveAnimator.Ping();
				clump.Add(Sequences.Wait(0.13f));
			}

			Hit hit = new Hit(target, t, 0)
			{
				canRetaliate = canRetaliate,
				countsAsHit = countsAsHit
			};
			hit.AddStatusEffect(effectToApply, amount);
			clump.Add(hit.Process());
			yield return clump.WaitForEnd();
			if (waitForAnimationEnd)
			{
				yield return Sequences.WaitForAnimationEnd(target);
			}
		}
	}

	public List<Entity> GetTargets(Hit hit = null, CardContainer[] wasInRows = null, CardContainer[] wasInSlots = null, Entity[] targets = null)
	{
		List<Entity> list = new List<Entity>();
		if (AppliesTo(ApplyToFlags.Self) && CanAffect(target))
		{
			list.Add(target);
		}

		if (AppliesTo(ApplyToFlags.Hand))
		{
			CardContainer handContainer = References.Player.handContainer;
			if ((object)handContainer != null && handContainer.Count > 0)
			{
				list.AddRange(References.Player.handContainer.Where((Entity card) => card != target && CheckConstraints(card)));
				goto IL_011f;
			}
		}

		if (AppliesTo(ApplyToFlags.RightCardInHand))
		{
			CardContainer handContainer = References.Player.handContainer;
			if ((object)handContainer != null && handContainer.Count > 0)
			{
				list.Add(References.Player.handContainer[0]);
			}
		}

		if (AppliesTo(ApplyToFlags.RandomCardInHand))
		{
			CardContainer handContainer = References.Player.handContainer;
			if ((object)handContainer != null && handContainer.Count > 0)
			{
				foreach (Entity item3 in References.Player.handContainer.InRandomOrder())
				{
					if (item3 != target && CheckConstraints(item3))
					{
						list.Add(item3);
						break;
					}
				}
			}
		}

		goto IL_011f;
		IL_011f:
		AppliesTo(ApplyToFlags.EnemyHand);
		if (AppliesTo(ApplyToFlags.Allies))
		{
			list.AddRange(target.GetAllAllies());
		}
		else if (AppliesTo(ApplyToFlags.AlliesInRow))
		{
			if (Battle.IsOnBoard(target))
			{
				list.AddRange(target.GetAlliesInRow());
			}
			else if (wasInRows != null)
			{
				CardContainer[] array = wasInRows;
				foreach (CardContainer rowContainer in array)
				{
					int rowIndex = References.Battle.GetRowIndex(rowContainer);
					foreach (Entity item4 in target.GetAlliesInRow(rowIndex))
					{
						if (!list.Contains(item4))
						{
							list.Add(item4);
						}
					}
				}
			}
		}
		else
		{
			if (AppliesTo(ApplyToFlags.FrontAlly))
			{
				List<int> list2 = new List<int>();
				if (wasInRows != null)
				{
					list2.AddRange(References.Battle.GetRowIndices(wasInRows));
				}
				else
				{
					Entity entity = ((hit != null) ? hit.target : target);
					CardContainer[] array = entity.containers;
					list2.AddRange((array != null && array.Length > 0) ? References.Battle.GetRowIndices(entity.containers) : References.Battle.GetRowIndices(entity.preContainers));
				}

				foreach (CardContainer item5 in list2.Select((int i) => References.Battle.GetRow(target.owner, i)))
				{
					foreach (Entity item6 in item5)
					{
						if ((bool)item6)
						{
							list.Add(item6);
							break;
						}
					}
				}
			}

			if (AppliesTo(ApplyToFlags.BackAlly))
			{
				List<int> list3 = new List<int>();
				if (wasInRows != null)
				{
					list3.AddRange(References.Battle.GetRowIndices(wasInRows));
				}
				else if (hit != null)
				{
					list3.AddRange(References.Battle.GetRowIndices(hit.target));
				}
				else
				{
					list3.AddRange(References.Battle.GetRowIndices(target.containers));
				}

				foreach (CardContainer item7 in list3.Select((int i) => References.Battle.GetRow(target.owner, i)))
				{
					for (int num = item7.Count - 1; num >= 0; num--)
					{
						Entity entity2 = item7[num];
						if ((bool)entity2)
						{
							list.Add(entity2);
							break;
						}
					}
				}
			}

			if (AppliesTo(ApplyToFlags.AllyInFrontOf))
			{
				CardContainer[] array = wasInSlots ?? target.actualContainers.ToArray();
				foreach (CardContainer cardContainer in array)
				{
					Entity entity3 = null;
					if (cardContainer is CardSlot item && cardContainer.Group is CardSlotLane cardSlotLane)
					{
						for (int num2 = cardSlotLane.slots.IndexOf(item) - 1; num2 >= 0; num2--)
						{
							entity3 = cardSlotLane.slots[num2].GetTop();
							if ((bool)entity3)
							{
								break;
							}
						}
					}

					if ((bool)entity3)
					{
						list.Add(entity3);
						break;
					}
				}
			}

			if (AppliesTo(ApplyToFlags.AllyBehind))
			{
				CardContainer[] array = wasInSlots ?? target.actualContainers.ToArray();
				foreach (CardContainer cardContainer2 in array)
				{
					Entity entity4 = null;
					if (cardContainer2 is CardSlot item2 && cardContainer2.Group is CardSlotLane cardSlotLane2)
					{
						for (int k = cardSlotLane2.slots.IndexOf(item2) + 1; k < cardSlotLane2.slots.Count; k++)
						{
							entity4 = cardSlotLane2.slots[k].GetTop();
							if ((bool)entity4)
							{
								break;
							}
						}
					}

					if ((bool)entity4)
					{
						list.Add(entity4);
						break;
					}
				}
			}
		}

		if (AppliesTo(ApplyToFlags.Enemies))
		{
			list.AddRange(target.GetAllEnemies());
		}
		else if (AppliesTo(ApplyToFlags.EnemiesInRow))
		{
			CardContainer[] array = wasInRows ?? target.containers;
			foreach (CardContainer rowContainer2 in array)
			{
				int rowIndex2 = References.Battle.GetRowIndex(rowContainer2);
				List<Entity> enemiesInRow = target.GetEnemiesInRow(rowIndex2);
				if (enemiesInRow != null && enemiesInRow.Count > 0)
				{
					list.AddRange(enemiesInRow);
				}
			}
		}

		else if (AppliesTo(ApplyToFlags.FrontEnemy))
		{
			CardContainer[] array = wasInRows ?? target.containers;
			foreach (CardContainer rowContainer3 in array)
			{
				int rowIndex3 = References.Battle.GetRowIndex(rowContainer3);
				List<Entity> enemiesInRow2 = target.GetEnemiesInRow(rowIndex3);
				if (enemiesInRow2 != null && enemiesInRow2.Count > 0)
				{
					list.Add(enemiesInRow2[0]);
				}
			}
		}

		if (AppliesTo(ApplyToFlags.Applier) && (bool)applier && CanAffect(applier))
		{
			list.Add(applier);
		}

		if (AppliesTo(ApplyToFlags.Attacker))
		{
			if (hit == null)
			{
				hit = target.lastHit;
			}

			if ((bool)hit?.attacker && CanAffect(hit.attacker))
			{
				list.Add(hit.attacker);
			}
		}

		if (AppliesTo(ApplyToFlags.Target))
		{
			if (targets != null)
			{
				list.AddRange(targets.Where(CanAffect));
			}
			else
			{
				if (hit == null)
				{
					hit = target.lastHit;
				}

				if ((bool)hit?.target && CanAffect(hit.target))
				{
					list.Add(hit.target);
				}
			}
		}

		if (AppliesTo(ApplyToFlags.RandomUnit))
		{
			List<Entity> cardsOnBoard = Battle.GetCardsOnBoard(target.owner);
			cardsOnBoard.AddRange(Battle.GetCardsOnBoard(Battle.GetOpponent(target.owner)));
			cardsOnBoard.Remove(target);
			RemoveIneligible(cardsOnBoard);
			if (cardsOnBoard.Count > 0)
			{
				list.Add(cardsOnBoard.RandomItem());
			}
		}

		if (AppliesTo(ApplyToFlags.RandomAlly))
		{
			List<Entity> cardsOnBoard2 = Battle.GetCardsOnBoard(target.owner);
			cardsOnBoard2.Remove(target);
			RemoveIneligible(cardsOnBoard2);
			if (cardsOnBoard2.Count > 0)
			{
				list.Add(cardsOnBoard2.RandomItem());
			}
		}

		if (AppliesTo(ApplyToFlags.RandomEnemy))
		{
			List<Entity> cardsOnBoard3 = Battle.GetCardsOnBoard(Battle.GetOpponent(target.owner));
			RemoveIneligible(cardsOnBoard3);
			if (cardsOnBoard3.Count > 0)
			{
				list.Add(cardsOnBoard3.RandomItem());
			}
		}

		if (AppliesTo(ApplyToFlags.RandomAllyInRow))
		{
			List<Entity> list4 = new List<Entity>();
			CardContainer[] array = wasInRows ?? target.containers;
			for (int j = 0; j < array.Length; j++)
			{
				foreach (Entity item8 in array[j])
				{
					if (!list4.Contains(item8))
					{
						list4.Add(item8);
					}
				}
			}

			RemoveIneligible(list4);
			if (list4.Count > 0)
			{
				list.Add(list4.RandomItem());
			}
		}

		if (AppliesTo(ApplyToFlags.RandomEnemyInRow))
		{
			List<Entity> list5 = new List<Entity>();
			CardContainer[] array = wasInRows ?? target.containers;
			foreach (CardContainer rowContainer4 in array)
			{
				int rowIndex4 = References.Battle.GetRowIndex(rowContainer4);
				foreach (Entity item9 in target.GetEnemiesInRow(rowIndex4))
				{
					if (!list5.Contains(item9))
					{
						list5.Add(item9);
					}
				}
			}

			RemoveIneligible(list5);
			if (list5.Count > 0)
			{
				list.Add(list5.RandomItem());
			}
		}

		if (applyInReverseOrder)
		{
			list.Reverse();
		}

		return list;
	}

	public void RemoveIneligible(IList<Entity> list)
	{
		for (int num = list.Count - 1; num >= 0; num--)
		{
			Entity entity = list[num];
			if (!CheckConstraints(entity))
			{
				list.RemoveAt(num);
			}
		}
	}

	public bool CanAffect(Entity entity)
	{
		if (!dealDamage)
		{
			return effectToApply.CanPlayOn(entity);
		}

		return entity.canBeHit;
	}

	public bool CheckConstraints(Entity entity)
	{
		if (CanAffect(entity))
		{
			return applyConstraints.All((TargetConstraint c) => c.Check(entity));
		}

		return false;
	}

	public StatusEffectApplyX()
	{
	}
}
