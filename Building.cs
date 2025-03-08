#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
	[Serializable]
	public class Icon
	{
		public string name;

		public GameObject obj;
	}

	public BuildingType type;

	public bool canSelect = true;

	[SerializeField]
	public UnityEventGameObject onSelect;

	[SerializeField]
	public GameObject toBuild;

	[SerializeField]
	public GameObject underConstruction;

	[SerializeField]
	public GameObject fullyBuilt;

	[SerializeField]
	public Icon[] icons;

	[SerializeField]
	public UnityEvent onDataUpdated;

	public bool buildStarted;

	public bool built;

	public List<string> unlocks;

	public List<string> uncheckedUnlocks;

	public GameObject currentActiveIcon;

	public List<string> checkedUnlocks
	{
		get
		{
			if (unlocks == null)
			{
				return null;
			}

			if (uncheckedUnlocks == null)
			{
				return unlocks;
			}

			return unlocks.Except(uncheckedUnlocks).ToList();
		}
	}

	public bool HasUncheckedUnlocks
	{
		get
		{
			List<string> list = uncheckedUnlocks;
			if (list != null)
			{
				return list.Count > 0;
			}

			return false;
		}
	}

	public void CheckIfUnlocked()
	{
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		buildStarted = type.started == null || MetaprogressionSystem.IsUnlocked(type.started, unlockedList);
		built = type.finished == null || MetaprogressionSystem.IsUnlocked(type.finished, unlockedList);
		if (!built && !buildStarted)
		{
			base.gameObject.SetActive(value: false);
			return;
		}

		base.gameObject.SetActive(value: true);
		RunUpdateEvent();
	}

	public void RunUpdateEvent()
	{
		onDataUpdated?.Invoke();
		if (built && type.unlocks != null && type.unlocks.Length != 0)
		{
			List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
			unlocks = type.unlocks.Select((UnlockData a) => a.name).Intersect(unlockedList).ToList();
			if (unlocks.Count > 0 && !type.unlockedCheckedKey.IsNullOrWhitespace())
			{
				List<string> list = SaveSystem.LoadProgressData<List<string>>(type.unlockedCheckedKey);
				uncheckedUnlocks = ((list == null) ? new List<string>(unlocks) : unlocks.Except(list).ToList());
				Debug.Log(string.Format("[{0}] has unchecked {1} unlocks [{2}]", base.name, uncheckedUnlocks.Count, string.Join(", ", uncheckedUnlocks)));
			}
		}

		UpdateSprite();
		SetSuitableIcon();
	}

	public bool Select()
	{
		try
		{
			if (canSelect && onSelect != null)
			{
				onSelect.Invoke(onSelect.GetPersistentTarget(0) as GameObject);
				return true;
			}
		}
		catch (ArgumentOutOfRangeException)
		{
			return false;
		}

		return false;
	}

	public void UpdateSprite()
	{
		if ((bool)toBuild)
		{
			toBuild.gameObject.SetActive(!buildStarted && !built);
		}

		if ((bool)underConstruction)
		{
			underConstruction.gameObject.SetActive(buildStarted && !built);
		}

		if ((bool)fullyBuilt)
		{
			fullyBuilt.gameObject.SetActive(built);
		}
	}

	public void SetSuitableIcon()
	{
		string icon = "";
		BuildingIconSetter[] components = GetComponents<BuildingIconSetter>();
		for (int i = 0; i < components.Length; i++)
		{
			icon = components[i].Get(this);
		}

		SetIcon(icon);
	}

	public void SetIcon(string name)
	{
		if ((bool)currentActiveIcon)
		{
			currentActiveIcon.SetActive(value: false);
		}

		if (!name.IsNullOrWhitespace())
		{
			Icon icon = icons.FirstOrDefault((Icon a) => a.name == name);
			if (icon != null)
			{
				currentActiveIcon = icon.obj;
				currentActiveIcon.SetActive(value: true);
			}
		}
	}

	public void Bloop()
	{
		base.transform.localScale = new Vector3(1.5f, 2f / 3f, 1f);
		LeanTween.cancel(base.gameObject);
		LeanTween.scale(base.gameObject, Vector3.one, 1.5f).setEase(LeanTweenType.easeOutElastic);
	}

	public void CreateDisplay(GameObject prefab)
	{
		UnityEngine.Object.FindObjectOfType<BuildingDisplay>(includeInactive: true)?.Create(prefab, this);
	}
}
