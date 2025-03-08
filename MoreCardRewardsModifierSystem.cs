#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class MoreCardRewardsModifierSystem : MonoBehaviour
{
	public const int toAdd = 1;

	public readonly List<int> tiersToAddTo = new List<int>();

	public void OnEnable()
	{
		Events.OnPullRewards += PullRewards;
		tiersToAddTo.Clear();
		List<int> list = new List<int> { 0, 1, 2, 3, 4 };
		Random.State state = Random.state;
		Random.InitState(Campaign.Data.Seed);
		for (int i = 0; i < 2; i++)
		{
			if (list.Count <= 0)
			{
				break;
			}

			tiersToAddTo.Add(list.TakeRandom());
		}

		Random.state = state;
		Debug.Log("MoreCardRewardsModifierSystem → tiers to add to: [" + string.Join(", ", tiersToAddTo) + "]");
	}

	public void OnDisable()
	{
		Events.OnPullRewards -= PullRewards;
	}

	public void PullRewards(object pulledBy, ref string poolName, ref int itemCount, ref List<DataFile> result)
	{
		if (pulledBy is CampaignNode campaignNode && tiersToAddTo.Contains(campaignNode.tier) && campaignNode.type is CampaignNodeTypeItem)
		{
			itemCount++;
			Debug.Log($"MoreCardRewardsModifierSystem → adding +{1} to Reward Pool \"{poolName}\" Pull ({campaignNode.type.name} node index: {campaignNode.id})");
		}
	}
}
