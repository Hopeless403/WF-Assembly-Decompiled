#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionInspect : PlayAction
{
	public Entity entity;

	public InspectSystem inspectSystem;

	public override bool IsRoutine => false;

	public ActionInspect(Entity entity, InspectSystem inspectSystem)
	{
		this.entity = entity;
		this.inspectSystem = inspectSystem;
	}

	public override void Process()
	{
		inspectSystem.Inspect(entity);
	}
}
