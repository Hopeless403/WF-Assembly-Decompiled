#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CurvedLinePoint : MonoBehaviour
{
	[HideInInspector]
	public bool showGizmo = true;

	[HideInInspector]
	public float gizmoSize = 0.1f;

	[HideInInspector]
	public Color gizmoColor = new Color(1f, 0f, 0f, 0.5f);

	public void OnDrawGizmos()
	{
		if (showGizmo)
		{
			Gizmos.color = gizmoColor;
			Gizmos.DrawSphere(base.transform.position, gizmoSize);
		}
	}

	public void OnDrawGizmosSelected()
	{
		CurvedLineRenderer component = base.transform.parent.GetComponent<CurvedLineRenderer>();
		if (component != null)
		{
			component.Update();
		}
	}
}
