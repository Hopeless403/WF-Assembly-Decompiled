#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Does Summon", menuName = "Target Constraints/Does Summon")]
public class TargetConstraintDoesSummon : TargetConstraint
{
	public override bool Check(Entity target)
	{
		return Check(target.data);
	}

	public override bool Check(CardData targetData)
	{
		bool flag = false;
		CardData.StatusEffectStacks[] startWithEffects = targetData.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in startWithEffects)
		{
			if ((statusEffectStacks.data is StatusEffectSummon statusEffectSummon && statusEffectSummon.summonCard.cardType.unit) || (statusEffectStacks.data is StatusEffectApplyX statusEffectApplyX && statusEffectApplyX.effectToApply is StatusEffectInstantSummon statusEffectInstantSummon && statusEffectInstantSummon.targetSummon.summonCard.cardType.unit))
			{
				flag = true;
				break;
			}
		}

		if (!flag)
		{
			startWithEffects = targetData.attackEffects;
			for (int i = 0; i < startWithEffects.Length; i++)
			{
				if (startWithEffects[i].data is StatusEffectInstantSummon statusEffectInstantSummon2 && statusEffectInstantSummon2.targetSummon.summonCard.cardType.unit)
				{
					flag = true;
					break;
				}
			}
		}

		if (!flag)
		{
			return not;
		}

		return !not;
	}
}
