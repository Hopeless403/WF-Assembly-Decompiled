#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Spikes", fileName = "Spikes")]
public class StatusEffectSpikes : StatusEffectData
{
	public override void Init()
	{
		base.PostHit += Check;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (hit.target == target && hit.canRetaliate && hit.Offensive && hit.BasicHit)
		{
			return hit.attacker != target;
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		if ((bool)hit.attacker && hit.attacker.canBeHit)
		{
			Hit hit2 = new Hit(target, hit.attacker, count)
			{
				canRetaliate = false,
				damageType = "spikes"
			};
			yield return hit2.Process();
		}
	}
}
