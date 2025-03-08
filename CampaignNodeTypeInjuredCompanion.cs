#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeInjuredCompanion", menuName = "Campaign/Node Type/Injured Companion")]
public class CampaignNodeTypeInjuredCompanion : CampaignNodeTypeEvent
{
	public override IEnumerator SetUp(CampaignNode node)
	{
		RunHistory mostRecentRun = InjuredCompanionEventSystem.GetMostRecentRun();
		if (mostRecentRun != null)
		{
			List<CardSaveData> eligibleCompanions = InjuredCompanionEventSystem.GetEligibleCompanions(mostRecentRun);
			if (eligibleCompanions.Count > 0)
			{
				CardSaveData cardSaveData = eligibleCompanions.RandomItem();
				CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
				CardData item = AddressableLoader.Get<CardData>("CardData", cardSaveData.name);
				component.PullOut("Units", item);
				node.data = new Dictionary<string, object> { { "cardSaveData", cardSaveData } };
			}
		}

		yield return null;
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return false;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineInjuredCompanion eventRoutineInjuredCompanion = Object.FindObjectOfType<EventRoutineInjuredCompanion>();
		eventRoutineInjuredCompanion.node = node;
		yield return eventRoutineInjuredCompanion.Populate();
	}
}
