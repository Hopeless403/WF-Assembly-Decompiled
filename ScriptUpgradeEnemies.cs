#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Upgrade Enemies", menuName = "Scripts/Upgrade Enemies")]
public class ScriptUpgradeEnemies : Script
{
	[Serializable]
	public struct Tier
	{
		public Vector2Int upgradesRange;

		public bool canAffectMiniboss;
	}

	[SerializeField]
	public string[] illegalCards = new string[1] { "Gobling" };

	[SerializeField]
	public CardUpgradeData[] upgradePool;

	[SerializeField]
	public Tier[] tiers;

	public override IEnumerator Run()
	{
		List<CardUpgradeData> currentPool = new List<CardUpgradeData>();
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (node.type.isBattle)
			{
				BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
				int num = ((tiers.Length > node.tier) ? tiers[node.tier].upgradesRange.Random() : 0);
				if (num > 0)
				{
					AddUpgrades(currentPool, saveCollection, num, tiers[node.tier].canAffectMiniboss);
				}
			}
		}

		yield break;
	}

	public void AddUpgrades(List<CardUpgradeData> currentPool, BattleWaveManager.WaveData[] waves, int upgradeCount, bool canUpgradeMiniboss)
	{
		foreach (BattleWaveManager.WaveData wave in waves)
		{
			if (TryAddUpgrade(currentPool, wave, canUpgradeMiniboss) && --upgradeCount <= 0)
			{
				break;
			}
		}
	}

	public bool TryAddUpgrade(List<CardUpgradeData> currentPool, BattleWaveManager.WaveData wave, bool canUpgradeMiniboss)
	{
		bool result = false;
		List<int> list = GenericPool<List<int>>.Get();
		list.Clear();
		for (int i = 0; i < wave.Count; i++)
		{
			list.Add(i);
		}

		foreach (int item in list.InRandomOrder())
		{
			if (TryAddUpgrade(currentPool, wave, item, canUpgradeMiniboss))
			{
				result = true;
				break;
			}
		}

		GenericPool<List<int>>.Release(list);
		return result;
	}

	public bool TryAddUpgrade(List<CardUpgradeData> currentPool, BattleWaveManager.WaveData wave, int cardIndex, bool canUpgradeMiniboss)
	{
		if (IllegalCard(wave.GetCardName(cardIndex)))
		{
			return false;
		}

		CardData cardData = wave.PeekCardData(cardIndex);
		if (!cardData)
		{
			return false;
		}

		if (cardData.cardType.miniboss && !canUpgradeMiniboss)
		{
			return false;
		}

		CardUpgradeData cardUpgradeData = ((currentPool.Count > 0) ? currentPool.FirstOrDefault((CardUpgradeData a) => wave.AddUpgradeToCard(cardIndex, a)) : null);
		if (!cardUpgradeData)
		{
			currentPool.AddRange(upgradePool.InRandomOrder());
			cardUpgradeData = currentPool.FirstOrDefault((CardUpgradeData a) => wave.AddUpgradeToCard(cardIndex, a));
		}

		if ((bool)cardUpgradeData)
		{
			currentPool.Remove(cardUpgradeData);
			return true;
		}

		return false;
	}

	public bool IllegalCard(string cardDataName)
	{
		return illegalCards.Contains(cardDataName);
	}
}
