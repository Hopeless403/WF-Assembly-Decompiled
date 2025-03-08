#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Double X", fileName = "Double X")]
public class StatusEffectInstantDoubleX : StatusEffectInstant
{
	public StatusEffectData statusToDouble;

	[SerializeField]
	public bool countsAsHit = true;

	public override IEnumerator Process()
	{
		StatusEffectData statusEffectData = target.statusEffects.Find((StatusEffectData a) => a.name == statusToDouble.name);
		if ((bool)statusEffectData)
		{
			target.curveAnimator.Ping();
			Hit hit = new Hit(applier, target, 0)
			{
				countsAsHit = countsAsHit
			};
			hit.AddStatusEffect(statusToDouble, statusEffectData.count);
			yield return hit.Process();
		}

		yield return base.Process();
	}
}
