#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;

public class ChallengeListenerSystemWinWithoutCharms : ChallengeListenerSystem
{
	public void OnEnable()
	{
		Events.OnCampaignEnd += CampaignEnd;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
	}

	public void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		if (result != Campaign.Result.Win)
		{
			return;
		}

		bool flag = false;
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			if (item.upgrades.Any((CardUpgradeData a) => a.type == CardUpgradeData.Type.Charm))
			{
				flag = true;
				break;
			}
		}

		if (!flag)
		{
			Complete();
		}
	}
}
