#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Copy Effects From Other Card In Deck", menuName = "Card Scripts/Copy Effects From Other Card In Deck")]
public class CardScriptCopyEffectsFromOtherCardInDeck : CardScript
{
	[SerializeField]
	public bool includeReserve = true;

	[SerializeField]
	public string[] eligibleTypes = new string[2] { "Leader", "Friendly" };

	public override void Run(CardData target)
	{
		List<CardData> list = new List<CardData>();
		list.AddRange(References.PlayerData.inventory.deck.Where((CardData a) => eligibleTypes.Contains(a.cardType.tag) && a != target));
		if (includeReserve)
		{
			list.AddRange(References.PlayerData.inventory.reserve.Where((CardData a) => eligibleTypes.Contains(a.cardType.tag) && a != target));
		}

		if (list.Count > 0)
		{
			GainEffects(target, list.TakeRandom());
		}
	}

	public static void GainEffects(CardData target, CardData toCopy)
	{
		target.attackEffects = CardData.StatusEffectStacks.Stack(target.attackEffects, toCopy.attackEffects);
		target.startWithEffects = CardData.StatusEffectStacks.Stack(target.startWithEffects, toCopy.startWithEffects.Where((CardData.StatusEffectStacks a) => !a.data.isStatus && a.data.HasDescOrIsKeyword));
		CardData.TraitStacks.Stack(ref target.traits, toCopy.traits);
	}
}
