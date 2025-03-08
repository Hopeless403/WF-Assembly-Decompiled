#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;

public class BuildingDisplay : MonoBehaviour
{
	[SerializeField]
	public UnityEvent onEnable;

	[SerializeField]
	public UnityEvent onDisable;

	[SerializeField]
	public int setSiblingIndex = 1;

	[SerializeField]
	public HelpPanelShower helpShower;

	public GameObject current;

	public void Create(GameObject prefab, Building building)
	{
		base.gameObject.SetActive(value: true);
		current = Object.Instantiate(prefab, base.transform);
		current.transform.SetSiblingIndex(setSiblingIndex);
		current.GetComponent<BuildingSequence>()?.Play(building);
		onEnable.Invoke();
		SetHelp(building.type);
	}

	public void SetHelp(BuildingType buildingType)
	{
		if (buildingType.helpKey.IsEmpty)
		{
			helpShower.gameObject.SetActive(value: false);
			return;
		}

		helpShower.gameObject.SetActive(value: true);
		helpShower.SetKey(buildingType.helpKey, buildingType.helpEmoteType);
	}

	public void End()
	{
		current.GetComponent<BuildingSequence>()?.building?.RunUpdateEvent();
		current.Destroy();
		current = null;
		base.gameObject.SetActive(value: false);
		onDisable.Invoke();
	}
}
