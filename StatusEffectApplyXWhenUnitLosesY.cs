#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Unit Loses Y", fileName = "Apply X When Unit Loses Y")]
public class StatusEffectApplyXWhenUnitLosesY : StatusEffectApplyX
{
	[SerializeField]
	public bool self = true;

	[SerializeField]
	public bool allies;

	[SerializeField]
	public bool enemies;

	[SerializeField]
	public string statusType = "block";

	[SerializeField]
	public bool whenAllLost;

	public readonly Dictionary<ulong, int> currentAmounts = new Dictionary<ulong, int>();

	public override void Init()
	{
		Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
	}

	public void OnDestroy()
	{
		Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (entity != target)
		{
			return false;
		}

		if (self)
		{
			StoreCurrentAmount(target);
		}

		if (allies)
		{
			foreach (Entity allAlly in target.GetAllAllies())
			{
				StoreCurrentAmount(allAlly);
			}
		}

		if (enemies)
		{
			foreach (Entity allEnemy in target.GetAllEnemies())
			{
				StoreCurrentAmount(allEnemy);
			}
		}

		return false;
	}

	public override bool RunDisableEvent(Entity entity)
	{
		if (entity == target)
		{
			currentAmounts.Clear();
		}

		return false;
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		if (!entity.enabled || !Battle.IsOnBoard(entity))
		{
			return;
		}

		if (entity == target)
		{
			if (self)
			{
				CheckEntity(entity);
			}
		}
		else if (entity.owner.team == target.owner.team)
		{
			if (allies)
			{
				CheckEntity(entity);
			}
		}

		else if (enemies)
		{
			CheckEntity(entity);
		}
	}

	public IEnumerator Lost(int amount)
	{
		if ((bool)this && target.IsAliveAndExists())
		{
			yield return Run(GetTargets(), amount);
		}
	}

	public int GetCurrentAmount(Entity entity)
	{
		return entity.FindStatus(statusType)?.count ?? 0;
	}

	public void StoreCurrentAmount(Entity entity)
	{
		currentAmounts[entity.data.id] = GetCurrentAmount(entity);
	}

	public void CheckEntity(Entity entity)
	{
		currentAmounts.TryGetValue(entity.data.id, out var value);
		int currentAmount = GetCurrentAmount(entity);
		currentAmounts[entity.data.id] = currentAmount;
		int num = currentAmount - value;
		if (num < 0 && (!whenAllLost || value == 0) && !target.silenced && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target))))
		{
			ActionQueue.Stack(new ActionSequence(Lost(-num))
			{
				note = base.name,
				priority = eventPriority
			}, fixedPosition: true);
		}
	}
}
