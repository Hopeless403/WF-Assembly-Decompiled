#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Anyone Takes Damage", fileName = "Apply X When Anyone Takes Damage")]
public class StatusEffectApplyXWhenAnyoneTakesDamage : StatusEffectApplyX
{
	[SerializeField]
	public string targetDamageType = "basic";

	public override void Init()
	{
		base.PostHit += CheckHit;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (target.enabled && target.alive && hit.Offensive && hit.damageType == targetDamageType)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator CheckHit(Hit hit)
	{
		return Run(GetTargets(hit));
	}
}
