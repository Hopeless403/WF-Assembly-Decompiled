using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class UnlockDataBuilder : DataFileBuilder<UnlockData, UnlockDataBuilder>
	{
		public UnlockDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public UnlockDataBuilder()
		{
		}

		public UnlockDataBuilder WithUnlockDescription(UnityEngine.Localization.LocalizedString str)
		{
			_data.unlockDesc = str;
			return this;
		}

		public UnlockDataBuilder WithUnlockTitle(UnityEngine.Localization.LocalizedString str)
		{
			_data.unlockTitle = str;
			return this;
		}

		public UnlockDataBuilder WithUnlockDescription(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("UI Text", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_unlockDesc", title);
			_data.unlockDesc = collection.GetString(_data.name + "_unlockDesc");
			return this;
		}

		public UnlockDataBuilder WithUnlockTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("UI Text", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_unlockTitle", title);
			_data.unlockTitle = collection.GetString(_data.name + "_unlockTitle");
			return this;
		}

		public UnlockDataBuilder WithRequires(params UnlockData[] requires)
		{
			_data.requires = requires;
			return this;
		}

		public UnlockDataBuilder WithRequires(params string[] requires)
		{
			_data.requires = requires.Select(Mod.Get<UnlockData>).ToArray();
			return this;
		}

		public UnlockDataBuilder WithLowPriority(float priority)
		{
			_data.lowPriority = priority;
			return this;
		}

		public UnlockDataBuilder WithBuilding(BuildingType relatedBuilding)
		{
			_data.relatedBuilding = relatedBuilding;
			SubscribeToBuildEvent(delegate(UnlockData data)
			{
				relatedBuilding.unlocks = relatedBuilding.unlocks.AddToArray(data);
			});
			return this;
		}

		public UnlockDataBuilder WithBuilding(string relatedBuilding)
		{
			return WithBuilding(Mod.Get<BuildingType>(relatedBuilding));
		}

		public UnlockDataBuilder WithType(UnlockData.Type type)
		{
			_data.type = type;
			return this;
		}
	}
}
