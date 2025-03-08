#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Frost", fileName = "Frost")]
public class StatusEffectFrost : StatusEffectData
{
	public int toClear;

	public int current;

	public int addedThisTurn;

	public override void Init()
	{
		base.OnActionPerformed += ActionPerformed;
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		if (trigger.entity == target)
		{
			addedThisTurn = 0;
		}

		return false;
	}

	public override bool RunStackEvent(int stacks)
	{
		current += stacks;
		target.tempDamage -= stacks;
		addedThisTurn += stacks;
		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (toClear == 0 && entity == target && count > 0 && targets != null && targets.Length > 0)
		{
			toClear = current - addedThisTurn;
		}

		return false;
	}

	public override bool RunActionPerformedEvent(PlayAction action)
	{
		if (toClear > 0)
		{
			return ActionQueue.Empty;
		}

		return false;
	}

	public IEnumerator ActionPerformed(PlayAction action)
	{
		yield return Clear(toClear);
		toClear = 0;
	}

	public IEnumerator Clear(int amount)
	{
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			current -= amount;
			target.tempDamage += amount;
			yield return CountDown(target, amount);
		}
	}

	public override bool RunEndEvent()
	{
		target.tempDamage += current;
		return false;
	}
}
