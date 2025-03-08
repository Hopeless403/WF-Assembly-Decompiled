#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;

public class ActionSelect : PlayAction
{
	public Entity entity;

	public Action<Entity> action;

	public override bool IsRoutine => false;

	public ActionSelect(Entity entity, Action<Entity> action)
	{
		this.entity = entity;
		this.action = action;
	}

	public override void Process()
	{
		action(entity);
	}
}
