#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit
{
	public Entity attacker;

	public Character owner;

	public readonly Entity target;

	public string damageType = "basic";

	public Trigger trigger;

	public int damage;

	public int damageBlocked;

	public int counterReduction;

	public float screenShake = 1f;

	public bool countsAsHit = true;

	public bool canBeNullified = true;

	public bool nullified;

	public List<CardData.StatusEffectStacks> statusEffects;

	public bool doAnimation = true;

	public bool canRetaliate = true;

	public bool dodged;

	public int extraOffensiveness;

	public bool processing;

	public int damageDealt { get; set; }

	public bool Offensive { get; set; }

	public bool Supportive
	{
		get
		{
			if (damage < 0)
			{
				return true;
			}

			if (statusEffects != null)
			{
				foreach (CardData.StatusEffectStacks statusEffect in statusEffects)
				{
					if (!statusEffect.data.offensive)
					{
						return true;
					}
				}
			}

			return false;
		}
	}

	public bool BasicHit => damageType == "basic";

	public int GetOffensiveness()
	{
		int num = Mathf.Max(0, damage) + damageBlocked + extraOffensiveness;
		if (statusEffects != null)
		{
			foreach (CardData.StatusEffectStacks statusEffect in statusEffects)
			{
				if (statusEffect.data.offensive)
				{
					num += statusEffect.count;
				}
			}
		}

		return num;
	}

	public int GetSupportiveness()
	{
		int num = Mathf.Max(0, -damage);
		if (statusEffects != null)
		{
			foreach (CardData.StatusEffectStacks statusEffect in statusEffects)
			{
				if (!statusEffect.data.offensive)
				{
					num += statusEffect.count;
				}
			}
		}

		return num;
	}

	public Hit(Entity attacker, Entity target)
	{
		this.attacker = attacker;
		if ((bool)attacker)
		{
			owner = attacker.owner;
			damage = Mathf.Max(0, attacker.damage.current + attacker.tempDamage.Value);
			countsAsHit = attacker.HasAttackIcon();
			if (countsAsHit)
			{
				Offensive = attacker.IsOffensive();
			}
		}

		this.target = target;
	}

	public Hit(Entity attacker, Entity target, int damage)
	{
		this.attacker = attacker;
		if ((bool)attacker)
		{
			owner = attacker.owner;
		}

		this.target = target;
		this.damage = damage;
		Offensive = damage > 0;
	}

	public void AddAttackerStatuses()
	{
		if (!attacker.data || attacker.attackEffects == null || attacker.silenced)
		{
			return;
		}

		foreach (CardData.StatusEffectStacks attackEffect in attacker.attackEffects)
		{
			int num = CalculateAttackEffectAmount(attackEffect.data, attackEffect.count);
			if (num > 0)
			{
				AddStatusEffect(attackEffect.data, num);
			}
		}
	}

	public int CalculateAttackEffectAmount(StatusEffectData data, int statusEffectStacks)
	{
		if (!attacker.silenced)
		{
			if (!data.stackable)
			{
				return statusEffectStacks;
			}

			return Mathf.Max(0, Mathf.RoundToInt((float)(statusEffectStacks + attacker.effectBonus) * attacker.effectFactor));
		}

		return 0;
	}

	public void AddStatusEffect(CardData.StatusEffectStacks statusEffect)
	{
		if (statusEffects == null)
		{
			statusEffects = new List<CardData.StatusEffectStacks>();
		}

		statusEffects.Add(statusEffect);
		if (!Offensive && countsAsHit && statusEffect.data.offensive)
		{
			Offensive = true;
		}
	}

	public void AddStatusEffect(StatusEffectData statusEffectData, int count)
	{
		AddStatusEffect(new CardData.StatusEffectStacks
		{
			data = statusEffectData,
			count = count
		});
	}

	public IEnumerator Process()
	{
		if (!target)
		{
			yield break;
		}

		processing = true;
		if (countsAsHit)
		{
			target.lastHit = this;
		}

		if (countsAsHit)
		{
			yield return StatusEffectSystem.HitEvent(this);
		}

		if (!dodged)
		{
			Events.InvokeEntityHit(this);
			if (!nullified)
			{
				if (damage != 0)
				{
					damageDealt = damage;
					int num = target.hp.current - damage;
					if (num > target.hp.max)
					{
						damageDealt += num - target.hp.max;
						num = target.hp.max;
					}

					target.hp.current = num;
				}

				target.counter.current = Mathf.Max(target.counter.current - counterReduction, 0);
			}

			Routine.Clump clump = new Routine.Clump();
			if (!nullified)
			{
				List<CardData.StatusEffectStacks> list = statusEffects;
				if (list != null && list.Count > 0)
				{
					foreach (CardData.StatusEffectStacks statusEffect in statusEffects)
					{
						clump.Add(StatusEffectSystem.Apply(target, attacker, statusEffect.data, statusEffect.count));
					}
				}
			}

			target.PromptUpdate();
			if (clump.Count > 0)
			{
				yield return clump.WaitForEnd();
			}
		}
		else
		{
			Events.InvokeEntityDodge(this);
		}

		processing = false;
		Events.InvokeEntityPostHit(this);
		if (countsAsHit)
		{
			yield return StatusEffectSystem.PostHitEvent(this);
		}

		yield return Sequences.Null();
	}

	public void FlagAsOffensive()
	{
		Offensive = true;
	}
}
