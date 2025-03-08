using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class CardTypeBuilder : DataFileBuilder<CardType, CardTypeBuilder>
	{
		public CardTypeBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public CardTypeBuilder()
		{
		}

		public CardTypeBuilder WithSortPriority(int sortPriority)
		{
			_data.sortPriority = sortPriority;
			return this;
		}

		public CardTypeBuilder WithIcon(Sprite icon)
		{
			_data.icon = icon;
			return this;
		}

		public CardTypeBuilder WithIcon(string icon)
		{
			_data.icon = Mod.GetImageSprite(icon);
			return this;
		}

		public CardTypeBuilder WithPrefabRef(AssetReference prefabRef)
		{
			_data.prefabRef = prefabRef;
			return this;
		}

		public CardTypeBuilder WithTextBoxSprite(Sprite icon)
		{
			_data.textBoxSprite = icon;
			return this;
		}

		public CardTypeBuilder WithTextBoxSprite(string icon)
		{
			_data.textBoxSprite = Mod.GetImageSprite(icon);
			return this;
		}

		public CardTypeBuilder WithNameTagSprite(Sprite icon)
		{
			_data.nameTagSprite = icon;
			return this;
		}

		public CardTypeBuilder WithNameTagSprite(string icon)
		{
			_data.nameTagSprite = Mod.GetImageSprite(icon);
			return this;
		}

		public CardTypeBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_type_title", title);
			_data.titleKey = collection.GetString(_data.name + "_type_title");
			return this;
		}

		public CardTypeBuilder WithTitle(UnityEngine.Localization.LocalizedString str)
		{
			_data.titleKey = str;
			return this;
		}

		public CardTypeBuilder WithCanDie(bool canDie)
		{
			_data.canDie = canDie;
			return this;
		}

		public CardTypeBuilder WithCanTakeCrown(bool canTakeCrown)
		{
			_data.canTakeCrown = canTakeCrown;
			return this;
		}

		public CardTypeBuilder WithCanRecall(bool canRecall)
		{
			_data.canRecall = canRecall;
			return this;
		}

		public CardTypeBuilder WithCanReserve(bool canReserve)
		{
			_data.canReserve = canReserve;
			return this;
		}

		public CardTypeBuilder WithItem(bool item)
		{
			_data.item = item;
			return this;
		}

		public CardTypeBuilder WithUnit(bool unit)
		{
			_data.unit = unit;
			return this;
		}

		public CardTypeBuilder WithTag(string tag)
		{
			_data.tag = tag;
			return this;
		}

		public CardTypeBuilder WithMiniboss(bool miniboss)
		{
			_data.miniboss = miniboss;
			return this;
		}

		public CardTypeBuilder WithDiscoverInJournal(bool discoverInJournal)
		{
			_data.discoverInJournal = discoverInJournal;
			return this;
		}

		public CardTypeBuilder WithDescriptionColours(Text.ColourProfileHex descriptionColours)
		{
			_data.descriptionColours = descriptionColours;
			return this;
		}
	}
}
