#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Multiple", fileName = "Instant Apply X")]
public class StatusEffectInstantMultiple : StatusEffectInstant
{
	public StatusEffectInstant[] effects;

	public StatusEffectApplyXInstant[] applyXEffects;

	public override bool CanStackActions => false;

	public override IEnumerator Process()
	{
		int amount = GetAmount();
		Routine.Clump clump = new Routine.Clump();
		StatusEffectInstant[] array = effects;
		foreach (StatusEffectInstant statusEffectInstant in array)
		{
			if (!statusEffectInstant.canBeBoosted || amount > 0)
			{
				clump.Add(StatusEffectSystem.Apply(target, applier, statusEffectInstant, amount, temporary: true));
			}
		}

		StatusEffectApplyXInstant[] array2 = applyXEffects;
		foreach (StatusEffectApplyXInstant statusEffectApplyXInstant in array2)
		{
			if (!statusEffectApplyXInstant.canBeBoosted || amount > 0)
			{
				clump.Add(StatusEffectSystem.Apply(target, applier, statusEffectApplyXInstant, amount, temporary: true));
			}
		}

		yield return clump.WaitForEnd();
		yield return base.Process();
	}
}
