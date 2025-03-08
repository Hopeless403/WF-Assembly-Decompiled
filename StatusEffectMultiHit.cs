#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Multi Hit", fileName = "Multi Hit")]
public class StatusEffectMultiHit : StatusEffectData
{
	[SerializeField]
	public bool clearAfter;

	public int attackCount;

	public Trigger originalTrigger;

	public List<ActionTrigger> additionalTriggers;

	public override void Init()
	{
		Events.OnEntityTrigger += EntityTrigger;
	}

	public void OnDestroy()
	{
		Events.OnEntityTrigger -= EntityTrigger;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target && target.alive && target.enabled)
		{
			attackCount--;
			if (entity.IsSnowed)
			{
				attackCount = 0;
			}

			if (attackCount <= 0)
			{
				Cancel();
			}
		}

		return false;
	}

	public void EntityTrigger(ref Trigger trigger)
	{
		if (trigger.entity != target || !trigger.countsAsTrigger)
		{
			return;
		}

		if (originalTrigger != null)
		{
			if (target.IsSnowed)
			{
				Cancel();
			}

			return;
		}

		attackCount = 1;
		originalTrigger = trigger;
		if (trigger.triggerAgainst)
		{
			for (int i = 0; i < count; i++)
			{
				AddTrigger(new ActionTriggerSubsequent(target, null, trigger.triggerAgainstTarget, trigger.triggerAgainstContainer)
				{
					note = "[" + target.name + "] Frenzy"
				});
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				AddTrigger(new ActionTrigger(target, null)
				{
					note = "[" + target.name + "] Frenzy"
				});
			}
		}
	}

	public void Cancel()
	{
		attackCount = 0;
		originalTrigger = null;
		if (additionalTriggers != null)
		{
			foreach (ActionTrigger additionalTrigger in additionalTriggers)
			{
				ActionQueue.Remove(additionalTrigger);
			}
		}

		additionalTriggers = null;
		if (clearAfter)
		{
			ActionQueue.Stack(new ActionSequence(Clear())
			{
				note = "Clear Temporary MultiHit"
			}, fixedPosition: true);
		}
	}

	public void AddTrigger(ActionTrigger trigger)
	{
		ActionQueue.Stack(trigger);
		if (additionalTriggers == null)
		{
			additionalTriggers = new List<ActionTrigger>();
		}

		additionalTriggers.Add(trigger);
		attackCount++;
	}

	public IEnumerator Clear()
	{
		int amount = count;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(target, amount);
		}
	}
}
