#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class CardDeactivator : MonoBehaviour
{
	public readonly List<GameObject> entities = new List<GameObject>();

	public readonly List<GameObject> toDisable = new List<GameObject>();

	public Camera _cam;

	public Camera cam => _cam ?? (_cam = Camera.main);

	public void OnEnable()
	{
		Events.OnEntityCreated += EntityCreated;
		Events.OnEntityDestroyed += EntityDestroyed;
	}

	public void OnDisable()
	{
		Events.OnEntityCreated -= EntityCreated;
		Events.OnEntityDestroyed -= EntityDestroyed;
	}

	public void EntityCreated(Entity entity)
	{
		entities.Add(entity.gameObject);
	}

	public void EntityDestroyed(Entity entity)
	{
		entities.Remove(entity.gameObject);
	}

	public void LateUpdate()
	{
		if (Transition.Running)
		{
			return;
		}

		foreach (GameObject entity in entities)
		{
			bool inView = cam.IsInCameraView(entity.transform.position, 0.1f);
			SetActiveness(entity.gameObject, inView);
		}

		for (int num = Mathf.CeilToInt((float)toDisable.Count * 0.1f) - 1; num >= 0; num--)
		{
			GameObject obj = toDisable[num];
			toDisable.RemoveAt(num);
			obj.gameObject.SetActive(value: false);
		}
	}

	public void SetActiveness(GameObject obj, bool inView)
	{
		if (inView)
		{
			if (!obj.activeSelf || toDisable.Contains(obj))
			{
				obj.SetActive(value: true);
				toDisable.Remove(obj);
			}
		}
		else if (obj.activeSelf && !toDisable.Contains(obj))
		{
			toDisable.Add(obj);
		}
	}
}
