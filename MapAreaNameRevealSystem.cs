#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class MapAreaNameRevealSystem : GameSystem
{
	public readonly Dictionary<int, CampaignNode> areaNameNodes = new Dictionary<int, CampaignNode>();

	public void OnEnable()
	{
		Events.OnMapNodeReveal += MapNodeReveal;
		foreach (CampaignNode node in References.Campaign.nodes)
		{
			if (!node.revealed && !node.type.interactable && node.type.letter.StartsWith("area"))
			{
				areaNameNodes[node.areaIndex] = node;
			}
		}
	}

	public void OnDisable()
	{
		Events.OnMapNodeReveal -= MapNodeReveal;
	}

	public void MapNodeReveal(MapNode mapNode)
	{
		if (areaNameNodes.TryGetValue(mapNode.campaignNode.areaIndex, out var value))
		{
			References.Map.FindNode(value).Reveal();
			areaNameNodes.Remove(value.areaIndex);
		}
	}
}
