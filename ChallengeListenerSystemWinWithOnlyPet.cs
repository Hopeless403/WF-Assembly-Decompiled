#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ChallengeListenerSystemWinWithOnlyPet : ChallengeListenerSystem
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

		string item = null;
		int num = 0;
		foreach (CardData item2 in References.PlayerData.inventory.deck)
		{
			if (item2.cardType.name == "Friendly")
			{
				if (++num > 1)
				{
					item = null;
					break;
				}

				item = item2.name;
			}
		}

		if (num == 1 && MetaprogressionSystem.GetUnlockedPets().Contains(item))
		{
			Complete();
		}
	}
}
