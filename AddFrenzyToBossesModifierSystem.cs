#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class AddFrenzyToBossesModifierSystem : GameSystem
{
	public StatusEffectData _effect;

	public StatusEffectData effect => _effect ?? (_effect = AddressableLoader.Get<StatusEffectData>("StatusEffectData", "MultiHit"));

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
		switch (cardData.cardType.name)
		{
			case "Miniboss":
			case "Boss":
			case "BossSmall":
				cardData.startWithEffects = CardData.StatusEffectStacks.Stack(cardData.startWithEffects, new CardData.StatusEffectStacks[1]
				{
					new CardData.StatusEffectStacks(effect, 1)
				});
				break;
		}
	}
}
