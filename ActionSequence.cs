#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionSequence : PlayAction
{
	public readonly Routine routine;

	public ActionSequence(IEnumerator enumerator)
	{
		routine = new Routine(enumerator, autoStart: false);
	}

	public override IEnumerator Run()
	{
		routine.Start();
		while (routine.IsRunning)
		{
			yield return null;
		}
	}
}
