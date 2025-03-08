#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X Pre Turn", fileName = "Apply X Pre Turn")]
public class StatusEffectApplyXPreTurn : StatusEffectApplyX
{
	[SerializeField]
	public bool mustHaveTarget;

	public bool running;

	public List<Entity> runAgainst;

	public override void Init()
	{
		if (queue)
		{
			Events.OnPreProcessTrigger += PreProcessTrigger;
			return;
		}

		Events.OnEntityTrigger += CheckTrigger;
		base.PreCardPlayed += CheckPreCardPlay;
	}

	public void OnDestroy()
	{
		if (queue)
		{
			Events.OnPreProcessTrigger -= PreProcessTrigger;
		}
		else
		{
			Events.OnEntityTrigger -= CheckTrigger;
		}
	}

	public void PreProcessTrigger(Entity entity)
	{
		if (entity == target && !running && target.enabled)
		{
			ActionQueue.Stack(new ActionSequence(Run(GetTargets())), fixedPosition: true);
		}
	}

	public void CheckTrigger(ref Trigger trigger)
	{
		if (!running && target.enabled && trigger.entity == target)
		{
			runAgainst = GetTargets();
			if (mustHaveTarget && (runAgainst == null || runAgainst.Count <= 0))
			{
				trigger.nullified = true;
			}
		}
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!running && target.enabled)
		{
			List<Entity> list = runAgainst;
			if (list != null && list.Count > 0)
			{
				return entity == target;
			}
		}

		return false;
	}

	public IEnumerator CheckPreCardPlay(Entity entity, Entity[] targets)
	{
		yield return RunSequence(runAgainst);
	}

	public IEnumerator RunSequence(List<Entity> targets)
	{
		running = true;
		yield return Run(targets);
		runAgainst = null;
		running = false;
	}
}
