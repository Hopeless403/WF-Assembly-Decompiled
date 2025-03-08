#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CampaignSaveData
{
	public CharacterSaveData[] characters;

	public CampaignNodeSaveData[] nodes;

	public int playerId;

	public string[] modifiers;

	public CampaignSaveData()
	{
	}

	public CampaignSaveData(Campaign campaign)
	{
		characters = campaign.characters.SaveArray<Character, CharacterSaveData>();
		nodes = campaign.nodes.SaveArray<CampaignNode, CampaignNodeSaveData>();
		playerId = Campaign.GetCharacterId(References.Player);
		modifiers = ((Campaign.Data.Modifiers != null) ? Campaign.Data.Modifiers.Select((GameModifierData a) => a.name).ToArray() : null);
	}

	public List<CampaignNode> LoadNodes()
	{
		List<CampaignNode> list = new List<CampaignNode>();
		CampaignNodeSaveData[] array = nodes;
		foreach (CampaignNodeSaveData campaignNodeSaveData in array)
		{
			list.Add(campaignNodeSaveData.Load());
		}

		return list;
	}
}
