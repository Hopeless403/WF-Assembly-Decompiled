#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X Pre Trigger", fileName = "Apply X Pre Trigger")]
public class StatusEffectApplyXPreTrigger : StatusEffectApplyX
{
	[SerializeField]
	public bool mustHaveTarget;

	[SerializeField]
	public bool oncePerTurn = true;

	public bool running;

	public bool hasRunThisTurn;

	public List<Entity> runAgainst;

	public override void Init()
	{
		base.PreTrigger += EntityPreTrigger;
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		return CheckTrigger(trigger);
	}

	public IEnumerator EntityPreTrigger(Trigger trigger)
	{
		if (oncePerTurn)
		{
			hasRunThisTurn = true;
		}

		running = true;
		yield return Run(runAgainst);
		runAgainst = null;
		running = false;
	}

	public bool CheckTrigger(Trigger trigger)
	{
		if (hasRunThisTurn || running || !target.enabled || trigger.entity != target)
		{
			return false;
		}

		runAgainst = GetTargets();
		if (mustHaveTarget && (runAgainst == null || runAgainst.Count <= 0))
		{
			trigger.nullified = true;
			return false;
		}

		return true;
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (hasRunThisTurn && entity == target)
		{
			hasRunThisTurn = false;
		}

		return false;
	}
}
