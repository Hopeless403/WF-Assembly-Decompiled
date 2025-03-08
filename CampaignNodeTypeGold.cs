#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignNodeTypeGold", menuName = "Campaign/Node Type/Gold")]
public class CampaignNodeTypeGold : CampaignNodeType
{
	[SerializeField]
	public Vector2Int amountRange = new Vector2Int(40, 80);

	[SerializeField]
	public float pauseAfter = 1.5f;

	public override IEnumerator SetUp(CampaignNode node)
	{
		node.data = new Dictionary<string, object> { 
		{
			"amount",
			amountRange.Random()
		} };
		yield return null;
	}

	public override IEnumerator Run(CampaignNode node)
	{
		Character player = References.Player;
		Vector3 position = Vector3.zero;
		MapNew mapNew = Object.FindObjectOfType<MapNew>();
		if ((object)mapNew != null)
		{
			MapNode mapNode = mapNew.FindNode(node);
			if ((object)mapNode != null)
			{
				position = mapNode.transform.position;
			}
		}

		if ((bool)player && (bool)player.data?.inventory)
		{
			Events.InvokeDropGold(node.data.Get<int>("amount"), "GoldCave", player, position);
		}

		node.data["amount"] = 0;
		node.SetCleared();
		yield return Sequences.Wait(pauseAfter);
		References.Map.Continue();
	}

	public override bool HasMissingData(CampaignNode node)
	{
		return false;
	}
}
