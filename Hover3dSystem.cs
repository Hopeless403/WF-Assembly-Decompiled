#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class Hover3dSystem : GameSystem
{
	public Camera cam;

	[SerializeField]
	public LayerMask layerMask;

	[SerializeField]
	public List<Hoverable3d> hoverList;

	public List<Hoverable3d> newList;

	public readonly RaycastHit[] hits = new RaycastHit[5];

	public void OnEnable()
	{
		cam = Camera.main;
		hoverList = new List<Hoverable3d>();
		newList = new List<Hoverable3d>();
	}

	public void Update()
	{
		int num = Physics.RaycastNonAlloc(cam.ScreenPointToRay(MonoBehaviourSingleton<Cursor3d>.instance.GetScreenPoint()), hits, 100f, layerMask);
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = hits[i];
			Hoverable3d component = raycastHit.transform.GetComponent<Hoverable3d>();
			if ((object)component != null)
			{
				newList.Add(component);
				if (!hoverList.Contains(component))
				{
					component.Hover();
				}
			}
		}

		foreach (Hoverable3d hover in hoverList)
		{
			if (!newList.Contains(hover))
			{
				hover.UnHover();
			}
		}

		hoverList.Clear();
		hoverList.AddRange(newList);
		newList.Clear();
	}
}
