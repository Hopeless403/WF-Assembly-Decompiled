#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Minibosses", menuName = "Scripts/Upgrade Minibosses")]
public class ScriptUpgradeMinibosses : Script
{
	[Serializable]
	public struct Profile
	{
		public string[] cardDataNames;

		public CardUpgradeData[] possibleUpgrades;
	}

	[SerializeField]
	public Profile[] profiles;

	[SerializeField]
	public CardUpgradeData[] defaultUpgrades;

	public override IEnumerator Run()
	{
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (node.type.isBattle && !(node.type.name == "CampaignNodeFinalBoss"))
			{
				BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
				AddUpgrade(saveCollection);
			}
		}

		yield break;
	}

	public void AddUpgrade(BattleWaveManager.WaveData[] waves)
	{
		foreach (BattleWaveManager.WaveData waveData in waves)
		{
			if (!waveData.isBossWave)
			{
				continue;
			}

			for (int j = 0; j < waveData.Count; j++)
			{
				CardData cardData = waveData.PeekCardData(j);
				if ((bool)cardData && cardData.cardType.miniboss)
				{
					CardUpgradeData upgrade = GetUpgrade(cardData);
					if ((object)upgrade != null)
					{
						waveData.AddUpgradeToCard(j, upgrade.Clone());
					}
				}
			}
		}
	}

	public CardUpgradeData GetUpgrade(CardData cardData)
	{
		Profile[] array = profiles;
		for (int i = 0; i < array.Length; i++)
		{
			Profile profile = array[i];
			if (profile.cardDataNames.Contains(cardData.name))
			{
				return profile.possibleUpgrades.RandomItem();
			}
		}

		return GetDefaultUpgrade(cardData);
	}

	public CardUpgradeData GetDefaultUpgrade(CardData cardData)
	{
		foreach (CardUpgradeData item in defaultUpgrades.InRandomOrder())
		{
			if (item.CanAssign(cardData))
			{
				return item;
			}
		}

		return null;
	}
}
