#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class Page : MonoBehaviour
{
	[Header("Dragging")]
	public bool canDrag = true;

	[Header("Zooming")]
	public bool canZoom = true;

	[ShowIf("canZoom")]
	[MinMaxSlider(-100f, 100f)]
	public Vector2 zoomRange = new Vector2(0f, 80f);

	[ShowIf("canZoom")]
	[Range(0f, 1f)]
	public float zoomSpeed = 0.1f;

	public bool dragging;

	public Vector3 dragGrabPos;

	public float zoom;

	public float zoomTarget;

	public bool inspecting;

	public void OnEnable()
	{
		Events.OnInspect += InspectStart;
		Events.OnInspectEnd += InspectEnd;
	}

	public void OnDisable()
	{
		Events.OnInspect -= InspectStart;
		Events.OnInspectEnd -= InspectEnd;
	}

	public void Update()
	{
		if (inspecting)
		{
			return;
		}

		if (canDrag)
		{
			if (dragging)
			{
				Drag();
			}

			if (dragging && !Input.GetMouseButton(2))
			{
				StopDrag();
			}

			if (Input.GetMouseButtonDown(2))
			{
				StartDrag();
			}
		}

		float num = 0f - Input.mouseScrollDelta.y;
		if (num > 0f)
		{
			zoomTarget = Mathf.Min(zoomTarget + num * zoomSpeed, 1f);
		}
		else if (num < 0f)
		{
			zoomTarget = Mathf.Max(zoomTarget + num * zoomSpeed, 0f);
		}

		if (zoom != zoomTarget)
		{
			zoom = Delta.Lerp(zoom, zoomTarget, 0.1f, Time.deltaTime);
			float value = zoomRange.x + (zoomRange.y - zoomRange.x) * zoom;
			base.transform.localPosition = base.transform.localPosition.WithZ(value);
		}
	}

	public void InspectStart(Entity entity)
	{
		inspecting = true;
		StopDrag();
	}

	public void InspectEnd(Entity entity)
	{
		inspecting = false;
	}

	public void StartDrag()
	{
		dragging = true;
		dragGrabPos = base.transform.position - Cursor3d.PositionWithZ;
	}

	public void Drag()
	{
		base.transform.position = Cursor3d.PositionWithZ + dragGrabPos;
	}

	public void StopDrag()
	{
		dragging = false;
	}
}
