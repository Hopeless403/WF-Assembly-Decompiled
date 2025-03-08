#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterRewards : MonoBehaviour
{
	public class Pool
	{
		public readonly List<DataFile> current = new List<DataFile>();

		public List<DataFile> list { get; set; }

		public void Add(DataFile item)
		{
			if (this.list == null)
			{
				List<DataFile> list2 = (this.list = new List<DataFile>());
			}

			this.list.Add(item);
		}

		public void Add(IEnumerable<DataFile> content)
		{
			if (this.list == null)
			{
				List<DataFile> list2 = (this.list = new List<DataFile>());
			}

			this.list.AddRange(content);
		}

		public void Remove(IEnumerable<string> itemNames)
		{
			foreach (string itemName in itemNames)
			{
				Remove(itemName);
			}
		}

		public void Remove(string itemName)
		{
			int num = list.RemoveAll((DataFile a) => a.name == itemName);
			Debug.Log($"Removed [{num}] instances of [{itemName}]");
		}

		public void PullOut(IEnumerable<DataFile> items)
		{
			CheckPopulate();
			foreach (DataFile item in items)
			{
				current.Remove(item);
			}
		}

		public void PullOut(DataFile item)
		{
			CheckPopulate();
			current.Remove(item);
		}

		public DataFile Pull()
		{
			CheckPopulate();
			DataFile result = current[0];
			current.RemoveAt(0);
			return result;
		}

		public DataFile[] Pull(int itemCount, bool allowDuplicates = false)
		{
			List<DataFile> list = new List<DataFile>();
			while (list.Count < itemCount)
			{
				CheckPopulate();
				for (int i = 0; i < current.Count; i++)
				{
					DataFile item = current[i];
					if (allowDuplicates || !list.Contains(item))
					{
						list.Add(item);
						current.RemoveAt(i);
						break;
					}
				}
			}

			return list.ToArray();
		}

		public DataFile[] Pull(int itemCount, bool allowDuplicates, Predicate<DataFile> match)
		{
			List<DataFile> list = new List<DataFile>();
			while (list.Count < itemCount)
			{
				CheckPopulate();
				List<DataFile> list2 = current.FindAll(match);
				if (list2.Count <= 0)
				{
					Populate();
					list2 = current.FindAll(match);
					if (list2.Count <= 0)
					{
						break;
					}
				}

				foreach (DataFile item in list2)
				{
					if (allowDuplicates || !list.Contains(item))
					{
						list.Add(item);
						current.RemoveAt(current.IndexOf(item));
						if (list.Count >= itemCount)
						{
							break;
						}
					}
				}
			}

			return list.ToArray();
		}

		public DataFile[] GetFromOriginalList(int itemCount, bool allowDuplicates)
		{
			List<DataFile> list = new List<DataFile>();
			while (list.Count < itemCount)
			{
				foreach (DataFile item in this.list.InRandomOrder())
				{
					if (allowDuplicates || !list.Contains(item))
					{
						list.Add(item);
						if (list.Count >= itemCount)
						{
							break;
						}
					}
				}
			}

			return list.ToArray();
		}

		public DataFile[] GetFromOriginalList(int itemCount, bool allowDuplicates, Predicate<DataFile> match)
		{
			List<DataFile> list = new List<DataFile>();
			while (list.Count < itemCount)
			{
				List<DataFile> list2 = current.FindAll(match);
				if (list2.Count <= 0)
				{
					break;
				}

				foreach (DataFile item in list2.InRandomOrder())
				{
					if (allowDuplicates || !list.Contains(item))
					{
						list.Add(item);
						if (list.Count >= itemCount)
						{
							break;
						}
					}
				}
			}

			return list.ToArray();
		}

		public void CheckPopulate()
		{
			if (current.Count <= 0)
			{
				Populate();
			}
		}

		public void Populate()
		{
			current.AddRange(list.InRandomOrder());
		}
	}

	public readonly Dictionary<string, Pool> poolLookup = new Dictionary<string, Pool>();

	public void Populate(ClassData classData)
	{
		poolLookup.Clear();
		RewardPool[] rewardPools = classData.rewardPools;
		foreach (RewardPool rewardPool in rewardPools)
		{
			Add(rewardPool);
		}
	}

	public void Add(RewardPool rewardPool)
	{
		if (!poolLookup.ContainsKey(rewardPool.type))
		{
			poolLookup[rewardPool.type] = new Pool();
		}

		for (int i = 0; i < rewardPool.copies; i++)
		{
			poolLookup[rewardPool.type].Add(rewardPool.list);
			Debug.Log($"Character Reward Pool [{rewardPool.type}] Populated with [{rewardPool.list.Count}] items from [{rewardPool.name}]");
		}
	}

	public List<DataFile> GetItemsInPool(string name)
	{
		if (poolLookup.TryGetValue(name, out var value))
		{
			return value.list;
		}

		return null;
	}

	public void Add(string poolName, IEnumerable<DataFile> items)
	{
		if (!poolLookup.ContainsKey(poolName))
		{
			poolLookup[poolName] = new Pool();
		}

		poolLookup[poolName].Add(items);
	}

	public void RemoveLockedCards()
	{
		List<UnlockData> remainingUnlocks = MetaprogressionSystem.GetRemainingUnlocks();
		if (poolLookup.TryGetValue("Items", out var value))
		{
			List<string> lockedItems = MetaprogressionSystem.GetLockedItems(remainingUnlocks);
			Debug.Log("Locked Items: [" + string.Join(", ", lockedItems) + "]");
			value.Remove(lockedItems);
		}

		if (poolLookup.TryGetValue("Units", out var value2))
		{
			List<string> lockedCompanions = MetaprogressionSystem.GetLockedCompanions(remainingUnlocks);
			Debug.Log("Locked Companions: [" + string.Join(", ", lockedCompanions) + "]");
			value2.Remove(lockedCompanions);
		}

		if (poolLookup.TryGetValue("Charms", out var value3))
		{
			List<string> lockedCharms = MetaprogressionSystem.GetLockedCharms(remainingUnlocks);
			Debug.Log("Locked Charms: [" + string.Join(", ", lockedCharms) + "]");
			value3.Remove(lockedCharms);
		}
	}

	public void RemoveCardsFromStartingDeck()
	{
		HashSet<string> hashSet = new HashSet<string>();
		HashSet<string> hashSet2 = new HashSet<string>();
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			switch (item.cardType.name)
			{
				case "Friendly":
					hashSet2.Add(item.name);
					break;
				case "Item":
				case "Clunker":
					hashSet.Add(item.name);
					break;
			}
		}

		if (hashSet.Count > 0)
		{
			Debug.Log("Removing Items: [" + string.Join(", ", hashSet) + "]");
			(poolLookup.ContainsKey("Items") ? poolLookup["Items"] : null)?.Remove(hashSet);
		}

		if (hashSet2.Count > 0)
		{
			Debug.Log("Removing Units: [" + string.Join(", ", hashSet2) + "]");
			(poolLookup.ContainsKey("Units") ? poolLookup["Units"] : null)?.Remove(hashSet2);
		}
	}

	public void RemoveCompanionsInFinalBossBattle()
	{
		Pool pool = null;
		CardSaveData[] array = SaveSystem.LoadProgressData<CardSaveData[]>("finalBossDeck", null);
		if (array == null)
		{
			return;
		}

		CardSaveData[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			CardData cardData = array2[i].Peek();
			if ((bool)cardData && cardData.cardType.name == "Friendly")
			{
				Debug.Log("Removing [" + cardData.name + "]");
				if (pool == null)
				{
					pool = (poolLookup.ContainsKey("Units") ? poolLookup["Units"] : null);
				}

				pool?.Remove(cardData.name);
			}
		}
	}

	public T Pull<T>(object pulledBy, string poolName) where T : DataFile
	{
		return Pull<T>(pulledBy, poolName, 1)[0];
	}

	public T[] Pull<T>(object pulledBy, string poolName, int itemCount, bool allowDuplicates = false) where T : DataFile
	{
		List<DataFile> list = Events.PullRewards(pulledBy, poolName, ref itemCount);
		if (poolLookup.ContainsKey(poolName))
		{
			Pool pool = poolLookup[poolName];
			if (list.Count > 0)
			{
				foreach (DataFile item in list)
				{
					pool.PullOut(item);
				}
			}

			if (itemCount > 0)
			{
				list.AddRange(pool.Pull(itemCount, allowDuplicates));
			}
		}

		return list.Cast<T>().ToArray();
	}

	public T[] Pull<T>(object pulledBy, string poolName, int itemCount, bool allowDuplicates, Predicate<DataFile> match) where T : DataFile
	{
		List<DataFile> list = Events.PullRewards(pulledBy, poolName, ref itemCount);
		if (poolLookup.ContainsKey(poolName))
		{
			Pool pool = poolLookup[poolName];
			if (list.Count > 0)
			{
				foreach (DataFile item in list)
				{
					pool.PullOut(item);
				}
			}

			if (itemCount > 0)
			{
				list.AddRange(pool.Pull(itemCount, allowDuplicates, match));
			}
		}

		return list.Cast<T>().ToArray();
	}

	public T[] GetFromOriginalList<T>(object pulledBy, string poolName, int itemCount, bool allowDuplicates) where T : DataFile
	{
		List<DataFile> list = Events.PullRewards(pulledBy, poolName, ref itemCount);
		if (poolLookup.ContainsKey(poolName))
		{
			Pool pool = poolLookup[poolName];
			if (itemCount > 0)
			{
				list.AddRange(pool.GetFromOriginalList(itemCount, allowDuplicates));
			}
		}

		return list.Cast<T>().ToArray();
	}

	public T[] GetFromOriginalList<T>(object pulledBy, string poolName, int itemCount, bool allowDuplicates, Predicate<DataFile> match) where T : DataFile
	{
		List<DataFile> list = Events.PullRewards(pulledBy, poolName, ref itemCount);
		if (poolLookup.ContainsKey(poolName))
		{
			Pool pool = poolLookup[poolName];
			if (itemCount > 0)
			{
				list.AddRange(pool.GetFromOriginalList(itemCount, allowDuplicates, match));
			}
		}

		return list.Cast<T>().ToArray();
	}

	public void PullOut(string poolName, IEnumerable<DataFile> items)
	{
		if (poolLookup.TryGetValue(poolName, out var value))
		{
			value.PullOut(items);
		}
	}

	public void PullOut(string poolName, DataFile item)
	{
		if (poolLookup.TryGetValue(poolName, out var value))
		{
			value.PullOut(item);
		}
	}
}
