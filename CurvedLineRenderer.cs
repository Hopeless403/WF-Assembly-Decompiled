#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CurvedLineRenderer : MonoBehaviour
{
	public float lineSegmentSize = 0.1f;

	public float lineWidth = 0.1f;

	public bool autoUpdate;

	public bool promptUpdate;

	[Header("Gizmos")]
	public bool showGizmos = true;

	public float gizmoSize = 0.1f;

	public Color gizmoColor = new Color(1f, 0f, 0f, 0.5f);

	public CurvedLinePoint[] linePoints = new CurvedLinePoint[0];

	public Vector3[] linePositions = new Vector3[0];

	public Vector3[] linePositionsOld = new Vector3[0];

	public void Update()
	{
		if (autoUpdate || promptUpdate)
		{
			UpdatePoints();
			promptUpdate = false;
		}
	}

	public void UpdatePoints()
	{
		GetPoints();
		SetPointsToLine();
	}

	public void GetPoints()
	{
		linePoints = GetComponentsInChildren<CurvedLinePoint>();
		linePositions = new Vector3[linePoints.Length];
		for (int i = 0; i < linePoints.Length; i++)
		{
			linePositions[i] = linePoints[i].transform.localPosition;
		}
	}

	public void SetPointsToLine()
	{
		if (linePositionsOld.Length != linePositions.Length)
		{
			linePositionsOld = new Vector3[linePositions.Length];
		}

		bool flag = false;
		for (int i = 0; i < linePositions.Length; i++)
		{
			if (linePositions[i] != linePositionsOld[i])
			{
				flag = true;
			}
		}

		if (flag)
		{
			LineRenderer component = GetComponent<LineRenderer>();
			Vector3[] array = LineSmoother.SmoothLine(linePositions, lineSegmentSize);
			component.positionCount = array.Length;
			component.SetPositions(array);
			component.startWidth = lineWidth;
			component.endWidth = lineWidth;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Update();
	}

	public void OnDrawGizmos()
	{
		if (linePoints.Length == 0)
		{
			GetPoints();
		}

		CurvedLinePoint[] array = linePoints;
		foreach (CurvedLinePoint obj in array)
		{
			obj.showGizmo = showGizmos;
			obj.gizmoSize = gizmoSize;
			obj.gizmoColor = gizmoColor;
		}
	}
}
