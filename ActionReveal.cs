#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionReveal : PlayAction
{
	public readonly Entity entity;

	public override bool IsRoutine => false;

	public ActionReveal(Entity entity, float pauseAfter = 0f)
	{
		this.entity = entity;
		base.pauseAfter = pauseAfter;
	}

	public override void Process()
	{
		if (entity.flipper.flipped)
		{
			entity.flipper.FlipUp();
		}

		entity.enabled = true;
	}
}
