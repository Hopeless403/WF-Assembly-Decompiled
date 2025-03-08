#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveBattleGenerator", menuName = "Battle Generation Scripts/Waves")]
public class BattleGenerationScriptWaves : BattleGenerationScript
{
	public override SaveCollection<BattleWaveManager.WaveData> Run(BattleData battleData, int points)
	{
		Debug.Log($"Creating Waves for [{battleData}]");
		List<BattleWavePoolData> list = new List<BattleWavePoolData>();
		int num = Mathf.RoundToInt((float)points * battleData.pointFactor);
		Debug.Log($"Points: {num}");
		WaveList waveList = new WaveList(num);
		BattleWavePoolData[] pools = battleData.pools;
		for (int i = 0; i < pools.Length; i++)
		{
			BattleWavePoolData battleWavePoolData = Object.Instantiate(pools[i]);
			while (battleWavePoolData.MustPull() && battleWavePoolData.CanPull())
			{
				waveList.Add(battleWavePoolData.Pull());
			}

			for (int j = 0; j < battleWavePoolData.weight; j++)
			{
				list.Add(battleWavePoolData);
			}
		}

		while (!waveList.Satisfied() && list.Count > 0)
		{
			BattleWavePoolData battleWavePoolData2 = list.RandomItem();
			if (battleWavePoolData2 != null && battleWavePoolData2.CanPull())
			{
				waveList.Add(battleWavePoolData2.Pull());
				continue;
			}

			list.Remove(battleWavePoolData2);
			Object.Destroy(battleWavePoolData2);
		}

		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			Object.Destroy(list[num2]);
			list.RemoveAt(num2);
		}

		AddGoldGivers(waveList, battleData);
		AddBonusUnits(waveList, battleData);
		List<BattleWaveManager.WaveData> list2 = new List<BattleWaveManager.WaveData>();
		int count = waveList.Count;
		for (int k = 0; k < count; k++)
		{
			BattleWaveManager.WaveDataBasic waveDataBasic = new BattleWaveManager.WaveDataBasic
			{
				counter = battleData.waveCounter
			};
			BattleWavePoolData.Wave wave = waveList.GetWave(k);
			List<string> list3 = new List<string>();
			foreach (CardData unit in wave.units)
			{
				list3.Add(unit.name);
				if (!waveDataBasic.isBossWave && unit.cardType.miniboss)
				{
					waveDataBasic.isBossWave = true;
				}
			}

			waveDataBasic.cards = list3.Select((string a) => new BattleWaveManager.Card(a)).ToArray();
			list2.Add(waveDataBasic);
		}

		return new SaveCollection<BattleWaveManager.WaveData>(list2);
	}
}
