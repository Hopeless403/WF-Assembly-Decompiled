#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Take Attack", fileName = "Take Attack")]
public class StatusEffectInstantTakeAttack : StatusEffectInstant
{
	[SerializeField]
	public StatusEffectData increaseAttackEffect;

	public override IEnumerator Process()
	{
		int amount = GetAmount();
		target.damage.max -= amount;
		target.damage.current -= amount;
		target.PromptUpdate();
		Hit hit = new Hit(target, applier, 0)
		{
			canRetaliate = false,
			countsAsHit = false
		};
		hit.AddStatusEffect(increaseAttackEffect, amount);
		yield return hit.Process();
		yield return base.Process();
	}
}
