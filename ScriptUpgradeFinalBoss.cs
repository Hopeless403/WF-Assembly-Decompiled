#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Final Boss", menuName = "Scripts/Upgrade Final Boss")]
public class ScriptUpgradeFinalBoss : Script
{
	[SerializeField]
	public CardUpgradeData attackUpgrade;

	[SerializeField]
	public CardUpgradeData effectsUpgrade;

	[SerializeField]
	public TargetConstraint canBeBoostedConstraint;

	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			AddUpgrade(node);
		}

		yield break;
	}

	public void AddUpgrade(CampaignNode node)
	{
		if (!node.type.isBattle || !node.type.isBoss || !(node.type.name == "CampaignNodeFinalBoss"))
		{
			return;
		}

		BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
		foreach (BattleWaveManager.WaveData waveData in saveCollection)
		{
			if (!waveData.isBossWave)
			{
				continue;
			}

			for (int j = 0; j < waveData.Count; j++)
			{
				CardData cardData = waveData.GetCardData(j);
				if (cardData.cardType.miniboss)
				{
					CardUpgradeData upgradeData = GetUpgrade(cardData).Clone();
					waveData.AddUpgradeToCard(j, upgradeData);
				}
			}
		}
	}

	public CardUpgradeData GetUpgrade(CardData cardData)
	{
		if (!canBeBoostedConstraint.Check(cardData))
		{
			return attackUpgrade;
		}

		return effectsUpgrade;
	}
}
