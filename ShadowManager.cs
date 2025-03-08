#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
	[SerializeField]
	public Shadow shadowPrefab;

	[SerializeField]
	public Vector2 limitX = new Vector2(-999f, 999f);

	[SerializeField]
	public Vector2 limitY = new Vector2(-999f, 999f);

	[SerializeField]
	public Vector2 limitZ = new Vector2(0f, 999f);

	[Header("Offset")]
	[SerializeField]
	public Vector3 offset = new Vector3(0.18f, -3.1f, 0f);

	[SerializeField]
	public Vector3 offsetBasedOnScale = new Vector3(0f, 0f, 0f);

	[Header("Scale")]
	[SerializeField]
	[Range(0f, 1f)]
	public float considerScale = 0.5f;

	[SerializeField]
	public Vector3 baseScale = new Vector3(2f / 3f, 2f / 3f, 2f / 3f);

	public readonly Dictionary<ulong, Shadow> active = new Dictionary<ulong, Shadow>();

	public readonly Queue<Shadow> pool = new Queue<Shadow>();

	public void OnEnable()
	{
		Events.OnEntityCreated += Assign;
		Events.OnEntityDestroyed += EntityDestroyed;
	}

	public void OnDisable()
	{
		Events.OnEntityCreated -= Assign;
		Events.OnEntityDestroyed -= EntityDestroyed;
	}

	public void Assign(Entity entity)
	{
		if (entity.inPlay)
		{
			Shadow shadow = Get();
			active[entity.data.id] = shadow;
			shadow.Assign(entity);
		}
	}

	public void EntityDestroyed(Entity entity)
	{
		if ((bool)entity.data && active.TryGetValue(entity.data.id, out var value))
		{
			active.Remove(entity.data.id);
			Pool(value);
		}
	}

	public void LateUpdate()
	{
		foreach (Shadow value in active.Values)
		{
			Transform obj = value.transform;
			Vector3 vector = value.target.lossyScale - baseScale;
			Vector3 vector2 = baseScale + vector * considerScale;
			Vector3 vector3 = Vector3.Scale(offset, vector2) + Vector3.Scale(offsetBasedOnScale, vector);
			obj.localScale = vector2;
			Vector3 position = value.target.position + vector3;
			position.x = Mathf.Clamp(position.x, limitX.x, limitX.y);
			position.y = Mathf.Clamp(position.y, limitY.x, limitY.y);
			position.z = Mathf.Clamp(position.z, limitZ.x, limitZ.y);
			obj.position = position;
			obj.localEulerAngles = new Vector3(0f, 0f, value.target.eulerAngles.z);
			value.UpdateAlpha();
		}
	}

	public Shadow Get()
	{
		Shadow obj = ((pool.Count > 0) ? pool.Dequeue() : Object.Instantiate(shadowPrefab, base.transform));
		obj.gameObject.SetActive(value: true);
		return obj;
	}

	public void Pool(Shadow shadow)
	{
		shadow.gameObject.SetActive(value: false);
		pool.Enqueue(shadow);
	}
}
