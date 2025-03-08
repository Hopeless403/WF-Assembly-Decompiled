#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Bonus Damage Equal To X", fileName = "Bonus Damage Equal To X")]
public class StatusEffectBonusDamageEqualToX : StatusEffectData
{
	public enum On
	{
		Self,
		Board,
		ScriptableAmount
	}

	[SerializeField]
	public On on;

	[SerializeField]
	[ShowIf("useScriptableAmount")]
	public ScriptableAmount scriptableAmount;

	[SerializeField]
	public bool add = true;

	[SerializeField]
	[HideIf("useScriptableAmount")]
	public bool health;

	[HideIf("health")]
	public string effectType = "shell";

	[SerializeField]
	public bool ping = true;

	public int currentAmount;

	public bool toReset;

	public bool useScriptableAmount => on == On.ScriptableAmount;

	public override void Init()
	{
		base.PreCardPlayed += Gain;
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target)
		{
			return CanTrigger();
		}

		return false;
	}

	public IEnumerator Gain(Entity entity, Entity[] targets)
	{
		int num = Find();
		if (!toReset || num != currentAmount)
		{
			if (toReset)
			{
				LoseCurrentAmount();
			}

			if (num > 0)
			{
				yield return GainAmount(num);
			}
		}
	}

	public IEnumerator GainAmount(int amount)
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

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target.owner.entity && toReset)
		{
			LoseCurrentAmount();
		}

		return false;
	}

	public void LoseCurrentAmount()
	{
		toReset = false;
		if (currentAmount != 0)
		{
			target.tempDamage -= currentAmount;
			currentAmount = 0;
			target.PromptUpdate();
		}
	}

	public int Find()
	{
		switch (on)
		{
			case On.Self:
				return FindOnSelf();
			case On.Board:
				return FindOnBoard();
			case On.ScriptableAmount:
				return scriptableAmount.Get(target);
			default:
				return 0;
		}
	}

	public int FindOnSelf()
	{
		int result = 0;
		if (health)
		{
			result = target.hp.current;
		}
		else
		{
			StatusEffectData statusEffectData = target.FindStatus(effectType);
			if ((bool)statusEffectData && statusEffectData.count > 0)
			{
				result = statusEffectData.count;
			}
		}

		return result;
	}

	public int FindOnBoard()
	{
		int num = 0;
		if (health)
		{
			return num + Battle.GetCardsOnBoard().Sum((Entity e) => target.hp.current);
		}

		return num + (from entity in Battle.GetCardsOnBoard()
			select entity.FindStatus(effectType) into effect
			where (bool)effect && effect.count > 0
			select effect).Sum((StatusEffectData effect) => effect.count);
	}
}
