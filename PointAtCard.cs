#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PointAtCard : MonoBehaviour
{
	[SerializeField]
	public Transform holder;

	[SerializeField]
	public float moveAmount = 0.25f;

	[SerializeField]
	public float lerp = 0.1f;

	public Vector3 targetPos;

	public Transform t;

	public Camera cam;

	public void OnEnable()
	{
		cam = Camera.main;
		t = base.transform;
		Events.OnEntityHover += EntityHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
	}

	public void Update()
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			SetTargetPos(MonoBehaviourSingleton<Cursor3d>.instance.transform.position);
		}

		t.localPosition = Delta.Lerp(t.localPosition, targetPos, lerp, Time.deltaTime);
	}

	public void EntityHover(Entity entity)
	{
		SetTargetPos(entity.transform.position);
	}

	public void SetTargetPos(Vector3 target)
	{
		Vector3 vectorTo = GetVectorTo(target);
		targetPos = vectorTo * moveAmount;
	}

	public Vector3 GetVectorTo(Vector3 to)
	{
		Vector3 result = cam.WorldToScreenPoint(to) - cam.WorldToScreenPoint(holder.position);
		if (result.magnitude > 1f)
		{
			result.Normalize();
		}

		return result;
	}
}
