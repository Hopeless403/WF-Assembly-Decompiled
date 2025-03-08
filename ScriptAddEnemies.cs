#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Enemies", menuName = "Scripts/Add Enemies")]
public class ScriptAddEnemies : Script
{
	[Serializable]
	public class Profile
	{
		public BattleData battleData;

		public int add = 2;

		public int toWave = 1;

		public bool randomPosition;

		public CardData[] pool;
	}

	[SerializeField]
	public Profile[] profiles;

	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (node.type.isBattle)
			{
				string targetBattleName = (string)node.data["battle"];
				Profile profile = profiles.FirstOrDefault((Profile a) => a.battleData.name == targetBattleName);
				if (profile != null)
				{
					BattleWaveManager.WaveData obj = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves")[profile.toWave];
					int count = obj.Count;
					int insertPos = (profile.randomPosition ? Dead.Random.Range(0, count) : count);
					InsertTo(obj, insertPos, profile.add, profile.pool);
				}
			}
		}

		yield break;
	}

	public static void InsertTo(BattleWaveManager.WaveData waveData, int insertPos, int count, CardData[] fromPool)
	{
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < count; i++)
		{
			if (list.Count <= 0)
			{
				list.AddRange(fromPool);
			}

			CardData cardData = list.TakeRandom();
			if (waveData is BattleWaveManager.WaveDataFull)
			{
				waveData.InsertCard(insertPos, cardData.Clone());
			}
			else
			{
				waveData.InsertCard(insertPos, cardData);
			}
		}
	}
}
