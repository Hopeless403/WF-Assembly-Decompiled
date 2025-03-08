#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;

public class BoostAllEffectsModifierSystem : GameSystem
{
	public const int add = 1;

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
		foreach (CardData.StatusEffectStacks item in cardData.attackEffects.Where((CardData.StatusEffectStacks e) => e.data.stackable))
		{
			item.count++;
		}

		foreach (CardData.StatusEffectStacks item2 in cardData.startWithEffects.Where((CardData.StatusEffectStacks e) => !e.data.isStatus && e.data.canBeBoosted))
		{
			item2.count++;
		}

		foreach (CardData.TraitStacks item3 in cardData.traits.Where((CardData.TraitStacks t) => t.data.keyword.canStack))
		{
			item3.count++;
		}
	}
}
