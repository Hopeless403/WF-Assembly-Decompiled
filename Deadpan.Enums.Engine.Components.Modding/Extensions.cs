using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public static class Extensions
	{
		public static CardData[] GetCategoryCardData(string category, bool mustBeFinal = true)
		{
			return AddressableLoader.GetGroup<CardData>("CardData").FindAll(Condition).ToArray();
			bool Condition(CardData a)
			{
				if ((a.cardType.name == category && !a.isEnemyClunker) || (a.cardType.name == "Clunker" && a.isEnemyClunker && category == "Enemy"))
				{
					if (mustBeFinal)
					{
						UnityEngine.Localization.LocalizedString titleKey = a.titleKey;
						if (titleKey != null)
						{
							return !titleKey.IsEmpty;
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public static StatusEffectDataBuilder SetSummonPrefabRef(this StatusEffectDataBuilder inst, string name = "SummonCreateCard")
		{
			inst.FreeModify(delegate(StatusEffectSummon summon)
			{
				summon.effectPrefabRef = new AssetReferenceGameObject(name);
			});
			return inst;
		}

		public static bool IsCharm(this CardUpgradeData inst)
		{
			return MetaprogressionSystem.Get<List<string>>("charms").Contains(inst.name);
		}

		public static void AddToCharms(this CardUpgradeData inst, UnlockData data)
		{
			MetaprogressionSystem.Add("charms", data.name, inst.name);
		}

		public static bool RemoveFromCharms(this CardUpgradeData inst, UnlockData data)
		{
			return MetaprogressionSystem.Remove("charms", data.name, inst.name);
		}

		public static bool IsCompanion(this CardData inst)
		{
			return MetaprogressionSystem.Get<List<string>>("companions").Contains(inst.name);
		}

		public static void AddToCompanions(this CardData inst)
		{
			MetaprogressionSystem.Add("companions", inst.name);
		}

		public static bool RemoveFromCompanions(this CardData inst)
		{
			return MetaprogressionSystem.Remove("companions", inst.name);
		}

		public static bool IsItem(this CardData inst)
		{
			return MetaprogressionSystem.Get<List<string>>("items").Contains(inst.name);
		}

		public static void AddToItems(this CardData inst)
		{
			MetaprogressionSystem.Add("items", inst.name);
		}

		public static bool RemoveFromItems(this CardData inst)
		{
			return MetaprogressionSystem.Remove("items", inst.name);
		}

		public static bool IsPet(this CardData inst)
		{
			return MetaprogressionSystem.Get<Dictionary<string, string>>("pets").ContainsKey(inst.name);
		}

		public static void AddToPets(this CardData inst, string requiredUnlock = null)
		{
			MetaprogressionSystem.Add("pets", inst.name, requiredUnlock);
		}

		public static bool RemoveFromPets(this CardData inst)
		{
			return MetaprogressionSystem.Remove<string, string>("pets", inst.name, null);
		}

		public static T[] RemoveFromArray<T>(this T[] sequence, T item)
		{
			return sequence.Where((T a) => !a.Equals(item)).ToArray();
		}

		public static T[] RemoveFromArray<T>(this T[] sequence, Func<T, bool> item)
		{
			return sequence.Where(item).ToArray();
		}

		public static string PrefixGUID(string name, WildfrostMod mod)
		{
			if (mod == null)
			{
				return name;
			}
			return mod.GUID + "." + name;
		}

		public static string GetGUID(string name)
		{
			int num = 0;
			int length = name.LastIndexOf('.') - num;
			return name.Substring(num, length);
		}

		public static WildfrostMod GetModFromGuid(string guid)
		{
			return Bootstrap.Mods.ToList().Find((WildfrostMod a) => a.GUID == guid);
		}

		public static CardAnimationProfile GetCardAnimationProfile(string name)
		{
			return Addressables.LoadAssetAsync<CardAnimationProfile>(name).WaitForCompletion();
		}

		public static TargetMode GetTargetMode(string name)
		{
			return Addressables.LoadAssetAsync<TargetMode>(name).WaitForCompletion();
		}

		public static void WithPools(this CardData data, params RewardPool[] pools)
		{
			for (int i = 0; i < pools.Length; i++)
			{
				pools[i].list.Add(data);
			}
		}

		public static void AddPool(this CardData data, RewardPool pool)
		{
			pool.list.Add(data);
		}

		public static void WithPools(this CardUpgradeData data, params RewardPool[] pools)
		{
			for (int i = 0; i < pools.Length; i++)
			{
				pools[i].list.Add(data);
			}
		}

		public static void AddPool(this CardUpgradeData data, RewardPool pool)
		{
			pool.list.Add(data);
		}

		public static UnityEngine.Localization.LocalizedString GetLocalizedString(string table, string key)
		{
			return LocalizationHelper.GetCollection(table, new LocaleIdentifier(SystemLanguage.English)).GetString(key);
		}

		public static HashSet<RewardPool> GetAllRewardPools()
		{
			HashSet<RewardPool> hashSet = new HashSet<RewardPool>();
			foreach (ClassData item in AddressableLoader.GetGroup<ClassData>("ClassData"))
			{
				hashSet.AddRange(item.rewardPools);
			}
			return hashSet;
		}

		public static Y Edit<T, Y>(this T data) where T : DataFile where Y : DataFileBuilder<T, Y>, new()
		{
			return new Y
			{
				Mod = (data.ModAdded ?? new InternalMod(null)),
				_data = data
			};
		}

		public static RewardPool GetRewardPool(string name)
		{
			foreach (ClassData item in AddressableLoader.GetGroup<ClassData>("ClassData"))
			{
				RewardPool[] rewardPools = item.rewardPools;
				foreach (RewardPool rewardPool in rewardPools)
				{
					if (rewardPool.name == name)
					{
						return rewardPool;
					}
				}
			}
			return null;
		}

		public static Texture2D ToTex(this string path)
		{
			Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false);
			if (!File.Exists(path))
			{
				return texture2D;
			}
			texture2D.LoadImage(File.ReadAllBytes(path));
			return texture2D;
		}

		public static Sprite ToSprite(this string path)
		{
			return path.ToTex().ToSprite();
		}

		public static Sprite ToSprite(this Texture2D t, Vector2? v = null)
		{
			Vector2 pivot = v ?? new Vector2(0.5f, 0.5f);
			return Sprite.Create(t, new Rect(0f, 0f, t.width, t.height), pivot);
		}
	}
}
