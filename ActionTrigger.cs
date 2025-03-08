#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionTrigger : PlayAction
{
	public readonly Entity entity;

	public readonly Entity triggeredBy;

	public string triggerType = "basic";

	public override bool IsRoutine => false;

	public ActionTrigger(Entity entity, Entity triggeredBy)
	{
		this.entity = entity;
		this.triggeredBy = triggeredBy;
	}

	public override void Process()
	{
		if (entity.IsAliveAndExists())
		{
			Events.InvokePreProcessTrigger(entity);
			ActionQueue.Stack(new ActionProcessTrigger(GetTrigger));
		}
	}

	public virtual Trigger GetTrigger()
	{
		Entity[] targets = (entity.targetMode ? entity.targetMode.GetTargets(entity, null, null) : null);
		return new Trigger(entity, triggeredBy, triggerType, targets);
	}
}
