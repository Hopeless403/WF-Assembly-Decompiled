#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Linq;

public class ActionProcessTrigger : PlayAction
{
	public Trigger trigger;

	public readonly Func<Trigger> GetTriggerMethod;

	public ActionProcessTrigger(Trigger trigger)
	{
		this.trigger = trigger;
	}

	public ActionProcessTrigger(Func<Trigger> GetTriggerMethod)
	{
		this.GetTriggerMethod = GetTriggerMethod;
	}

	public override IEnumerator Run()
	{
		if (trigger == null && GetTriggerMethod != null)
		{
			trigger = GetTriggerMethod();
		}

		Events.InvokeEntityPreTrigger(ref trigger);
		yield return StatusEffectSystem.PreTriggerEvent(trigger);
		bool num = trigger.entity.HasAttackIcon();
		if (!num && trigger.entity.attackEffects.Count <= 0)
		{
			trigger.targets = null;
		}

		if (num)
		{
			Entity[] targets = trigger.targets;
			if ((targets == null || targets.Length <= 0) && NoTargetTextSystem.Exists())
			{
				yield return NoTargetTextSystem.Run(trigger.entity, NoTargetType.NoTargetToAttack);
			}
		}

		if (trigger.targets != null)
		{
			trigger.targets = trigger.targets.Where((Entity t) => t.IsAliveAndExists()).ToArray();
		}

		trigger.entity.triggeredBy = trigger.triggeredBy;
		Events.InvokeEntityTrigger(ref trigger);
		if (!trigger.nullified && !trigger.entity.IsSnowed)
		{
			Entity[] targets = trigger.targets;
			if (targets != null && targets.Length > 0)
			{
				yield return trigger.Process();
				yield return Sequences.Wait(0.167f);
			}
			else
			{
				yield return trigger.Process();
			}
		}

		Events.InvokeEntityTriggered(ref trigger);
		trigger.entity.triggeredBy = null;
	}
}
