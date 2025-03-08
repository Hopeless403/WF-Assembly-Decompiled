#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public abstract class EntityDisplay : MonoBehaviourCacheTransform, IPoolable
{
	public Entity entity;

	public CardHover hover;

	[BoxGroup("Icon Groups")]
	public RectTransform healthLayoutGroup;

	[BoxGroup("Icon Groups")]
	public RectTransform damageLayoutGroup;

	[BoxGroup("Icon Groups")]
	public RectTransform counterLayoutGroup;

	[BoxGroup("Icon Groups")]
	public RectTransform crownLayoutGroup;

	public Dictionary<string, RectTransform> iconGroups;

	public StatusIcon healthIcon;

	public StatusIcon damageIcon;

	public StatusIcon counterIcon;

	public StatusIcon reactionIcon;

	[ReadOnly]
	public bool init;

	public bool promptUpdateDescription;

	public virtual void Awake()
	{
		iconGroups = new Dictionary<string, RectTransform>();
		if (healthLayoutGroup != null)
		{
			iconGroups["health"] = healthLayoutGroup;
		}

		if (damageLayoutGroup != null)
		{
			iconGroups["damage"] = damageLayoutGroup;
		}

		if (counterLayoutGroup != null)
		{
			iconGroups["counter"] = counterLayoutGroup;
		}

		if (crownLayoutGroup != null)
		{
			iconGroups["crown"] = crownLayoutGroup;
		}
	}

	public virtual void Reset()
	{
		init = false;
	}

	public virtual IEnumerator UpdateData(bool doPing = false)
	{
		yield return UpdateDisplay(doPing);
		init = true;
		Events.InvokeEntityDataUpdated(entity);
	}

	public virtual IEnumerator UpdateDisplay(bool doPing = true)
	{
		if ((bool)healthLayoutGroup)
		{
			if (!healthIcon)
			{
				if (entity.hp.max > 0 || entity.hp.current > 0 || ((bool)entity.data && entity.data.hasHealth))
				{
					healthIcon = SetStatusIcon("health", "health", entity.hp, doPing);
				}
			}
			else
			{
				UpdateStatusIcon(healthIcon, entity.hp, doPing);
			}
		}

		if ((bool)damageLayoutGroup)
		{
			if (!damageIcon)
			{
				if (entity.HasAttackIcon())
				{
					damageIcon = SetStatusIcon("damage", "damage", entity.damage + entity.tempDamage, doPing);
				}
			}
			else
			{
				UpdateStatusIcon(damageIcon, entity.damage + entity.tempDamage, doPing);
			}
		}

		if ((bool)counterLayoutGroup)
		{
			if (!counterIcon)
			{
				if (entity.counter.max > 0)
				{
					counterIcon = SetStatusIcon("counter", "counter", entity.counter, doPing);
				}
			}
			else
			{
				UpdateStatusIcon(counterIcon, entity.counter, doPing);
			}

			if (!reactionIcon)
			{
				if (entity.statusEffects.Any((StatusEffectData a) => a.isReaction))
				{
					reactionIcon = SetStatusIcon("reaction", "counter", Stat.Default, doPing);
				}
			}
			else
			{
				UpdateStatusIcon(reactionIcon, Stat.Default, doPing);
			}
		}

		foreach (StatusEffectData statusEffect in entity.statusEffects)
		{
			if (statusEffect.visible && !statusEffect.iconGroupName.IsNullOrEmpty())
			{
				SetStatusIcon(statusEffect.type, statusEffect.iconGroupName, new Stat(statusEffect.count, 0), doPing);
			}
		}

		foreach (KeyValuePair<string, RectTransform> iconGroup in iconGroups)
		{
			foreach (RectTransform item in iconGroup.Value)
			{
				StatusIcon component = item.GetComponent<StatusIcon>();
				if ((bool)component)
				{
					component.CheckRemove();
				}
			}
		}

		Events.InvokeEntityDisplayUpdated(entity);
		yield return null;
	}

	public virtual Canvas GetCanvas()
	{
		return null;
	}

	public StatusIcon FindStatusIcon(string type)
	{
		StatusIcon statusIcon = null;
		foreach (KeyValuePair<string, RectTransform> iconGroup in iconGroups)
		{
			foreach (RectTransform item in iconGroup.Value)
			{
				StatusIcon component = item.GetComponent<StatusIcon>();
				if ((bool)component && component.type == type)
				{
					statusIcon = component;
					break;
				}
			}

			if ((bool)statusIcon)
			{
				break;
			}
		}

		return statusIcon;
	}

	public StatusIcon SetStatusIcon(string type, string iconGroupName, Stat value, bool doPing = true)
	{
		StatusIcon statusIcon = FindStatusIcon(type);
		if ((bool)statusIcon)
		{
			UpdateStatusIcon(statusIcon, value, doPing);
		}
		else
		{
			statusIcon = CardManager.NewStatusIcon(type, iconGroups[iconGroupName]);
			if (!statusIcon)
			{
				Debug.LogError("Status Icon for [" + type + "] NOT FOUND!");
			}
			else
			{
				if ((bool)hover)
				{
					CardHover component = statusIcon.GetComponent<CardHover>();
					component.master = hover;
					component.enabled = true;
				}

				statusIcon.Assign(entity);
				statusIcon.SetValue(value, doPing);
				if (doPing)
				{
					statusIcon.CreateEvent();
					Events.InvokeStatusIconCreated(statusIcon);
				}
			}
		}

		return statusIcon;
	}

	public static void UpdateStatusIcon(StatusIcon icon, Stat value, bool doPing = true)
	{
		icon.SetValue(value, doPing);
	}

	public void RemoveStatusIcon(string type, string iconGroupName)
	{
		StatusIcon statusIcon = FindStatusIcon(type);
		if (statusIcon != null)
		{
			statusIcon.Destroy();
		}
	}

	public void ClearStatusIcons()
	{
		foreach (KeyValuePair<string, RectTransform> iconGroup in iconGroups)
		{
			iconGroup.Value.DestroyAllChildren();
		}
	}

	public virtual void OnGetFromPool()
	{
		init = false;
		promptUpdateDescription = false;
	}

	public virtual void OnReturnToPool()
	{
		ClearStatusIcons();
	}

	public EntityDisplay()
	{
	}
}
