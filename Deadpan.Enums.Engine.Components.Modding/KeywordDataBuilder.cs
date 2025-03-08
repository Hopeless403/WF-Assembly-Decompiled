using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class KeywordDataBuilder : DataFileBuilder<KeywordData, KeywordDataBuilder>
	{
		public KeywordDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public KeywordDataBuilder()
		{
		}

		public KeywordDataBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Tooltips", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_title", title);
			_data.titleKey = collection.GetString(_data.name + "_title");
			return this;
		}

		public KeywordDataBuilder WithDescription(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Tooltips", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_desc", title);
			_data.descKey = collection.GetString(_data.name + "_desc");
			return this;
		}

		public KeywordDataBuilder WithTitleColour(Color? theColour = null)
		{
			if (!theColour.HasValue)
			{
				theColour = new Color(1f, 0.7921569f, 0.3411765f, 1f);
			}
			_data.titleColour = theColour.Value;
			return this;
		}

		public KeywordDataBuilder WithBodyColour(Color? theColour = null)
		{
			if (!theColour.HasValue)
			{
				theColour = Color.white;
			}
			_data.bodyColour = theColour.Value;
			return this;
		}

		public KeywordDataBuilder WithNoteColour(Color? theColour = null)
		{
			if (!theColour.HasValue)
			{
				theColour = Color.gray;
			}
			_data.noteColour = theColour.Value;
			return this;
		}

		public KeywordDataBuilder WithPanelColour(Color theColour)
		{
			_data.panelColor = theColour;
			return this;
		}

		public KeywordDataBuilder WithPanelSprite(string image)
		{
			_data.panelSprite = Mod.GetImageSprite(image);
			return this;
		}

		public KeywordDataBuilder WithIconName(string iconName)
		{
			_data.iconName = iconName;
			return this;
		}

		public KeywordDataBuilder WithIconTint(Color hexColor)
		{
			_data.iconTintHex = hexColor.ToHexRGB();
			return this;
		}

		public KeywordDataBuilder WithShow(bool show = true)
		{
			_data.show = show;
			return this;
		}

		public KeywordDataBuilder WithShowName(bool show)
		{
			_data.showName = show;
			return this;
		}

		public KeywordDataBuilder WithShowIcon(bool show = true)
		{
			_data.showIcon = show;
			return this;
		}

		public KeywordDataBuilder WithCanStack(bool show)
		{
			_data.canStack = show;
			return this;
		}
	}
}
