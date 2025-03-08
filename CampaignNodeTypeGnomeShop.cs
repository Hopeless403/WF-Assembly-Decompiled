#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeGnomeShop", menuName = "Campaign/Node Type/Gnome Shop")]
public class CampaignNodeTypeGnomeShop : CampaignNodeTypeEvent
{
	[SerializeField]
	public int poolSize = 10;

	[SerializeField]
	public int rerollPrice = 10;

	[SerializeField]
	public int rerollPriceAdd = 5;

	public override IEnumerator SetUp(CampaignNode node)
	{
		CardData[] fromOriginalList = References.Player.GetComponent<CharacterRewards>().GetFromOriginalList<CardData>(node, "Items", poolSize, allowDuplicates: false);
		EventRoutineGnomeShop.Data value = new EventRoutineGnomeShop.Data
		{
			pool = fromOriginalList.Select((CardData a) => a.name).ToArray(),
			price = rerollPrice,
			priceAdd = rerollPriceAdd
		};
		node.data = new Dictionary<string, object> { { "data", value } };
		yield break;
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return false;
	}

	public override IEnumerator Populate(CampaignNode node)
	{
		EventRoutineGnomeShop eventRoutineGnomeShop = Object.FindObjectOfType<EventRoutineGnomeShop>();
		eventRoutineGnomeShop.node = node;
		yield return eventRoutineGnomeShop.Populate();
	}
}
