#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionTriggerSubsequent : ActionTriggerAgainst
{
	public ActionTriggerSubsequent(Entity entity, Entity triggeredBy, Entity target, CardContainer targetContainer)
		: base(entity, triggeredBy, target, targetContainer)
	{
	}

	public override Entity[] GetTargets()
	{
		if (!entity.targetMode)
		{
			return null;
		}

		return entity.targetMode.GetSubsequentTargets(entity, target, targetContainer);
	}
}
