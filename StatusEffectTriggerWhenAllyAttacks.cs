#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Reactions/Trigger When Ally In Row Attacks", fileName = "Trigger When Ally In Row Attacks")]
public class StatusEffectTriggerWhenAllyAttacks : StatusEffectReaction
{
	[SerializeField]
	public bool allyInRow = true;

	[SerializeField]
	public bool againstTarget;

	public readonly HashSet<Entity> prime = new HashSet<Entity>();

	public override bool RunHitEvent(Hit hit)
	{
		if (target.enabled && Battle.IsOnBoard(target) && hit.countsAsHit && hit.Offensive && (bool)hit.target && hit.trigger != null && CheckEntity(hit.attacker))
		{
			prime.Add(hit.attacker);
		}

		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (prime.Count > 0 && prime.Contains(entity) && targets != null && targets.Length > 0)
		{
			prime.Remove(entity);
			if (Battle.IsOnBoard(target) && CanTrigger())
			{
				Run(entity, targets);
			}
		}

		return false;
	}

	public void Run(Entity attacker, Entity[] targets)
	{
		if (againstTarget)
		{
			foreach (Entity entity in targets)
			{
				ActionQueue.Stack(new ActionTriggerAgainst(target, attacker, entity, null), fixedPosition: true);
			}
		}
		else
		{
			ActionQueue.Stack(new ActionTrigger(target, attacker), fixedPosition: true);
		}
	}

	public bool CheckEntity(Entity entity)
	{
		if ((bool)entity && entity.owner.team == target.owner.team && entity != target && CheckRow(entity) && Battle.IsOnBoard(entity) && CheckDuplicate(entity))
		{
			return CheckDuplicate(entity.triggeredBy);
		}

		return false;
	}

	public bool CheckRow(Entity entity)
	{
		if (allyInRow)
		{
			return entity.containers.ContainsAny(target.containers);
		}

		return true;
	}

	public bool CheckDuplicate(Entity entity)
	{
		if (!entity.IsAliveAndExists())
		{
			return true;
		}

		foreach (StatusEffectData statusEffect in entity.statusEffects)
		{
			if (statusEffect.name == base.name)
			{
				return false;
			}
		}

		return true;
	}
}
