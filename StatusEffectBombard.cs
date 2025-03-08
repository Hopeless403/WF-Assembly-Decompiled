#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Bombard", fileName = "Bombard")]
public class StatusEffectBombard : StatusEffectData
{
	public class Slot
	{
		public readonly CardSlot slot;

		public readonly bool friendly;

		public readonly bool front;

		public Slot(CardSlot slot, bool friendly, bool front)
		{
			this.slot = slot;
			this.friendly = friendly;
			this.front = front;
		}
	}

	[Serializable]
	public class SlotList
	{
		public int[] list;
	}

	[SerializeField]
	public Vector2Int targetCountRange = new Vector2Int(2, 2);

	[SerializeField]
	[Range(0f, 1f)]
	public float hitFriendlyChance = 0.1f;

	[SerializeField]
	public float delayBetweenTargets = 0.1f;

	[SerializeField]
	public float delayAfter = 0.1f;

	[SerializeField]
	public int maxFrontTargets;

	public List<CardContainer> targetList = new List<CardContainer>();

	public bool triggered;

	public int[] storedTargetList;

	public override object GetMidBattleData()
	{
		List<CardContainer> list = targetList;
		if (list != null && list.Count > 0)
		{
			List<int> list2 = new List<int>();
			List<CardSlot> slots = References.Battle.GetSlots();
			foreach (CardContainer target in targetList)
			{
				if (target is CardSlot item)
				{
					int num = slots.IndexOf(item);
					if (num >= 0)
					{
						list2.Add(num);
					}
				}
			}

			return new SlotList
			{
				list = list2.ToArray()
			};
		}

		return null;
	}

	public override void RestoreMidBattleData(object data)
	{
		if (data is SlotList slotList)
		{
			storedTargetList = slotList.list;
		}
	}

	public override void Init()
	{
		base.OnEnable += Enable;
		base.OnCardPlayed += CardPlayed;
		base.OnActionPerformed += ActionPerformed;
		Events.OnEntityTrigger += EntityTrigger;
	}

	public void OnDestroy()
	{
		Events.OnEntityTrigger -= EntityTrigger;
	}

	public void EntityTrigger(ref Trigger trigger)
	{
		if (trigger.entity == target && CanTrigger() && trigger.type == "basic")
		{
			trigger = new TriggerBombard(trigger.entity, trigger.triggeredBy, "bombard", trigger.targets, targetList.ToArray());
		}
	}

	public override bool RunEnableEvent(Entity entity)
	{
		return entity == target;
	}

	public IEnumerator Enable(Entity entity)
	{
		yield return SetTargets();
	}

	public override bool RunDisableEvent(Entity entity)
	{
		if (entity == target)
		{
			foreach (CardContainer target in targetList)
			{
				Events.InvokeAbilityTargetRemove(target);
			}
		}

		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		return entity == target;
	}

	public IEnumerator CardPlayed(Entity entity, Entity[] targets)
	{
		if (CanTrigger())
		{
			triggered = true;
			yield return Sequences.WaitForAnimationEnd(target);
			yield return Sequences.Wait(delayAfter);
		}
	}

	public override bool RunActionPerformedEvent(PlayAction action)
	{
		if (triggered)
		{
			return ActionQueue.Empty;
		}

		return false;
	}

	public IEnumerator ActionPerformed(PlayAction action)
	{
		triggered = false;
		yield return SetTargets();
	}

	public override bool RunEndEvent()
	{
		foreach (CardContainer target in targetList)
		{
			Events.InvokeAbilityTargetRemove(target);
		}

		return false;
	}

	public IEnumerator SetTargets()
	{
		foreach (CardContainer target in targetList)
		{
			Events.InvokeAbilityTargetRemove(target);
		}

		if (storedTargetList != null)
		{
			List<CardSlot> slots = References.Battle.GetSlots();
			int[] array = storedTargetList;
			foreach (int index in array)
			{
				CardSlot item = slots[index];
				targetList.Add(item);
			}

			storedTargetList = null;
		}
		else
		{
			List<Slot> list = new List<Slot>();
			List<CardContainer> rows = Battle.instance.GetRows(base.target.owner);
			List<CardContainer> rows2 = Battle.instance.GetRows(Battle.GetOpponent(base.target.owner));
			for (int k = 0; k < Battle.instance.rowCount; k++)
			{
				if (rows[k] is CardSlotLane cardSlotLane)
				{
					list.AddRange(cardSlotLane.slots.Select((CardSlot t, int i) => new Slot(t, friendly: true, i == 0)));
				}

				if (rows2[k] is CardSlotLane cardSlotLane2)
				{
					list.AddRange(cardSlotLane2.slots.Select((CardSlot t, int i) => new Slot(t, friendly: false, i == 0)));
				}
			}

			list.Shuffle();
			if (maxFrontTargets <= 0)
			{
				list.RemoveAll((Slot a) => a.front);
			}

			(targetList ?? (targetList = new List<CardContainer>())).Clear();
			int num = 0;
			int num2 = targetCountRange.Random();
			while (num2 > 0 && list.Count > 0)
			{
				bool friendly = UnityEngine.Random.Range(0f, 1f) < hitFriendlyChance;
				Slot slot = list.Find((Slot a) => a.friendly == friendly) ?? list[0];
				targetList.Add(slot.slot);
				num2--;
				list.Remove(slot);
				if (slot.front && ++num >= maxFrontTargets)
				{
					list.RemoveAll((Slot a) => a.front);
				}
			}
		}

		if ((bool)base.target)
		{
			targetList.Sort(delegate(CardContainer a, CardContainer b)
			{
				int num3 = Mathf.RoundToInt(Mathf.Sign(base.target.transform.position.x));
				float x = a.holder.position.x;
				float x2 = b.holder.position.x;
				return (num3 != 1) ? x2.CompareTo(x) : x.CompareTo(x2);
			});
		}

		foreach (CardContainer target2 in targetList)
		{
			Events.InvokeAbilityTargetAdd(target2);
			yield return Sequences.Wait(delayBetweenTargets);
		}
	}
}
