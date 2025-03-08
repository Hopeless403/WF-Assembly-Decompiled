#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class TargetingMultiple : TargetingDisplay
{
	[SerializeField]
	public GameObject targetPrefab;

	public readonly List<GameObject> targets = new List<GameObject>();

	public Entity target;

	public Entity[] entitiesToHit;

	public bool showingTargets;

	public override void Show(TargetingArrowSystem system)
	{
		target = system.target;
		entitiesToHit = target.targetMode.GetTargets(target, null, null);
		Events.OnContainerHover += ContainerHover;
		Events.OnContainerUnHover += ContainerUnHover;
	}

	public override void Hide()
	{
		HideTargets();
		Events.OnContainerHover -= ContainerHover;
		Events.OnContainerUnHover -= ContainerUnHover;
	}

	public void ContainerHover(CardContainer cardContainer)
	{
		if ((bool)cardContainer && target.containers.Length != 0 && cardContainer == target.containers[0])
		{
			HideTargets();
		}
	}

	public void ContainerUnHover(CardContainer cardContainer)
	{
		if (!showingTargets && (bool)target && target.containers.Length != 0 && cardContainer == target.containers[0])
		{
			ShowTargets();
		}
	}

	public void ShowTargets()
	{
		if (showingTargets)
		{
			return;
		}

		if (entitiesToHit != null)
		{
			Entity[] array = entitiesToHit;
			foreach (Entity entity in array)
			{
				GameObject item = Object.Instantiate(targetPrefab, entity.transform.position, Quaternion.identity, base.transform);
				targets.Add(item);
			}
		}

		showingTargets = true;
	}

	public void HideTargets()
	{
		if (!showingTargets)
		{
			return;
		}

		foreach (GameObject target in targets)
		{
			target.Destroy();
		}

		targets.Clear();
		showingTargets = false;
	}
}
