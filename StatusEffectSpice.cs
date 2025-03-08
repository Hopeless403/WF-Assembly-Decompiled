#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Spice", fileName = "Spice")]
public class StatusEffectSpice : StatusEffectData
{
	public bool cardPlayed;

	public int current;

	public int amountToClear;

	public override void Init()
	{
		base.OnActionPerformed += ActionPerformed;
	}

	public override bool RunStackEvent(int stacks)
	{
		current += stacks;
		target.tempDamage += stacks;
		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!cardPlayed && entity == target && count > 0 && targets != null && targets.Length != 0)
		{
			cardPlayed = true;
			amountToClear = current;
		}

		return false;
	}

	public override bool RunActionPerformedEvent(PlayAction action)
	{
		if (cardPlayed)
		{
			return ActionQueue.Empty;
		}

		return false;
	}

	public IEnumerator ActionPerformed(PlayAction action)
	{
		cardPlayed = false;
		yield return Clear(amountToClear);
	}

	public IEnumerator Clear(int amount)
	{
		int amount2 = amount;
		Events.InvokeStatusEffectCountDown(this, ref amount2);
		if (amount2 != 0)
		{
			current -= amount2;
			target.tempDamage -= amount2;
			yield return CountDown(target, amount2);
		}
	}

	public override bool RunEndEvent()
	{
		target.tempDamage -= current;
		return false;
	}
}
