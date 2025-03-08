#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

[CreateAssetMenu(menuName = "Campaign/Final Boss Generation Settings", fileName = "FinalBossGenerationSettings")]
public class FinalBossGenerationSettings : ScriptableObject
{
	[Serializable]
	public struct ReplaceCard
	{
		public CardData card;

		public CardData[] options;
	}

	[Header("Replace cards...")]
	[SerializeField]
	public ReplaceCard[] replaceCards;

	[Header("Things to ignore...")]
	[SerializeField]
	public CardUpgradeData[] ignoreUpgrades;

	[SerializeField]
	public TraitData[] ignoreTraits;

	[Header("Effects to change...")]
	[SerializeField]
	public FinalBossEffectSwapper[] effectSwappers;

	[Header("Scripts to run on cards...")]
	[SerializeField]
	public CardScript[] runOnAll;

	[SerializeField]
	public FinalBossCardModifier[] cardModifiers;

	[SerializeField]
	public CardScript[] leaderScripts;

	[Header("New enemies to add")]
	[SerializeField]
	public FinalBossEnemyGenerator[] enemyOptions;

	public void ReplaceCards(IList<CardData> cards)
	{
		ReplaceCard[] array = replaceCards;
		for (int i = 0; i < array.Length; i++)
		{
			ReplaceCard replace = array[i];
			CardData[] array2 = cards.Where((CardData a) => a.name == replace.card.name).ToArray();
			foreach (CardData item in array2)
			{
				int index = cards.IndexOf(item);
				CardData cardData = replace.options.RandomItem();
				cards.RemoveAt(index);
				cards.Insert(index, cardData);
				Debug.Log(replace.card.name + " Replaced With " + cardData.name);
			}
		}
	}

	public void Process(CardData leader, IList<CardData> cards)
	{
		RemoveInjuries(cards);
		RemoveUpgrades(cards);
		RemoveTraits(cards);
		ProcessEffects(cards);
		RunScripts(leader, cards);
	}

	public IEnumerable<CardData> GenerateBonusEnemies(int count, IEnumerable<CardData> basedOnDeck, CardData[] defaultEnemies)
	{
		WeightedRandomPool<CardData> weightedRandomPool = new WeightedRandomPool<CardData>();
		foreach (CardData card in basedOnDeck)
		{
			FinalBossEnemyGenerator finalBossEnemyGenerator = enemyOptions.FirstOrDefault((FinalBossEnemyGenerator a) => a.fromCards.Any((CardData b) => b.name == card.name));
			if ((bool)finalBossEnemyGenerator)
			{
				weightedRandomPool.Add(finalBossEnemyGenerator.enemy);
				Debug.Log($"{finalBossEnemyGenerator.enemy} added to weighted pool");
			}
		}

		List<CardData> list = new List<CardData>();
		int num = Mathf.Min(count, weightedRandomPool.Count);
		for (int i = 0; i < num; i++)
		{
			CardData item = weightedRandomPool.Pull().Clone();
			list.Add(item);
		}

		if (list.Count < count)
		{
			foreach (CardData item2 in defaultEnemies.InRandomOrder())
			{
				list.Add(item2.Clone());
				if (list.Count >= count)
				{
					break;
				}
			}
		}

		return list;
	}

	public static void RemoveInjuries(IEnumerable<CardData> cards)
	{
		foreach (CardData card in cards)
		{
			if (card.injuries != null && card.injuries.Count > 0)
			{
				card.injuries.Clear();
				Debug.Log("Injuries removed from " + card.name);
			}
		}
	}

	public void RemoveUpgrades(IList<CardData> cards)
	{
		CardUpgradeData[] array = ignoreUpgrades;
		foreach (CardUpgradeData cardUpgradeData in array)
		{
			foreach (CardData card in cards)
			{
				if (card.upgrades.Contains(cardUpgradeData))
				{
					cardUpgradeData.UnAssign(card);
					Debug.Log("[" + cardUpgradeData.name + "] Removed from " + card.name);
				}
			}
		}
	}

	public void ProcessEffects(IEnumerable<CardData> cards)
	{
		Dictionary<string, FinalBossEffectSwapper> dictionary = new Dictionary<string, FinalBossEffectSwapper>();
		FinalBossEffectSwapper[] array = effectSwappers;
		foreach (FinalBossEffectSwapper finalBossEffectSwapper in array)
		{
			dictionary[finalBossEffectSwapper.effect.name] = finalBossEffectSwapper;
		}

		foreach (CardData card in cards)
		{
			for (int num = card.startWithEffects.Length - 1; num >= 0; num--)
			{
				CardData.StatusEffectStacks statusEffectStacks = card.startWithEffects[num];
				if ((bool)statusEffectStacks.data && dictionary.TryGetValue(statusEffectStacks.data.name, out var value))
				{
					value.Process(card, statusEffectStacks, num);
				}
			}
		}
	}

	public void RemoveTraits(IList<CardData> cards)
	{
		TraitData[] array = ignoreTraits;
		foreach (TraitData trait in array)
		{
			foreach (CardData card in cards)
			{
				CardData.TraitStacks traitStacks = card.traits.FirstOrDefault((CardData.TraitStacks a) => a.data.name == trait.name);
				if (traitStacks != null)
				{
					card.traits.Remove(traitStacks);
					Debug.Log($"[{trait}] Removed from {card.name}");
				}
			}
		}
	}

	public void RunScripts(CardData leader, IList<CardData> cards)
	{
		CardScript[] array = runOnAll;
		foreach (CardScript cardScript in array)
		{
			foreach (CardData card2 in cards)
			{
				Debug.Log("Running [" + cardScript.name + "] on " + card2.name);
				cardScript.Run(card2);
			}
		}

		foreach (CardData card in cards)
		{
			FinalBossCardModifier finalBossCardModifier = cardModifiers.FirstOrDefault((FinalBossCardModifier a) => a.card.name == card.name);
			if ((bool)finalBossCardModifier)
			{
				finalBossCardModifier.Run(card);
			}
		}

		if ((bool)leader)
		{
			array = leaderScripts;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Run(leader);
			}
		}
	}
}
