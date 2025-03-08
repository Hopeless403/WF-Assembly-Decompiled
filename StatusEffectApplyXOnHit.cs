#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X On Hit", fileName = "Apply X On Hit")]
public class StatusEffectApplyXOnHit : StatusEffectApplyX
{
	[SerializeField]
	public bool postHit;

	[Header("Modify Damage")]
	[SerializeField]
	public int addDamageFactor;

	[SerializeField]
	public float multiplyDamageFactor = 1f;

	public readonly List<Hit> storedHit = new List<Hit>();

	public override void Init()
	{
		if (postHit)
		{
			base.PostHit += CheckHit;
		}
		else
		{
			base.OnHit += CheckHit;
		}
	}

	public override bool RunPreAttackEvent(Hit hit)
	{
		if (hit.attacker == target && target.alive && target.enabled && (bool)hit.target)
		{
			if (addDamageFactor != 0 || multiplyDamageFactor != 1f)
			{
				bool flag = true;
				TargetConstraint[] array = applyConstraints;
				foreach (TargetConstraint targetConstraint in array)
				{
					if (!targetConstraint.Check(hit.target) && (!(targetConstraint is TargetConstraintHasStatus targetConstraintHasStatus) || !targetConstraintHasStatus.CheckWillApply(hit)))
					{
						flag = false;
						break;
					}
				}

				if (flag)
				{
					int amount = GetAmount();
					if (addDamageFactor != 0)
					{
						hit.damage += amount * addDamageFactor;
					}

					if (multiplyDamageFactor != 1f)
					{
						hit.damage = Mathf.RoundToInt((float)hit.damage * multiplyDamageFactor);
					}
				}
			}

			if (!hit.Offensive && (hit.damage > 0 || ((bool)effectToApply && effectToApply.offensive)))
			{
				hit.FlagAsOffensive();
			}

			storedHit.Add(hit);
		}

		return false;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (storedHit.Contains(hit))
		{
			return hit.Offensive;
		}

		return false;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (storedHit.Contains(hit))
		{
			return hit.Offensive;
		}

		return false;
	}

	public IEnumerator CheckHit(Hit hit)
	{
		if ((bool)effectToApply)
		{
			yield return Run(GetTargets(hit), hit.damage + hit.damageBlocked);
		}

		storedHit.Remove(hit);
	}
}
