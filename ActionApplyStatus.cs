#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionApplyStatus : PlayAction
{
	public Entity target;

	public Entity applier;

	public StatusEffectData status;

	public int stacks;

	public bool temporary;

	public ActionApplyStatus(Entity target, Entity applier, StatusEffectData status, int stacks, bool temporary = false)
	{
		this.target = target;
		this.applier = applier;
		this.status = status;
		this.stacks = stacks;
		this.temporary = temporary;
	}

	public override IEnumerator Run()
	{
		yield return StatusEffectSystem.Apply(target, applier, status, stacks, temporary);
		yield return Sequences.Wait(0.6f);
	}
}
