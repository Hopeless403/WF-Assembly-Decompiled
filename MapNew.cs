#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MapNew : MonoBehaviour
{
	[SerializeField]
	public Transform positioner;

	[SerializeField]
	public Transform nodesGroup;

	[SerializeField]
	public MapPath pathPrefab;

	[SerializeField]
	public MapStamp stamp;

	public static Vector3 prePosition = Vector3.zero;

	public readonly List<MapNode> nodes = new List<MapNode>();

	public readonly List<MapPath> paths = new List<MapPath>();

	[SerializeField]
	public Sprite[] detailSprites;

	public bool active = true;

	public MapPath[] fadedPaths;

	public MapNode hoverNode;

	public MapNode[] fadedNodes;

	public bool interactable = true;

	public void OnEnable()
	{
		Events.OnMapNodeHover += HoverNode;
		Events.OnMapNodeUnHover += UnHoverNode;
	}

	public void OnDisable()
	{
		Events.OnMapNodeHover -= HoverNode;
		Events.OnMapNodeUnHover -= UnHoverNode;
	}

	public IEnumerator Start()
	{
		yield return new WaitUntil(() => !Transition.Running);
		References.Map = this;
		nodesGroup.DestroyAllChildren();
		nodes.Clear();
		paths.Clear();
		foreach (CampaignNode node in Campaign.instance.nodes)
		{
			CreateNode(node);
		}

		foreach (MapNode node2 in nodes)
		{
			List<MapNode> list = new List<MapNode>();
			if (node2.campaignNode.connections != null)
			{
				foreach (CampaignNode.Connection connection in node2.campaignNode.connections)
				{
					MapNode mapNode = FindNode(Campaign.GetNode(connection.otherId));
					list.Add(mapNode);
					mapNode.connectedTo++;
				}
			}

			node2.connections = list.ToArray();
		}

		foreach (MapNode node3 in nodes)
		{
			if (!node3.campaignNode.revealed)
			{
				node3.gameObject.SetActive(value: false);
			}
		}

		CreatePaths();
		positioner.localPosition = prePosition;
		yield return Sequences.Wait(0.5f);
		Continue();
	}

	public IEnumerator Restart()
	{
		yield return SceneManager.Unload("Campaign");
		new Routine(Transition.To("Campaign"));
	}

	public MapNode CreateNode(CampaignNode campaignNode)
	{
		MapNode mapNode = Object.Instantiate(campaignNode.type.mapNodePrefab, nodesGroup);
		mapNode.transform.localPosition = campaignNode.position.WithZ(0f);
		mapNode.Assign(campaignNode);
		nodes.Add(mapNode);
		mapNode.map = this;
		mapNode.name = $"MapNode{nodes.Count}";
		return mapNode;
	}

	public MapNode FindNode(CampaignNode campaignNode)
	{
		return nodes.Find((MapNode a) => a.campaignNode == campaignNode);
	}

	public List<MapPath> FindPaths(MapNode fromNode)
	{
		return paths.FindAll((MapPath a) => a.StartNode == fromNode);
	}

	public void CreatePaths()
	{
		MapNode mapNode = null;
		foreach (MapNode node in nodes)
		{
			if (node.connections.Length != 0)
			{
				mapNode = node;
				break;
			}
		}

		new List<MapNode>();
		while (mapNode != null)
		{
			if (mapNode.connections.Length == 1)
			{
				CreatePath(mapNode, mapNode.connections[0]);
				mapNode = mapNode.connections[0];
			}
			else if (mapNode.connections.Length > 1)
			{
				MapNode mapNode2 = null;
				MapNode[] connections = mapNode.connections;
				foreach (MapNode mapNode3 in connections)
				{
					List<MapNode> list = new List<MapNode> { mapNode, mapNode3 };
					MapNode mapNode4 = mapNode3;
					while (mapNode4 != null)
					{
						if (mapNode4.connectedTo == 1 && mapNode4.connections.Length != 0)
						{
							list.Add(mapNode4.connections[0]);
							mapNode4 = mapNode4.connections[0];
						}
						else
						{
							mapNode2 = mapNode4;
							mapNode4 = null;
						}
					}

					CreatePath(list.ToArray());
				}

				if (mapNode2 != null)
				{
					mapNode = mapNode2;
				}
			}
			else
			{
				mapNode = null;
			}
		}
	}

	public MapPath CreatePath(params MapNode[] nodes)
	{
		MapPath mapPath = Object.Instantiate(pathPrefab, nodesGroup);
		foreach (MapNode node in nodes)
		{
			mapPath.Add(node);
		}

		mapPath.Setup();
		paths.Add(mapPath);
		return mapPath;
	}

	public IEnumerator Reveal()
	{
		MapNode currentNode = FindNode(Campaign.FindCharacterNode(References.Player));
		yield return Sequences.Wait(0.5f);
		yield return Reveal(currentNode);
	}

	public IEnumerator Reveal(MapNode fromNode)
	{
		if (fromNode.campaignNode.type.isBattle && !fromNode.campaignNode.cleared)
		{
			yield break;
		}

		fromNode.Reveal();
		List<MapNode> endNodes = new List<MapNode>();
		List<MapPath> list = FindPaths(fromNode);
		foreach (MapPath path in list)
		{
			yield return path.Reveal();
			MapNode endNode = path.EndNode;
			if (!endNodes.Contains(endNode))
			{
				endNodes.Add(endNode);
			}
		}

		foreach (MapNode item in endNodes)
		{
			if (!item.campaignNode.type.isBattle)
			{
				yield return Reveal(item);
			}
		}
	}

	public void UpdateInteractability(bool forceCanSkip = false)
	{
		MapNode mapNode = FindNode(Campaign.FindCharacterNode(References.Player));
		List<MapNode> allConnections = GetAllConnections(mapNode, forceCanSkip);
		foreach (MapNode node in nodes)
		{
			bool flag = allConnections.Contains(node);
			node.reachable = flag || node.campaignNode.cleared;
			node.hoverable = interactable && flag;
			bool flag2 = mapNode.connections.Contains(node);
			bool flag3 = mapNode.campaignNode.cleared || !mapNode.campaignNode.type.mustClear;
			node.SetSelectable(interactable && flag2 && flag3);
		}

		mapNode.SetSelectable(interactable && mapNode.interactable && !mapNode.campaignNode.cleared);
		foreach (MapPath path in paths)
		{
			if (path.gameObject.activeSelf)
			{
				path.CheckReachable();
			}
		}
	}

	public void Focus()
	{
		MapNode startNode = FindNode(Campaign.FindCharacterNode(References.Player));
		MapNode[] array = GetAllConnections(startNode).ToArray();
		if (array.Length != 0)
		{
			FocusOn(array);
		}
	}

	public void FocusOn(params MapNode[] nodes)
	{
		Vector3 zero = Vector3.zero;
		foreach (MapNode mapNode in nodes)
		{
			zero += mapNode.transform.localPosition;
		}

		zero /= (float)nodes.Length;
		Vector3 to = -zero;
		LeanTween.moveLocal(positioner.gameObject, to, 0.5f).setEase(LeanTweenType.easeInOutQuad);
		prePosition = to;
	}

	public List<MapNode> GetAllConnections(MapNode startNode, bool forceCanSkip = false)
	{
		List<MapNode> list = new List<MapNode>();
		List<MapNode> list2 = new List<MapNode> { startNode };
		while (list2.Count > 0)
		{
			MapNode mapNode = list2[0];
			list2.RemoveAt(0);
			if (!list.Contains(mapNode))
			{
				list.Add(mapNode);
			}

			if (!(mapNode.campaignNode.cleared || forceCanSkip) && !mapNode.campaignNode.type.canSkip)
			{
				continue;
			}

			MapNode[] connections = mapNode.connections;
			foreach (MapNode item in connections)
			{
				if (!list2.Contains(item))
				{
					list2.Add(item);
				}
			}
		}

		return list;
	}

	public bool TryMoveTo(MapNode node)
	{
		Character player = References.Player;
		CampaignNode campaignNode = Campaign.FindCharacterNode(player);
		if (campaignNode == node.campaignNode || campaignNode.connections.Exists((CampaignNode.Connection a) => a.otherId == node.campaignNode.id))
		{
			if (!node.campaignNode.type.isBattle || player.GetCompanionCount() <= player.data.companionLimit)
			{
				MoveTo(player, node);
				Enter(player, node);
				return true;
			}

			if (player.entity.display is CharacterDisplay characterDisplay)
			{
				characterDisplay.deckDisplay.companionLimitSequence.Begin();
			}

			return false;
		}

		return false;
	}

	public static void MoveTo(Character character, MapNode node)
	{
		Campaign.MoveCharacter(character, node.campaignNode);
	}

	public void Enter(Character character, MapNode node)
	{
		interactable = false;
		UpdateInteractability();
		stamp.Stamp(node.transform.position);
		Campaign.TryEnterNode(node.campaignNode);
	}

	public static IEnumerator CheckCompanionLimit()
	{
		Character player = References.Player;
		EntityDisplay display = player.entity.display;
		CharacterDisplay playerDisplay = display as CharacterDisplay;
		if ((object)playerDisplay != null && player.GetCompanionCount() > player.data.companionLimit)
		{
			playerDisplay.deckDisplay.companionLimitSequence.Begin();
			yield return new WaitUntil(() => !playerDisplay.deckDisplay.companionLimitSequence.IsRunning);
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Continue(bool forceCanSkip = false)
	{
		interactable = true;
		foreach (MapNode node in nodes)
		{
			node.Refresh();
		}

		MapNode fromNode = FindNode(Campaign.FindCharacterNode(References.Player));
		StartCoroutine(Reveal(fromNode));
		UpdateInteractability(forceCanSkip);
		Focus();
		stamp.FadeOut();
	}

	public void HoverNode(MapNode node)
	{
		if (hoverNode != null)
		{
			UnHoverNode(hoverNode);
		}

		MapNode mapNode = FindNode(Campaign.FindCharacterNode(References.Player));
		if (mapNode.connections.Length <= 1 || !(node != mapNode) || node.connectedTo != 1)
		{
			return;
		}

		List<MapPath> list = FindPaths(mapNode);
		MapPath mapPath = null;
		List<MapPath> list2 = new List<MapPath>();
		foreach (MapPath item in list)
		{
			if (item.ContainsNode(node))
			{
				mapPath = item;
			}
			else
			{
				list2.Add(item);
			}
		}

		if (!(mapPath != null))
		{
			return;
		}

		hoverNode = node;
		foreach (MapPath item2 in list)
		{
			if (item2 != mapPath)
			{
				item2.FadeTo(0.5f, 0f);
			}
		}

		List<MapNode> allConnections = GetAllConnections(node);
		List<MapNode> list3 = new List<MapNode>();
		foreach (MapNode node2 in nodes)
		{
			bool flag = allConnections.Contains(node2);
			if (!node2.campaignNode.cleared && !flag && !mapPath.ContainsNode(node2))
			{
				list3.Add(node2);
				node2.color = new Color(1f, 1f, 1f, 0.5f);
			}
		}

		fadedPaths = list2.ToArray();
		fadedNodes = list3.ToArray();
	}

	public void UnHoverNode(MapNode node)
	{
		if (!(hoverNode == node))
		{
			return;
		}

		if (fadedPaths != null)
		{
			MapPath[] array = fadedPaths;
			foreach (MapPath mapPath in array)
			{
				if (mapPath.reachable)
				{
					mapPath.FadeTo(1f, 0f);
				}
			}

			fadedPaths = null;
		}

		if (fadedNodes != null)
		{
			MapNode[] array2 = fadedNodes;
			foreach (MapNode mapNode in array2)
			{
				if (mapNode.reachable)
				{
					mapNode.color = Color.white;
				}
			}

			fadedNodes = null;
		}

		hoverNode = null;
	}
}
