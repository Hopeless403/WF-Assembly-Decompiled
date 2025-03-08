#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPath : MonoBehaviour
{
	public struct Node
	{
		public MapNode mapNode;

		public Transform point;
	}

	[SerializeField]
	public Transform linePointPrefab;

	[SerializeField]
	public Transform lastPoint;

	[SerializeField]
	public Vector2 midPointOffset = new Vector2(0f, 1f);

	[SerializeField]
	public float pathShort = 1f;

	[SerializeField]
	public Vector2 pathNodePull = new Vector2(0.75f, 1.2f);

	[SerializeField]
	public LineRenderer line;

	[SerializeField]
	public CurvedLineRenderer curve;

	[Header("Tweens")]
	[SerializeField]
	public LeanTweenType revealEase = LeanTweenType.easeInOutCubic;

	[SerializeField]
	public float revealSpeed = 1f;

	public List<Node> nodes = new List<Node>();

	public bool reachable = true;

	public float alpha = 1f;

	public LTDescr alphaTween;

	public MapNode StartNode { get; set; }

	public MapNode EndNode => nodes[nodes.Count - 1].mapNode;

	public int NodeCount => nodes.Count;

	public bool ContainsNode(MapNode node)
	{
		return nodes.Exists((Node a) => a.mapNode == node);
	}

	public void Add(MapNode node)
	{
		Vector3 localPosition = node.transform.localPosition;
		if (lastPoint != null)
		{
			Vector3 localPosition2 = (lastPoint.localPosition + localPosition) * 0.5f + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * midPointOffset.Random();
			AddPoint(localPosition2);
		}

		if ((object)StartNode == null)
		{
			MapNode mapNode2 = (StartNode = node);
		}

		Transform point = AddPoint(localPosition);
		nodes.Add(new Node
		{
			mapNode = node,
			point = point
		});
		lastPoint = point;
	}

	public MapNode Get(int index)
	{
		return nodes[index].mapNode;
	}

	public MapNode[] GetNodes()
	{
		List<MapNode> list = new List<MapNode>();
		foreach (Node node in nodes)
		{
			list.Add(node.mapNode);
		}

		return list.ToArray();
	}

	public void Setup()
	{
		if (nodes.Count > 1)
		{
			Transform point = nodes[0].point;
			PullTowards(point.transform, nodes[1].point.transform.localPosition, pathShort);
			Transform point2 = nodes[nodes.Count - 1].point;
			PullTowards(point2.transform, nodes[nodes.Count - 2].point.transform.localPosition, pathShort);
			for (int i = 1; i < nodes.Count - 1; i++)
			{
				Transform obj = nodes[i - 1].mapNode.transform;
				Transform point3 = nodes[i].point.transform;
				Transform transform = nodes[i + 1].mapNode.transform;
				Vector3 towards = (obj.localPosition + transform.localPosition) * 0.5f;
				PullTowards(point3, towards, pathNodePull.Random());
			}

			curve.UpdatePoints();
			bool flag = true;
			foreach (Node node in nodes)
			{
				if (!node.mapNode.campaignNode.revealed)
				{
					flag = false;
					break;
				}
			}

			if (!flag)
			{
				Hide();
			}

			return;
		}

		throw new Exception("MapPath Error: MUST HAVE more than 1 node in the path");
	}

	public void PullTowards(Transform point, Vector3 towards, float amount)
	{
		Vector3 localPosition = point.localPosition;
		Vector3 vector = (towards - localPosition).normalized * amount;
		point.localPosition += vector;
	}

	public Transform AddPoint(Vector3 localPosition)
	{
		Transform obj = UnityEngine.Object.Instantiate(linePointPrefab, base.transform);
		obj.localPosition = localPosition;
		obj.gameObject.SetActive(value: true);
		return obj;
	}

	public void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public IEnumerator Reveal()
	{
		if (base.gameObject.activeSelf)
		{
			yield break;
		}

		base.gameObject.SetActive(value: true);
		SetAlpha(1f);
		UpdatePathVisibility(0f);
		float delayPerNode = 0.2f / revealSpeed;
		float num = (float)nodes.Count * delayPerNode;
		LeanTween.value(base.gameObject, 0f, 1f, num).setEase(revealEase).setOnUpdate(delegate(float a)
		{
			UpdatePathVisibility(a);
		});
		Events.InvokeMapPathReveal(num);
		foreach (Node node in nodes)
		{
			yield return Sequences.Wait(delayPerNode);
			node.mapNode.Reveal();
		}
	}

	public void CheckReachable()
	{
		if (!reachable)
		{
			return;
		}

		reachable = false;
		int num = Mathf.Max(2, nodes.Count - 1);
		for (int i = 1; i < num; i++)
		{
			if (nodes[i].mapNode.reachable)
			{
				reachable = true;
				break;
			}
		}

		if (!reachable)
		{
			SetUnreachable();
		}
	}

	public void SetUnreachable()
	{
		reachable = false;
		FadeTo(0f);
	}

	public void FadeTo(float alpha, float time = 0.3f)
	{
		if (Mathf.Abs(this.alpha - alpha) > 0.01f)
		{
			if (time > 0f)
			{
				alphaTween = LeanTween.value(base.gameObject, this.alpha, alpha, time).setEase(LeanTweenType.easeInOutQuad).setOnUpdate(SetAlpha);
			}
			else
			{
				SetAlpha(alpha);
			}

			this.alpha = alpha;
		}
	}

	public void UpdatePathVisibility(float value)
	{
		int num = 1;
		if (value <= 0.01f)
		{
			line.widthCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 0f, 0f, 0f));
		}
		else if (value >= 0.99f)
		{
			line.widthCurve = new AnimationCurve(new Keyframe(0f, num, 0f, 0f), new Keyframe(1f, num, 0f, 0f));
		}
		else
		{
			line.widthCurve = new AnimationCurve(new Keyframe(0f, num, 0f, 0f), new Keyframe(value - 0.01f, num, 0f, 0f), new Keyframe(value, 0f, 0f, 0f));
		}
	}

	public void SetColour(Color value)
	{
		line.colorGradient.colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(value, 0f),
			new GradientColorKey(value, 1f)
		};
	}

	public void SetAlpha(float value)
	{
		Gradient colorGradient = line.colorGradient;
		colorGradient.alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(value, 0f),
			new GradientAlphaKey(value, 1f)
		};
		line.colorGradient = colorGradient;
	}
}
