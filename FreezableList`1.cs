#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FreezableList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	public readonly List<T> currentList = new List<T>();

	public readonly List<T> nextList = new List<T>();

	public int freezeCount;

	public bool updateRequired;

	public bool sortRequired;

	public bool autoSort;

	public IComparer<T> autoSortComparer;

	public T this[int index]
	{
		get
		{
			return currentList[index];
		}
		set
		{
			nextList[index] = value;
		}
	}

	public int Count => currentList.Count;

	public bool IsReadOnly => false;

	public FreezableList()
	{
	}

	public FreezableList(bool autoSort)
	{
		this.autoSort = autoSort;
	}

	public FreezableList(bool autoSort, IComparer<T> autoSortComparer)
	{
		this.autoSort = autoSort;
		this.autoSortComparer = autoSortComparer;
	}

	public void Add(T item)
	{
		int count = nextList.Count;
		int index = count;
		if (autoSort)
		{
			for (int i = 0; i < count; i++)
			{
				if (autoSortComparer.Compare(item, nextList[i]) <= 0)
				{
					index = i;
					break;
				}
			}
		}

		nextList.Insert(index, item);
		if (freezeCount <= 0)
		{
			currentList.Insert(index, item);
			return;
		}

		updateRequired = true;
		if (!autoSort)
		{
			sortRequired = true;
		}
	}

	public void Insert(int index, T item)
	{
		throw new NotImplementedException();
	}

	public bool Remove(T item)
	{
		bool result = nextList.Remove(item);
		if (freezeCount <= 0)
		{
			currentList.Remove(item);
			return result;
		}

		updateRequired = true;
		return result;
	}

	public void RemoveAt(int index)
	{
		nextList.RemoveAt(index);
		if (freezeCount <= 0)
		{
			currentList.RemoveAt(index);
		}
		else
		{
			updateRequired = true;
		}
	}

	public void Clear()
	{
		nextList.Clear();
		if (freezeCount <= 0)
		{
			currentList.Clear();
		}
		else
		{
			updateRequired = true;
		}
	}

	public bool Contains(T item)
	{
		return currentList.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		currentList.CopyTo(array, arrayIndex);
	}

	public int IndexOf(T item)
	{
		return currentList.IndexOf(item);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return currentList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return currentList.GetEnumerator();
	}

	public T[] ToArray()
	{
		return nextList.ToArray();
	}

	public List<T> ToList()
	{
		return new List<T>(nextList);
	}

	public void Freeze()
	{
		freezeCount++;
	}

	public void Thaw()
	{
		freezeCount--;
		if (freezeCount <= 0)
		{
			if (updateRequired && sortRequired && autoSort)
			{
				Sort(nextList, autoSortComparer);
				sortRequired = false;
			}

			TryUpdate();
			if (autoSort && sortRequired)
			{
				TrySort(autoSortComparer);
			}
		}
	}

	public void TryUpdate()
	{
		if (updateRequired)
		{
			currentList.Clear();
			currentList.AddRange(nextList);
			updateRequired = false;
		}
	}

	public void TrySort(IComparer<T> comparer)
	{
		if (sortRequired)
		{
			Sort(currentList, comparer);
			nextList.Clear();
			nextList.AddRange(currentList);
			sortRequired = false;
		}
	}

	public static void Sort(List<T> list, IComparer<T> comparer)
	{
		T[] collection = list.OrderBy((T a) => a, comparer).ToArray();
		list.Clear();
		list.AddRange(collection);
	}
}
