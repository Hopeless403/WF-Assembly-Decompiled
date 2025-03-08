using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using HarmonyLib.Tools;
using JetBrains.Annotations;
using Steamworks;
using Steamworks.Ugc;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public abstract class WildfrostMod : IComparable<WildfrostMod>
	{
		[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
		public sealed class ConfigItemAttribute : Attribute
		{
			public string comment;

			public string forceTitle;

			public string fieldName;

			public string defaultValue;

			public string Title
			{
				get
				{
					if (!string.IsNullOrEmpty(forceTitle))
					{
						return forceTitle;
					}
					return fieldName;
				}
			}

			[Obsolete("Use new constructor", true)]
			public ConfigItemAttribute(object defaultValue, string forceTitle = null)
			{
				comment = "";
				this.forceTitle = forceTitle;
				this.defaultValue = TypeDescriptor.GetConverter(defaultValue.GetType()).ConvertToString(defaultValue);
			}

			public ConfigItemAttribute(object defaultValue, string comment = "", string forceTitle = null)
			{
				this.comment = comment;
				this.forceTitle = forceTitle;
				this.defaultValue = TypeDescriptor.GetConverter(defaultValue.GetType()).ConvertToString(defaultValue);
			}
		}

		private struct ConfigStorage
		{
			public class FileIsCorrupted : Exception
			{
				public FileIsCorrupted(string s)
					: base(s)
				{
				}
			}

			public (ConfigItemAttribute atr, FieldInfo field)[] Store;

			public (string title, string value)[] Read;

			public WildfrostMod Mod;

			public void WriteToFile(string name)
			{
				StringBuilder stringBuilder = new StringBuilder();
				(ConfigItemAttribute, FieldInfo)[] store = Store;
				for (int i = 0; i < store.Length; i++)
				{
					(ConfigItemAttribute, FieldInfo) tuple = store[i];
					tuple.Item1.fieldName = tuple.Item2.Name;
					stringBuilder.AppendLine("//" + tuple.Item1.comment);
					stringBuilder.AppendLine(tuple.Item2.FieldType.FullName + " : " + tuple.Item1.Title + " = " + tuple.Item1.defaultValue);
				}
				File.WriteAllText(name, stringBuilder.ToString());
			}

			public void ReadFromFile(string name)
			{
				int num = 0;
				while (true)
				{
					try
					{
						IEnumerable<string> enumerable = File.ReadLines(name);
						(ConfigItemAttribute, FieldInfo)[] store = Store;
						for (int i = 0; i < store.Length; i++)
						{
							(ConfigItemAttribute, FieldInfo) tuple = store[i];
							tuple.Item1.fieldName = tuple.Item2.Name;
						}
						List<(ConfigItemAttribute, FieldInfo)> list = Store.ToList();
						foreach (string item in enumerable)
						{
							if (item.StartsWith("//"))
							{
								continue;
							}
							int num2 = item.IndexOf(':');
							int num3 = item.IndexOf('=');
							int i = num2 - 1 - 0;
							string text = item.Substring(0, i);
							i = num2 + 2;
							int length = num3 - 1 - i;
							string title = item.Substring(i, length);
							int length2 = item.Length;
							length = num3 + 2;
							i = length2 - length;
							string text2 = item.Substring(length, i);
							(ConfigItemAttribute, FieldInfo) tuple2 = list.Find(((ConfigItemAttribute atr, FieldInfo field) a) => a.atr.Title == title);
							var (configItemAttribute, fieldInfo) = tuple2;
							if (configItemAttribute == null && fieldInfo == null)
							{
								throw new FileIsCorrupted("Config error, no store");
							}
							if (tuple2.Item2 == null)
							{
								throw new Exception("Config value not found " + tuple2.Item1.Title);
							}
							Type type = null;
							Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
							for (i = 0; i < assemblies.Length; i++)
							{
								Type type2 = assemblies[i].GetType(text);
								if (type2 != null)
								{
									type = type2;
									break;
								}
							}
							if (type == null)
							{
								throw new Exception("Unknown value type " + text);
							}
							object value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(text2);
							tuple2.Item2.SetValue(Mod, value);
						}
						break;
					}
					catch (FileIsCorrupted)
					{
						num++;
						if (num > 1)
						{
							throw;
						}
						File.Delete(name);
						WriteToFile(name);
					}
				}
			}
		}

		internal struct BuilderToBuilt<T, Y> where T : DataFile where Y : DataFileBuilder<T, Y>, new()
		{
			internal List<T> built;

			internal List<Y> builder;

			public BuilderToBuilt(List<T> dataFiles, List<Y> builders)
			{
				built = dataFiles;
				builder = builders;
			}

			public void OnAfterAllModBuildsEvent()
			{
				for (int i = 0; i < builder.Count; i++)
				{
					Y val = builder[i];
					T d = built[i];
					val.OnAfterAllModBuildsEvent(d);
				}
			}
		}

		public class DebugLoggerTextWriter : TextWriter
		{
			public override Encoding Encoding => Encoding.UTF8;

			public override void Write(string value)
			{
				value = "[HARMONY] " + value;
				Debug.Log(value);
			}

			public override void WriteLine(string value)
			{
				value = "[HARMONY] " + value;
				Debug.Log(value + "\n");
			}
		}

		public string ModDirectory;

		protected Sprite _iconSprite;

		protected Harmony HarmonyInstance;

		public static List<Type> AllBuiledrs;

		public static List<Type> AllDataTypes;

		public virtual string IconPath => Path.Combine(ModDirectory, "icon.png");

		public virtual Sprite IconSprite => _iconSprite ?? (_iconSprite = IconPath.ToSprite());

		public abstract string GUID { get; }

		public abstract string[] Depends { get; }

		public abstract string Title { get; }

		public abstract string Description { get; }

		public bool HasLoaded { get; private set; }

		public virtual string ImagesDirectory => Path.Combine(ModDirectory, "images");

		public virtual TMPro.TMP_SpriteAsset SpriteAsset { get; }

		public T GetAsset<T>(string name)
		{
			return Addressables.LoadAssetAsync<T>(name).WaitForCompletion();
		}

		public Sprite GetImageSprite(string relPath)
		{
			return RelToAbsPath(relPath).ToSprite();
		}

		public string RelToAbsPath(params string[] relPath)
		{
			string[] array = new string[relPath.Length + 1];
			array[0] = ModDirectory;
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = relPath[i - 1];
			}
			return Path.Combine(array);
		}

		public CardData.TraitStacks CreateTraitStack(string name, int amount)
		{
			return new CardData.TraitStacks(Get<TraitData>(name), amount);
		}

		public CardData.StatusEffectStacks CreateEffectStack(string name, int amount)
		{
			return new CardData.StatusEffectStacks(Get<StatusEffectData>(name), amount);
		}

		[Obsolete("Use one without type parameters", true)]
		public CardData.StatusEffectStacks CreateEffectStack<T>(string name, int amount) where T : StatusEffectData
		{
			return new CardData.StatusEffectStacks(Get<StatusEffectData>(name), amount);
		}

		public T Get<T>(string name) where T : DataFile
		{
			string assetName = Extensions.PrefixGUID(name, this);
			T val = AddressableLoader.Get<T>(typeof(T).Name, assetName);
			if ((bool)(UnityEngine.Object)val)
			{
				return val;
			}
			return AddressableLoader.Get<T>(typeof(T).Name, name);
		}

		public T GetStatusEffect<T>(string name) where T : StatusEffectData
		{
			string assetName = Extensions.PrefixGUID(name, this);
			StatusEffectData statusEffectData = AddressableLoader.Get<StatusEffectData>("StatusEffectData", assetName);
			if ((bool)statusEffectData)
			{
				return (T)statusEffectData;
			}
			return (T)AddressableLoader.Get<StatusEffectData>("StatusEffectData", name);
		}

		internal static WildfrostMod[] GetLastMods()
		{
			string[] array = SaveSystem.LoadProgressData<string[]>("lastSavedMods");
			if (array != null)
			{
				return array.Select(Extensions.GetModFromGuid).ToArray();
			}
			return new WildfrostMod[0];
		}

		internal static void SetLastMods(WildfrostMod[] enabled)
		{
			string[] value = enabled.Select((WildfrostMod a) => a.GUID).ToArray();
			SaveSystem.SaveProgressData("lastSavedMods", value);
		}

		private ConfigStorage FromConfigs()
		{
			(ConfigItemAttribute, FieldInfo)[] configs = GetConfigs();
			ConfigStorage result = default(ConfigStorage);
			result.Store = configs;
			result.Mod = this;
			return result;
		}

		public (ConfigItemAttribute atr, FieldInfo field)[] GetConfigs()
		{
			List<(ConfigItemAttribute, FieldInfo)> list = new List<(ConfigItemAttribute, FieldInfo)>();
			FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				ConfigItemAttribute customAttribute = fieldInfo.GetCustomAttribute<ConfigItemAttribute>();
				if (customAttribute != null)
				{
					list.Add((customAttribute, fieldInfo));
				}
			}
			return list.ToArray();
		}

		public WildfrostMod(string modDirectory)
			: this()
		{
			ModDirectory = modDirectory;
		}

		public string ImagePath(string fileName)
		{
			return Path.Combine(ImagesDirectory, fileName);
		}

		public WildfrostMod[] GetDependancies(bool throwIfNotFound = true)
		{
			ILookup<string, WildfrostMod> look = Bootstrap.Mods.ToLookup((WildfrostMod a) => a.GUID);
			List<(string, WildfrostMod)> list = Depends.Select((string a) => (a, look[a].FirstOrDefault())).ToList();
			if (throwIfNotFound)
			{
				(string, WildfrostMod) tuple = list.Find(((string a, WildfrostMod) a) => a.Item2 == null);
				if (tuple.Item2 == null && !string.IsNullOrEmpty(tuple.Item1))
				{
					throw new Exception("Mod " + tuple.Item1 + " not found! While it is a dependency for " + GUID);
				}
			}
			return list.Select<(string, WildfrostMod), WildfrostMod>(((string a, WildfrostMod) a) => a.Item2).ToArray();
		}

		public void LoadDependancies()
		{
			WildfrostMod[] dependancies = GetDependancies();
			for (int i = 0; i < dependancies.Length; i++)
			{
				dependancies[i].ModLoad();
			}
		}

		private void UnloadModsDependantOnThis()
		{
			foreach (WildfrostMod item in Bootstrap.Mods.Where((WildfrostMod a) => a.HasLoaded && a.Depends.Contains(GUID)))
			{
				item.ModUnload();
			}
		}

		public void ModLoad()
		{
			if (!HasLoaded)
			{
				LoadDependancies();
				ConfigStorage configStorage = FromConfigs();
				string text = Path.Combine(ModDirectory, "config.cfg");
				if (!File.Exists(text))
				{
					configStorage.WriteToFile(text);
				}
				configStorage.ReadFromFile(text);
				Load();
				UpdateSave();
				Events.InvokeModLoaded(this);
			}
		}

		private void UpdateSave()
		{
			SetLastMods(Bootstrap.Mods.ToList().FindAll((WildfrostMod a) => a.HasLoaded).ToArray());
		}

		public void ModToggle()
		{
			if (HasLoaded)
			{
				ModUnload();
			}
			else if (!HasLoaded)
			{
				ModLoad();
			}
		}

		protected virtual void Load()
		{
			HarmonyInstance.PatchAll(GetType().Assembly);
			List<Type> allBuiledrs = AllBuiledrs;
			List<Type> allDataTypes = AllDataTypes;
			Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
			foreach (Type dataType2 in allDataTypes)
			{
				Type type = allBuiledrs.Find((Type a) => a.BaseType.GetGenericArguments()[0] == dataType2);
				if (!(type == null))
				{
					object value = typeof(WildfrostMod).GetMethod("AddAllTAssetsToGroup", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(dataType2, type).Invoke(this, new object[0]);
					dictionary.Add(dataType2, value);
				}
			}
			foreach (Type dataType in allDataTypes)
			{
				Type type2 = allBuiledrs.Find((Type a) => a.BaseType.GetGenericArguments()[0] == dataType);
				if (!(type2 == null))
				{
					object obj = dictionary[dataType];
					typeof(BuilderToBuilt<, >).MakeGenericType(dataType, type2).GetMethod("OnAfterAllModBuildsEvent", BindingFlags.Instance | BindingFlags.Public).Invoke(obj, new object[0]);
				}
			}
			HasLoaded = true;
		}

		private BuilderToBuilt<T, Y> AddAllTAssetsToGroup<T, Y>() where T : DataFile where Y : DataFileBuilder<T, Y>, new()
		{
			List<Y> list = AddAssets<Y, T>();
			if (list == null)
			{
				BuilderToBuilt<T, Y> result = default(BuilderToBuilt<T, Y>);
				result.builder = new List<Y>();
				result.built = new List<T>();
				return result;
			}
			List<T> list2 = list.Select((Y a) => a.Build()).ToList();
			if (list2 != null)
			{
				foreach (T item in list2)
				{
					item.ModAdded = this;
					if (item is CardData cardData && !cardData.targetMode)
					{
						throw new Exception("Card must have a target mode " + cardData.name);
					}
				}
				AddressableLoader.AddRangeToGroup(typeof(T).Name, list2);
			}
			return new BuilderToBuilt<T, Y>(list2, list);
		}

		[CanBeNull]
		public virtual List<T> AddAssets<T, Y>() where T : DataFileBuilder<Y, T>, new() where Y : DataFile
		{
			return null;
		}

		public void ModUnload()
		{
			if (HasLoaded)
			{
				Unload();
				UnloadModsDependantOnThis();
				UpdateSave();
				Events.InvokeModUnloaded(this);
			}
		}

		protected virtual void Unload()
		{
			if (!HasLoaded)
			{
				return;
			}
			HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
			HasLoaded = false;
			foreach (Type item in typeof(WildfrostMod).Assembly.GetTypes().ToList().FindAll((Type a) => a.BaseType == typeof(DataFile)))
			{
				typeof(WildfrostMod).GetMethod("RemoveeAllTAssetsFromGroup", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(item).Invoke(this, new object[0]);
			}
		}

		private void RemoveeAllTAssetsFromGroup<T>() where T : DataFile
		{
			T[] array = UnityEngine.Object.FindObjectsOfType<T>();
			foreach (T val in array)
			{
				if (val.ModAdded == this)
				{
					if (val is CardData inst)
					{
						inst.RemoveFromPets();
					}
					AddressableLoader.RemoveFromGroup(typeof(T).Name, val);
					val.Destroy();
				}
			}
		}

		private WildfrostMod()
		{
			HarmonyInstance = new Harmony(GUID);
		}

		static WildfrostMod()
		{
			AllBuiledrs = typeof(WildfrostMod).Assembly.GetTypes().ToList().FindAll(delegate(Type a)
			{
				Type baseType = a.BaseType;
				return (object)baseType != null && baseType.IsGenericType && a.BaseType.GetGenericTypeDefinition() == typeof(DataFileBuilder<, >);
			});
			AllDataTypes = typeof(WildfrostMod).Assembly.GetTypes().ToList().FindAll((Type a) => a.BaseType == typeof(DataFile));
			Harmony.DEBUG = true;
			HarmonyFileLog.Enabled = true;
			HarmonyLib.Tools.Logger.ChannelFilter = HarmonyLib.Tools.Logger.LogChannel.Info | HarmonyLib.Tools.Logger.LogChannel.Debug;
			HarmonyFileLog.Writer = new DebugLoggerTextWriter();
		}

		public void WriteLine(string text)
		{
			Debug.Log("[" + GUID + "] " + text);
		}

		public void WriteWarn(string text)
		{
			Debug.LogWarning("[" + GUID + "] " + text);
		}

		public void WriteError(string text)
		{
			Debug.LogError("[" + GUID + "] " + text);
		}

		public int CompareTo(WildfrostMod other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			return string.Compare(GUID, other.GUID, StringComparison.Ordinal);
		}

		public async void UpdateOrPublishWorkshop()
		{
			List<Item> entries = (await new Query(UgcType.Items, UserUGCList.Published, UserUGCListSortOrder.CreationOrderDesc, SteamClient.SteamId).WithMetadata(true).WithTag("Mod").GetPageAsync(1)).Value.Entries.ToList().FindAll((Item a) => a.Result != Result.FileNotFound);
			Item curItem = entries.Find((Item a) => a.Metadata == GUID);
			PublishResult result;
			string[] depends;
			if (entries.Count == 0 || curItem.Equals(default(Item)))
			{
				result = await Editor.NewCommunityFile.WithTitle(Title).WithDescription(Description).WithTag("Mod")
					.ForAppId(SteamClient.AppId)
					.WithPublicVisibility()
					.WithPreviewFile(IconPath)
					.WithContent(ModDirectory)
					.WithMetaData(GUID)
					.SubmitAsync();
				Item? item = await Item.GetAsync(result.FileId);
				depends = Depends;
				foreach (string depend2 in depends)
				{
					Item item2 = entries.Find((Item a) => a.Metadata == depend2);
					if (!curItem.Equals(default(Item)))
					{
						item?.AddDependency(item2.Id);
					}
				}
				Debug.Log("Upload result " + result);
				return;
			}
			result = await new Editor(curItem.Id).WithTitle(Title).WithDescription(Description).WithTag("Mod")
				.ForAppId(SteamClient.AppId)
				.WithPublicVisibility()
				.WithPreviewFile(IconPath)
				.WithContent(ModDirectory)
				.WithMetaData(GUID)
				.SubmitAsync();
			Item? item3 = await Item.GetAsync(result.FileId);
			depends = Depends;
			foreach (string depend in depends)
			{
				Item item4 = entries.Find((Item a) => a.Metadata == depend);
				if (!curItem.Equals(default(Item)))
				{
					item3?.AddDependency(item4.Id);
				}
			}
			Debug.Log("Update result " + result);
		}
	}
}
