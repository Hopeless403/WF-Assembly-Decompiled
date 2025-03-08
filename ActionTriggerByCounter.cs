#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionTriggerByCounter : ActionTrigger
{
	public override bool IsRoutine => false;

	public ActionTriggerByCounter(Entity entity, Entity triggeredBy)
		: base(entity, triggeredBy)
	{
	}

	public override void Process()
	{
		if (entity.IsAliveAndExists())
		{
			Events.InvokePreProcessTrigger(entity);
			ActionQueue.Stack(new ActionProcessTrigger(GetTrigger));
		}
	}
}
