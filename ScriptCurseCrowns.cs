#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Curse Crowns", menuName = "Scripts/Curse Crowns")]
public class ScriptCurseCrowns : Script
{
	[SerializeField]
	public int curseBossCrowns = 1;

	[SerializeField]
	public int curseShopCrowns = 1;

	[SerializeField]
	public int[] legalBossTiers = new int[1] { 2 };

	[SerializeField]
	public int[] legalShopTiers = new int[4] { 2, 3, 4, 5 };

	[SerializeField]
	public CardUpgradeData[] cursedCrowns;

	public override IEnumerator Run()
	{
		List<CardUpgradeData> pool = PopulatePool();
		int num = curseBossCrowns;
		int num2 = curseShopCrowns;
		foreach (CampaignNode item in References.Campaign.nodes.Where((CampaignNode a) => a.type.interactable && a.dataLinkedTo == -1 && a.tier >= 0 && a.data != null).InRandomOrder())
		{
			if (item.type is CampaignNodeTypeBoss && TryCurseBossCrown(item, pool))
			{
				if (--num <= 0 && num2 <= 0)
				{
					break;
				}
			}
			else if (item.type is CampaignNodeTypeShop && TryCurseShopCrown(item, pool) && --num2 <= 0 && num <= 0)
			{
				break;
			}
		}

		yield break;
	}

	public bool TryCurseBossCrown(CampaignNode node, List<CardUpgradeData> pool)
	{
		if (!legalBossTiers.Contains(node.tier))
		{
			return false;
		}

		CampaignNodeTypeBoss.RewardData rewardData = node.data.Get<CampaignNodeTypeBoss.RewardData>("rewards");
		if (rewardData != null)
		{
			foreach (BossRewardData.Data reward in rewardData.rewards)
			{
				if (reward.type == BossRewardData.Type.Crown && reward is BossRewardDataCrown.Data data && TryPullFromPool(pool, out var upgradeData))
				{
					data.upgradeDataName = upgradeData.name;
					return true;
				}
			}
		}

		return false;
	}

	public bool TryCurseShopCrown(CampaignNode node, List<CardUpgradeData> pool)
	{
		if (!legalShopTiers.Contains(node.tier))
		{
			return false;
		}

		ShopRoutine.Data data = node.data.Get<ShopRoutine.Data>("shopData");
		if (data != null && TryPullFromPool(pool, out var upgradeData))
		{
			data.crownType = upgradeData.name;
			return true;
		}

		return false;
	}

	public static bool TryPullFromPool(List<CardUpgradeData> pool, out CardUpgradeData upgradeData)
	{
		if (pool.Count > 0)
		{
			upgradeData = pool.TakeRandom();
			return true;
		}

		upgradeData = null;
		return false;
	}

	public List<CardUpgradeData> PopulatePool()
	{
		List<CardUpgradeData> list = new List<CardUpgradeData>();
		list.AddRange(cursedCrowns);
		return list;
	}
}
