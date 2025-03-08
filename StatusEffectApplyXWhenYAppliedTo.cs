#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Y Applied To", fileName = "Apply X When Y Applied To")]
public class StatusEffectApplyXWhenYAppliedTo : StatusEffectApplyX
{
	[SerializeField]
	public bool instead;

	public bool whenAnyApplied;

	[HideIf("whenAnyApplied")]
	public string[] whenAppliedTypes = new string[1] { "snow" };

	[SerializeField]
	public ApplyToFlags whenAppliedToFlags;

	[SerializeField]
	public bool mustReachAmount;

	[Header("Adjust Amount Applied")]
	[SerializeField]
	public bool adjustAmount;

	[SerializeField]
	[ShowIf("adjustAmount")]
	public int addAmount;

	[SerializeField]
	[ShowIf("adjustAmount")]
	public float multiplyAmount = 1f;

	public override void Init()
	{
		base.PostApplyStatus += Run;
	}

	public bool CheckType(StatusEffectData effectData)
	{
		if (effectData.isStatus)
		{
			if (!whenAnyApplied)
			{
				return whenAppliedTypes.Contains(effectData.type);
			}

			return true;
		}

		return false;
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if ((adjustAmount || instead) && target.enabled && !TargetSilenced() && (target.alive || !targetMustBeAlive) && (bool)apply.effectData && apply.count > 0 && CheckType(apply.effectData) && CheckTarget(apply.target))
		{
			if (instead)
			{
				apply.effectData = effectToApply;
			}

			if (adjustAmount)
			{
				apply.count += addAmount;
				apply.count = Mathf.RoundToInt((float)apply.count * multiplyAmount);
			}
		}

		return false;
	}

	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if (target.enabled && !TargetSilenced() && (bool)apply.effectData && apply.count > 0 && CheckType(apply.effectData) && CheckTarget(apply.target))
		{
			return CheckAmount(apply);
		}

		return false;
	}

	public IEnumerator Run(StatusEffectApply apply)
	{
		return Run(GetTargets(), apply.count);
	}

	public bool CheckFlag(ApplyToFlags whenAppliedTo)
	{
		return (whenAppliedToFlags & whenAppliedTo) != 0;
	}

	public bool CheckTarget(Entity entity)
	{
		if (!Battle.IsOnBoard(target))
		{
			return false;
		}

		if (entity == target)
		{
			return CheckFlag(ApplyToFlags.Self);
		}

		if (entity.owner == target.owner)
		{
			return CheckFlag(ApplyToFlags.Allies);
		}

		if (entity.owner != target.owner)
		{
			return CheckFlag(ApplyToFlags.Enemies);
		}

		return false;
	}

	public bool CheckAmount(StatusEffectApply apply)
	{
		if (!mustReachAmount)
		{
			return true;
		}

		StatusEffectData statusEffectData = apply.target.FindStatus(apply.effectData.type);
		if ((bool)statusEffectData)
		{
			return statusEffectData.count >= GetAmount();
		}

		return false;
	}
}
