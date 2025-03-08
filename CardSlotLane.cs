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
using UnityEngine.UI;

public class CardSlotLane : CardContainer
{
	public class PushData
	{
		public readonly Dictionary<Entity, List<CardContainer>> dict = new Dictionary<Entity, List<CardContainer>>();

		public void Add(PushData other)
		{
			foreach (KeyValuePair<Entity, List<CardContainer>> item in other.dict)
			{
				Add(item.Key, item.Value);
			}
		}

		public void Add(Entity entity, CardContainer container)
		{
			if (dict.ContainsKey(entity))
			{
				dict[entity].Add(container);
				return;
			}

			dict[entity] = new List<CardContainer> { container };
		}

		public void Add(Entity entity, List<CardContainer> containers)
		{
			if (dict.ContainsKey(entity))
			{
				dict[entity].AddRange(containers);
			}
			else
			{
				dict[entity] = containers;
			}
		}

		public void Execute()
		{
			foreach (KeyValuePair<Entity, List<CardContainer>> item in dict)
			{
				item.Key.RemoveFromContainers();
			}

			foreach (KeyValuePair<Entity, List<CardContainer>> item2 in dict)
			{
				foreach (CardContainer item3 in item2.Value)
				{
					item3.Add(item2.Key);
				}
			}
		}
	}

	public List<CardSlot> slots;

	[Required(null)]
	public CardSlot slotPrefab;

	[Required(null)]
	public HorizontalLayoutGroup layout;

	public bool autoMoveForwards = true;

	public override int Count
	{
		get
		{
			int num = 0;
			foreach (CardSlot slot in slots)
			{
				num += slot.Count;
			}

			return num;
		}
		set
		{
			base.Count = value;
		}
	}

	public override Entity this[int index]
	{
		get
		{
			int skips = GetSkips(index);
			for (int i = index + skips; i < max; i++)
			{
				CardSlot cardSlot = slots[i];
				if (cardSlot != null && !cardSlot.Empty)
				{
					return cardSlot[0];
				}
			}

			return null;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		new Routine(SetSizeRoutine(size, cardScale));
	}

	public IEnumerator SetSizeRoutine(int size, float cardScale)
	{
		CreateSlots(size);
		yield return null;
		holder.sizeDelta = ((RectTransform)layout.transform).sizeDelta;
	}

	public override void MoveChildrenForward()
	{
		for (int i = 1; i < max; i++)
		{
			CardSlot cardSlot = slots[i];
			Entity top = cardSlot.GetTop();
			if (!top || top.positionPriority <= 0)
			{
				continue;
			}

			int num = 0;
			List<CardSlot> list = new List<CardSlot> { cardSlot };
			if (top.height > 1)
			{
				CardContainer[] secondaryContainers = cardSlot.GetSecondaryContainers(top);
				foreach (CardContainer cardContainer in secondaryContainers)
				{
					if (cardContainer is CardSlot item && cardContainer.Group is CardSlotLane)
					{
						list.Add(item);
					}
				}
			}

			for (int num2 = i - 1; num2 >= 0; num2--)
			{
				bool flag = true;
				foreach (CardSlot item3 in list)
				{
					if (!(item3.Group as CardSlotLane).slots[num2].Empty)
					{
						flag = false;
						break;
					}
				}

				if (!flag)
				{
					break;
				}

				num++;
			}

			if (num <= 0)
			{
				continue;
			}

			if (list.Count > 1)
			{
				foreach (CardSlot item4 in list)
				{
					if (item4.IsPrimaryContainer(top))
					{
						list.Remove(item4);
						list.Insert(0, item4);
						break;
					}
				}
			}

			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				CardSlot cardSlot2 = list[num3];
				CardSlotLane obj = cardSlot2.Group as CardSlotLane;
				int num4 = obj.slots.IndexOf(cardSlot2);
				CardSlot cardSlot3 = obj.slots[num4 - num];
				cardSlot2.Remove(top);
				cardSlot3.Add(top);
			}
		}

		for (int num5 = max - 2; num5 >= 0; num5--)
		{
			CardSlot cardSlot4 = slots[num5];
			Entity top2 = cardSlot4.GetTop();
			if ((bool)top2 && top2.positionPriority < 0)
			{
				int num6 = 0;
				List<CardSlot> list2 = new List<CardSlot> { cardSlot4 };
				if (top2.height > 1)
				{
					CardContainer[] secondaryContainers = cardSlot4.GetSecondaryContainers(top2);
					foreach (CardContainer cardContainer2 in secondaryContainers)
					{
						if (cardContainer2 is CardSlot item2 && cardContainer2.Group is CardSlotLane)
						{
							list2.Add(item2);
						}
					}
				}

				for (int k = num5 + 1; k < max; k++)
				{
					bool flag2 = true;
					foreach (CardSlot item5 in list2)
					{
						if (!(item5.Group as CardSlotLane).slots[k].Empty)
						{
							flag2 = false;
							break;
						}
					}

					if (!flag2)
					{
						break;
					}

					num6++;
				}

				if (num6 > 0)
				{
					if (list2.Count > 1)
					{
						foreach (CardSlot item6 in list2)
						{
							if (item6.IsPrimaryContainer(top2))
							{
								list2.Remove(item6);
								list2.Insert(0, item6);
								break;
							}
						}
					}

					for (int num7 = list2.Count - 1; num7 >= 0; num7--)
					{
						CardSlot cardSlot5 = list2[num7];
						CardSlotLane obj2 = cardSlot5.Group as CardSlotLane;
						int num8 = obj2.slots.IndexOf(cardSlot5);
						CardSlot cardSlot6 = obj2.slots[num8 + num6];
						cardSlot5.Remove(top2);
						cardSlot6.Add(top2);
					}
				}
			}
		}
	}

	public void SetDirection(int direction)
	{
		layout.reverseArrangement = direction == 1;
	}

	public void CreateSlots(int count)
	{
		layout.transform.DestroyAllChildren();
		slots.Clear();
		CardSlot cardSlot = null;
		for (int i = 0; i < count; i++)
		{
			CardSlot cardSlot2 = UnityEngine.Object.Instantiate(slotPrefab, layout.transform);
			cardSlot2.name = $"{base.name} [Slot {i + 1}]";
			cardSlot2.owner = owner;
			cardSlot2.Group = this;
			slots.Add(cardSlot2);
			if ((bool)cardSlot)
			{
				cardSlot.shoveTo.Add(cardSlot2);
				cardSlot2.shoveTo.Add(cardSlot);
			}

			cardSlot = cardSlot2;
		}
	}

	public override void Add(Entity entity)
	{
		if (Count >= max)
		{
			return;
		}

		if (entity.positionPriority >= 0)
		{
			for (int i = 0; i < max; i++)
			{
				CardSlot cardSlot = slots[i];
				Entity top = cardSlot.GetTop();
				if (!top)
				{
					cardSlot.Add(entity);
					break;
				}

				if (top.positionPriority >= entity.positionPriority)
				{
					continue;
				}

				bool flag = true;
				for (int j = i + 1; j < max; j++)
				{
					Entity top2 = slots[j].GetTop();
					if ((bool)top2 && top2.positionPriority >= entity.positionPriority)
					{
						flag = false;
						break;
					}
				}

				if (flag)
				{
					Insert(i, entity);
					break;
				}
			}

			return;
		}

		for (int num = max - 1; num >= 0; num--)
		{
			CardSlot cardSlot2 = slots[num];
			Entity top3 = cardSlot2.GetTop();
			if (!top3)
			{
				cardSlot2.Add(entity);
				break;
			}

			if (top3.positionPriority <= entity.positionPriority)
			{
				Insert(num, entity);
				break;
			}
		}
	}

	public override void Insert(int index, Entity entity)
	{
		if (Count >= max)
		{
			return;
		}

		CardSlot cardSlot = slots[index];
		if ((bool)cardSlot && cardSlot.Empty)
		{
			cardSlot.Add(entity);
			return;
		}

		bool flag = PushForwards(index);
		if (!flag)
		{
			flag = PushBackwards(index);
		}

		if (flag)
		{
			cardSlot.Add(entity);
		}
	}

	public override bool PushForwards(int fromIndex)
	{
		bool num = CanPush(fromIndex);
		if (num)
		{
			GetPushData(fromIndex).Execute();
		}

		return num;
	}

	public override bool PushBackwards(int fromIndex)
	{
		bool num = CanPush(fromIndex, 1);
		if (num)
		{
			GetPushData(fromIndex, 1).Execute();
		}

		return num;
	}

	public bool CanPush(int fromIndex, int direction = -1)
	{
		bool result = true;
		Entity top = slots[fromIndex].GetTop();
		if ((bool)top)
		{
			CardContainer[] containers = top.containers;
			for (int i = 0; i < containers.Length; i++)
			{
				if (containers[i] is CardSlotLane cardSlotLane)
				{
					int num = cardSlotLane.IndexOf(top) + direction;
					if (num >= 0 && num < cardSlotLane.max)
					{
						if (!cardSlotLane.CanPush(num, direction))
						{
							result = false;
							break;
						}

						continue;
					}

					result = false;
					break;
				}

				result = false;
				break;
			}
		}

		return result;
	}

	public PushData GetPushData(int fromIndex, int direction = -1)
	{
		PushData pushData = new PushData();
		Entity top = slots[fromIndex].GetTop();
		if ((bool)top)
		{
			CardContainer[] containers = top.containers;
			for (int i = 0; i < containers.Length; i++)
			{
				if (containers[i] is CardSlotLane cardSlotLane)
				{
					int num = cardSlotLane.IndexOf(top) + direction;
					if (num >= 0 && num < cardSlotLane.max)
					{
						CardSlot container = cardSlotLane.slots[num];
						pushData.Add(top, container);
						pushData.Add(cardSlotLane.GetPushData(num, direction));
					}
				}
			}
		}

		return pushData;
	}

	public override void Remove(Entity entity)
	{
		foreach (CardSlot slot in slots)
		{
			if (slot.Count > 0 && slot[0] == entity)
			{
				slot.Remove(entity);
			}
		}
	}

	public override void RemoveAt(int index)
	{
		CardSlot cardSlot = slots[index];
		if ((bool)cardSlot && !cardSlot.Empty)
		{
			cardSlot.RemoveAt(0);
		}
	}

	public int GetSkips(int upToIndex)
	{
		int num = 0;
		for (int i = 0; i <= upToIndex; i++)
		{
			CardSlot cardSlot = slots[i];
			if (cardSlot == null || cardSlot.Empty)
			{
				num++;
			}
		}

		return num;
	}

	public override Entity GetTop()
	{
		if (max <= 0)
		{
			return null;
		}

		if (slots[0].Empty)
		{
			return null;
		}

		return slots[0][0];
	}

	public override int IndexOf(Entity item)
	{
		for (int i = 0; i < max; i++)
		{
			if (slots[i].Contains(item))
			{
				return i;
			}
		}

		return -1;
	}

	public override bool Contains(Entity item)
	{
		return slots.Any((CardSlot slot) => slot.Contains(item));
	}

	public override Entity[] ToArray()
	{
		List<Entity> list = new List<Entity>();
		foreach (CardSlot slot in slots)
		{
			if (slot.Count > 0)
			{
				list.Add(slot[0]);
			}
		}

		return list.ToArray();
	}

	public override IEnumerator<Entity> GetEnumerator()
	{
		foreach (CardSlot slot in slots)
		{
			foreach (Entity item in slot)
			{
				yield return item;
			}
		}
	}
}
