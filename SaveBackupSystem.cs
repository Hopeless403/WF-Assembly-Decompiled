#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class SaveBackupSystem : GameSystem
{
	public const float backupCampaignTimer = 5f;

	public static float backupCampaignCooldown = 5f;

	public void OnEnable()
	{
		Events.OnCampaignStart += CampaignStart;
		Events.OnCampaignSaved += CampaignSaved;
	}

	public void OnDisable()
	{
		Events.OnCampaignStart -= CampaignStart;
		Events.OnCampaignSaved -= CampaignSaved;
	}

	public void Update()
	{
		if (backupCampaignCooldown > 0f)
		{
			backupCampaignCooldown -= Time.unscaledDeltaTime;
		}
	}

	public static void CampaignStart()
	{
		string text = SaveSystem.folderName + "/";
		Backup(text + "Save.sav");
		Backup(text + "Stats.sav");
		Backup(text + "History.sav");
	}

	public static void CampaignSaved()
	{
		if (!(backupCampaignCooldown > 0f) && (bool)References.Campaign && Campaign.Data.GameMode.doSave)
		{
			Backup(string.Concat(SaveSystem.folderName + "/", "Campaign", Campaign.Data.GameMode.saveFileName, ".sav"));
			backupCampaignCooldown = 5f;
		}
	}

	public static void Backup(string filePath)
	{
		StopWatch.Start();
		ES3.CreateBackup(filePath, SaveSystem.settings);
		Debug.Log($"Backup Created ({filePath}) ({StopWatch.Stop()} ms)");
	}
}
