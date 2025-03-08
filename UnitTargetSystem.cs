#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class UnitTargetSystem : GameSystem
{
	[SerializeField]
	public UnitTarget targetPrefab;

	public Entity dragging;

	public Entity hover;

	public readonly List<GameObject> targets = new List<GameObject>();

	public void OnEnable()
	{
		Events.OnEntityDrag += EntityDrag;
		Events.OnEntityRelease += EntityRelease;
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityDrag -= EntityDrag;
		Events.OnEntityRelease -= EntityRelease;
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
	}

	public void Start()
	{
		Events.OnSettingChanged += SettingChanged;
		if (Settings.Load("UnitTargets", 1) != 1)
		{
			base.enabled = false;
		}
	}

	public void OnDestroy()
	{
		Events.OnSettingChanged -= SettingChanged;
	}

	public void EntityDrag(Entity entity)
	{
		dragging = entity;
		if ((bool)hover)
		{
			HideTargets();
		}
	}

	public void EntityRelease(Entity entity)
	{
		if (dragging == entity)
		{
			dragging = null;
		}
	}

	public void EntityHover(Entity entity)
	{
		if (!dragging)
		{
			if ((bool)hover)
			{
				EntityUnHover(hover);
			}

			hover = entity;
			if (entity.inPlay && entity.counter.max > 0 && entity.HasAttackIcon() && Battle.IsOnBoard(hover))
			{
				ShowTargets(hover);
			}
		}
	}

	public void EntityUnHover(Entity entity)
	{
		if (hover == entity)
		{
			hover = null;
			HideTargets();
		}
	}

	public void SettingChanged(string key, object value)
	{
		if (key == "UnitTargets" && value is int)
		{
			int num = (int)value;
			if (num == 0 && base.enabled)
			{
				base.enabled = false;
			}
			else if (num == 1 && !base.enabled)
			{
				base.enabled = true;
			}
		}
	}

	public void ShowTargets(Entity entity)
	{
		Entity[] potentialTargets = entity.targetMode.GetPotentialTargets(entity, null, null);
		if (potentialTargets != null)
		{
			bool random = entity.targetMode.Random;
			StatusEffectData statusEffectData = entity.FindStatus("frenzy");
			bool frenzy = (object)statusEffectData != null && statusEffectData.count > 0;
			Entity[] array = potentialTargets;
			foreach (Entity entity2 in array)
			{
				UnitTarget unitTarget = Object.Instantiate(targetPrefab, entity2.transform.position, Quaternion.identity, base.transform);
				unitTarget.SetAimless(random);
				unitTarget.SetFrenzy(frenzy);
				targets.Add(unitTarget.gameObject);
			}
		}
	}

	public void HideTargets()
	{
		foreach (GameObject target in targets)
		{
			target.Destroy();
		}

		targets.Clear();
	}
}
