#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Hit", fileName = "Apply X When Hit")]
public class StatusEffectApplyXWhenHit : StatusEffectApplyX
{
	[SerializeField]
	public TargetConstraint[] attackerConstraints;

	public override void Init()
	{
		base.PostHit += CheckHit;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (target.enabled && hit.target == target && hit.canRetaliate && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target))) && hit.Offensive && hit.BasicHit)
		{
			return CheckAttackerConstraints(hit.attacker);
		}

		return false;
	}

	public IEnumerator CheckHit(Hit hit)
	{
		return Run(GetTargets(hit, GetTargetContainers(), GetTargetActualContainers()), hit.damage + hit.damageBlocked);
	}

	public bool CheckAttackerConstraints(Entity attacker)
	{
		if (attackerConstraints != null)
		{
			TargetConstraint[] array = attackerConstraints;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].Check(attacker))
				{
					return false;
				}
			}
		}

		return true;
	}
}
