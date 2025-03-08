#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilityTargetSystem : GameSystem
{
	public Transform targetGroup;

	public GameObject targetPrefab;

	public Dictionary<CardContainer, GameObject> currentTargets;

	public void OnEnable()
	{
		Events.OnAbilityTargetAdd += AddTarget;
		Events.OnAbilityTargetRemove += RemoveTarget;
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnAbilityTargetAdd -= AddTarget;
		Events.OnAbilityTargetRemove -= RemoveTarget;
		Events.OnSceneChanged -= SceneChanged;
		Clear();
	}

	public void AddTarget(CardContainer container)
	{
		GameObject gameObject = Object.Instantiate(targetPrefab, targetGroup);
		gameObject.transform.position = container.transform.position;
		if (currentTargets == null)
		{
			currentTargets = new Dictionary<CardContainer, GameObject>();
		}

		currentTargets[container] = gameObject;
	}

	public void RemoveTarget(CardContainer container)
	{
		if (currentTargets != null && currentTargets.ContainsKey(container))
		{
			currentTargets[container].Destroy();
			currentTargets.Remove(container);
		}
	}

	public void SceneChanged(Scene scene)
	{
		Clear();
	}

	public void Clear()
	{
		if (currentTargets == null)
		{
			return;
		}

		foreach (KeyValuePair<CardContainer, GameObject> item in currentTargets.Where((KeyValuePair<CardContainer, GameObject> pair) => pair.Value != null))
		{
			item.Value.Destroy();
		}

		currentTargets = null;
	}
}
