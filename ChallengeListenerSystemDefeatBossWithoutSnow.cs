#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ChallengeListenerSystemDefeatBossWithoutSnow : ChallengeListenerSystem
{
	public const string effectType = "snow";

	public int nodeId;

	public bool snowApplied;

	public bool isBossBattle;

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
		Events.OnBattleLoaded += BattleLoaded;
		Events.OnBattleSaved += BattleSaved;
		Events.OnBattleEnd += BattleEnd;
		Events.OnStatusEffectApplied += StatusApplied;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
		Events.OnBattleLoaded -= BattleLoaded;
		Events.OnBattleSaved -= BattleSaved;
		Events.OnBattleEnd -= BattleEnd;
		Events.OnStatusEffectApplied -= StatusApplied;
	}

	public void StatusApplied(StatusEffectApply apply)
	{
		if (isBossBattle && !snowApplied && apply.target.owner.team == References.Battle.enemy.team && apply.effectData.type == "snow" && apply.target.data.cardType.miniboss)
		{
			snowApplied = true;
		}
	}

	public void BattleStart()
	{
		CampaignNode campaignNode = Campaign.FindCharacterNode(References.Player);
		isBossBattle = campaignNode.type.isBattle && campaignNode.type.isBoss;
		if (isBossBattle)
		{
			nodeId = campaignNode.id;
			snowApplied = false;
		}
	}

	public void BattleLoaded()
	{
		CampaignNode campaignNode = Campaign.FindCharacterNode(References.Player);
		isBossBattle = campaignNode.type.isBattle && campaignNode.type.isBoss;
		if (isBossBattle)
		{
			nodeId = SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "DefeatBossWithoutSnowNodeId", -1);
			if (nodeId == campaignNode.id)
			{
				snowApplied = SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "DefeatBossWithoutSnowSnowApplied", defaultValue: false);
			}
		}
	}

	public void BattleSaved()
	{
		if (isBossBattle && nodeId >= 0)
		{
			SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "DefeatBossWithoutSnowNodeId", nodeId);
			SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "DefeatBossWithoutSnowSnowApplied", snowApplied);
		}
	}

	public void BattleEnd()
	{
		if (isBossBattle && !snowApplied && References.Battle.winner == References.Player)
		{
			Complete();
		}

		isBossBattle = false;
		snowApplied = false;
		nodeId = -1;
	}
}
