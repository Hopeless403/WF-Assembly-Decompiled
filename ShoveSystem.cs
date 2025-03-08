#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShoveSystem : GameSystem
{
	public static readonly List<Entity> shovedFrom = new List<Entity>();

	public static Entity dragging;

	public static readonly int[] shoveDirs = new int[2] { -1, 1 };

	public static bool Active { get; set; }

	public static CardSlot Slot { get; set; }

	public static Vector3 Position => Slot.transform.position - Vector3.Scale(dragging.offset.localPosition, dragging.transform.localScale);

	public static bool Fix { get; set; }

	public void OnEnable()
	{
		Events.OnEntityDrag += DragStart;
		Events.OnEntityRelease += DragEnd;
		Events.OnSlotHover += SlotHover;
		Events.OnSlotUnHover += SlotUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityDrag -= DragStart;
		Events.OnEntityRelease -= DragEnd;
		Events.OnSlotHover -= SlotHover;
		Events.OnSlotUnHover -= SlotUnHover;
	}

	public static void DragStart(Entity entity)
	{
		dragging = entity;
	}

	public static void DragEnd(Entity entity)
	{
		dragging = null;
		if (Active && !Fix)
		{
			ClearShove();
		}
	}

	public static void SlotHover(CardSlot slot)
	{
		if (Slot != null && Slot != slot)
		{
			ClearShove();
		}
	}

	public static void SlotUnHover(CardSlot slot)
	{
		if (Active && Slot != null && Slot == slot && !Fix)
		{
			ClearShove();
		}
	}

	public static bool CanShove(Entity shovee, Entity shover, out Dictionary<Entity, List<CardSlot>> shoveData)
	{
		shoveData = new Dictionary<Entity, List<CardSlot>>();
		if (!Events.CheckEntityShove(shovee))
		{
			return false;
		}

		List<int> list = shoveDirs.ToList();
		if (shover.positionPriority > shovee.positionPriority)
		{
			list.Remove(-1);
		}

		if (shovee.positionPriority > shover.positionPriority)
		{
			list.Remove(1);
		}

		bool flag = false;
		foreach (int item in list)
		{
			CardSlot[] array = FindSlots(shovee, item);
			if (array != null && array.Length != 0)
			{
				flag = CanShoveTo(shovee, shover, item, array, out shoveData);
				if (flag)
				{
					break;
				}
			}
		}

		if (!flag && (shover == null || shover.data == null || shover.data.canShoveToOtherRow))
		{
			flag = CanShoveToOtherRow(shovee, shover, out shoveData);
		}

		return flag;
	}

	public static CardSlot[] FindSlots(Entity shovee, int dir)
	{
		bool flag = false;
		List<CardSlot> list = new List<CardSlot>();
		CardContainer[] containers = shovee.containers;
		for (int i = 0; i < containers.Length; i++)
		{
			if (!(containers[i] is CardSlotLane cardSlotLane))
			{
				flag = true;
				break;
			}

			int num = cardSlotLane.IndexOf(shovee) + dir;
			if (num < 0 || num >= cardSlotLane.max)
			{
				flag = true;
				break;
			}

			list.Add(cardSlotLane.slots[num]);
		}

		if (!flag)
		{
			return list.ToArray();
		}

		return null;
	}

	public static bool CanShoveTo(Entity shovee, Entity shover, int dir, CardSlot[] slots, out Dictionary<Entity, List<CardSlot>> shoveData)
	{
		shoveData = new Dictionary<Entity, List<CardSlot>>();
		int num = 1;
		Queue<KeyValuePair<Entity, CardSlot[]>> queue = new Queue<KeyValuePair<Entity, CardSlot[]>>();
		queue.Enqueue(new KeyValuePair<Entity, CardSlot[]>(shovee, slots));
		List<Entity> list = new List<Entity>();
		bool result = false;
		while (queue.Count > 0)
		{
			KeyValuePair<Entity, CardSlot[]> keyValuePair = queue.Dequeue();
			Entity key = keyValuePair.Key;
			list.Add(key);
			CardSlot[] value = keyValuePair.Value;
			if (value == null || value.Length == 0)
			{
				break;
			}

			List<CardSlot> list2 = new List<CardSlot>();
			CardSlot[] array = value;
			foreach (CardSlot cardSlot in array)
			{
				if (shoveData.ContainsKey(key))
				{
					shoveData[key].Add(cardSlot);
				}
				else
				{
					shoveData[key] = new List<CardSlot> { cardSlot };
				}

				Entity top = cardSlot.GetTop();
				if (top != null && top != shover)
				{
					list2.Add(cardSlot);
				}
			}

			num--;
			foreach (CardSlot item in list2)
			{
				Entity blockingEntity = item.GetTop();
				if (!list.Contains(blockingEntity) && !queue.Any((KeyValuePair<Entity, CardSlot[]> p) => p.Key == blockingEntity))
				{
					CardSlot[] value2 = FindSlots(blockingEntity, dir);
					queue.Enqueue(new KeyValuePair<Entity, CardSlot[]>(blockingEntity, value2));
					num++;
				}
			}
		}

		if (num <= 0)
		{
			result = true;
		}

		return result;
	}

	public static bool CanShoveToOtherRow(Entity shovee, Entity shover, out Dictionary<Entity, List<CardSlot>> shoveData)
	{
		shoveData = new Dictionary<Entity, List<CardSlot>>();
		if (shovee.containers.Length != 1)
		{
			return false;
		}

		if (!(shovee.containers[0] is CardSlotLane cardSlotLane))
		{
			return false;
		}

		int a = cardSlotLane.IndexOf(shovee);
		bool flag = false;
		foreach (CardContainer item in cardSlotLane.shoveTo)
		{
			if (!(item is CardSlotLane cardSlotLane2))
			{
				continue;
			}

			int num = cardSlotLane2.max - cardSlotLane2.Count;
			if (cardSlotLane2.Contains(shover))
			{
				num++;
			}

			if (num <= 0)
			{
				continue;
			}

			int index = Mathf.Min(a, cardSlotLane2.max - 1);
			CardSlot cardSlot = cardSlotLane2.slots[index];
			int[] array = shoveDirs;
			foreach (int dir in array)
			{
				flag = CanShoveTo(shovee, shover, dir, new CardSlot[1] { cardSlot }, out shoveData);
				if (flag)
				{
					break;
				}
			}

			if (flag)
			{
				break;
			}
		}

		return flag;
	}

	public static void ShowShove(CardSlot fromContainer, Dictionary<Entity, List<CardSlot>> shoveData)
	{
		Active = true;
		Slot = fromContainer;
		float time = 0.3f;
		LeanTweenType ease = LeanTweenType.easeOutQuart;
		foreach (KeyValuePair<Entity, List<CardSlot>> shoveDatum in shoveData)
		{
			Entity key = shoveDatum.Key;
			List<CardSlot> value = shoveDatum.Value;
			GameObject gameObject = key.gameObject;
			LeanTween.cancel(gameObject);
			Vector3 zero = Vector3.zero;
			foreach (CardSlot item in value)
			{
				zero += item.transform.position;
			}

			zero /= (float)value.Count;
			LeanTween.move(gameObject, zero, time).setEase(ease);
			shovedFrom.Add(key);
		}
	}

	public static IEnumerator DoShove(Dictionary<Entity, List<CardSlot>> shoveData, bool updatePositions = false)
	{
		foreach (KeyValuePair<Entity, List<CardSlot>> shoveDatum in shoveData)
		{
			shoveDatum.Key.RemoveFromContainers();
		}

		HashSet<CardContainer> hashSet = new HashSet<CardContainer>();
		foreach (KeyValuePair<Entity, List<CardSlot>> shoveDatum2 in shoveData)
		{
			Entity key = shoveDatum2.Key;
			foreach (CardSlot item in shoveDatum2.Value)
			{
				item.Add(key);
				hashSet.Add(item);
			}
		}

		if (updatePositions)
		{
			foreach (CardContainer item2 in hashSet)
			{
				item2.TweenChildPositions();
			}
		}

		Routine.Clump clump = new Routine.Clump();
		foreach (KeyValuePair<Entity, List<CardSlot>> shoveDatum3 in shoveData)
		{
			Events.InvokeEntityMove(shoveDatum3.Key);
			clump.Add(StatusEffectSystem.CardMoveEvent(shoveDatum3.Key));
		}

		yield return clump.WaitForEnd();
		Deactivate();
	}

	public static void ClearShove()
	{
		foreach (Entity item in shovedFrom)
		{
			foreach (CardContainer actualContainer in item.actualContainers)
			{
				actualContainer.TweenChildPosition(item);
			}
		}

		Deactivate();
	}

	public static void Deactivate()
	{
		shovedFrom.Clear();
		Slot = null;
		Fix = false;
		Active = false;
	}
}
