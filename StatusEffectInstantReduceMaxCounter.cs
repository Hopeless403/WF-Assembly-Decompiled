#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Reduce Max Counter", fileName = "Reduce Max Counter")]
public class StatusEffectInstantReduceMaxCounter : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		target.counter.current = Mathf.Max(0, target.counter.current - GetAmount());
		target.counter.max = Mathf.Max(1, target.counter.max - GetAmount());
		target.PromptUpdate();
		yield return base.Process();
	}
}
