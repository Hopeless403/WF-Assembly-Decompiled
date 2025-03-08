using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public static class LocalizationHelper
	{
		[HarmonyPatch(typeof(UnityEngine.Localization.LocalizedString), "GetLocalizedString", new Type[] { })]
		private class PatchLocalie1
		{
			[HarmonyPostfix]
			private static void Postfix(ref string __result, UnityEngine.Localization.LocalizedString __instance)
			{
				if (string.IsNullOrEmpty(__result))
				{
					StringTable collection = GetCollection(__instance.TableReference.TableCollectionName, LocalizationSettings.SelectedLocale.Identifier);
					long keyId = ((__instance.TableEntryReference.ReferenceType == TableEntryReference.Type.Id) ? __instance.TableEntryReference.KeyId : collection.SharedData.GetId(__instance.TableEntryReference.Key));
					if (collection.TryGetValue(keyId, out var value))
					{
						__result = value.Value;
					}
				}
			}
		}

		[HarmonyPatch(typeof(LocalizedStringDatabase), "GenerateLocalizedString", new Type[]
		{
			typeof(StringTable),
			typeof(StringTableEntry),
			typeof(TableReference),
			typeof(TableEntryReference),
			typeof(Locale),
			typeof(IList<object>)
		})]
		private class PatchLocalie2
		{
			[HarmonyPostfix]
			private static void Postfix(ref string __result, LocalizedStringDatabase __instance, StringTable table, StringTableEntry entry, TableReference tableReference, TableEntryReference tableEntryReference, Locale locale, IList<object> arguments)
			{
				if (string.IsNullOrEmpty(__result))
				{
					StringTable collection = GetCollection(tableReference.TableCollectionName, locale.Identifier);
					long keyId = ((tableEntryReference.ReferenceType == TableEntryReference.Type.Id) ? tableEntryReference.KeyId : collection.SharedData.GetId(tableEntryReference.Key));
					if (collection.TryGetValue(keyId, out var value))
					{
						__result = value.Value;
					}
				}
			}
		}

		[HarmonyPatch(typeof(LocalizedStringDatabase), "GetLocalizedString", new Type[]
		{
			typeof(TableReference),
			typeof(TableEntryReference),
			typeof(IList<object>),
			typeof(Locale),
			typeof(FallbackBehavior)
		})]
		private class PatchLocalie3
		{
			[HarmonyPostfix]
			private static void Postfix(ref string __result, LocalizedStringDatabase __instance, TableReference tableReference, TableEntryReference tableEntryReference, IList<object> arguments, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings)
			{
				if (string.IsNullOrEmpty(__result))
				{
					if (locale == null)
					{
						locale = LocalizationSettings.SelectedLocale;
					}
					StringTable collection = GetCollection(tableReference.TableCollectionName, locale.Identifier);
					long keyId = ((tableEntryReference.ReferenceType == TableEntryReference.Type.Id) ? tableEntryReference.KeyId : collection.SharedData.GetId(tableEntryReference.Key));
					if (collection.TryGetValue(keyId, out var value))
					{
						__result = value.Value;
					}
				}
			}
		}

		private static readonly Dictionary<string, StringTable> stringTables;

		private static Harmony _harmony;

		public static Locale TryAddLocale(LocaleIdentifier locali)
		{
			if (LocalizationSettings.AvailableLocales.Locales.All((Locale a) => a.Identifier != locali))
			{
				Locale locale = Locale.CreateLocale(locali);
				locale.SortOrder = (ushort)LocalizationSettings.AvailableLocales.Locales.Count;
				LocalizationSettings.AvailableLocales.Locales.Add(locale);
				return locale;
			}
			return LocalizationSettings.AvailableLocales.Locales.Find((Locale a) => a.Identifier == locali);
		}

		static LocalizationHelper()
		{
			stringTables = new Dictionary<string, StringTable>();
			_harmony = new Harmony("wildfrost");
			_harmony.PatchAll(typeof(LocalizationHelper).Assembly);
		}

		public static StringTable GetCollection(string name, LocaleIdentifier identifier)
		{
			string text = name + "_" + identifier.Code;
			StringTable stringTable = Addressables.LoadAssetAsync<StringTable>(text).WaitForCompletion();
			if (stringTable == null)
			{
				if (stringTables.TryGetValue(text, out var value))
				{
					return value;
				}
				StringTable stringTable2 = ScriptableObject.CreateInstance<StringTable>();
				stringTable2.name = text;
				stringTable2.LocaleIdentifier = identifier;
				StringTable stringTable3 = Addressables.LoadAssetAsync<StringTable>(name + "_" + new LocaleIdentifier(SystemLanguage.English).Code).WaitForCompletion();
				stringTable2.SharedData = stringTable3.SharedData;
				stringTables.Add(text, stringTable2);
				return stringTable2;
			}
			return stringTable;
		}

		public static UnityEngine.Localization.LocalizedString GetString(this StringTable table, string key)
		{
			TableReference table2 = table.TableCollectionName;
			UnityEngine.Localization.LocalizedString localizedString = new UnityEngine.Localization.LocalizedString();
			localizedString.SetReference(table2, key);
			return localizedString;
		}

		public static void SetString(this StringTable table, string key, string value)
		{
			long id = table.SharedData.GetId(key);
			if (id == 0L)
			{
				id = table.SharedData.AddKey(key).Id;
			}
			if (!table.ContainsKey(id))
			{
				table.AddEntry(key, value);
			}
			else
			{
				table[id].Value = value;
			}
		}
	}
}
