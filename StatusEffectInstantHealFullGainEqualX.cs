#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Heal Full, Gain Equal X", fileName = "Heal Full, Gain Equal X")]
public class StatusEffectInstantHealFullGainEqualX : StatusEffectInstant
{
	[SerializeField]
	public StatusEffectData effectToGain;

	public override IEnumerator Process()
	{
		int num = target.hp.max - target.hp.current;
		target.curveAnimator.Ping();
		target.hp.current = target.hp.max;
		Hit hit = new Hit(target, target, 0);
		hit.AddStatusEffect(effectToGain, GetAmount() * num);
		yield return hit.Process();
		yield return base.Process();
	}
}
