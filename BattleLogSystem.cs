#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class BattleLogSystem : GameSystem
{
	public List<BattleLog> list = new List<BattleLog>();

	public static readonly Dictionary<string, string> damageTypes = new Dictionary<string, string>
	{
		{ "shroom", "<sprite name=shroom>" },
		{ "spikes", "<sprite name=teeth>" },
		{ "overload", "<sprite name=overload>" }
	};

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logTurnKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logHitKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logDamageKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logSpecialDamageKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logDestroyKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logConsumedKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logEatenKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logSacrificedKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logBlockKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logStatusKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logStatusFromKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logHealKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logRestoredKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logBoostKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logDamageUpKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logDamageUpSelfKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logDamageDownKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logDamageDownSelfKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logHealthUpKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logHealthUpSelfKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logHealthDownKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logHealthDownSelfKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logCounterUpKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logCounterUpSelfKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logCounterDownKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logCounterDownSelfKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logEnterBattleKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logRecalledKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logSummonedKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logBattleWinKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logBattleLoseKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString logFleeKey;

	public void OnEnable()
	{
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnBattleTurnEnd += TurnEnd;
		Events.OnEntityHit += Hit;
		Events.OnEntityMove += EntityMove;
		Events.OnStatusEffectApplied += StatusApplied;
		Events.OnEntityPostHit += PostHit;
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntityFlee += EntityFlee;
		Events.OnBattleEnd += BattleEnd;
	}

	public void OnDisable()
	{
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnBattleTurnEnd -= TurnEnd;
		Events.OnEntityHit -= Hit;
		Events.OnEntityMove -= EntityMove;
		Events.OnStatusEffectApplied -= StatusApplied;
		Events.OnEntityPostHit -= PostHit;
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntityFlee -= EntityFlee;
		Events.OnBattleEnd -= BattleEnd;
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (phase == Battle.Phase.Init)
		{
			list.Clear();
		}
	}

	public void TurnEnd(int turnNumber)
	{
		if (!References.Battle.ended)
		{
			Log(logTurnKey, BattleLogType.Turn, turnNumber + 1);
		}
	}

	public void Hit(Hit hit)
	{
		if (hit.countsAsHit && hit.Offensive)
		{
			if (!hit.BasicHit || !hit.attacker || !hit.attacker.data)
			{
				LogDamage(hit.target, hit.damage + hit.damageBlocked, hit.damageType);
			}
			else
			{
				LogHit(hit.attacker, hit.target, hit.damage + hit.damageBlocked, hit.damageType);
			}
		}
	}

	public void EntityMove(Entity entity)
	{
		if (Battle.IsOnBoard(entity) && !Battle.IsOnBoard(entity.preContainers))
		{
			Log(logEnterBattleKey, BattleLogType.Enter, GetBattleEntity(entity));
		}
	}

	public void StatusApplied(StatusEffectApply apply)
	{
		if ((bool)apply.effectData && !apply.effectData.type.IsNullOrWhitespace() && apply.count > 0)
		{
			switch (apply.effectData.type)
			{
				case "heal":
					LogHeal(apply.applier, apply.target);
					break;
				case "damage up":
					LogDamageUp(apply.applier, apply.target, apply.count);
					break;
				case "damage down":
					LogDamageDown(apply.applier, apply.target, apply.count);
					break;
				case "max health up":
					LogHealthUp(apply.applier, apply.target, apply.count);
					break;
				case "max health down":
					LogHealthDown(apply.applier, apply.target, apply.count);
					break;
				case "counter up":
				case "max counter up":
					LogCounterUp(apply.applier, apply.target, apply.count);
					break;
				case "counter down":
				case "max counter down":
					LogCounterDown(apply.applier, apply.target, apply.count);
					break;
				default:
					LogStatus(apply.applier, apply.target, apply.effectData, apply.count);
					break;
			}
		}
	}

	public void PostHit(Hit hit)
	{
		if (hit.countsAsHit && hit.Offensive && hit.damageBlocked > 0)
		{
			LogBlock(hit.target, hit.damageBlocked);
		}
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if (deathType != DeathType.Consume)
		{
			if (DeathSystem.KilledByOwnTeam(entity))
			{
				Log(logSacrificedKey, BattleLogType.Die, GetBattleEntity(entity));
			}
			else
			{
				Log(logDestroyKey, BattleLogType.Die, GetBattleEntity(entity));
			}
		}
		else
		{
			Log(logConsumedKey, BattleLogType.Consume, GetBattleEntity(entity));
		}
	}

	public void EntityFlee(Entity entity)
	{
		Log(logFleeKey, BattleLogType.Flee, GetBattleEntity(entity));
	}

	public void BattleEnd()
	{
		if (References.Battle.winner == References.Battle.player)
		{
			Log(logBattleWinKey, BattleLogType.Win);
		}
		else
		{
			Log(logBattleLoseKey, BattleLogType.Win);
		}
	}

	public void LogHit(Entity attacker, Entity target, int damage, string damageType)
	{
		if ((bool)target)
		{
			Log(logHitKey, BattleLogType.Attack, GetBattleEntity(attacker), GetBattleEntity(target), damage);
		}
	}

	public void LogDamage(Entity target, int damage, string damageType)
	{
		if (damage > 0)
		{
			if (damageTypes.TryGetValue(damageType, out var value))
			{
				Log(logSpecialDamageKey, BattleLogType.Debuff, GetBattleEntity(target), damage, value);
			}
			else
			{
				Log(logDamageKey, BattleLogType.Debuff, GetBattleEntity(target), damage);
			}
		}
	}

	public void LogBlock(Entity target, int damageBlocked)
	{
		Log(logBlockKey, BattleLogType.Buff, GetBattleEntity(target), damageBlocked);
	}

	public void LogStatus(Entity applier, Entity target, StatusEffectData status, int count)
	{
		if (status.isStatus)
		{
			BattleLogType type = (status.offensive ? BattleLogType.Debuff : BattleLogType.Buff);
			if ((bool)applier && applier.data.id != target.data.id)
			{
				Log(logStatusFromKey, type, GetBattleEntity(applier), GetBattleEntity(target), count, status.type);
			}
			else
			{
				Log(logStatusKey, type, GetBattleEntity(target), count, status.type);
			}
		}
	}

	public void LogHeal(Entity healer, Entity target)
	{
		Log(logHealKey, BattleLogType.Heal, GetBattleEntity(healer), GetBattleEntity(target));
	}

	public void LogRestore(Entity target, int amount)
	{
		Log(logRestoredKey, BattleLogType.Heal, GetBattleEntity(target), amount);
	}

	public void LogDamageUp(Entity applier, Entity target, int amount)
	{
		if (!applier || applier == target)
		{
			Log(logDamageUpSelfKey, BattleLogType.Buff, GetBattleEntity(target), amount);
		}
		else
		{
			Log(logDamageUpKey, BattleLogType.Buff, GetBattleEntity(applier), GetBattleEntity(target), amount);
		}
	}

	public void LogDamageDown(Entity applier, Entity target, int amount)
	{
		if (!applier || applier == target)
		{
			Log(logDamageDownSelfKey, BattleLogType.Debuff, GetBattleEntity(target), amount);
		}
		else
		{
			Log(logDamageDownKey, BattleLogType.Debuff, GetBattleEntity(applier), GetBattleEntity(target), amount);
		}
	}

	public void LogHealthUp(Entity applier, Entity target, int amount)
	{
		if (!applier || applier == target)
		{
			Log(logHealthUpSelfKey, BattleLogType.Buff, GetBattleEntity(target), amount);
		}
		else
		{
			Log(logHealthUpKey, BattleLogType.Buff, GetBattleEntity(applier), GetBattleEntity(target), amount);
		}
	}

	public void LogHealthDown(Entity applier, Entity target, int amount)
	{
		if (!applier || applier == target)
		{
			Log(logHealthDownSelfKey, BattleLogType.Debuff, GetBattleEntity(target), amount);
		}
		else
		{
			Log(logHealthDownKey, BattleLogType.Debuff, GetBattleEntity(applier), GetBattleEntity(target), amount);
		}
	}

	public void LogCounterUp(Entity applier, Entity target, int amount)
	{
		if (!applier || applier == target)
		{
			Log(logCounterUpSelfKey, BattleLogType.Debuff, GetBattleEntity(target), amount);
		}
		else
		{
			Log(logCounterUpKey, BattleLogType.Debuff, GetBattleEntity(applier), GetBattleEntity(target), amount);
		}
	}

	public void LogCounterDown(Entity applier, Entity target, int amount)
	{
		if (!applier || applier == target)
		{
			Log(logCounterDownSelfKey, BattleLogType.Buff, GetBattleEntity(target), amount);
		}
		else
		{
			Log(logCounterDownKey, BattleLogType.Buff, GetBattleEntity(applier), GetBattleEntity(target), amount);
		}
	}

	public void Log(UnityEngine.Localization.LocalizedString textKey, BattleLogType type, params object[] args)
	{
		BattleLog battleLog = default(BattleLog);
		battleLog.textKey = textKey;
		battleLog.type = type;
		battleLog.args = args;
		BattleLog item = battleLog;
		list.Add(item);
	}

	public static BattleEntity GetBattleEntity(Entity entity)
	{
		BattleEntity battleEntity = default(BattleEntity);
		battleEntity.cardType = entity.data.cardType.name;
		battleEntity.friendly = entity.owner.team == References.Player.team;
		battleEntity.forceTitle = entity.data.forceTitle;
		BattleEntity result = battleEntity;
		if (result.forceTitle.IsNullOrWhitespace())
		{
			if (entity.data.titleKey.IsEmpty)
			{
				result.forceTitle = entity.data.titleFallback;
			}
			else
			{
				result.titleKey = entity.data.titleKey;
			}
		}

		return result;
	}
}
