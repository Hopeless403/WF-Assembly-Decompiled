#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CampaignNodeSaveData : ILoadable<CampaignNode>
{
	public string name;

	public int id;

	public string typeName;

	public int seed;

	public Vector2 position;

	public int tier;

	public int positionIndex;

	public int areaIndex;

	public List<CampaignNode.Connection> connections;

	public List<int> characters;

	public bool revealed;

	public bool cleared;

	public bool glow;

	public bool finalNode;

	public Dictionary<string, object> data;

	public CampaignNodeSaveData()
	{
	}

	public CampaignNodeSaveData(CampaignNode node)
	{
		name = node.name;
		id = node.id;
		typeName = node.type.name;
		seed = node.seed;
		position = node.position;
		tier = node.tier;
		positionIndex = node.positionIndex;
		areaIndex = node.areaIndex;
		connections = node.connections;
		characters = node.characters;
		revealed = node.revealed;
		cleared = node.cleared;
		glow = node.glow;
		finalNode = node.finalNode;
		data = node.data;
	}

	public CampaignNode Load()
	{
		CampaignNodeType type = AddressableLoader.Get<CampaignNodeType>("CampaignNodeType", typeName);
		CampaignNode campaignNode = new CampaignNode();
		campaignNode.SetType(type);
		campaignNode.name = name;
		campaignNode.id = id;
		campaignNode.position = position;
		campaignNode.tier = tier;
		campaignNode.positionIndex = positionIndex;
		campaignNode.areaIndex = areaIndex;
		campaignNode.seed = seed;
		campaignNode.connections = connections;
		campaignNode.characters = characters;
		campaignNode.revealed = revealed;
		campaignNode.cleared = cleared;
		campaignNode.glow = glow;
		campaignNode.finalNode = finalNode;
		campaignNode.data = data;
		return campaignNode;
	}
}
