#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Increase Effects", fileName = "Increase Effects")]
public class StatusEffectInstantIncreaseEffects : StatusEffectInstant
{
	[SerializeField]
	public bool allEffects = true;

	[SerializeField]
	public bool allAttackEffects;

	[SerializeField]
	public bool allPassiveEffects;

	[SerializeField]
	[HideIf("allEffects")]
	public int attackEffectIndex = -1;

	[SerializeField]
	[HideIf("allEffects")]
	public int passiveEffectIndex = -1;

	public override IEnumerator Process()
	{
		int amount = GetAmount();
		if ((bool)target.curveAnimator)
		{
			target.curveAnimator.Ping();
		}

		if (allEffects || (allAttackEffects && allPassiveEffects))
		{
			target.effectBonus += amount;
			target.PromptUpdate();
		}
		else if (allAttackEffects)
		{
			foreach (CardData.StatusEffectStacks attackEffect in target.attackEffects)
			{
				attackEffect.count += amount;
			}

			if (target.display is Card card)
			{
				card.promptUpdateDescription = true;
			}

			target.PromptUpdate();
		}

		else if (allPassiveEffects)
		{
			foreach (StatusEffectData statusEffect in target.statusEffects)
			{
				statusEffect.count += amount;
			}

			if (target.display is Card card2)
			{
				card2.promptUpdateDescription = true;
			}

			target.PromptUpdate();
		}
		else
		{
			if (attackEffectIndex >= 0)
			{
				target.attackEffects[attackEffectIndex].count += amount;
				if (target.display is Card card3)
				{
					card3.promptUpdateDescription = true;
				}

				target.PromptUpdate();
			}

			if (passiveEffectIndex >= 0)
			{
				target.statusEffects[passiveEffectIndex].count += amount;
				if (target.display is Card card4)
				{
					card4.promptUpdateDescription = true;
				}

				target.PromptUpdate();
			}
		}

		yield return base.Process();
	}
}
