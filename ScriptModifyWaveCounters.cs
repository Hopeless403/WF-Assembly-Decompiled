#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Modify Wave Counters", menuName = "Scripts/Modify Wave Counters")]
public class ScriptModifyWaveCounters : Script
{
	[SerializeField]
	public int change = -1;

	[SerializeField]
	public bool normalBattles = true;

	[SerializeField]
	public bool bossBattles = true;

	[SerializeField]
	public bool finalBossBattles = true;

	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (!node.type.isBattle || (!normalBattles && node.type.name == "CampaignNodeBattle") || (!bossBattles && node.type.name == "CampaignNodeBoss"))
			{
				continue;
			}

			if (!finalBossBattles)
			{
				string text = node.type.name;
				if (text == "CampaignNodeFinalBoss" || text == "CampaignNodeFinalFinalBoss")
				{
					continue;
				}
			}

			BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
			BattleWaveManager.WaveData[] array = saveCollection;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].counter += change;
			}

			node.data["waves"] = new SaveCollection<BattleWaveManager.WaveData>(saveCollection);
		}

		yield break;
	}
}
