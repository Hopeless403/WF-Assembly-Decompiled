#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleWaveManager : MonoBehaviour
{
	[Serializable]
	public class Wave
	{
		public int counter;

		public List<CardData> units;

		public bool isBossWave;

		public bool spawned;

		public Wave()
		{
		}

		public Wave(WaveData data)
		{
			counter = data.counter;
			units = new List<CardData>();
			int count = data.Count;
			for (int i = 0; i < count; i++)
			{
				units.Add(data.GetCardData(i));
			}

			isBossWave = data.isBossWave;
		}

		public override string ToString()
		{
			return string.Format("Wave ({0}) counter: {1}", string.Join(", ", units), counter);
		}
	}

	[Serializable]
	public abstract class WaveData
	{
		public int counter;

		public bool isBossWave;

		public virtual int Count => 0;

		public abstract void AddCard(CardData card);

		public abstract void InsertCard(int index, CardData card);

		public abstract CardData GetCardData(int index);

		public abstract string GetCardName(int index);

		public abstract CardData PeekCardData(int index);

		public abstract bool AddUpgradeToCard(int index, CardUpgradeData upgradeData);

		public WaveData()
		{
		}
	}

	[Serializable]
	public class WaveDataBasic : WaveData
	{
		public Card[] cards;

		public override int Count => cards.Length;

		public override void AddCard(CardData card)
		{
			List<Card> list = cards.ToList();
			list.Add(new Card(card.name));
			cards = list.ToArray();
		}

		public override void InsertCard(int index, CardData card)
		{
			List<Card> list = cards.ToList();
			list.Insert(index, new Card(card.name));
			cards = list.ToArray();
		}

		public override CardData GetCardData(int index)
		{
			Card card = cards[index];
			CardData cardDataClone = AddressableLoader.GetCardDataClone(card.cardName);
			if (card.upgradeNames != null)
			{
				foreach (string upgradeName in card.upgradeNames)
				{
					CardUpgradeData cardUpgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", upgradeName);
					if ((bool)cardUpgradeData)
					{
						cardUpgradeData.Clone().Assign(cardDataClone);
					}
				}
			}

			return cardDataClone;
		}

		public override CardData PeekCardData(int index)
		{
			return AddressableLoader.Get<CardData>("CardData", cards[index].cardName);
		}

		public override string GetCardName(int index)
		{
			return cards[index].cardName;
		}

		public override bool AddUpgradeToCard(int index, CardUpgradeData upgradeData)
		{
			Card card = cards[index];
			CardData cardData = AddressableLoader.Get<CardData>("CardData", card.cardName);
			if ((bool)cardData && upgradeData.CanAssign(cardData))
			{
				card.AddUpgrade(upgradeData.name);
				return true;
			}

			return false;
		}

		public Card Get(int index)
		{
			return cards[index];
		}

		public WaveDataFull ConvertToFull()
		{
			WaveDataFull waveDataFull = new WaveDataFull();
			List<CardSaveData> list = new List<CardSaveData>();
			for (int i = 0; i < cards.Length; i++)
			{
				list.AddIfNotNull(new CardSaveData(PeekCardData(i)));
			}

			waveDataFull.cardDatas = list.ToArray();
			for (int j = 0; j < cards.Length; j++)
			{
				Card obj = cards[j];
				CardSaveData cardSaveData = waveDataFull.cardDatas[j];
				List<CardUpgradeSaveData> list2 = new List<CardUpgradeSaveData>();
				foreach (string upgradeName in obj.upgradeNames)
				{
					list2.Add(new CardUpgradeSaveData(upgradeName));
				}

				cardSaveData.upgrades = list2.ToArray();
			}

			return waveDataFull;
		}
	}

	[Serializable]
	public class WaveDataFull : WaveData
	{
		public CardSaveData[] cardDatas;

		public override int Count => cardDatas.Length;

		public override void AddCard(CardData card)
		{
			List<CardSaveData> list = cardDatas.ToList();
			list.Add(new CardSaveData(card));
			cardDatas = list.ToArray();
		}

		public override void InsertCard(int index, CardData card)
		{
			List<CardSaveData> list = cardDatas.ToList();
			list.Insert(index, new CardSaveData(card));
			cardDatas = list.ToArray();
		}

		public override CardData GetCardData(int index)
		{
			return cardDatas[index].Load(keepId: false);
		}

		public override CardData PeekCardData(int index)
		{
			return AddressableLoader.Get<CardData>("CardData", cardDatas[index].name);
		}

		public override string GetCardName(int index)
		{
			return cardDatas[index].name;
		}

		public override bool AddUpgradeToCard(int index, CardUpgradeData upgradeData)
		{
			bool result = false;
			CardData cardData = cardDatas[index].Load(keepId: false);
			if (upgradeData.CanAssign(cardData))
			{
				upgradeData.Assign(cardData);
				cardDatas[index] = new CardSaveData(cardData);
				result = true;
			}

			return result;
		}
	}

	[Serializable]
	public class Card
	{
		public string cardName;

		public List<string> upgradeNames;

		public Card()
		{
		}

		public Card(string cardName)
		{
			this.cardName = cardName;
		}

		public void AddUpgrade(string upgradeName)
		{
			if (upgradeNames == null)
			{
				upgradeNames = new List<string>();
			}

			upgradeNames.Add(upgradeName);
		}
	}

	public List<Wave> list;

	public Queue<Entity[]> remainingWaves;

	public void AddWave(Wave wave)
	{
		if (list == null)
		{
			list = new List<Wave>();
		}

		list.Add(wave);
		Debug.Log($"\"{wave}\" Added");
	}

	public void AddEntities(Entity[] entities)
	{
		if (remainingWaves == null)
		{
			remainingWaves = new Queue<Entity[]>();
		}

		remainingWaves.Enqueue(entities);
	}

	public Entity[] Pull()
	{
		return remainingWaves.Dequeue();
	}

	public Entity[] Peek()
	{
		return remainingWaves.Peek();
	}
}
