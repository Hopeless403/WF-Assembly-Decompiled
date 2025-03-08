#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Gain Trait", fileName = "Gain Trait")]
public class StatusEffectInstantGainTrait : StatusEffectInstant
{
	public TraitData traitToGain;

	public override IEnumerator Process()
	{
		int amount = GetAmount();
		target.GainTrait(traitToGain, amount);
		yield return target.UpdateTraits();
		target.display.promptUpdateDescription = true;
		target.PromptUpdate();
		yield return base.Process();
	}
}
