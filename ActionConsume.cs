#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionConsume : PlayAction
{
	public readonly Entity target;

	public bool blocked { get; set; }

	public ActionConsume(Entity target)
	{
		this.target = target;
	}

	public override IEnumerator Run()
	{
		if (!blocked)
		{
			yield return Sequences.WaitForAnimationEnd(target);
			target.alive = true;
			yield return target.Kill(DeathType.Consume);
		}
	}

	public void Block()
	{
		target.alive = true;
		blocked = true;
	}
}
