#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class CampaignBattleRewardPool : MonoBehaviour
{
	[Serializable]
	public struct Pool
	{
		public string name;

		public List<CampaignNodeType> nodeTypes;

		public RewardData[] rewards;

		public List<RewardData> pool;

		public RewardData Pull()
		{
			if (pool == null)
			{
				pool = new List<RewardData>();
			}

			if (pool.Count <= 0)
			{
				pool.AddRange(rewards);
				pool.Shuffle();
			}

			if (pool.Count > 0)
			{
				RewardData result = pool[0];
				pool.RemoveAt(0);
				return result;
			}

			throw new Exception("CampaignBattleRewardPool IS EMPTY! God Dammit!");
		}
	}

	public Pool[] pools;
}
