#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bezier))]
public class TargetingArrow : TargetingDisplay
{
	[Serializable]
	public class ArrowHeadStyle
	{
		public string name;

		public GameObject gameObject;

		public Transform transform;

		public SpriteRenderer renderer;

		public bool directional;

		public Color headColor;

		public Gradient lineColor;
	}

	[SerializeField]
	public Transform head;

	[SerializeField]
	[Range(0f, 1f)]
	public float curviness = 0.5f;

	[SerializeField]
	[Range(0f, 1f)]
	public float arch;

	[SerializeField]
	public ArrowHeadStyle[] headStyles;

	[SerializeField]
	public string defaultStyleName = "Default";

	public Dictionary<string, ArrowHeadStyle> _headStyleLookup;

	public ArrowHeadStyle currentHeadStyle;

	public Bezier _bezier;

	public Dictionary<string, ArrowHeadStyle> headStyleLookup
	{
		get
		{
			if (_headStyleLookup == null)
			{
				_headStyleLookup = new Dictionary<string, ArrowHeadStyle>();
				ArrowHeadStyle[] array = headStyles;
				foreach (ArrowHeadStyle arrowHeadStyle in array)
				{
					_headStyleLookup[arrowHeadStyle.name] = arrowHeadStyle;
				}
			}

			return _headStyleLookup;
		}
	}

	public Bezier bezier => _bezier ?? (_bezier = GetComponent<Bezier>());

	public override void UpdatePosition(Transform target)
	{
		UpdatePosition(base.transform.position, target.position);
	}

	public override void UpdatePosition(Vector3 start, Vector3 end)
	{
		float y = Mathf.Lerp(start.y, end.y, curviness);
		Vector3 p = new Vector3(start.x, y, start.z - arch);
		bezier.UpdateCurve(start, p, end);
		if (!head)
		{
			return;
		}

		head.position = bezier.GetPoint(bezier.pointCount - 1);
		ArrowHeadStyle arrowHeadStyle = currentHeadStyle;
		if (arrowHeadStyle == null || !arrowHeadStyle.directional)
		{
			return;
		}

		Transform transform = arrowHeadStyle.transform;
		if ((object)transform != null && bezier.pointCount > 1)
		{
			Vector3 point = bezier.GetPoint(bezier.pointCount - 2);
			Vector3 vector = transform.position - point;
			float num = Mathf.Atan(vector.y / vector.x) * 57.29578f;
			if (vector.x < 0f)
			{
				num += 180f;
			}

			if (!float.IsNaN(num))
			{
				transform.localEulerAngles = new Vector3(0f, 0f, num);
			}
		}
	}

	public override void EntityHover(Entity entity)
	{
		SetStyle("Target");
	}

	public override void ContainerHover(CardContainer cardContainer, TargetingArrowSystem system)
	{
		if (cardContainer == system.target.owner.discardContainer && system.target.CanRecall())
		{
			system.snapToContainer = cardContainer;
			SetStyle("Discard");
		}
		else if (system.target.targetMode.TargetRow && system.target.CanPlayOn(cardContainer))
		{
			system.snapToContainer = cardContainer;
			SetStyle("TargetRow");
		}
	}

	public override void SlotHover(CardSlot slot, TargetingArrowSystem system)
	{
		if (system.target.data.playOnSlot && system.target.CanPlayOn(slot))
		{
			system.snapToContainer = slot;
			SetStyle("Target");
		}
	}

	public void SetStyle(string name)
	{
		if ((bool)currentHeadStyle?.gameObject)
		{
			currentHeadStyle.gameObject.SetActive(value: false);
		}

		currentHeadStyle = (headStyleLookup.ContainsKey(name) ? headStyleLookup[name] : headStyleLookup[defaultStyleName]);
		if ((bool)currentHeadStyle?.gameObject)
		{
			currentHeadStyle.gameObject.SetActive(value: true);
		}

		if ((bool)currentHeadStyle.renderer)
		{
			currentHeadStyle.renderer.color = currentHeadStyle.headColor;
		}

		bezier.lineRenderer.colorGradient = currentHeadStyle.lineColor;
	}

	public override void ResetStyle()
	{
		SetStyle(defaultStyleName);
	}
}
