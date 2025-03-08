#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class RunHistorySystem : GameSystem
{
	public const int max = 50;

	public void OnEnable()
	{
		Events.OnCampaignEnd += CampaignEnd;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
	}

	public static void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		Debug.Log("Saving Run History...");
		List<RunHistory> list = SaveSystem.LoadHistoryData<List<RunHistory>>("list") ?? new List<RunHistory>();
		list.Add(new RunHistory(Campaign.Data.GameMode, result, stats, playerData));
		if (list.Count > 50)
		{
			list.RemoveAt(0);
		}

		SaveSystem.SaveHistoryData("list", list);
		Debug.Log("Run History Saved!");
	}
}
