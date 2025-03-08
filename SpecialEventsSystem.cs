#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEventsSystem : MonoBehaviour
{
	[Serializable]
	public struct Event
	{
		public CampaignNodeType nodeType;

		public UnlockData requiresUnlock;

		public string[] replaceNodeTypes;

		public int minTier;

		public Vector2Int perTier;

		public Vector2Int perRun;
	}

	[SerializeField]
	public Event[] events;

	public void OnEnable()
	{
		Events.OnPreCampaignPopulate += PreCampaignPopulate;
	}

	public void OnDisable()
	{
		Events.OnPreCampaignPopulate -= PreCampaignPopulate;
	}

	public void PreCampaignPopulate()
	{
		List<List<CampaignNode>> tiers = CreateListOfNodes();
		Event[] array = events;
		foreach (Event specialEvent in array)
		{
			InsertSpecialEvent(tiers, specialEvent);
		}
	}

	public static void InsertSpecialEvent(List<List<CampaignNode>> tiers, Event specialEvent)
	{
		if ((bool)specialEvent.requiresUnlock && Campaign.Data.GameMode.mainGameMode && !MetaprogressionSystem.IsUnlocked(specialEvent.requiresUnlock))
		{
			return;
		}

		int num = specialEvent.perRun.Random();
		int num2 = 0;
		int[] array = new int[tiers.Count];
		do
		{
			foreach (List<CampaignNode> item in tiers.InRandomOrder())
			{
				int num3 = specialEvent.perTier.Random();
				if (num3 > 0)
				{
					foreach (CampaignNode item2 in item)
					{
						if (item2.tier < specialEvent.minTier || array[item2.tier] >= specialEvent.perTier.y)
						{
							break;
						}

						if (specialEvent.replaceNodeTypes.Contains(item2.type.name))
						{
							Debug.Log($"SpecialEventSystem → Replacing [{item2} ({item2.type.name}) tier {item2.tier}] with ({specialEvent.nodeType.name})");
							item2.SetType(specialEvent.nodeType);
							array[item2.tier]++;
							num2++;
							if (array[item2.tier] >= num3 || num2 >= num)
							{
								break;
							}
						}
					}
				}

				if (num2 >= num)
				{
					break;
				}
			}
		}
		while (num2 < specialEvent.perRun.x);
	}

	public static List<List<CampaignNode>> CreateListOfNodes()
	{

		List<List<CampaignNode>> list = new List<List<CampaignNode>>();
		int a = 0;
		foreach (CampaignNode node in Campaign.instance.nodes)
		{
			if (node.tier >= 0)
			{
				a = Mathf.Max(a, node.tier + 1);
				while (list.Count <= node.tier)
				{
					list.Add(new List<CampaignNode>());
				}

				int index = UnityEngine.Random.Range(0, list[node.tier].Count - 1);
				list[node.tier].Insert(index, node);
			}
		}

		return list;
	}
}
