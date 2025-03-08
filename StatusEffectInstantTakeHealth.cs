#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Take Health", fileName = "Take Health")]
public class StatusEffectInstantTakeHealth : StatusEffectInstant
{
	[SerializeField]
	public StatusEffectData increaseHealthEffect;

	public override IEnumerator Process()
	{
		int amount = GetAmount();
		int num = Mathf.Min(target.hp.current, amount);
		target.hp.max -= amount;
		target.hp.current -= amount;
		target.PromptUpdate();
		Hit hit = new Hit(target, applier, 0)
		{
			canRetaliate = false,
			countsAsHit = false
		};
		hit.AddStatusEffect(increaseHealthEffect, num);
		yield return hit.Process();
		yield return base.Process();
	}
}
