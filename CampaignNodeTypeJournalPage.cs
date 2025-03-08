#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeJournalPage", menuName = "Campaign/Node Type/Journal Page")]
public class CampaignNodeTypeJournalPage : CampaignNodeType
{
	[SerializeField]
	public UnlockData unlock;

	public override IEnumerator Run(CampaignNode node)
	{
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		if (!unlockedList.Contains(unlock.name))
		{
			unlockedList.Add(unlock.name);
			SaveSystem.SaveProgressData("unlocked", unlockedList);
		}

		node.SetCleared();
		PauseMenu pauseMenu = Object.FindObjectOfType<PauseMenu>(includeInactive: true);
		if ((object)pauseMenu != null)
		{
			pauseMenu.OpenLorePages();
			yield return new WaitUntil(() => !pauseMenu.gameObject.activeSelf);
		}

		References.Map.Continue();
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return false;
	}
}
