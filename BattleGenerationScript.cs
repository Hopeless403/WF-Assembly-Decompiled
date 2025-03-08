#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public abstract class BattleGenerationScript : ScriptableObject
{
	public class WaveList
	{
		public List<BattleWavePoolData.Wave> list;

		public int value;

		public int targetValue;

		public int Count => list.Count;

		public WaveList(int targetValue)
		{
			this.targetValue = targetValue;
		}

		public void Add(BattleWavePoolData.Wave wave)
		{
			if (list == null)
			{
				list = new List<BattleWavePoolData.Wave>();
			}

			if (!wave.fixedOrder)
			{
				wave.units.Shuffle();
			}

			int num = list.Count;
			int positionPriority = wave.positionPriority;
			for (int i = 0; i < list.Count; i++)
			{
				int positionPriority2 = list[i].positionPriority;
				if (positionPriority2 < positionPriority)
				{
					continue;
				}

				num = i;
				if (positionPriority2 != positionPriority)
				{
					break;
				}

				for (int j = i + 1; j < list.Count; j++)
				{
					if (list[j].positionPriority > positionPriority)
					{
						num = RandomInclusive.Range(num, j);
						break;
					}
				}

				break;
			}

			list.Insert(num, wave);
			value += wave.value;
		}

		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
		}

		public BattleWavePoolData.Wave GetWave(int waveIndex)
		{
			return list[waveIndex];
		}

		public void AddUnit(int waveIndex, CardData unit)
		{
			GetWave(waveIndex).units.Add(unit);
		}

		public bool Satisfied()
		{
			return value >= targetValue;
		}
	}

	public virtual SaveCollection<BattleWaveManager.WaveData> Run(BattleData battleData, int points)
	{
		return default(SaveCollection<BattleWaveManager.WaveData>);
	}

	public void AddGoldGivers(WaveList waves, BattleData battleData)
	{
		if (battleData.goldGivers <= 0 || battleData.goldGiverPool.Length == 0)
		{
			return;
		}

		List<int> list = new List<int>();
		for (int i = 1; i < waves.Count - 1; i++)
		{
			list.Add(i);
		}

		for (int j = 0; j < battleData.goldGivers; j++)
		{
			if (list.Count <= 0)
			{
				break;
			}

			int num = list.RandomItem();
			list.Remove(num);
			waves.AddUnit(num, battleData.goldGiverPool.RandomItem());
		}
	}

	public void AddBonusUnits(WaveList waves, BattleData battleData)
	{
		int num = battleData.bonusUnitRange.Random();
		if (num <= 0 || battleData.bonusUnitPool.Length == 0)
		{
			return;
		}

		List<int> list = new List<int>();
		for (int i = 0; i < waves.Count; i++)
		{
			if (waves.GetWave(i).CanAddTo())
			{
				list.Add(i);
			}
		}

		for (int j = 0; j < num; j++)
		{
			if (list.Count <= 0)
			{
				break;
			}

			int num2 = list.RandomItem();
			list.Remove(num2);
			waves.AddUnit(num2, battleData.bonusUnitPool.RandomItem());
		}
	}

	public BattleGenerationScript()
	{
	}
}
