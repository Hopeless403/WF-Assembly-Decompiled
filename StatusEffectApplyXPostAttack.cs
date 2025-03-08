#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X Post Attack", fileName = "Apply X Post Attack")]
public class StatusEffectApplyXPostAttack : StatusEffectApplyX
{
	public override void Init()
	{
		base.PostAttack += CheckHit;
	}

	public override bool RunPostAttackEvent(Hit hit)
	{
		if (target.enabled && hit.attacker == target && target.alive)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator CheckHit(Hit hit)
	{
		yield return Run(GetTargets(hit));
	}
}
