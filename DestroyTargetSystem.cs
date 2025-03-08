#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyTargetSystem : GameSystem
{
	[SerializeField]
	public Transform targetPrefab;

	[SerializeField]
	public Vector3 iconOffset;

	public Entity dragging;

	public Entity hover;

	public readonly Dictionary<Transform, Entity> targets = new Dictionary<Transform, Entity>();

	public readonly List<Transform> targetPool = new List<Transform>();

	public readonly HashSet<Entity> toIndicate = new HashSet<Entity>();

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

	public void Update()
	{
		foreach (KeyValuePair<Transform, Entity> target in targets)
		{
			Transform offset = target.Value.offset;
			Vector3 lossyScale = offset.lossyScale;
			target.Key.position = offset.position + Vector3.Scale(iconOffset, lossyScale);
			target.Key.localScale = lossyScale;
		}
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
			if (entity.inPlay)
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
		if (entity.silenced)
		{
			return;
		}

		foreach (StatusEffectData statusEffect in entity.statusEffects)
		{
			StatusEffectApplyX applyEffect = statusEffect as StatusEffectApplyX;
			if ((object)applyEffect != null && !statusEffect.doesDamage)
			{
				if (CheckStatusEffectType(applyEffect))
				{
					if ((applyEffect.applyToFlags & StatusEffectApplyX.ApplyToFlags.Hand) != 0)
					{
						CheckShowIconsForHand(applyEffect, toIndicate);
					}

					if ((applyEffect.applyToFlags & StatusEffectApplyX.ApplyToFlags.RightCardInHand) != 0)
					{
						CheckShowIconsForRightCardInHand(applyEffect, toIndicate);
					}

					continue;
				}

				StatusEffectData effectToApply = applyEffect.effectToApply;
				StatusEffectInstantDestroyCardsInHandAndApplyXForEach destroyAllEffect = effectToApply as StatusEffectInstantDestroyCardsInHandAndApplyXForEach;
				if ((object)destroyAllEffect != null)
				{
					toIndicate.AddRange(References.Player.handContainer.Where((Entity card) => card != applyEffect.target && destroyAllEffect.CheckConstraints(card)));
				}
			}
			else
			{
				if (!(statusEffect is StatusEffectRecycle statusEffectRecycle))
				{
					continue;
				}

				int num = statusEffectRecycle.GetAmount();
				foreach (Entity item in References.Player.handContainer)
				{
					if (item.data.name == statusEffectRecycle.cardToRecycle)
					{
						toIndicate.Add(item);
						if (--num <= 0)
						{
							break;
						}
					}
				}
			}
		}

		foreach (Entity item2 in toIndicate)
		{
			ShowIcon(item2);
		}

		toIndicate.Clear();
	}

	public static bool CheckStatusEffectType(StatusEffectApplyX applyEffect)
	{
		if (CheckApplyStatusEffectType(applyEffect.effectToApply))
		{
			return true;
		}

		if (applyEffect.effectToApply is StatusEffectInstantMultiple statusEffectInstantMultiple)
		{
			StatusEffectInstant[] effects = statusEffectInstantMultiple.effects;
			for (int i = 0; i < effects.Length; i++)
			{
				if (CheckApplyStatusEffectType(effects[i]))
				{
					return true;
				}
			}
		}

		return false;
	}

	public static bool CheckApplyStatusEffectType(StatusEffectData applyEffect)
	{
		if (!(applyEffect is StatusEffectInstantKill))
		{
			return applyEffect is StatusEffectInstantSacrifice;
		}

		return true;
	}

	public static void CheckShowIconsForHand(StatusEffectApplyX applyEffect, HashSet<Entity> toIndicate)
	{
		toIndicate.AddRange(References.Player.handContainer.Where((Entity card) => card != applyEffect.target && applyEffect.CheckConstraints(card)));
	}

	public static void CheckShowIconsForRightCardInHand(StatusEffectApplyX applyEffect, HashSet<Entity> toIndicate)
	{
		CardContainer handContainer = References.Player.handContainer;
		if ((object)handContainer != null && handContainer.Count > 0 && applyEffect.CheckConstraints(References.Player.handContainer[0]))
		{
			toIndicate.Add(References.Player.handContainer[0]);
		}
	}

	public void HideTargets()
	{
		foreach (KeyValuePair<Transform, Entity> target in targets)
		{
			target.Key.gameObject.SetActive(value: false);
			targetPool.Add(target.Key);
		}

		targets.Clear();
	}

	public void ShowIcon(Entity entity)
	{
		Transform key = PullIcon();
		targets.Add(key, entity);
	}

	public Transform PullIcon()
	{
		Transform transform;
		if (targetPool.Count > 0)
		{
			transform = targetPool[0];
			targetPool.RemoveAt(0);
		}
		else
		{
			transform = Object.Instantiate(targetPrefab, base.transform);
		}

		transform.gameObject.SetActive(value: true);
		return transform;
	}
}
