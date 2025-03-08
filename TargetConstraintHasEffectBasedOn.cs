#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Has Effect Based On", menuName = "Target Constraints/Has Effect Based On")]
public class TargetConstraintHasEffectBasedOn : TargetConstraint
{
	[SerializeField]
	public string basedOnStatusType;

	public override bool Check(CardData targetData)
	{
		CardData.StatusEffectStacks[] attackEffects = targetData.attackEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in attackEffects)
		{
			if (statusEffectStacks.data is StatusEffectInstantDoubleX statusEffectInstantDoubleX && statusEffectInstantDoubleX.statusToDouble.type == basedOnStatusType)
			{
				return !not;
			}

			if (statusEffectStacks.data.type == basedOnStatusType)
			{
				return !not;
			}
		}

		attackEffects = targetData.startWithEffects;
		for (int i = 0; i < attackEffects.Length; i++)
		{
			StatusEffectData data = attackEffects[i].data;
			if (!(data is StatusEffectApplyXWhenYAppliedTo statusEffectApplyXWhenYAppliedTo))
			{
				if (!(data is StatusEffectApplyXWhenYAppliedToAlly statusEffectApplyXWhenYAppliedToAlly))
				{
					if (!(data is StatusEffectApplyXWhenYAppliedToSelf statusEffectApplyXWhenYAppliedToSelf))
					{
						if (!(data is StatusEffectApplyX statusEffectApplyX))
						{
							if (data is StatusEffectBonusDamageEqualToX statusEffectBonusDamageEqualToX && statusEffectBonusDamageEqualToX.effectType == basedOnStatusType)
							{
								return !not;
							}
						}
						else if ((bool)statusEffectApplyX.effectToApply && statusEffectApplyX.effectToApply.type == basedOnStatusType)
						{
							return !not;
						}
					}
					else if (statusEffectApplyXWhenYAppliedToSelf.whenAppliedType == basedOnStatusType || ((bool)statusEffectApplyXWhenYAppliedToSelf.effectToApply && statusEffectApplyXWhenYAppliedToSelf.effectToApply.type == basedOnStatusType))
					{
						return !not;
					}
				}
				else if (statusEffectApplyXWhenYAppliedToAlly.whenAppliedType == basedOnStatusType || ((bool)statusEffectApplyXWhenYAppliedToAlly.effectToApply && statusEffectApplyXWhenYAppliedToAlly.effectToApply.type == basedOnStatusType))
				{
					return !not;
				}
			}
			else if (statusEffectApplyXWhenYAppliedTo.whenAppliedTypes.Contains(basedOnStatusType) || ((bool)statusEffectApplyXWhenYAppliedTo.effectToApply && statusEffectApplyXWhenYAppliedTo.effectToApply.type == basedOnStatusType))
			{
				return !not;
			}
		}

		return not;
	}

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}
}
