#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public struct SaveCollection<T> : ICloneable
{
	public T[] collection;

	public int Count => collection.Length;

	public T this[int index]
	{
		get
		{
			return collection[index];
		}
		set
		{
			collection[index] = value;
		}
	}

	public SaveCollection(T[] collection)
	{
		this.collection = collection;
	}

	public SaveCollection(List<T> collection)
	{
		this.collection = collection.ToArray();
	}

	public SaveCollection(T item)
	{
		collection = new T[1] { item };
	}

	public void Add(T item)
	{
		List<T> list = collection.ToList();
		list.Add(item);
		collection = list.ToArray();
	}

	public void Remove(int index)
	{
		List<T> list = collection.ToList();
		list.RemoveAt(index);
		collection = list.ToArray();
	}

	public object Clone()
	{
		SaveCollection<T> saveCollection = default(SaveCollection<T>);
		saveCollection.collection = collection.ToArray();
		return saveCollection;
	}
}
