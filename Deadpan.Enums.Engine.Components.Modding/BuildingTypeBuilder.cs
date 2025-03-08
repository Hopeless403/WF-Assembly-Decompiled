using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class BuildingTypeBuilder : DataFileBuilder<BuildingType, BuildingTypeBuilder>
	{
		public BuildingTypeBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public BuildingTypeBuilder()
		{
		}

		public BuildingTypeBuilder WithTitle(UnityEngine.Localization.LocalizedString title)
		{
			_data.titleKey = title;
			return this;
		}

		public BuildingTypeBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_building_title", title);
			_data.titleKey = collection.GetString(_data.name + "_building_title");
			return this;
		}

		public BuildingTypeBuilder WithHelp(UnityEngine.Localization.LocalizedString title)
		{
			_data.helpKey = title;
			return this;
		}

		public BuildingTypeBuilder WithHelp(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_building_help", title);
			_data.helpKey = collection.GetString(_data.name + "_building_help");
			return this;
		}

		public BuildingTypeBuilder WithHelpEmoteType(Prompt.Emote.Type helpEmoteType = Prompt.Emote.Type.Explain)
		{
			_data.helpEmoteType = helpEmoteType;
			return this;
		}

		public BuildingTypeBuilder WithStarted(UnlockData started)
		{
			_data.started = started;
			return this;
		}

		public BuildingTypeBuilder WithFinished(UnlockData finished)
		{
			_data.finished = finished;
			return this;
		}

		public BuildingTypeBuilder WithUnlocks(params UnlockData[] unlocks)
		{
			_data.unlocks = unlocks;
			return this;
		}

		public BuildingTypeBuilder WithUnlockedCheckedKey(string unlockedCheckedKey)
		{
			_data.unlockedCheckedKey = unlockedCheckedKey;
			return this;
		}
	}
}
