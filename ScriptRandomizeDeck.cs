#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Randomize Deck", menuName = "Scripts/Randomize Deck")]
public class ScriptRandomizeDeck : Script
{
	[SerializeField]
	public Vector2Int cardsToRemove = new Vector2Int(0, 2);

	[SerializeField]
	public Vector2Int cardsToAdd = new Vector2Int(1, 4);

	[SerializeField]
	public Vector2Int charmsToAdd = new Vector2Int(1, 3);

	[SerializeField]
	[Range(0f, 1f)]
	public float addCharmChance = 0.25f;

	[SerializeField]
	[Range(0f, 1f)]
	public float replaceChance = 0.5f;

	public override IEnumerator Run()
	{
		GetPools(out var cards, out var charms);
		RemoveCards();
		AddCards(cards);
		if (Random.Range(0f, 1f) < addCharmChance)
		{
			AddCharms(charms);
		}

		References.PlayerData.inventory.deck.Sort((CardData a, CardData b) => a.cardType.sortPriority.CompareTo(b.cardType.sortPriority));
		yield break;
	}

	public static void GetPools(out List<CardData> cards, out List<CardUpgradeData> charms)
	{
		cards = new List<CardData>();
		charms = new List<CardUpgradeData>();
		RewardPool[] rewardPools = References.PlayerData.classData.rewardPools;
		for (int i = 0; i < rewardPools.Length; i++)
		{
			foreach (DataFile item3 in rewardPools[i].list)
			{
				if (!(item3 is CardData item))
				{
					if (item3 is CardUpgradeData item2)
					{
						charms.Add(item2);
					}
				}
				else
				{
					cards.Add(item);
				}
			}
		}
	}

	public void RemoveCards()
	{
		int num = cardsToRemove.Random();
		for (int i = 0; i < num; i++)
		{
			int index = Random.Range(1, References.PlayerData.inventory.deck.Count);
			References.PlayerData.inventory.deck.RemoveAt(index);
		}
	}

	public void AddCards(List<CardData> cards)
	{
		int num = cardsToAdd.Random();
		List<CardData> added = new List<CardData>();
		for (int i = 0; i < num; i++)
		{
			AddCard(References.PlayerData.inventory.deck, cards.TakeRandom().Clone(), added);
		}
	}

	public void AddCard(CardDataList deck, CardData cardDataClone, List<CardData> added)
	{
		bool num = Random.Range(0f, 1f) < replaceChance;
		int index = Random.Range(1, deck.Count);
		if (num && !added.Contains(deck[index]))
		{
			deck.RemoveAt(index);
		}

		deck.Insert(index, cardDataClone);
	}

	public void AddCharms(List<CardUpgradeData> charms)
	{
		int num = charmsToAdd.Random();
		for (int i = 0; i < num; i++)
		{
			bool flag = false;
			while (!flag && charms.Count > 0)
			{
				flag = TryAddCharm(References.PlayerData.inventory.deck, charms.TakeRandom());
			}

			if (charms.Count <= 0)
			{
				break;
			}
		}
	}

	public static bool TryAddCharm(CardDataList deck, CardUpgradeData upgradeData)
	{
		bool result = false;
		foreach (CardData item in deck.InRandomOrder())
		{
			if (upgradeData.CanAssign(item))
			{
				upgradeData.Clone().Assign(item);
				result = true;
				break;
			}
		}

		return result;
	}
}
