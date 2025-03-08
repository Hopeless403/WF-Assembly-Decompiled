#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class JournalCardManagerPopulator : MonoBehaviour
{
	[Serializable]
	public class Category
	{
		public string name;

		public bool enemy;

		public string[] cardTypes;

		public List<string> cardNames;

		public bool CheckAdd(CardData cardData, bool enemy)
		{
			if (this.enemy == enemy && cardTypes.Contains(cardData.cardType.name))
			{
				cardNames.Add(cardData.name);
				return true;
			}

			return false;
		}
	}

	[SerializeField]
	public Category[] categories;

	public bool subbed;

	public bool populated { get; set; }

	public void OnEnable()
	{
		if (!subbed)
		{
			Events.OnModLoaded += ModToggled;
			Events.OnModUnloaded += ModToggled;
			subbed = true;
		}

		if (!populated)
		{
			Populate();
		}
	}

	public void ModToggled(WildfrostMod mod)
	{
		if (populated)
		{
			Clear();
		}
	}

	public void Clear()
	{
		Category[] array = categories;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].cardNames.Clear();
		}

		populated = false;
	}

	public void Populate()
	{
		populated = true;
		StopWatch.Start();
		List<string> usedCards = new List<string>();
		List<string> playerCards = new List<string>();
		List<string> list = new List<string>();
		ClassData[] classes = References.Classes;
		foreach (ClassData classData in classes)
		{
			foreach (CardData item in classData.startingInventory.deck)
			{
				StoreAsPlayerCard(item);
			}

			foreach (CardData item2 in classData.startingInventory.reserve)
			{
				StoreAsPlayerCard(item2);
			}

			RewardPool[] rewardPools = classData.rewardPools;
			foreach (RewardPool rewardPool in rewardPools)
			{
				if (list.Contains(rewardPool.name))
				{
					continue;
				}

				list.Add(rewardPool.name);
				foreach (DataFile item3 in rewardPool.list)
				{
					if (item3 is CardData cardData2)
					{
						StoreAsPlayerCard(cardData2);
					}
				}
			}
		}

		foreach (CardData item4 in AddressableLoader.GetGroup<CardData>("CardData"))
		{
			if (!(item4.mainSprite == null) && !(item4.mainSprite.name == "Nothing"))
			{
				string text = item4.cardType.name;
				if ((!(text == "Boss") && !(text == "BossSmall")) || !(item4.name != "FinalBoss2"))
				{
					text = item4.cardType.name;
					bool flag = text == "Friendly" || text == "Item" || playerCards.Contains(item4.name);
					ProcessCard(item4, !flag);
				}
			}
		}

		foreach (BattleData item5 in AddressableLoader.GetGroup<BattleData>("BattleData"))
		{
			BattleWavePoolData[] pools = item5.pools;
			for (int i = 0; i < pools.Length; i++)
			{
				BattleWavePoolData.Wave[] waves = pools[i].waves;
				for (int j = 0; j < waves.Length; j++)
				{
					foreach (CardData unit in waves[j].units)
					{
						if ((bool)unit && unit.cardType.miniboss)
						{
							string text = unit.cardType.name;
							if (text == "Boss" || text == "BossSmall")
							{
								ProcessCard(unit, enemy: true);
							}
						}
					}
				}
			}
		}

		Debug.Log($"Journal Card Manager Population Done! ({StopWatch.Stop()}ms)");
		void ProcessCard(CardData cardData, bool enemy)
		{
			if (!usedCards.Contains(cardData.name))
			{
				usedCards.Add(cardData.name);
				Category[] array = categories;
				for (int k = 0; k < array.Length && !array[k].CheckAdd(cardData, enemy); k++)
				{
				}
			}
		}
		void StoreAsPlayerCard(CardData cardData)
		{
			if (!cardData || playerCards.Contains(cardData.name))
			{
				return;
			}

			playerCards.Add(cardData.name);
			foreach (string createdByThi in CreatedByLookup.GetCreatedByThis(cardData.name))
			{
				playerCards.Add(createdByThi);
			}
		}
	}

	public Category GetCategory(string name)
	{
		Category[] array = categories;
		foreach (Category category in array)
		{
			if (category.name == name)
			{
				return category;
			}
		}

		return null;
	}
}
