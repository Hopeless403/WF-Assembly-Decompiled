#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class UpgradeHolder : MonoBehaviour
{
	[SerializeField]
	public AssetReference prefabRef;

	[SerializeField]
	public CardCharmDragHandler dragHandler;

	[SerializeField]
	public List<UpgradeDisplay> list = new List<UpgradeDisplay>();

	[SerializeField]
	public bool autoUpdateOverrideInputs;

	[Button(null, EButtonEnableMode.Always)]
	public virtual void SetPositions()
	{
		foreach (RectTransform item in base.transform)
		{
			item.anchoredPosition = Vector2.zero;
		}
	}

	public virtual UpgradeDisplay Create(CardUpgradeData upgradeData)
	{
		AsyncOperationHandle<GameObject> asyncOperationHandle = prefabRef.InstantiateAsync(base.transform);
		asyncOperationHandle.WaitForCompletion();
		UpgradeDisplay component = asyncOperationHandle.Result.GetComponent<UpgradeDisplay>();
		component.gameObject.SetActive(value: true);
		component.SetData(upgradeData);
		component.name = upgradeData.name;
		if ((bool)dragHandler)
		{
			CardCharmInteraction component2 = component.GetComponent<CardCharmInteraction>();
			if ((object)component2 != null)
			{
				component2.dragHandler = dragHandler;
				component2.onDrag.AddListener(dragHandler.Drag);
				component2.onDragEnd.AddListener(dragHandler.Release);
			}
		}

		Add(component);
		return component;
	}

	public virtual void Add(UpgradeDisplay upgrade)
	{
		Insert(0, upgrade);
	}

	public virtual void Insert(int index, UpgradeDisplay upgrade)
	{
		list.Insert(index, upgrade);
		upgrade.transform.SetParent(base.transform, worldPositionStays: false);
		upgrade.transform.localPosition = Vector3.zero;
		upgrade.transform.localEulerAngles = Vector3.zero;
		if (autoUpdateOverrideInputs)
		{
			SetInputOverrides();
		}
	}

	public virtual void Remove(UpgradeDisplay upgrade)
	{
		list.Remove(upgrade);
		upgrade.transform.SetParent(null, worldPositionStays: false);
		if (autoUpdateOverrideInputs)
		{
			SetInputOverrides();
		}
	}

	public virtual void Remove(CardUpgradeData upgradeData)
	{
		UpgradeDisplay upgradeDisplay = list.Find((UpgradeDisplay a) => a.data == upgradeData);
		if (upgradeDisplay != null)
		{
			Remove(upgradeDisplay);
		}
	}

	public virtual int IndexOf(UpgradeDisplay upgrade)
	{
		return list.IndexOf(upgrade);
	}

	public virtual void Clear()
	{
		list.Clear();
		base.transform.DestroyAllChildren();
	}

	public abstract void SetInputOverrides();

	public UpgradeHolder()
	{
	}
}
