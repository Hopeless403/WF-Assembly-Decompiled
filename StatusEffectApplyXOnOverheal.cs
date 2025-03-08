#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X On Overheal", fileName = "Apply X On Overheal")]
public class StatusEffectApplyXOnOverheal : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnHit += Check;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target && hit.damage < 0)
		{
			return target.hp.current - hit.damage - target.hp.max > 0;
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		return Run(GetTargets(hit), target.hp.current - hit.damage - target.hp.max);
	}
}
