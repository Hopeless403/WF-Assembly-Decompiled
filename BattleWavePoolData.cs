#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle Wave Pool Data", menuName = "Battle Wave Pool")]
public class BattleWavePoolData : ScriptableObject
{
	[Serializable]
	public struct Wave
	{
		public List<CardData> units;

		public int value;

		public int positionPriority;

		public bool fixedOrder;

		public int maxSize;

		public bool CanAddTo()
		{
			if (maxSize > 0)
			{
				return units.Count < maxSize;
			}

			return true;
		}
	}

	[Range(1f, 5f)]
	public int weight = 1;

	public int forcePulls;

	public int maxPulls = 999;

	public Wave[] waves;

	public int pullCount;

	public List<Wave> workingList;

	public bool CanPull()
	{
		return pullCount < maxPulls;
	}

	public bool MustPull()
	{
		return pullCount < forcePulls;
	}

	public Wave Pull()
	{
		if (workingList == null)
		{
			workingList = new List<Wave>();
		}

		if (workingList.Count <= 0)
		{
			workingList.AddRange(waves);
		}

		if (workingList.Count > 0)
		{
			int index = workingList.RandomIndex();
			Wave result = workingList[index];
			workingList.RemoveAt(index);
			pullCount++;
			return result;
		}

		throw new Exception("BattleWavePoolData \"waves\" list is empty!");
	}

	public void Reset()
	{
		pullCount = 0;
		workingList = null;
	}
}
