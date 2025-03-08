#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Allies Attack", fileName = "Apply X When Allies Attack")]
public class StatusEffectApplyXWhenAlliesAttack : StatusEffectApplyX
{
	public override void Init()
	{
		base.PreAttack += HitCheck;
	}

	public override bool RunPreAttackEvent(Hit hit)
	{
		if (target.enabled && target.alive && hit.attacker.owner == target.owner && hit.attacker != target)
		{
			return hit.Offensive;
		}

		return false;
	}

	public IEnumerator HitCheck(Hit hit)
	{
		return Run(GetTargets(hit));
	}
}
