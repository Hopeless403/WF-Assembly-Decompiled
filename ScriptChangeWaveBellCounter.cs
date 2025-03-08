#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Wave Bell Counter", menuName = "Scripts/Change Wave Bell Counter")]
public class ScriptChangeWaveBellCounter : Script
{
	[SerializeField]
	public bool set;

	[SerializeField]
	[HideIf("set")]
	public bool add = true;

	[SerializeField]
	public int value = 1;

	public override IEnumerator Run()
	{
		if (set)
		{
			Set();
		}
		else if (add)
		{
			Add();
		}

		yield break;
	}

	public void Add()
	{
		foreach (CampaignNode node in Campaign.instance.nodes)
		{
			if (!node.cleared && node.type.isBattle && node.data.TryGetValue("waves", out var obj) && obj is SaveCollection<BattleWaveManager.WaveData> waves)
			{
				Add(waves);
			}
		}
	}

	public void Set()
	{
		foreach (CampaignNode node in Campaign.instance.nodes)
		{
			if (!node.cleared && node.type.isBattle && node.data.TryGetValue("waves", out var obj) && obj is SaveCollection<BattleWaveManager.WaveData> waves)
			{
				Set(waves);
			}
		}
	}

	public void Add(SaveCollection<BattleWaveManager.WaveData> waves)
	{
		for (int i = 0; i < waves.Count; i++)
		{
			waves[i].counter += value;
		}
	}

	public void Set(SaveCollection<BattleWaveManager.WaveData> waves)
	{
		for (int i = 0; i < waves.Count; i++)
		{
			waves[i].counter = value;
		}
	}
}
