#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Focus", fileName = "Focus")]
public class StatusEffectFocus : StatusEffectData
{
	public override void Init()
	{
		base.PostApplyStatus += ApplyStatus;
	}

	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if ((bool)apply.effectData && apply.count > 0 && apply.effectData.type == type)
		{
			return apply.target != target;
		}

		return false;
	}

	public IEnumerator ApplyStatus(StatusEffectApply apply)
	{
		int amount = count;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(target, amount);
		}
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		if (!trigger.nullified && trigger.countsAsTrigger && trigger.entity.owner.team != target.owner.team && trigger.type == "basic" && Battle.IsOnBoard(trigger.entity))
		{
			trigger.targets = trigger.entity.targetMode.GetTargets(trigger.entity, target, target.containers.RandomItem());
		}

		return false;
	}
}
