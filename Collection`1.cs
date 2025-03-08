#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public class Collection<T> : ScriptableObject where T : class
{
	[Serializable]
	public struct Entry<T1>
	{
		public T1 value;

		public float weight;
	}

	public float nullWeight;

	[SerializeField]
	public Entry<T>[] weightedList;

	public T this[int index] => weightedList[index].value;

	public void AddWeight(int index, float weight)
	{
		weightedList[index].weight += weight;
	}

	public T RandomItem()
	{
		int num = RandomIndex();
		if (num >= 0)
		{
			return weightedList[num].value;
		}

		return null;
	}

	public virtual int RandomIndex()
	{
		int result = -1;
		if (weightedList.Length != 0)
		{
			float num = nullWeight;
			Entry<T>[] array = weightedList;
			for (int i = 0; i < array.Length; i++)
			{
				Entry<T> entry = array[i];
				if (entry.weight > 0f)
				{
					num += entry.weight;
				}
			}

			if (num > 0f)
			{
				float num2 = UnityEngine.Random.value * num;
				if (num2 >= nullWeight)
				{
					num2 -= nullWeight;
					for (int j = 0; j < weightedList.Length; j++)
					{
						Entry<T> entry2 = weightedList[j];
						if (entry2.weight > 0f)
						{
							if (num2 < entry2.weight)
							{
								result = j;
								break;
							}

							num2 -= entry2.weight;
						}
					}
				}
			}
		}

		return result;
	}
}
