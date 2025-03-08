#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Null", fileName = "Null")]
public class StatusEffectNull : StatusEffectData
{
	public bool primed;

	public override void Init()
	{
		base.OnTurnEnd += TurnEnd;
	}

	public override bool RunBeginEvent()
	{
		target.silenceCount++;
		return false;
	}

	public override bool RunEndEvent()
	{
		target.silenceCount--;
		return false;
	}

	public override bool RunTurnStartEvent(Entity entity)
	{
		if (!primed && entity == target && Battle.IsOnBoard(entity))
		{
			primed = true;
		}

		return false;
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target)
		{
			return primed;
		}

		return false;
	}

	public IEnumerator TurnEnd(Entity entity)
	{
		int amount = 1;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(entity, amount);
		}
	}
}
