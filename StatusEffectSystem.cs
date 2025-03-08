#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectSystem : GameSystem
{
	public class StatusEffectComparer : IComparer<StatusEffectData>
	{
		public int Compare(StatusEffectData x, StatusEffectData y)
		{
			int num = ((x != null) ? x.eventPriority : 0);
			int num2 = ((y != null) ? y.eventPriority : 0);
			if (num >= num2)
			{
				return 1;
			}

			return -1;
		}
	}

	public static readonly FreezableList<StatusEffectData> activeEffects = new FreezableList<StatusEffectData>(autoSort: true, new StatusEffectComparer());

	public static int eventRoutineCount;

	public static bool EventsRunning => eventRoutineCount > 0;

	public static IEnumerator Apply(Entity target, Entity applier, StatusEffectData effectData, int count, bool temporary = false, Action<StatusEffectData> onEffectApplied = null, bool fireEvents = true, bool applyEvenIfZero = false)
	{
		bool flag = true;
		if (effectData.targetConstraints != null && effectData.targetConstraints.Any((TargetConstraint constraint) => !constraint.Check(target)))
		{
			flag = false;
		}

		if (!flag)
		{
			yield break;
		}

		activeEffects.Freeze();
		StatusEffectApply s = new StatusEffectApply(applier, target, effectData, count);
		if (fireEvents)
		{
			yield return ApplyStatusEvent(s);
		}

		if ((bool)s.effectData && (s.count > 0 || applyEvenIfZero || !s.effectData.canBeBoosted || s.effectData is StatusEffectInstantMultiple))
		{
			StatusEffectData effect = target.statusEffects.Find((StatusEffectData a) => a.name == s.effectData.name);
			if ((bool)effect && effect.stackable)
			{
				Debug.Log($"Stacking [{s.effectData.name} {s.count}] on top of [{target.name}]");
				effect.count += s.count;
				if (temporary)
				{
					effect.temporary += s.count;
				}

				effect.applier = applier;
			}
			else
			{
				Debug.Log($"[{s.effectData.name} {s.count}] applied to [{target.name}]");
				effect = s.effectData.Instantiate();
				effect.Apply(s.count, target, applier);
				if (temporary)
				{
					effect.temporary = s.count;
				}

				activeEffects.Add(effect);
				onEffectApplied?.Invoke(effect);
				if (effect.RunBeginEvent() && effect.HasBeginRoutine)
				{
					yield return effect.BeginRoutine();
				}
			}

			if ((bool)applier)
			{
				effect.applierOwner = applier.owner;
			}

			if (effect.RunStackEvent(s.count) && effect.HasStackRoutine)
			{
				yield return effect.StackRoutine(s.count);
			}

			target.PromptUpdate();
		}

		if (target.startingEffectsApplied && target.display.init && fireEvents)
		{
			Events.InvokeStatusEffectApplied(s);
			yield return PostApplyStatusEvent(s);
		}

		activeEffects.Thaw();
	}

	public static IEnumerator Clear()
	{
		activeEffects.Freeze();
		for (int i = activeEffects.Count - 1; i >= 0; i--)
		{
			yield return activeEffects[i].Remove();
		}

		activeEffects.Thaw();
	}

	public static IEnumerator EntityEnableEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunEnableEvent(entity) && statusEffectData.HasEnableRoutine)
			{
				yield return statusEffectData.EnableRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator EntityDisableEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunDisableEvent(entity) && statusEffectData.HasDisableRoutine)
			{
				yield return statusEffectData.DisableRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator TurnStartEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunTurnStartEvent(entity) && statusEffectData.HasTurnStartRoutine)
			{
				yield return statusEffectData.TurnStartRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator TurnEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunTurnEvent(entity) && statusEffectData.HasTurnRoutine)
			{
				yield return statusEffectData.TurnRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator TurnEndEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunTurnEndEvent(entity) && statusEffectData.HasTurnEndRoutine)
			{
				yield return statusEffectData.TurnEndRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator PreAttackEvent(Hit hit)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunPreAttackEvent(hit) && statusEffectData.HasPreAttackRoutine)
			{
				yield return statusEffectData.PreAttackRoutine(hit);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator HitEvent(Hit hit)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunHitEvent(hit) && statusEffectData.HasHitRoutine)
			{
				yield return statusEffectData.HitRoutine(hit);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator PostHitEvent(Hit hit)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunPostHitEvent(hit) && statusEffectData.HasPostHitRoutine)
			{
				yield return statusEffectData.PostHitRoutine(hit);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator PostAttackEvent(Hit hit)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunPostAttackEvent(hit) && statusEffectData.HasPostAttackRoutine)
			{
				yield return statusEffectData.PostAttackRoutine(hit);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator ApplyStatusEvent(StatusEffectApply apply)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunApplyStatusEvent(apply) && statusEffectData.HasApplyStatusRoutine)
			{
				yield return statusEffectData.ApplyStatusRoutine(apply);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator PostApplyStatusEvent(StatusEffectApply apply)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunPostApplyStatusEvent(apply) && statusEffectData.HasPostApplyStatusRoutine)
			{
				yield return statusEffectData.PostApplyStatusRoutine(apply);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator EntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c2 = activeEffects.Count;
		for (int j = c2 - 1; j >= 0; j--)
		{
			int count = activeEffects.Count;
			if (count < c2)
			{
				j -= c2 - count;
				c2 = count;
			}

			if (j < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[j];
			if (statusEffectData.target != null && statusEffectData.RunEntityDestroyedEvent(entity, deathType) && statusEffectData.HasEntityDestroyedRoutine)
			{
				yield return statusEffectData.EntityDestroyedRoutine(entity, deathType);
			}
		}

		if (entity.statusEffects != null)
		{
			c2 = entity.statusEffects.Count;
			for (int j = c2 - 1; j >= 0; j--)
			{
				int count2 = activeEffects.Count;
				if (count2 < c2)
				{
					j -= c2 - count2;
					c2 = count2;
				}

				if (j < 0)
				{
					break;
				}

				StatusEffectData statusEffectData2 = entity.statusEffects[j];
				if (statusEffectData2.target != null && statusEffectData2.RunDisableEvent(entity) && statusEffectData2.HasDisableRoutine)
				{
					yield return statusEffectData2.DisableRoutine(entity);
				}
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator CardMoveEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunCardMoveEvent(entity) && statusEffectData.HasCardMoveRoutine)
			{
				yield return statusEffectData.CardMoveRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator PreTriggerEvent(Trigger trigger)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunPreTriggerEvent(trigger) && statusEffectData.HasPreTriggerRoutine)
			{
				yield return statusEffectData.PreTriggerRoutine(trigger);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator PreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunPreCardPlayedEvent(entity, targets) && statusEffectData.HasPreCardPlayedRoutine)
			{
				yield return statusEffectData.PreCardPlayedRoutine(entity, targets);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator CardPlayedEvent(Entity entity, Entity[] targets)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunCardPlayedEvent(entity, targets) && statusEffectData.HasCardPlayedRoutine)
			{
				yield return statusEffectData.CardPlayedRoutine(entity, targets);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator EffectBonusChangedEvent(Entity entity)
	{
		eventRoutineCount++;
		if (entity.statusEffects != null)
		{
			activeEffects.Freeze();
			int c = entity.statusEffects.Count;
			for (int i = c - 1; i >= 0; i--)
			{
				int count = activeEffects.Count;
				if (count < c)
				{
					i -= c - count;
					c = count;
				}

				if (i < 0)
				{
					break;
				}

				StatusEffectData statusEffectData = entity.statusEffects[i];
				if (statusEffectData.target != null && statusEffectData.RunEffectBonusChangedEvent() && statusEffectData.HasEffectBonusChangedRoutine)
				{
					yield return statusEffectData.EffectBonusChangedRoutine();
				}
			}

			activeEffects.Thaw();
		}

		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator ActionPerformedEvent(PlayAction action)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunActionPerformedEvent(action) && statusEffectData.HasActionPerformedRoutine)
			{
				yield return statusEffectData.ActionPerformedRoutine(action);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}

	public static IEnumerator BuildEvent(Entity entity)
	{
		eventRoutineCount++;
		activeEffects.Freeze();
		int c = activeEffects.Count;
		for (int i = c - 1; i >= 0; i--)
		{
			int count = activeEffects.Count;
			if (count < c)
			{
				i -= c - count;
				c = count;
			}

			if (i < 0)
			{
				break;
			}

			StatusEffectData statusEffectData = activeEffects[i];
			if (statusEffectData.target != null && statusEffectData.RunBuildEvent(entity) && statusEffectData.HasBuildRoutine)
			{
				yield return statusEffectData.BuildRoutine(entity);
			}
		}

		activeEffects.Thaw();
		eventRoutineCount--;
		yield return Sequences.Null();
	}
}
