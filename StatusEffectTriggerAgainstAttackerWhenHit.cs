#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Reactions/Trigger Against Attacker When Hit", fileName = "Trigger Against Attacker When Hit")]
public class StatusEffectTriggerAgainstAttackerWhenHit : StatusEffectReaction
{
	public Entity attacker;

	public Entity previousTarget;

	public int? effectiveHealthState;

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (effectiveHealthState.HasValue && entity == target)
		{
			previousTarget = null;
			effectiveHealthState = null;
		}

		return false;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target && hit.canRetaliate && !attacker && hit.Offensive && hit.BasicHit && (bool)hit.target && (bool)hit.attacker)
		{
			Trigger trigger = hit.trigger;
			if (trigger != null && trigger.countsAsTrigger)
			{
				attacker = hit.attacker;
			}
		}

		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if ((bool)attacker && entity == attacker)
		{
			attacker = null;
			if (Battle.IsOnBoard(target) && CanTrigger())
			{
				Run(entity);
			}
		}

		return false;
	}

	public void Run(Entity entity)
	{
		if (!entity.data || !entity.data.cardType.unit)
		{
			Entity entity2 = entity.owner?.entity;
			if ((bool)entity2 && entity2.canBeHit)
			{
				entity = entity.owner?.entity;
				if (entity == target.owner?.entity)
				{
					entity = null;
				}
			}
		}

		if ((bool)entity && entity.canBeHit && entity.IsAliveAndExists())
		{
			int num = CalculateEffectiveHealthState();
			if (effectiveHealthState.HasValue && (bool)previousTarget && previousTarget == entity && num == effectiveHealthState.Value)
			{
				Debug.LogWarning("Smackback infinite loop detected!");
				return;
			}

			previousTarget = entity;
			effectiveHealthState = num;
			CardContainer targetRow = Trigger.GetTargetRow(target, entity);
			ActionQueue.Stack(new ActionTriggerSubsequent(target, entity, entity, targetRow)
			{
				triggerType = "smackback"
			}, fixedPosition: true);
		}
	}

	public static int CalculateEffectiveHealthState()
	{
		int num = 0;
		foreach (Entity item in Battle.GetCardsOnBoard())
		{
			num += GetEffectiveHealth(item);
		}

		return num;
	}

	public static int GetEffectiveHealth(Entity entity)
	{
		int num = entity.hp.current;
		foreach (StatusEffectData statusEffect in entity.statusEffects)
		{
			switch (statusEffect.type)
			{
				case "shell":
				case "scrap":
				case "block":
					num += statusEffect.count;
					break;
				case "overload":
					num -= statusEffect.count;
					break;
				case "shroom":
					num -= Mathf.Min(statusEffect.count, entity.hp.current);
					break;
			}
		}

		return num;
	}
}
