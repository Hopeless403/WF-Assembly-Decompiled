#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class Cursor3d : MonoBehaviourSingleton<Cursor3d>
{
	public Camera _cam;

	public LayerMask layerMask;

	public bool usingMouse = true;

	public bool usingTouch;

	public GameObject mouseObj;

	public bool showVirtualPointerState = true;

	public static readonly Vector3 offset = new Vector3(0f, 0f, -1f);

	public readonly RaycastHit[] hits = new RaycastHit[1];

	public Camera cam => _cam ?? (_cam = Camera.main);

	public static Vector3 Position { get; set; }

	public static Vector3 PositionWithZ { get; set; }

	public void OnEnable()
	{
		CustomCursor.UpdateState();
	}

	public void Update()
	{
		if (usingMouse && Physics.RaycastNonAlloc(cam.ScreenPointToRay(InputSystem.MousePosition + offset), hits, 1000f, layerMask) > 0)
		{
			SetPosition(hits[0].point);
		}
	}

	public Vector2 GetScreenPoint()
	{
		return cam.WorldToScreenPoint(base.transform.position);
	}

	public void SetPosition(Vector3 position)
	{
		base.transform.position = position;
		PositionWithZ = position;
		Position = PositionWithZ.WithZ(0f);
	}
}
