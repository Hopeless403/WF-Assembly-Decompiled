#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeBoss", menuName = "Campaign/Node Type/Boss")]
public class CampaignNodeTypeBoss : CampaignNodeTypeBattle
{
	[Serializable]
	public class RewardData
	{
		public List<BossRewardData.Data> rewards;

		public int canTake;
	}

	public static void GetRewards(CampaignNode node)
	{
		BossRewardPool bossRewardPool = MonoBehaviourSingleton<References>.instance.bossRewardPools.FirstOrDefault((BossRewardPool a) => a.areaIndex == node.areaIndex);
		if ((bool)bossRewardPool)
		{
			List<BossRewardData.Data> rewards = GetRewards(bossRewardPool);
			CampaignNode campaignNode = node;
			if (campaignNode.data == null)
			{
				campaignNode.data = new Dictionary<string, object>();
			}

			node.data.Add("rewards", new RewardData
			{
				rewards = rewards,
				canTake = bossRewardPool.canTake
			});
		}
	}

	public static List<BossRewardData.Data> GetRewards(BossRewardPool pool)
	{
		List<BossRewardData.Data> list = new List<BossRewardData.Data>();
		UnityEngine.Random.InitState(Campaign.FindCharacterNode(References.Player).seed);
		foreach (BossRewardData item2 in pool.bossRewards.InRandomOrder())
		{
			BossRewardData.Data item = item2.Pull();
			list.Add(item);
		}

		return list;
	}
}
