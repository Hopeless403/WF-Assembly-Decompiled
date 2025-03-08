#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Unit Is Hit", fileName = "Apply X When Unit Is Hit")]
public class StatusEffectApplyXWhenUnitIsHit : StatusEffectApplyX
{
	[SerializeField]
	public bool ally = true;

	[SerializeField]
	public bool enemy;

	[SerializeField]
	public TargetConstraint[] unitConstraints;

	[SerializeField]
	public TargetConstraint[] attackerConstraints;

	public override void Init()
	{
		base.PostHit += Check;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (target.enabled && target.alive && hit.canRetaliate && hit.BasicHit && hit.Offensive && CheckTeam(hit.target) && (!targetMustBeAlive || (hit.target.alive && Battle.IsOnBoard(hit.target))) && Battle.IsOnBoard(target) && CheckConstraints(hit.target))
		{
			return CheckAttackerConstraints(hit.attacker);
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		if ((bool)contextEqualAmount)
		{
			int amount = contextEqualAmount.Get(hit.target);
			yield return Run(GetTargets(hit), amount);
		}
		else
		{
			yield return Run(GetTargets(hit));
		}
	}

	public bool CheckTeam(Entity entity)
	{
		if (target.owner == entity.owner)
		{
			return ally;
		}

		if (target.owner != entity.owner)
		{
			return enemy;
		}

		return false;
	}

	public new bool CheckConstraints(Entity entity)
	{
		if (unitConstraints != null)
		{
			return unitConstraints.All((TargetConstraint c) => c.Check(entity));
		}

		return true;
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
