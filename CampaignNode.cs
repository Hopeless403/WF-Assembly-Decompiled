#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

[Serializable]
public class CampaignNode : ISaveable<CampaignNodeSaveData>
{
	[Serializable]
	public class Connection
	{
		public int otherId;

		public float direction;
	}

	public string name = "Salty Spicelands";

	public int id;

	public int seed;

	public Vector2 position;

	public int tier;

	public int positionIndex;

	public int areaIndex;

	public List<Connection> connections;

	public List<int> characters;

	public bool revealed;

	public bool cleared;

	public float radius;

	public bool glow;

	public bool finalNode;

	public Dictionary<string, object> data;

	public int connectedTo;

	public int pathId = -1;

	public int dataLinkedTo = -1;

	public List<int> linkedToThis;

	public CampaignNodeType type { get; set; }

	public void CopyData(CampaignNode otherNode)
	{
		data = otherNode.data.ToDictionary((KeyValuePair<string, object> entry) => entry.Key, CloneDataValue);
	}

	public object CloneDataValue(KeyValuePair<string, object> pair)
	{
		if (pair.Value is ICloneable cloneable)
		{
			return cloneable.Clone();
		}

		return pair.Value;
	}

	public CampaignNode()
	{
	}

	public CampaignNode(CampaignNodeType type, float x, float y, int tier, int positionIndex, int areaIndex, float radius)
	{
		SetType(type);
		position = new Vector2(x, y);
		this.tier = tier;
		this.positionIndex = positionIndex;
		this.areaIndex = areaIndex;
		this.radius = radius;
		seed = Dead.Random.Seed();
		connections = new List<Connection>();
		characters = new List<int>();
		finalNode = type.finalNode;
		if (!type.canEnter)
		{
			cleared = true;
		}
	}

	public virtual IEnumerator SetUp()
	{
		return null;
	}

	public virtual IEnumerator Run()
	{
		return null;
	}

	public void ConnectTo(CampaignNode other)
	{
		other.connectedTo++;
		Connection connection = new Connection
		{
			otherId = other.id
		};
		if (connections.Count == 0)
		{
			connection.direction = 1f;
		}
		else
		{
			float direction = connections[0].direction;
			connection.direction = direction * -1f;
		}

		connections.Insert(0, connection);
	}

	public bool CanReceiveCharacter(Character character)
	{
		return character.GetCompanionCount() <= character.data.companionLimit;
	}

	public void SetType(CampaignNodeType type)
	{
		this.type = type;
		name = type.zoneName;
		revealed = type.startRevealed;
	}

	public string GetDesc()
	{
		string text = "";
		List<string> list = new List<string>();
		foreach (int character in characters)
		{
			List<RewardData> list2 = Campaign.GetCharacter(character).GetComponent<BattleRewards>()?.rewards;
			if (list2 == null)
			{
				continue;
			}

			foreach (RewardData item in list2)
			{
				list.Add(item.title);
			}
		}

		if (list.Count > 0)
		{
			string text2 = "#BABABA";
			if (list.Count == 1)
			{
				text = "<color=" + text2 + ">Reward:</color>\n" + list[0];
			}
			else
			{
				text = "<color=" + text2 + ">Rewards:</color>";
				foreach (string item2 in list)
				{
					text = text + "\n" + item2;
				}
			}
		}

		return text;
	}

	public void SetCleared()
	{
		if (!cleared)
		{
			cleared = true;
			Campaign.PromptSave();
		}
	}

	public CampaignNodeSaveData Save()
	{
		return new CampaignNodeSaveData(this);
	}
}
