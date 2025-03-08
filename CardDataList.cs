#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardDataList : IList<CardData>, ICollection<CardData>, IEnumerable<CardData>, IEnumerable
{
	[SerializeField]
	public List<CardData> list = new List<CardData>();

	public int Count => list.Count;

	public bool IsReadOnly => false;

	public CardData this[int index]
	{
		get
		{
			return list[index];
		}
		set
		{
			list[index] = value;
		}
	}

	public IEnumerator<CardData> GetEnumerator()
	{
		return list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public void Add(CardData item)
	{
		list.Add(item);
	}

	public void Clear()
	{
		list.Clear();
	}

	public bool Contains(CardData item)
	{
		if (!item)
		{
			return false;
		}

		foreach (CardData item2 in list)
		{
			if (item2.id == item.id)
			{
				return true;
			}
		}

		return false;
	}

	public void CopyTo(CardData[] array, int arrayIndex)
	{
		list.CopyTo(array, arrayIndex);
	}

	public bool Remove(CardData item)
	{
		int num = IndexOf(item);
		if (num >= 0)
		{
			list.RemoveAt(num);
			return true;
		}

		return false;
	}

	public int IndexOf(CardData item)
	{
		if (!item)
		{
			return -1;
		}

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].id == item.id)
			{
				return i;
			}
		}

		return -1;
	}

	public void Insert(int index, CardData item)
	{
		list.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		list.RemoveAt(index);
	}

	public void Sort()
	{
		list.Sort();
	}

	public void Sort(Comparison<CardData> comparison)
	{
		list.Sort(comparison);
	}

	public void Sort(IComparer<CardData> comparer)
	{
		list.Sort(comparer);
	}

	public void Sort(int index, int count, IComparer<CardData> comparer)
	{
		list.Sort(index, count, comparer);
	}

	public List<CardData> FindAll(Predicate<CardData> predicate)
	{
		return list.FindAll(predicate);
	}
}
