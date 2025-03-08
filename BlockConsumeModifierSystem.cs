#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class BlockConsumeModifierSystem : GameSystem
{
	public const int toBlockPerBattle = 1;

	public int toBlock;

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
		Events.OnBattleLoaded += BattleLoaded;
		Events.OnActionQueued += ActionQueued;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
		Events.OnBattleLoaded -= BattleLoaded;
		Events.OnActionQueued -= ActionQueued;
	}

	public void BattleStart()
	{
		toBlock = 1;
		SaveData();
	}

	public void BattleLoaded()
	{
		LoadData();
	}

	public void ActionQueued(PlayAction action)
	{
		if (toBlock > 0 && action is ActionConsume actionConsume && !actionConsume.blocked)
		{
			toBlock--;
			actionConsume.Block();
			SaveData();
		}
	}

	public void SaveData()
	{
		int id = Campaign.FindCharacterNode(References.Battle.player).id;
		SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "BlockConsumeModifierSystemBattleId", id);
		SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "BlockConsumeModifierSystemToBlock", toBlock);
	}

	public void LoadData()
	{
		int id = Campaign.FindCharacterNode(References.Battle.player).id;
		if (SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "BlockConsumeModifierSystemBattleId", -1) == id)
		{
			toBlock = SaveSystem.LoadCampaignData(Campaign.Data.GameMode, "BlockConsumeModifierSystemToBlock", 0);
		}
	}
}
