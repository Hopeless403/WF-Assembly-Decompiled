#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Double Negative Effects", fileName = "Double Negative Effects")]
public class StatusEffectInstantDoubleNegativeEffects : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		Hit hit = new Hit(applier, target, 0);
		for (int num = target.statusEffects.Count - 1; num >= 0; num--)
		{
			StatusEffectData statusEffectData = target.statusEffects[num];
			if (statusEffectData.IsNegativeStatusEffect())
			{
				hit.AddStatusEffect(statusEffectData, statusEffectData.count);
			}
		}

		if (hit.Offensive)
		{
			yield return hit.Process();
		}

		yield return base.Process();
	}
}
