#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Specific Enemies", menuName = "Scripts/Upgrade Specific Enemies")]
public class ScriptUpgradeSpecificEnemies : Script
{
	[Serializable]
	public class Profile
	{
		public CardData cardData;

		public CardUpgradeData[] upgrades;
	}

	[SerializeField]
	public Profile[] profiles;

	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (!node.type.isBattle)
			{
				continue;
			}

			BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
			foreach (BattleWaveManager.WaveData waveData in saveCollection)
			{
				for (int j = 0; j < waveData.Count; j++)
				{
					string cardName = waveData.GetCardName(j);
					Profile profile = profiles.FirstOrDefault((Profile a) => a.cardData.name == cardName);
					if (profile != null)
					{
						CardUpgradeData[] upgrades = profile.upgrades;
						foreach (CardUpgradeData upgradeData in upgrades)
						{
							waveData.AddUpgradeToCard(j, upgradeData);
						}
					}
				}
			}
		}

		yield break;
	}
}
