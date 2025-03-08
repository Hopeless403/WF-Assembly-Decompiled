using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class GameModifierDataBuilder : DataFileBuilder<GameModifierData, GameModifierDataBuilder>
	{
		public GameModifierDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public GameModifierDataBuilder()
		{
		}

		public GameModifierDataBuilder WithValue(int value = 100)
		{
			_data.value = value;
			return this;
		}

		public GameModifierDataBuilder WithVisible(bool visible = true)
		{
			_data.visible = visible;
			return this;
		}

		public GameModifierDataBuilder WithBellSprite(Sprite bellSprite)
		{
			_data.bellSprite = bellSprite;
			return this;
		}

		public GameModifierDataBuilder WithBellSprite(string bellSprite)
		{
			_data.bellSprite = Mod.GetImageSprite(bellSprite);
			return this;
		}

		public GameModifierDataBuilder WithDingerSprite(Sprite dingerSprite)
		{
			_data.dingerSprite = dingerSprite;
			return this;
		}

		public GameModifierDataBuilder WithDingerSprite(string dingerSprite)
		{
			_data.dingerSprite = Mod.GetImageSprite(dingerSprite);
			return this;
		}

		public GameModifierDataBuilder WithTitle(UnityEngine.Localization.LocalizedString title)
		{
			_data.titleKey = title;
			return this;
		}

		public GameModifierDataBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_modifier_title", title);
			_data.titleKey = collection.GetString(_data.name + "_modifier_title");
			return this;
		}

		public GameModifierDataBuilder WithDescription(UnityEngine.Localization.LocalizedString title)
		{
			_data.descriptionKey = title;
			return this;
		}

		public GameModifierDataBuilder WithDescription(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_modifier_desc", title);
			_data.descriptionKey = collection.GetString(_data.name + "_modifier_desc");
			return this;
		}

		public GameModifierDataBuilder WithSystemsToAdd(params string[] systemsToAdd)
		{
			_data.systemsToAdd = systemsToAdd;
			return this;
		}

		public GameModifierDataBuilder WithSetupScripts(params Script[] setupScripts)
		{
			_data.setupScripts = setupScripts;
			return this;
		}

		public GameModifierDataBuilder WithStartScripts(params Script[] startScripts)
		{
			_data.startScripts = startScripts;
			return this;
		}

		public GameModifierDataBuilder WithScriptPriority(int scriptPriority)
		{
			_data.scriptPriority = scriptPriority;
			return this;
		}

		public GameModifierDataBuilder WithBlockedBy(params GameModifierData[] blockedBy)
		{
			_data.blockedBy = blockedBy;
			return this;
		}

		public GameModifierDataBuilder WithLinkedStormBell(HardModeModifierData linkedStormBell)
		{
			_data.linkedStormBell = linkedStormBell;
			return this;
		}

		public GameModifierDataBuilder WithRingSfxEvent(EventReference ringSfxEvent)
		{
			_data.ringSfxEvent = ringSfxEvent;
			return this;
		}

		public GameModifierDataBuilder WithRingSfxPitch()
		{
			_data.ringSfxPitch = new Vector2(1f, 1f);
			return this;
		}

		public GameModifierDataBuilder WithRingSfxPitch(Vector2 ringSfxPitch)
		{
			_data.ringSfxPitch = ringSfxPitch;
			return this;
		}
	}
}
