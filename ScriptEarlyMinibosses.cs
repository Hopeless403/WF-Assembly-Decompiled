#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Early Minibosses", menuName = "Scripts/Early Minibosses")]
public class ScriptEarlyMinibosses : Script
{
	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (node.type.isBattle && !node.type.isBoss)
			{
				List<BattleWaveManager.WaveData> list = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves").ToList();
				BattleWaveManager.WaveData waveData = list.FirstOrDefault((BattleWaveManager.WaveData a) => a.isBossWave);
				if (waveData != null)
				{
					int num = list.IndexOf(waveData);
					list.RemoveAt(num);
					list.Insert(num - 1, waveData);
					node.data["waves"] = new SaveCollection<BattleWaveManager.WaveData>(list);
				}
			}
		}

		yield break;
	}
}
