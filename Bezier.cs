#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bezier : MonoBehaviour
{
	public LineRenderer lineRenderer;

	public int quality = 30;

	public int pointCount => lineRenderer.positionCount;

	public Vector3 GetPoint(int index)
	{
		return lineRenderer.GetPosition(index);
	}

	public void Start()
	{
		if ((object)lineRenderer == null)
		{
			lineRenderer = GetComponent<LineRenderer>();
		}
	}

	public void UpdateCurve(Vector3 p0, Vector3 p1, Vector3 p2)
	{
		lineRenderer.positionCount = quality + 1;
		lineRenderer.SetPosition(0, p0);
		for (int i = 1; i <= quality; i++)
		{
			float t = (float)i / (float)quality;
			Vector3 position = Calculate(t, p0, p1, p2);
			lineRenderer.SetPosition(i, position);
		}
	}

	public void UpdateCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		lineRenderer.positionCount = quality + 1;
		lineRenderer.SetPosition(0, p0);
		for (int i = 1; i <= quality; i++)
		{
			float t = (float)i / (float)quality;
			Vector3 position = Calculate(t, p0, p1, p2, p3);
			lineRenderer.SetPosition(i, position);
		}
	}

	public Vector3 Calculate(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = t * t;
		return num2 * p0 + 2f * num * t * p1 + num3 * p2;
	}

	public Vector3 Calculate(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float num4 = num3 * num;
		float num5 = num2 * t;
		return num4 * p0 + 3f * num3 * t * p1 + 3f * num * num2 * p2 + num5 * p3;
	}
}
