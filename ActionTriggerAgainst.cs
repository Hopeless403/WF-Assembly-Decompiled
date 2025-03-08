#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionTriggerAgainst : ActionTrigger
{
	public Entity target;

	public readonly CardContainer targetContainer;

	public Trigger trigger;

	public bool countsAsTrigger = true;

	public override bool IsRoutine => false;

	public ActionTriggerAgainst(Entity entity, Entity triggeredBy, Entity target, CardContainer targetContainer)
		: base(entity, triggeredBy)
	{
		this.target = target;
		this.targetContainer = targetContainer;
	}

	public override void Process()
	{
		if (entity.IsAliveAndExists())
		{
			Events.InvokePreProcessTrigger(entity);
			ActionQueue.Stack(new ActionProcessTrigger(GetTrigger));
		}
	}

	public override Trigger GetTrigger()
	{
		Entity[] targets = GetTargets();
		if (trigger == null)
		{
			trigger = new Trigger(entity, triggeredBy, triggerType, targets);
		}

		trigger.triggerAgainst = true;
		trigger.triggerAgainstTarget = target;
		trigger.triggerAgainstContainer = targetContainer;
		trigger.countsAsTrigger = countsAsTrigger;
		return trigger;
	}

	public virtual Entity[] GetTargets()
	{
		if (!countsAsTrigger)
		{
			return new Entity[1] { target };
		}

		if (!entity.targetMode)
		{
			return null;
		}

		return entity.targetMode.GetTargets(entity, target, targetContainer);
	}
}
