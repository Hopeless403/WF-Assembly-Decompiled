#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Ongoing/Increase Max Counter", fileName = "Ongoing Increase Max Counter")]
public class StatusEffectOngoingMaxCounter : StatusEffectOngoing
{
	public override IEnumerator Add(int add)
	{
		target.counter.current = Mathf.Max(0, target.counter.current + add);
		target.counter.max = Mathf.Max(1, target.counter.max + add);
		target.PromptUpdate();
		yield return Sequences.Null();
	}

	public override IEnumerator Remove(int remove)
	{
		target.counter.current = Mathf.Max(0, target.counter.current - remove);
		target.counter.max = Mathf.Max(1, target.counter.max - remove);
		target.PromptUpdate();
		yield return Sequences.Null();
	}
}
