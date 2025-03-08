#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BoostMinibossHealthModifierSystem : GameSystem
{
	public const float healthFactor = 1.1f;

	public void OnEnable()
	{
		Events.OnCardDataCreated += CardDataCreated;
	}

	public void OnDisable()
	{
		Events.OnCardDataCreated -= CardDataCreated;
	}

	public void CardDataCreated(CardData cardData)
	{
		string text = cardData.cardType.name;
		if (text == "Miniboss" || text == "Boss")
		{
			if (cardData.hasHealth)
			{
				BoostHealth(cardData, 1.1f);
			}
			else
			{
				BoostStatusEffect(cardData, "scrap", 1.1f);
			}
		}
	}

	public void BoostHealth(CardData cardData, float factor)
	{
		float f = (float)cardData.hp * factor;
		cardData.hp = Mathf.RoundToInt(f);
	}

	public void BoostStatusEffect(CardData cardData, string statusType, float factor)
	{
		CardData.StatusEffectStacks[] startWithEffects = cardData.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in startWithEffects)
		{
			if (!(statusEffectStacks.data.type != statusType))
			{
				float f = (float)statusEffectStacks.count * factor;
				statusEffectStacks.count = Mathf.RoundToInt(f);
			}
		}
	}
}
