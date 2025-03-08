#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeCompanion", menuName = "Campaign/Node Type/Companion")]
public class CampaignNodeTypeCompanion : CampaignNodeTypeEvent
{
	[SerializeField]
	public int choices = 3;

	[SerializeField]
	public List<CardData> force;

	public override IEnumerator SetUp(CampaignNode node)
	{
		yield return null;
		CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
		List<CardData> list = force.Clone();
		if (list.Count > 0)
		{
			component.PullOut("Units", list);
		}

		int itemCount = choices - list.Count;
		list.AddRange(component.Pull<CardData>(node, "Units", itemCount));
		node.data = new Dictionary<string, object>
		{
			{ "damage", 0 },
			{
				"cards",
				list.ToSaveCollectionOfNames()
			}
		};
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineCompanion eventRoutineCompanion = Object.FindObjectOfType<EventRoutineCompanion>();
		eventRoutineCompanion.node = node;
		yield return eventRoutineCompanion.Populate();
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return MissingCardSystem.HasMissingData(node.data.GetSaveCollection<string>("cards"));
	}
}
