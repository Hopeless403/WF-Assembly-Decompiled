#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Stealth", fileName = "Stealth")]
public class StatusEffectStealth : StatusEffectData
{
	public bool cardPlayed;

	public override void Init()
	{
		base.OnActionPerformed += ActionPerformed;
	}

	public override bool RunBeginEvent()
	{
		target.cannotBeHitCount++;
		return false;
	}

	public override bool RunEndEvent()
	{
		target.cannotBeHitCount--;
		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!cardPlayed && entity == target && count > 0 && targets != null && targets.Length != 0)
		{
			cardPlayed = true;
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
		yield return CountDown();
	}

	public IEnumerator CountDown()
	{
		int amount = 1;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(target, amount);
		}
	}
}
