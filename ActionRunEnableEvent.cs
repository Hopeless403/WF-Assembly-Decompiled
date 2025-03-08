#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionRunEnableEvent : PlayAction
{
	public readonly Entity entity;

	public ActionRunEnableEvent(Entity entity)
	{
		this.entity = entity;
	}

	public override IEnumerator Run()
	{
		if (entity.StillExists())
		{
			entity.enabled = true;
			Events.InvokeEntityEnabled(entity);
			yield return StatusEffectSystem.EntityEnableEvent(entity);
		}
	}
}
