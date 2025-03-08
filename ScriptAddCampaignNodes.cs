#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Campaign Nodes", menuName = "Scripts/Add Campaign Nodes")]
public class ScriptAddCampaignNodes : Script
{
	[Serializable]
	public class Event
	{
		public CampaignNodeType type;

		public Vector2Int tierRange;
	}

	[SerializeField]
	public Event[] events;

	public override IEnumerator Run()
	{
		Routine.Clump clump = new Routine.Clump();
		Event[] array = events;
		foreach (Event @event in array)
		{
			int num = @event.tierRange.Random();
			List<CampaignNode> list = new List<CampaignNode>();
			foreach (CampaignNode node2 in References.Campaign.nodes)
			{
				if (node2.tier == num && node2.type.interactable && node2.connections.Count > 1)
				{
					list.Add(node2);
				}
			}

			CampaignNode campaignNode = list.RandomItem();
			int num2 = References.Campaign.nodes.IndexOf(campaignNode);
			CampaignNode campaignNode2 = new CampaignNode(@event.type, campaignNode.position.x, campaignNode.position.y, num, campaignNode.positionIndex, campaignNode.areaIndex, campaignNode.radius);
			References.Campaign.nodes.Insert(num2 + 1, campaignNode2);
			campaignNode2.id = num2;
			campaignNode2.connections = campaignNode.connections.Clone();
			int count = References.Campaign.nodes.Count;
			for (int j = num2 + 1; j < count; j++)
			{
				CampaignNode campaignNode3 = References.Campaign.nodes[j];
				campaignNode3.id++;
				foreach (CampaignNode.Connection connection in campaignNode3.connections)
				{
					connection.otherId++;
				}
			}

			List<int> list2 = new List<int> { campaignNode2.id };
			while (list2.Count > 0)
			{
				CampaignNode node = Campaign.GetNode(list2[0]);
				node.position.x += 1f;
				list2.RemoveAt(0);
				foreach (int item in node.connections.Select((CampaignNode.Connection a) => a.otherId))
				{
					if (!list2.Contains(item))
					{
						list2.Add(item);
					}
				}
			}

			campaignNode.connections.Clear();
			campaignNode.ConnectTo(campaignNode2);
			clump.Add(campaignNode2.type.SetUp(campaignNode2));
		}

		yield return clump.WaitForEnd();
		CampaignGenerator.ShuffleNodes(References.Campaign.nodes);
	}
}
