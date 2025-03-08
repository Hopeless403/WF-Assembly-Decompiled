#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Deployed", fileName = "Apply X When Deployed")]
public class StatusEffectApplyXWhenDeployed : StatusEffectApplyX
{
	[SerializeField]
	public bool whenSelfDeployed = true;

	[SerializeField]
	public bool whenAllyDeployed;

	[SerializeField]
	public bool whenEnemyDeployed;

	public Hit hackyHit;

	public bool isAlreadyOnBoard;

	public override object GetMidBattleData()
	{
		return Battle.IsOnBoard(target);
	}

	public override void RestoreMidBattleData(object data)
	{
		if (data is bool)
		{
			bool flag = (bool)data;
			isAlreadyOnBoard = flag && Battle.IsOnBoard(target);
		}
	}

	public override void Init()
	{
		base.OnEnable += Activate;
		base.OnCardMove += Activate;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (whenSelfDeployed && entity == target && Battle.IsOnBoard(target))
		{
			if (isAlreadyOnBoard)
			{
				isAlreadyOnBoard = false;
				return false;
			}

			hackyHit = null;
			return true;
		}

		if (whenAllyDeployed && target.enabled && IsAlly(target, entity) && Battle.IsOnBoard(target) && Battle.IsOnBoard(entity))
		{
			hackyHit = new Hit(target, entity);
			return true;
		}

		if (whenEnemyDeployed && target.enabled && IsEnemy(target, entity) && Battle.IsOnBoard(target) && Battle.IsOnBoard(entity))
		{
			hackyHit = new Hit(target, entity);
			return true;
		}

		return false;
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (!target.enabled || !entity.enabled)
		{
			return false;
		}

		if (whenSelfDeployed && entity == target && WasMovedOnToBoard(entity))
		{
			hackyHit = null;
			return true;
		}

		if (whenAllyDeployed && IsAlly(target, entity) && WasMovedOnToBoard(entity))
		{
			hackyHit = new Hit(target, entity);
			return true;
		}

		if (whenEnemyDeployed && IsEnemy(target, entity) && WasMovedOnToBoard(entity))
		{
			hackyHit = new Hit(target, entity);
			return true;
		}

		return false;
	}

	public IEnumerator Activate(Entity entity)
	{
		if ((bool)contextEqualAmount)
		{
			int amount = contextEqualAmount.Get(entity);
			yield return Run(GetTargets(hackyHit), amount);
		}
		else
		{
			yield return Run(GetTargets(hackyHit));
		}
	}

	public static bool WasMovedOnToBoard(Entity entity)
	{
		if (Battle.IsOnBoard(entity))
		{
			return !Battle.IsOnBoard(entity.preContainers);
		}

		return false;
	}

	public static bool IsAlly(Entity a, Entity b)
	{
		if (a != b)
		{
			return a.owner.team == b.owner.team;
		}

		return false;
	}

	public static bool IsEnemy(Entity a, Entity b)
	{
		return a.owner.team != b.owner.team;
	}
}
