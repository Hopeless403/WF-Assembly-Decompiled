#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeCopyItem", menuName = "Campaign/Node Type/Copy Item")]
public class CampaignNodeTypeCopyItem : CampaignNodeTypeEvent
{
	[SerializeField]
	public int canCopy = 1;

	public override IEnumerator SetUp(CampaignNode node)
	{
		node.data = new Dictionary<string, object>
		{
			{ "canCopy", canCopy },
			{ "enterCount", 0 }
		};
		yield return null;
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return false;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineCopyItem eventRoutineCopyItem = Object.FindObjectOfType<EventRoutineCopyItem>();
		eventRoutineCopyItem.node = node;
		yield return eventRoutineCopyItem.Populate();
	}
}
