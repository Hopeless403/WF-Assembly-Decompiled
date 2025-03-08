#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetaprogressionSystem : GameSystem
{
	public static readonly Dictionary<string, object> data = new Dictionary<string, object>
	{
		{
			"pets",
			new Dictionary<string, string>
			{
				{ "Wolfie", null },
				{ "BerryPet", "Pet 1" },
				{ "DemonPet", "Pet 2" },
				{ "DrawPet", "Pet 3" },
				{ "Jagzag", "Pet 4" },
				{ "InkPet", "Pet 4a" },
				{ "BoostPet", "Pet 5" }
			}
		},
		{
			"items",
			new List<string> { "Slapcrackers", "Snowcracker", "Hooker", "ScrapPile", "MegaMimik", "Krono" }
		},
		{
			"companions",
			new List<string> { "TinyTyko", "Bombom", "Blue", "LuminCat", "TheBaker", "Havok" }
		},
		{
			"events",
			new List<string> { "CampaignNodeCopyItem", "CampaignNodeCharmShop", "CampaignNodeCurseItems" }
		},
		{
			"buildings",
			new List<string> { "InventorHut", "IcebreakerHut", "HotSpring" }
		},
		{
			"charms",
			new Dictionary<string, string>
			{
				{ "Charm 1", "CardUpgradeFury" },
				{ "Charm 2", "CardUpgradeSnowImmune" },
				{ "Charm 3", "CardUpgradeAttackIncreaseCounter" },
				{ "Charm 4", "CardUpgradeAttackRemoveEffects" },
				{ "Charm 5", "CardUpgradeShellBecomesSpice" },
				{ "Charm 6", "CardUpgradeEffigy" },
				{ "Charm 7", "CardUpgradeShroomReduceHealth" },
				{ "Charm 8", "CardUpgradeAttackConsume" },
				{ "Charm 9", "CardUpgradeBlue" },
				{ "Charm 10", "CardUpgradeGreed" },
				{ "Charm 11", "CardUpgradeRemoveCharmLimit" },
				{ "Charm 12", "CardUpgradeFrenzyReduceAttack" },
				{ "Charm 13", "CardUpgradeConsumeAddHealth" },
				{ "Charm 14", "CardUpgradeAttackAndHealth" },
				{ "Charm 15", "CardUpgradeCritical" },
				{ "Charm 16", "CardUpgradeSpark" },
				{ "Charm 17", "CardUpgradeBootleg" },
				{ "Charm 18", "CardUpgradeHunger" },
				{ "Charm 19", "CardUpgradeShadeClay" },
				{ "Charm 20", "CardUpgradePlink" },
				{ "Charm 21", "CardUpgradeFlameberry" },
				{ "Charm 22", "CardUpgradeGlass" },
				{ "Charm 23", "CardUpgradeMuncher" },
				{ "Charm 24", "CardUpgradeHeartmist" },
				{ "Charm 25", "CardUpgradeHeartburn" },
				{ "Charm 26", "CardUpgradeMime" },
				{ "Charm 27", "CardUpgradeShredder" }
			}
		}
	};

	public static T Get<T>(string key) where T : class
	{
		return data[key] as T;
	}

	public static void Add<T, Y>(string key, Y keyValue, T value) where T : class where Y : class
	{
		Dictionary<Y, T> dictionary = (Dictionary<Y, T>)data[key];
		if (!dictionary.TryAdd(keyValue, value))
		{
			dictionary[keyValue] = value;
		}

		data[key] = dictionary;
	}

	public static bool Remove<T, Y>(string key, Y keyValue, T value) where T : class where Y : class
	{
		Dictionary<Y, T> dictionary = (Dictionary<Y, T>)data[key];
		bool result = dictionary.Remove(keyValue);
		data[key] = dictionary;
		return result;
	}

	public static void Add<T>(string key, T value) where T : class
	{
		List<T> list = (List<T>)data[key];
		list.Add(value);
		data[key] = list;
	}

	public static bool Remove<T>(string key, T value) where T : class
	{
		List<T> list = (List<T>)data[key];
		bool result = list.Remove(value);
		data[key] = list;
		return result;
	}

	public static T Get<T>(string key, T defaultValue)
	{
		if (data.ContainsKey(key))
		{
			object obj = data[key];
			if (obj is T)
			{
				return (T)obj;
			}
		}

		if (!SaveSystem.Enabled)
		{
			return defaultValue;
		}

		return SaveSystem.LoadProgressData(key, defaultValue);
	}

	public static void Set<T>(string key, T value)
	{
		if (SaveSystem.Enabled)
		{
			SaveSystem.SaveProgressData(key, value);
		}
	}

	public static List<string> GetUnlockedList()
	{
		return SaveSystem.LoadProgressData("unlocked", new List<string>());
	}

	public static IEnumerable<UnlockData> GetUnlocked(Predicate<UnlockData> match)
	{
		return from n in GetUnlockedList()
			select AddressableLoader.Get<UnlockData>("UnlockData", n) into unlock
			where unlock != null && unlock.IsActive && match(unlock)
			select unlock;
	}

	public static List<UnlockData> GetRemainingUnlocks(List<string> alreadyUnlocked = null)
	{
		if (alreadyUnlocked == null)
		{
			alreadyUnlocked = GetUnlockedList();
		}

		return (from a in AddressableLoader.GetGroup<UnlockData>("UnlockData")
			where (a.IsActive && !alreadyUnlocked.Contains(a.name)) || !a.IsActive
			orderby a.lowPriority
			select a).ToList();
	}

	public static bool IsUnlocked(UnlockData unlockData, List<string> alreadyUnlocked = null)
	{
		if (!(unlockData == null))
		{
			if (unlockData.IsActive)
			{
				return IsUnlocked(unlockData.name, alreadyUnlocked);
			}

			return false;
		}

		return true;
	}

	public static bool IsUnlocked(string unlockDataName, List<string> alreadyUnlocked = null)
	{
		if (alreadyUnlocked == null)
		{
			alreadyUnlocked = GetUnlockedList();
		}

		return alreadyUnlocked.Contains(unlockDataName);
	}

	public static List<ClassData> GetLockedClasses()
	{
		List<string> unlockedList = GetUnlockedList();
		return References.Classes.Where((ClassData c) => c.requiresUnlock != null && !IsUnlocked(c.requiresUnlock, unlockedList)).ToList();
	}

	public static List<string> GetLockedItems(List<UnlockData> remainingUnlocks)
	{
		int num = remainingUnlocks.Count((UnlockData a) => a.type == UnlockData.Type.Item);
		List<string> list = Get<List<string>>("items");
		int num2 = list.Count - num;
		List<string> list2 = new List<string>();
		for (int i = num2; i < list.Count; i++)
		{
			list2.Add(list[i]);
		}

		return list2;
	}

	public static List<string> GetLockedCompanions(List<UnlockData> remainingUnlocks)
	{
		int num = remainingUnlocks.Count((UnlockData a) => a.type == UnlockData.Type.Companion);
		List<string> list = Get<List<string>>("companions");
		int num2 = list.Count - num;
		List<string> list2 = new List<string>();
		for (int i = num2; i < list.Count; i++)
		{
			list2.Add(list[i]);
		}

		return list2;
	}

	public static List<string> GetLockedCharms(List<UnlockData> remainingUnlocks)
	{
		Dictionary<string, string> dictionary = Get<Dictionary<string, string>>("charms");
		List<string> list = new List<string>();
		foreach (UnlockData remainingUnlock in remainingUnlocks)
		{
			string value;
			if (remainingUnlock.type == UnlockData.Type.Charm && dictionary.TryGetValue(remainingUnlock.name, out value))
			{
				list.Add(value);
			}
		}

		return list;
	}

	public static string[] GetAllPets()
	{
		return GetPetDict().Keys.ToArray();
	}

	public static Dictionary<string, string> GetPetDict()
	{
		return Get<Dictionary<string, string>>("pets");
	}

	public static string[] GetUnlockedPets()
	{
		Dictionary<string, string> petDict = GetPetDict();
		List<string> list = SaveSystem.LoadProgressData<List<string>>("petHutUnlocks", null);
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, string> item2 in petDict)
		{
			string key;
			string value;
			item2.Deconstruct(out key, out value);
			string item = key;
			string text = value;
			if (text == null || (list != null && list.Contains(text)))
			{
				list2.Add(item);
			}
		}

		return list2.ToArray();
	}

	public static void SetUnlocksReady(string unlockName)
	{
		List<string> list = SaveSystem.LoadProgressData("townNew", new List<string>());
		list.Add(unlockName);
		SaveSystem.SaveProgressData("townNew", list);
		UnlockReadyIcon[] array = UnityEngine.Object.FindObjectsOfType<UnlockReadyIcon>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Set(true);
		}
	}

	public static bool CheckUnlockRequirements(UnlockData unlock, ICollection<string> alreadyUnlocked)
	{
		return unlock.requires.All((UnlockData requirement) => requirement.IsActive && alreadyUnlocked.Contains(requirement.name));
	}

	public static bool AnyUnlocksReady()
	{
		return SaveSystem.LoadProgressData("townNew", new List<string>()).Count > 0;
	}
}
