#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

public abstract class StatusEffectData : DataFile
{
	public delegate IEnumerator EffectEventHandler();

	public delegate IEnumerator EffectStackEventHandler(int stacks);

	public delegate IEnumerator EffectCharacterEventHandler(Character character);

	public delegate IEnumerator EffectEntityEventHandler(Entity entity);

	public delegate IEnumerator EffectEntityDeathEventHandler(Entity entity, DeathType deathType);

	public delegate IEnumerator EffectCardPlayEventHandler(Entity entity, Entity[] targets);

	public delegate IEnumerator EffectTriggerEventHandler(Trigger trigger);

	public delegate IEnumerator EffectCardMoveEventHandler(Entity entity, CardContainer[] fromContainers);

	public delegate IEnumerator EffectHitEventHandler(Hit hit);

	public delegate IEnumerator EffectApplyEventHandler(StatusEffectApply apply);

	public delegate IEnumerator EffectActionPerformedHandler(PlayAction action);

	public static ulong idCurrent;

	public ulong id;

	public StatusEffectData original;

	public bool isClone;

	public bool isStatus;

	public bool isReaction;

	public bool isKeyword;

	public string type;

	public string keyword;

	public string iconGroupName;

	public bool visible;

	public bool stackable = true;

	public bool offensive;

	public bool makesOffensive;

	public bool doesDamage;

	public bool canBeBoosted;

	[InfoBox("Description that will be added to the card", EInfoBoxType.Normal)]
	public UnityEngine.Localization.LocalizedString textKey;

	public string textInsert;

	public int textOrder;

	[TextArea]
	[SerializeField]
	public string desc;

	[SerializeField]
	public string descColorHex;

	[SerializeField]
	public int descOrder;

	public KeywordData[] hiddenKeywords;

	public string applyFormat;

	public UnityEngine.Localization.LocalizedString applyFormatKey;

	[InfoBox("\"Reaction\" Effects should be affected by snow", EInfoBoxType.Normal)]
	[SerializeField]
	public bool affectedBySnow;

	[InfoBox("Higher priority effects will run FIRST", EInfoBoxType.Normal)]
	public int eventPriority;

	public bool removeOnDiscard;

	public bool preventDeath;

	[Header("Constraints that must be met for this to be applied")]
	public TargetConstraint[] targetConstraints;

	public bool removing;

	public int temporary;

	[HideInInspector]
	public Entity applier;

	[HideInInspector]
	public Character applierOwner;

	[HideInInspector]
	public Entity target;

	[ReadOnly]
	public int count;

	public bool HasDesc
	{
		get
		{
			if (!Instant)
			{
				return !textKey.IsEmpty;
			}

			return false;
		}
	}

	public bool HasDescOrIsKeyword
	{
		get
		{
			if (!isKeyword)
			{
				return HasDesc;
			}

			return true;
		}
	}

	public virtual bool Instant => false;

	public virtual bool CanStackActions => true;

	public virtual bool HasBeginRoutine => this.OnBegin != null;

	public virtual bool HasEndRoutine => this.OnEnd != null;

	public virtual bool HasEnableRoutine => this.OnEnable != null;

	public virtual bool HasDisableRoutine => this.OnDisable != null;

	public virtual bool HasStackRoutine => this.OnStack != null;

	public virtual bool HasTurnStartRoutine
	{
		get
		{
			if (this.OnTurnStart != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasTurnRoutine
	{
		get
		{
			if (this.OnTurn != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasTurnEndRoutine
	{
		get
		{
			if (this.OnTurnEnd != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasPreAttackRoutine
	{
		get
		{
			if (this.PreAttack != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasHitRoutine
	{
		get
		{
			if (this.OnHit != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasPostHitRoutine
	{
		get
		{
			if (this.PostHit != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasPostAttackRoutine
	{
		get
		{
			if (this.PostAttack != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasApplyStatusRoutine
	{
		get
		{
			if (this.OnApplyStatus != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasPostApplyStatusRoutine
	{
		get
		{
			if (this.PostApplyStatus != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasEntityDestroyedRoutine
	{
		get
		{
			if (this.OnEntityDestroyed != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasPreTriggerRoutine
	{
		get
		{
			if (this.PreTrigger != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasPreCardPlayedRoutine
	{
		get
		{
			if (this.PreCardPlayed != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasCardMoveRoutine
	{
		get
		{
			if (this.OnCardMove != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasCardPlayedRoutine
	{
		get
		{
			if (this.OnCardPlayed != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasEffectBonusChangedRoutine
	{
		get
		{
			if (this.OnEffectBonusChanged != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasActionPerformedRoutine
	{
		get
		{
			if (this.OnActionPerformed != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public virtual bool HasBuildRoutine
	{
		get
		{
			if (this.OnBuild != null)
			{
				return HasActiveTarget;
			}

			return false;
		}
	}

	public bool HasActiveTarget
	{
		get
		{
			if (target != null)
			{
				return target.enabled;
			}

			return false;
		}
	}

	public event EffectEventHandler OnBegin;

	public event EffectEventHandler OnEnd;

	public event EffectEntityEventHandler OnEnable;

	public event EffectEntityEventHandler OnDisable;

	public event EffectStackEventHandler OnStack;

	public event EffectEntityEventHandler OnTurnStart;

	public event EffectEntityEventHandler OnTurn;

	public event EffectEntityEventHandler OnTurnEnd;

	public event EffectHitEventHandler PreAttack;

	public event EffectHitEventHandler OnHit;

	public event EffectHitEventHandler PostHit;

	public event EffectHitEventHandler PostAttack;

	public event EffectApplyEventHandler OnApplyStatus;

	public event EffectApplyEventHandler PostApplyStatus;

	public event EffectEntityDeathEventHandler OnEntityDestroyed;

	public event EffectEntityEventHandler OnCardMove;

	public event EffectTriggerEventHandler PreTrigger;

	public event EffectCardPlayEventHandler PreCardPlayed;

	public event EffectCardPlayEventHandler OnCardPlayed;

	public event EffectEventHandler OnEffectBonusChanged;

	public event EffectActionPerformedHandler OnActionPerformed;

	public event EffectEntityEventHandler OnBuild;

	public StatusEffectData Instantiate()
	{
		StatusEffectData statusEffectData = GetOriginal();
		StatusEffectData statusEffectData2 = Object.Instantiate(statusEffectData);
		statusEffectData2.id = idCurrent++;
		statusEffectData2.name = base.name;
		statusEffectData2.original = statusEffectData;
		statusEffectData2.isClone = true;
		return statusEffectData2;
	}

	public StatusEffectData GetOriginal()
	{
		if (!isClone)
		{
			return this;
		}

		return original;
	}

	public string GetDesc(int amount, bool silenced = false)
	{
		string text = Text.GetEffectText(textKey, textInsert, amount, silenced);
		if (!descColorHex.IsNullOrWhitespace())
		{
			text = "<color=#" + descColorHex + ">" + text + "</color>";
		}

		return text;
	}

	public string GetPlainDesc()
	{
		return desc;
	}

	public string GetApplyFormat()
	{
		UnityEngine.Localization.LocalizedString localizedString = applyFormatKey;
		if (localizedString == null || localizedString.IsEmpty)
		{
			return null;
		}

		return Text.HandleBracketTags(applyFormatKey.GetLocalizedString());
	}

	public virtual object GetMidBattleData()
	{
		return null;
	}

	public virtual void RestoreMidBattleData(object data)
	{
	}

	public bool CanPlayOn(Entity target)
	{
		TargetConstraint[] array = targetConstraints;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Check(target))
			{
				return false;
			}
		}

		return true;
	}

	public virtual bool CanTrigger()
	{
		if (target.enabled && !target.silenced)
		{
			if (affectedBySnow)
			{
				if (!target.IsSnowed)
				{
					return !target.paused;
				}

				return false;
			}

			return true;
		}

		return false;
	}

	public bool IsNegativeStatusEffect()
	{
		if (offensive && visible)
		{
			return isStatus;
		}

		return false;
	}

	public virtual bool RunBeginEvent()
	{
		return true;
	}

	public virtual bool RunEndEvent()
	{
		return true;
	}

	public virtual bool RunEnableEvent(Entity entity)
	{
		return true;
	}

	public virtual bool RunDisableEvent(Entity entity)
	{
		return true;
	}

	public virtual bool RunStackEvent(int stacks)
	{
		return true;
	}

	public virtual bool RunTurnStartEvent(Entity entity)
	{
		return true;
	}

	public virtual bool RunTurnEvent(Entity entity)
	{
		return true;
	}

	public virtual bool RunTurnEndEvent(Entity entity)
	{
		return true;
	}

	public virtual bool RunPreAttackEvent(Hit hit)
	{
		return true;
	}

	public virtual bool RunHitEvent(Hit hit)
	{
		return true;
	}

	public virtual bool RunPostHitEvent(Hit hit)
	{
		return true;
	}

	public virtual bool RunPostAttackEvent(Hit hit)
	{
		return true;
	}

	public virtual bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		return true;
	}

	public virtual bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		return true;
	}

	public virtual bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		return true;
	}

	public virtual bool RunCardMoveEvent(Entity entity)
	{
		return true;
	}

	public virtual bool RunPreTriggerEvent(Trigger trigger)
	{
		return true;
	}

	public virtual bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		return true;
	}

	public virtual bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		return true;
	}

	public virtual bool RunEffectBonusChangedEvent()
	{
		return true;
	}

	public virtual bool RunActionPerformedEvent(PlayAction action)
	{
		return true;
	}

	public virtual bool RunBuildEvent(Entity entity)
	{
		return true;
	}

	public virtual IEnumerator BeginRoutine()
	{
		return this.OnBegin();
	}

	public virtual IEnumerator EndRoutine()
	{
		return this.OnEnd();
	}

	public virtual IEnumerator EnableRoutine(Entity entity)
	{
		return this.OnEnable(entity);
	}

	public virtual IEnumerator DisableRoutine(Entity entity)
	{
		return this.OnDisable(entity);
	}

	public virtual IEnumerator StackRoutine(int stacks)
	{
		return this.OnStack(stacks);
	}

	public virtual IEnumerator TurnStartRoutine(Entity entity)
	{
		return this.OnTurnStart(entity);
	}

	public virtual IEnumerator TurnRoutine(Entity entity)
	{
		return this.OnTurn(entity);
	}

	public virtual IEnumerator TurnEndRoutine(Entity entity)
	{
		return this.OnTurnEnd(entity);
	}

	public virtual IEnumerator PreAttackRoutine(Hit hit)
	{
		return this.PreAttack(hit);
	}

	public virtual IEnumerator HitRoutine(Hit hit)
	{
		return this.OnHit(hit);
	}

	public virtual IEnumerator PostHitRoutine(Hit hit)
	{
		return this.PostHit(hit);
	}

	public virtual IEnumerator PostAttackRoutine(Hit hit)
	{
		return this.PostAttack(hit);
	}

	public virtual IEnumerator ApplyStatusRoutine(StatusEffectApply apply)
	{
		return this.OnApplyStatus(apply);
	}

	public virtual IEnumerator PostApplyStatusRoutine(StatusEffectApply apply)
	{
		return this.PostApplyStatus(apply);
	}

	public virtual IEnumerator EntityDestroyedRoutine(Entity entity, DeathType deathType)
	{
		return this.OnEntityDestroyed(entity, deathType);
	}

	public virtual IEnumerator CardMoveRoutine(Entity entity)
	{
		return this.OnCardMove(entity);
	}

	public virtual IEnumerator PreTriggerRoutine(Trigger trigger)
	{
		return this.PreTrigger(trigger);
	}

	public virtual IEnumerator PreCardPlayedRoutine(Entity entity, Entity[] targets)
	{
		return this.PreCardPlayed(entity, targets);
	}

	public virtual IEnumerator CardPlayedRoutine(Entity entity, Entity[] targets)
	{
		return this.OnCardPlayed(entity, targets);
	}

	public virtual IEnumerator EffectBonusChangedRoutine()
	{
		return this.OnEffectBonusChanged();
	}

	public virtual IEnumerator ActionPerformedRoutine(PlayAction action)
	{
		return this.OnActionPerformed(action);
	}

	public virtual IEnumerator BuildRoutine(Entity entity)
	{
		return this.OnBuild(entity);
	}

	public void Apply(int count, Entity target, Entity applier)
	{
		this.count = count;
		this.target = target;
		this.applier = applier;
		target.statusEffects.Add(this);
		Init();
	}

	public virtual void Init()
	{
	}

	public IEnumerator CountDown(Entity entity, int amount)
	{
		if ((bool)target && target.enabled && entity == target)
		{
			yield return RemoveStacks(amount, removeTemporary: false);
		}
	}

	public virtual IEnumerator RemoveStacks(int amount, bool removeTemporary)
	{
		count -= amount;
		if (removeTemporary)
		{
			temporary -= amount;
		}

		if (count <= 0)
		{
			yield return Remove();
		}

		target.PromptUpdate();
	}

	public IEnumerator Remove()
	{
		if (!removing)
		{
			removing = true;
			target.statusEffects.Remove(this);
			StatusEffectSystem.activeEffects.Remove(this);
			target.PromptUpdate();
			if (RunEndEvent() && HasEndRoutine)
			{
				yield return EndRoutine();
			}

			Destroy();
		}
	}

	public void Destroy()
	{
		if ((bool)this)
		{
			string arg = (target ? target.name : "null");
			Debug.Log($"[{base.name} {count}] removed from [{arg}]");
			StatusEffectSystem.activeEffects.Remove(this);
			Object.Destroy(this);
		}
	}

	public virtual int GetAmount()
	{
		if (!target || target.silenced)
		{
			return 0;
		}

		if (!canBeBoosted)
		{
			return count;
		}

		return Mathf.Max(0, Mathf.RoundToInt((float)(count + target.effectBonus) * target.effectFactor));
	}

	public Entity GetDamager()
	{
		if (!applier)
		{
			if (!applierOwner || !applierOwner.entity)
			{
				return null;
			}

			return applierOwner.entity;
		}

		return applier;
	}

	public CardContainer[] GetTargetContainers()
	{
		CardContainer[] containers = target.containers;
		if (containers != null && containers.Length != 0)
		{
			return containers;
		}

		return target.preContainers;
	}

	public CardContainer[] GetTargetActualContainers()
	{
		List<CardContainer> actualContainers = target.actualContainers;
		if (actualContainers != null && actualContainers.Count != 0)
		{
			return actualContainers.ToArray();
		}

		return target.preActualContainers;
	}

	public StatusEffectData()
	{
	}
}
