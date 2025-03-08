#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Bonus Damage Equal To Enemy Stat", fileName = "Bonus Damage Equal To Enemy Stat")]
public class StatusEffectBonusDamageEqualToEnemyStat : StatusEffectData
{
	[SerializeField]
	public SelectScript<Entity> selectEnemy;

	[SerializeField]
	public ScriptableAmount scriptableAmount;

	[SerializeField]
	public bool add = true;

	[SerializeField]
	public bool ping = true;

	public int currentAmount;

	public bool toReset;

	public override void Init()
	{
		base.PreCardPlayed += Gain;
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target && !toReset)
		{
			return CanTrigger();
		}

		return false;
	}

	public IEnumerator Gain(Entity entity, Entity[] targets)
	{
		Entity enemy = SelectEnemy();
		int amount = GetAmount(enemy);
		if (amount != 0)
		{
			toReset = true;
			int value = target.tempDamage.Value;
			if (add)
			{
				target.tempDamage += amount;
			}
			else
			{
				target.tempDamage.Value = amount;
			}

			currentAmount = target.tempDamage.Value - value;
			target.PromptUpdate();
			if (ping)
			{
				target.curveAnimator.Ping();
				yield return Sequences.Wait(0.5f);
			}
		}
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target.owner.entity && toReset)
		{
			toReset = false;
			if (currentAmount != 0)
			{
				target.tempDamage -= currentAmount;
				currentAmount = 0;
				target.PromptUpdate();
			}
		}

		return false;
	}

	public Entity SelectEnemy()
	{
		List<Entity> enemies = target.GetEnemies();
		return selectEnemy.Run(enemies).FirstOrDefault();
	}

	public int GetAmount(Entity enemy)
	{
		if (!scriptableAmount)
		{
			return GetAmount();
		}

		if (!enemy)
		{
			return 0;
		}

		return scriptableAmount.Get(enemy);
	}
}
