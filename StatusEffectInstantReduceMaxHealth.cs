#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Reduce Max Health", fileName = "Reduce Max Health")]
public class StatusEffectInstantReduceMaxHealth : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		target.hp.max -= GetAmount();
		target.hp.current -= GetAmount();
		target.PromptUpdate();
		yield return base.Process();
	}
}
