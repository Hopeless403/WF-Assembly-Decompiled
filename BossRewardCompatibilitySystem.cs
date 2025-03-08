#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BossRewardCompatibilitySystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnCampaignLoaded += CampaignLoaded;
	}

	public void OnDisable()
	{
		Events.OnCampaignLoaded -= CampaignLoaded;
	}

	public void CampaignLoaded()
	{
		bool flag = false;
		foreach (CampaignNode node in Campaign.instance.nodes)
		{
			if (!node.type.isBattle || !(node.type is CampaignNodeTypeBoss) || node.data.ContainsKey("rewards"))
			{
				continue;
			}

			if (!flag)
			{
				CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
				if ((object)component != null)
				{
					component.Populate(References.PlayerData.classData);
					component.RemoveLockedCards();
					flag = true;
				}
			}

			CampaignNodeTypeBoss.GetRewards(node);
			Debug.Log($"Boss Node [{node.name}][{node.id}] doesn't contain any boss reward data! Pulling new rewards!");
		}
	}
}
