#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeMuncher", menuName = "Campaign/Node Type/Muncher")]
public class CampaignNodeTypeMuncher : CampaignNodeTypeEvent
{
	[SerializeField]
	public int canEat = 2;

	public override IEnumerator SetUp(CampaignNode node)
	{
		node.data = new Dictionary<string, object>
		{
			{ "canEat", canEat },
			{ "enterCount", 0 },
			{ "openCount", 0 },
			{ "thankCount", 0 }
		};
		yield return null;
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return false;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineMuncher eventRoutineMuncher = Object.FindObjectOfType<EventRoutineMuncher>();
		eventRoutineMuncher.node = node;
		yield return eventRoutineMuncher.Populate();
	}
}
