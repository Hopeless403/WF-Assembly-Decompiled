#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class RunHistory
{
	public string gameModeName;

	public Campaign.Result result;

	public CampaignStats stats;

	public ClassSaveData tribe;

	public InventorySaveData inventory;

	public RunHistory()
	{
	}

	public RunHistory(GameMode gameMode, Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		gameModeName = gameMode.name;
		this.result = result;
		this.stats = stats;
		tribe = playerData.classData.Save();
		inventory = playerData.inventory.Save();
	}
}
