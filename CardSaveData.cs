#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CardSaveData : ILoadable<CardData>
{
	public ulong id;

	public string name;

	public string title;

	public int hp;

	public int damage;

	public int counter;

	public Vector3 random3;

	public CardUpgradeSaveData[] upgrades;

	public StatusEffectSaveData[] attackEffects;

	public StatusEffectSaveData[] startWithEffects;

	public TraitSaveData[] traits;

	public StatusEffectSaveData[] injuries;

	public Dictionary<string, object> customData;

	public CardSaveData()
	{
	}

	public CardSaveData(CardData cardData)
	{
		id = cardData.id;
		name = cardData.name;
		title = cardData.forceTitle;
		hp = cardData.hp;
		damage = cardData.damage;
		counter = cardData.counter;
		random3 = cardData.random3;
		upgrades = cardData.upgrades.SaveArray<CardUpgradeData, CardUpgradeSaveData>();
		attackEffects = cardData.attackEffects.SaveArray<CardData.StatusEffectStacks, StatusEffectSaveData>();
		startWithEffects = cardData.startWithEffects.SaveArray<CardData.StatusEffectStacks, StatusEffectSaveData>();
		traits = cardData.traits.SaveArray<CardData.TraitStacks, TraitSaveData>();
		injuries = cardData.injuries.SaveArray<CardData.StatusEffectStacks, StatusEffectSaveData>();
		customData = cardData.customData;
		if ((bool)cardData.original && cardData.cardType != cardData.original.cardType)
		{
			if (customData == null)
			{
				customData = new Dictionary<string, object>();
			}

			customData["OverrideCardType"] = cardData.cardType.name;
		}
	}

	public CardData Peek()
	{
		return AddressableLoader.Get<CardData>("CardData", name);
	}

	public CardData Load()
	{
		return Load(keepId: true);
	}

	public CardData Load(bool keepId)
	{
		CardData cardData = AddressableLoader.Get<CardData>("CardData", name);
		bool num = !cardData;
		CardData cardData2 = ((!num) ? (keepId ? cardData.Clone(random3, id, runCreateScripts: false) : cardData.Clone(random3, runCreateScripts: false)) : (keepId ? MissingCardSystem.GetCloneWithId(name, random3, id) : MissingCardSystem.GetClone(name)));
		cardData2.customData = customData;
		cardData2.attackEffects = attackEffects.LoadArray<CardData.StatusEffectStacks, StatusEffectSaveData>();
		cardData2.startWithEffects = startWithEffects.LoadArray<CardData.StatusEffectStacks, StatusEffectSaveData>();
		cardData2.traits = traits.LoadList<CardData.TraitStacks, TraitSaveData>();
		cardData2.injuries = injuries.LoadList<CardData.StatusEffectStacks, StatusEffectSaveData>();
		if (!num)
		{
			cardData2.forceTitle = title;
		}

		cardData2.hp = hp;
		cardData2.damage = damage;
		cardData2.counter = counter;
		if (cardData2.customData != null && cardData2.customData.ContainsKey("OverrideCardType"))
		{
			cardData2.cardType = AddressableLoader.Get<CardType>("CardType", cardData2.customData.Get<string>("OverrideCardType"));
		}

		CardUpgradeSaveData[] array = upgrades;
		if (array != null && array.Length > 0)
		{
			cardData2.upgrades = upgrades.LoadList<CardUpgradeData, CardUpgradeSaveData>();
			if (cardData2.upgrades.Any((CardUpgradeData a) => a.becomesTargetedCard))
			{
				cardData2.hasAttack = true;
				if (cardData2.playType == Card.PlayType.None)
				{
					cardData2.playType = Card.PlayType.Play;
				}

				cardData2.needsTarget = true;
			}
		}

		return cardData2;
	}
}
