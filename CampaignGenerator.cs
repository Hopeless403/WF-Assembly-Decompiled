#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignGenerator", menuName = "Campaign/Generator")]
public class CampaignGenerator : ScriptableObject
{
	public class Line
	{
		public float x1;

		public float y1;

		public float x2;

		public float y2;

		public Line(float x1, float y1, float x2, float y2)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}
	}

	public class Node
	{
		public class Connection
		{
			public Node node;

			public Color color = Color.gray;

			public Connection(Node node)
			{
				this.node = node;
			}

			public override string ToString()
			{
				return $"Connection to {node}";
			}
		}

		public float x;

		public float y;

		public float r;

		public int tier;

		public int positionIndex;

		public string type;

		public Color color = Color.white;

		public CampaignNode campaignNode;

		public int areaIndex;

		public readonly List<Connection> connections = new List<Connection>();

		public int connectionCount;

		public bool interactable = true;

		public Node(float x, float y, float r, int tier, int positionIndex, int areaIndex, string type)
		{
			this.x = x;
			this.y = y;
			this.r = r;
			this.tier = tier;
			this.positionIndex = positionIndex;
			this.areaIndex = areaIndex;
			this.type = type;
		}

		public void Connect(Node other)
		{
			connections.Add(new Connection(other));
			connectionCount++;
			other.connectionCount++;
		}

		public override string ToString()
		{
			return $"Node ({x}, {y})";
		}
	}

	[SerializeField]
	public string seed;

	[SerializeField]
	public Vector2 nodeSizeRange = new Vector2(1.8f, 2f);

	[SerializeField]
	public float nodeCreationRand = 1.5f;

	[SerializeField]
	public Vector2 nodeSpacing = new Vector2(3.2f, 2f);

	[SerializeField]
	public Vector2 battleDistanceRange = new Vector2(13f, 14f);

	[SerializeField]
	public Vector2 mapDirection = new Vector2(0.67f, 0.33f);

	[SerializeField]
	public TextAsset[] presets;

	[SerializeField]
	public bool instant = true;

	[SerializeField]
	public bool restart;

	[SerializeField]
	public bool restartOnError = true;

	public virtual IEnumerator Generate(Campaign campaign)
	{
		SetSeed();
		Debug.Log($"[{this}] GENERATING");
		StopWatch.Start();
		campaign.nodes.Clear();
		Dictionary<string, CampaignNodeType> nodeDict = new Dictionary<string, CampaignNodeType>();
		foreach (CampaignNodeType item in AddressableLoader.GetGroup<CampaignNodeType>("CampaignNodeType"))
		{
			if (!item.letter.IsNullOrWhitespace())
			{
				nodeDict[item.letter] = item;
			}
		}

		List<Node> nodes = new List<Node>();
		int attempt = 0;
		while (true)
		{
			Task<bool> task = TryGenerate(campaign, attempt, nodes, nodeDict);
			yield return new WaitUntil(() => task.IsCompleted);
			bool error = task.Result;
			attempt++;
			if (!error)
			{
				break;
			}

			Debug.Log("Generation failed!");
			nodes.Clear();
			yield return new WaitUntil(() => restart || (error && restartOnError));
			restart = false;
		}

		Debug.Log($"DONE! ({StopWatch.Stop()}ms) [{attempt} attempts]");
		foreach (Node item2 in nodes.OrderByDescending((Node a) => a.interactable))
		{
			CampaignNodeType type = nodeDict[item2.type];
			item2.campaignNode = new CampaignNode(type, item2.x, item2.y, item2.tier, item2.positionIndex, item2.areaIndex, item2.r);
			campaign.nodes.Add(item2.campaignNode);
			item2.campaignNode.id = campaign.nodes.Count - 1;
		}

		foreach (Node item3 in nodes)
		{
			foreach (Node.Connection connection2 in item3.connections)
			{
				item3.campaignNode.ConnectTo(connection2.node.campaignNode);
			}
		}

		Stack<CampaignNode> stack = new Stack<CampaignNode>();
		List<int> list = new List<int>();
		stack.Push(campaign.nodes[0]);
		while (stack.Count > 0)
		{
			CampaignNode campaignNode = stack.Pop();
			list.Add(campaignNode.id);
			bool flag = campaignNode.connections.Count > 1;
			for (int i = 0; i < campaignNode.connections.Count; i++)
			{
				CampaignNode.Connection connection = campaignNode.connections[i];
				if (!list.Contains(connection.otherId))
				{
					CampaignNode node = Campaign.GetNode(connection.otherId);
					bool flag2 = node.connectedTo > 1;
					node.pathId = (flag2 ? (-1) : (flag ? i : campaignNode.pathId));
					stack.Push(node);
				}
			}
		}
	}

	public async Task<bool> TryGenerate(Campaign campaign, int attempt, List<Node> nodes, IReadOnlyDictionary<string, CampaignNodeType> nodeDict)
	{
		Debug.Log($"Attempt #{attempt + 1}");
		campaign.preset = ChoosePreset();
		string[] lines = GetPresetLines(campaign.preset);
		Events.InvokeCampaignLoadPreset(ref lines);
		int campaignLength = GetCampaignLength(lines);
		campaign.battleTiers = lines[2];
		List<int> list = new List<int>();
		string battleTiers = campaign.battleTiers;
		for (int i = 0; i < battleTiers.Length; i++)
		{
			int item = int.Parse(battleTiers[i].ToString());
			list.Add(item);
		}

		string text = lines[3];
		int num = 0;
		float num2 = 0f;
		List<Node> list2 = new List<Node>();
		for (int j = 0; j < campaignLength; j++)
		{
			string text2 = text[j].ToString();
			int.TryParse(text2, out var result);
			List<string> list3 = new List<string>();
			for (int k = 0; k <= 1; k++)
			{
				string text3 = lines[k][j].ToString();
				if (!text3.IsNullOrWhitespace())
				{
					list3.Add(text3);
				}
			}

			if (result != num)
			{
				CampaignNodeType type = nodeDict["area" + text2];
				float y = nodeSpacing.y * 0.25f.WithRandomSign();
				Node node = CreateNode(num2 - nodeSpacing.x * 0.5f, y, type, list[j], j, result);
				nodes.Add(node);
				node.interactable = false;
			}

			num = result;
			float num3 = (float)(-(list3.Count - 1)) * nodeSpacing.y * 0.5f;
			List<Node> list4 = new List<Node>();
			foreach (string item3 in list3)
			{
				CampaignNodeType type2 = nodeDict[item3];
				Node item2 = CreateNode(num2, num3, type2, list[j], j, result);
				nodes.Add(item2);
				list4.Add(item2);
				num3 += nodeSpacing.y;
			}

			CampaignNodeType type3 = nodeDict[text2];
			for (int l = 0; l < 2; l++)
			{
				Node node2 = CreateNode(num2, nodeSpacing.y * UnityEngine.Random.Range(-0.5f, 0.5f), type3, -1, 0, result);
				nodes.Add(node2);
				node2.interactable = false;
			}

			num2 += nodeSpacing.x;
			if (list2.Count > 0)
			{
				int num4 = Mathf.Max(list4.Count, list2.Count);
				for (int m = 0; m < num4; m++)
				{
					Node other = list4[m % list4.Count];
					list2[m % list2.Count].Connect(other);
				}
			}

			list2 = list4;
		}

		Events.InvokeCampaignNodesCreated(ref nodes, nodeSpacing);
		await Task.Run(delegate
		{
			ShuffleNodes(nodes);
		});
		bool flag = false;
		foreach (Node node3 in nodes)
		{
			if (!node3.interactable)
			{
				continue;
			}

			foreach (Node.Connection connection in node3.connections)
			{
				Line line = new Line(node3.x, node3.y, connection.node.x, connection.node.y);
				foreach (Node node4 in nodes)
				{
					if (node4 != node3 && node4 != connection.node && node4.interactable && ((node4.x > node3.x && node4.x < connection.node.x) || (node4.x > connection.node.x && node4.x < node3.x)) && ((node4.y > node3.y && node4.y < connection.node.y) || (node4.y > connection.node.y && node4.y < node3.y)) && LineIntersectsCircle(line, node4.x, node4.y, node4.r))
					{
						Debug.Log($"Error: [{connection}] INTERSECTS [{node4}]");
						connection.color = Color.yellow;
						node4.color = Color.red;
						flag = true;
						break;
					}
				}

				if (flag)
				{
					break;
				}
			}

			if (flag)
			{
				break;
			}
		}

		return flag;
	}

	public void SetSeed()
	{
		if (!seed.IsNullOrWhitespace())
		{
			UnityEngine.Random.InitState(seed.ToSeed());
		}
	}

	public TextAsset ChoosePreset()
	{
		if (presets.Length != 0)
		{
			return presets.RandomItem();
		}

		throw new Exception("Campaign Generation Error: No Presets Found!");
	}

	public static string[] GetPresetLines(TextAsset preset)
	{
		string[] array = Regex.Split(preset.text, "\r\n|\n|\r");
		if (array.Length > 1)
		{
			int num = array.Length;
			string[] array2 = new string[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i];
			}

			return array2;
		}

		throw new Exception("Campaign Generation Error: Preset [" + preset.name + "] Must Contain AT LEAST 2 lines (1st for nodes, last for battle tiers)");
	}

	public static int GetCampaignLength(string[] lines)
	{
		int length = lines[0].Length;
		for (int i = 1; i < lines.Length; i++)
		{
			if (lines[i].Length != length)
			{
				throw new Exception("Campaign Generation Error: Preset line length mismatch — all lines in the preset file must be the same length");
			}
		}

		return length;
	}

	public Node CreateNode(float x, float y, CampaignNodeType type, int tier, int positionIndex, int areaIndex, float r = 1f)
	{
		Vector2 vector = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * UnityEngine.Random.Range(0f, nodeCreationRand);
		x += vector.x;
		y += vector.y;
		return new Node(x, y, nodeSizeRange.Random() * type.size * r, tier, positionIndex, areaIndex, type.letter);
	}

	public static void ShuffleNodes(List<Node> nodes)
	{
		bool flag = true;
		int num = 10000;
		while (flag && num > 0)
		{
			flag = false;
			foreach (Node node in nodes)
			{
				foreach (Node node2 in nodes)
				{
					if (node != node2)
					{
						Vector2 vector = new Vector2(node.x - node2.x, node.y - node2.y);
						if (vector.magnitude < node.r + node2.r)
						{
							Vector2 vector2 = vector.normalized * 0.01f;
							node.x += vector2.x;
							node.y += vector2.y;
							node2.x -= vector2.x;
							node2.y -= vector2.y;
							flag = true;
						}
					}
				}

				foreach (Node.Connection connection in node.connections)
				{
					Vector2 vector3 = new Vector2(node.x - connection.node.x, node.y - connection.node.y);
					float num2 = vector3.magnitude - (node.r + connection.node.r);
					if (num2 > 0f)
					{
						Vector2 vector4 = vector3.normalized * 0.001f * num2;
						node.x -= vector4.x;
						node.y -= vector4.y;
						connection.node.x += vector4.x;
						connection.node.y += vector4.y;
					}
				}
			}

			num--;
		}
	}

	public static void ShuffleNodes(List<CampaignNode> nodes)
	{
		bool flag = true;
		int num = 10000;
		while (flag && num > 0)
		{
			flag = false;
			foreach (CampaignNode node in nodes)
			{
				foreach (CampaignNode node2 in nodes)
				{
					if (node != node2)
					{
						Vector2 vector = new Vector2(node.position.x - node2.position.x, node.position.y - node2.position.y);
						if (vector.magnitude < node.radius + node2.radius)
						{
							Vector2 vector2 = vector.normalized * 0.01f;
							node.position.x += vector2.x;
							node.position.y += vector2.y;
							node2.position.x -= vector2.x;
							node2.position.y -= vector2.y;
							flag = true;
						}
					}
				}

				foreach (CampaignNode.Connection c in node.connections)
				{
					CampaignNode campaignNode = nodes.FirstOrDefault((CampaignNode a) => a.id == c.otherId);
					if (campaignNode != null)
					{
						Vector2 vector3 = new Vector2(node.position.x - campaignNode.position.x, node.position.y - campaignNode.position.y);
						float num2 = vector3.magnitude - (node.radius + campaignNode.radius);
						if (num2 > 0f)
						{
							Vector2 vector4 = vector3.normalized * (0.001f * num2);
							node.position.x -= vector4.x;
							node.position.y -= vector4.y;
							campaignNode.position.x += vector4.x;
							campaignNode.position.y += vector4.y;
						}
					}
				}
			}

			num--;
		}
	}

	public static bool LineIntersectsCircle(Line line, float cx, float cy, float r)
	{
		float num = line.x1 - cx;
		float num2 = line.x2 - cx;
		float num3 = line.y1 - cy;
		float num4 = line.y2 - cy;
		float num5 = num2 - num;
		float num6 = num4 - num3;
		float num7 = num5 * num5 + num6 * num6;
		float num8 = num * num4 - num2 * num3;
		return r * r * num7 > num8 * num8;
	}
}
