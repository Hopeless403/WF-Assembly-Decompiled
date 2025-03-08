#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BlockForMinibossesModifierSystem : GameSystem
{
	public const float healthFactor = 0.75f;

	public const float blockPerHealthLost = 0.25f;

	public static StatusEffectData _effect;

	public static StatusEffectData effect => _effect ?? (_effect = AddressableLoader.Get<StatusEffectData>("StatusEffectData", "Block"));

	public void OnEnable()
	{
		Events.OnCardDataCreated += CardDataCreated;
	}

	public void OnDisable()
	{
		Events.OnCardDataCreated -= CardDataCreated;
	}

	public static void CardDataCreated(CardData cardData)
	{
		if (cardData.cardType.name == "Miniboss" && cardData.hasHealth)
		{
			int hp = cardData.hp;
			cardData.hp = Mathf.CeilToInt((float)cardData.hp * 0.75f);
			int num = hp - cardData.hp;
			int count = Mathf.Max(1, Mathf.CeilToInt((float)num * 0.25f));
			cardData.startWithEffects = CardData.StatusEffectStacks.Stack(cardData.startWithEffects, new CardData.StatusEffectStacks[1]
			{
				new CardData.StatusEffectStacks(effect, count)
			});
		}
	}
}
