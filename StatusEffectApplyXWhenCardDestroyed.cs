#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Card Destroyed", fileName = "Apply X When Card Destroyed")]
public class StatusEffectApplyXWhenCardDestroyed : StatusEffectApplyX
{
	[SerializeField]
	public bool canBeAlly = true;

	[SerializeField]
	public bool canBeEnemy = true;

	[SerializeField]
	public bool mustBeSacrificed;

	[SerializeField]
	public bool mustBeOnBoard = true;

	[SerializeField]
	public TargetConstraint[] constraints;

	public override void Init()
	{
		base.OnEntityDestroyed += Check;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (target.enabled && target.alive && CheckTeam(entity) && Battle.IsOnBoard(target) && (!mustBeOnBoard || Battle.IsOnBoard(entity)) && CheckConstraints(entity))
		{
			if (mustBeSacrificed)
			{
				return DeathSystem.KilledByOwnTeam(entity);
			}

			return true;
		}

		return false;
	}

	public IEnumerator Check(Entity entity, DeathType deathType)
	{
		if ((bool)contextEqualAmount)
		{
			int amount = contextEqualAmount.Get(entity);
			yield return Run(GetTargets(entity.lastHit), amount);
		}
		else
		{
			yield return Run(GetTargets(entity.lastHit));
		}
	}

	public bool CheckTeam(Entity entity)
	{
		if (target.owner == entity.owner)
		{
			return canBeAlly;
		}

		if (target.owner != entity.owner)
		{
			return canBeEnemy;
		}

		return false;
	}

	public new bool CheckConstraints(Entity entity)
	{
		if (constraints != null)
		{
			return constraints.All((TargetConstraint c) => c.Check(entity));
		}

		return true;
	}
}
