using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class StatusEffectDataBuilder : DataFileBuilder<StatusEffectData, StatusEffectDataBuilder>
	{
		public StatusEffectDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public StatusEffectDataBuilder()
		{
		}

		public StatusEffectDataBuilder WithIsStatus(bool value)
		{
			_data.isStatus = value;
			return this;
		}

		public StatusEffectDataBuilder WithIsReaction(bool value)
		{
			_data.isReaction = value;
			return this;
		}

		public StatusEffectDataBuilder WithIsKeyword(bool value)
		{
			_data.isKeyword = value;
			return this;
		}

		public StatusEffectDataBuilder WithType(string type)
		{
			_data.type = type;
			return this;
		}

		public StatusEffectDataBuilder WithKeyword(string type)
		{
			_data.keyword = type;
			return this;
		}

		public StatusEffectDataBuilder WithIconGroupName(string type)
		{
			_data.iconGroupName = type;
			return this;
		}

		public StatusEffectDataBuilder WithVisible(bool value)
		{
			_data.visible = value;
			return this;
		}

		public StatusEffectDataBuilder WithStackable(bool value)
		{
			_data.stackable = value;
			return this;
		}

		public StatusEffectDataBuilder WithOffensive(bool value)
		{
			_data.offensive = value;
			return this;
		}

		public StatusEffectDataBuilder WithMakesOffensive(bool value)
		{
			_data.makesOffensive = value;
			return this;
		}

		public StatusEffectDataBuilder WithDoesDamage(bool value)
		{
			_data.doesDamage = value;
			return this;
		}

		public StatusEffectDataBuilder WithCanBeBoosted(bool value)
		{
			_data.canBeBoosted = value;
			return this;
		}

		public StatusEffectDataBuilder WithText(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Card Text", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_text", title);
			_data.textKey = collection.GetString(_data.name + "_text");
			return this;
		}

		public StatusEffectDataBuilder WithTextInsert(string value)
		{
			_data.textInsert = value;
			return this;
		}

		public StatusEffectDataBuilder WithOrder(int order)
		{
			_data.textOrder = order;
			return this;
		}
	}
}
