#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeCharm", menuName = "Campaign/Node Type/Charm")]
public class CampaignNodeTypeCharm : CampaignNodeTypeEvent
{
	[SerializeField]
	public CardUpgradeData force;

	public override IEnumerator SetUp(CampaignNode node)
	{
		yield return null;
		CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
		CardUpgradeData cardUpgradeData;
		if ((bool)force)
		{
			cardUpgradeData = force;
			component.PullOut("Charms", cardUpgradeData);
		}
		else
		{
			cardUpgradeData = component.Pull<CardUpgradeData>(node, "Charms");
		}

		node.data = new Dictionary<string, object>
		{
			{ "open", false },
			{ "charm", cardUpgradeData.name }
		};
	}

	public override bool HasMissingData(CampaignNode node)
	{
		if (node.data.TryGetValue("charm", out var value) && value is string assetName)
		{
			return AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", assetName) == null;
		}

		return true;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineCharm eventRoutineCharm = Object.FindObjectOfType<EventRoutineCharm>();
		eventRoutineCharm.node = node;
		yield return eventRoutineCharm.Populate();
	}
}
