#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Unit Is Killed", fileName = "Apply X When Unit Is Killed")]
public class StatusEffectApplyXWhenUnitIsKilled : StatusEffectApplyX
{
	[SerializeField]
	public bool ally = true;

	[SerializeField]
	public bool enemy;

	[SerializeField]
	public bool sacrificed;

	[SerializeField]
	public TargetConstraint[] unitConstraints;

	public override void Init()
	{
		base.OnEntityDestroyed += Check;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (target.enabled && target.alive && CheckTeam(entity) && Battle.IsOnBoard(entity) && Battle.IsOnBoard(target) && CheckUnitConstraints(entity))
		{
			if (sacrificed)
			{
				return DeathSystem.KilledByOwnTeam(entity);
			}

			return true;
		}

		return false;
	}

	public IEnumerator Check(Entity entity, DeathType deathType)
	{
		if (entity.LastHitStillProcessing())
		{
			yield return entity.WaitForLastHitToFinishProcessing();
		}

		if (CheckUnitConstraints(entity))
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
	}

	public bool CheckTeam(Entity entity)
	{
		if (!DeathSystem.CheckTeamIsAlly(entity, target))
		{
			return enemy;
		}

		return ally;
	}

	public bool CheckUnitConstraints(Entity entity)
	{
		if (unitConstraints != null)
		{
			return unitConstraints.All((TargetConstraint c) => c.Check(entity));
		}

		return true;
	}
}
