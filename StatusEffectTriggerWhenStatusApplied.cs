#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Reactions/Trigger When Status Applied", fileName = "Trigger When Status Applied")]
public class StatusEffectTriggerWhenStatusApplied : StatusEffectReaction
{
	public enum TriggerType
	{
		Normal,
		Target,
		Applier
	}

	[SerializeField]
	public StatusEffectData targetStatus;

	[SerializeField]
	public bool friendlyFire = true;

	[SerializeField]
	public bool selfFire = true;

	[SerializeField]
	public TriggerType triggerType;

	public int busy;

	public int triggerIndex;

	public override void Init()
	{
		base.PostApplyStatus += Run;
	}

	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if (target.enabled && Battle.IsOnBoard(target) && Check(apply))
		{
			return CanTrigger();
		}

		return false;
	}

	public IEnumerator Run(StatusEffectApply apply)
	{
		int i = triggerIndex + busy;
		busy++;
		yield return new WaitUntil(() => triggerIndex == i);
		switch (triggerType)
		{
			case TriggerType.Normal:
				ActionQueue.Stack(new ActionTrigger(target, apply.applier), fixedPosition: true);
				break;
			case TriggerType.Target:
			if ((bool)apply.target && apply.target.alive)
			{
				CardContainer targetRow2 = Trigger.GetTargetRow(target, apply.target);
				ActionQueue.Stack(new ActionTriggerSubsequent(target, apply.applier, apply.target, targetRow2), fixedPosition: true);
				}
	
				break;
			case TriggerType.Applier:
			if ((bool)apply.applier && apply.applier.alive)
			{
				CardContainer targetRow = Trigger.GetTargetRow(target, apply.target);
				ActionQueue.Stack(new ActionTriggerSubsequent(target, apply.applier, apply.applier, targetRow), fixedPosition: true);
				}
	
				break;
		}

		busy--;
		triggerIndex++;
	}

	public bool Check(StatusEffectApply apply)
	{
		if ((!friendlyFire && apply.applier?.owner == target.owner) || (!selfFire && apply.applier == target))
		{
			return false;
		}

		if (apply.effectData?.type == targetStatus?.type && CheckDuplicate(apply.applier))
		{
			return CheckDuplicate(apply.applier.triggeredBy);
		}

		return false;
	}

	public bool CheckDuplicate(Entity entity)
	{
		if (!entity)
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
