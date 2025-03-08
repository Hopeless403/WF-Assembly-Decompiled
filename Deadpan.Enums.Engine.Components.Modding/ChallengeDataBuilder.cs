using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class ChallengeDataBuilder : DataFileBuilder<ChallengeData, ChallengeDataBuilder>
	{
		public ChallengeDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public ChallengeDataBuilder WithTitle(UnityEngine.Localization.LocalizedString str)
		{
			_data.titleKey = str;
			return this;
		}

		public ChallengeDataBuilder WithText(UnityEngine.Localization.LocalizedString str)
		{
			_data.textKey = str;
			return this;
		}

		public ChallengeDataBuilder WithRewardText(UnityEngine.Localization.LocalizedString str)
		{
			_data.rewardKey = str;
			return this;
		}

		public ChallengeDataBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Challenges", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_title", title);
			_data.titleKey = collection.GetString(_data.name + "_title");
			return this;
		}

		public ChallengeDataBuilder WithText(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Challenges", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_text", title);
			_data.textKey = collection.GetString(_data.name + "_text");
			return this;
		}

		public ChallengeDataBuilder()
		{
		}

		public ChallengeDataBuilder WithRewardText(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Challenges", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_reward", title);
			_data.rewardKey = collection.GetString(_data.name + "_reward");
			return this;
		}

		public ChallengeDataBuilder WithGoal(int amountGoal)
		{
			_data.goal = amountGoal;
			return this;
		}

		public ChallengeDataBuilder WithListener(ChallengeListener listener)
		{
			_data.listener = listener;
			return this;
		}

		public ChallengeDataBuilder WithListener(string listener)
		{
			_data.listener = Mod.Get<ChallengeListener>(listener);
			return this;
		}

		public ChallengeDataBuilder WithIcon(Sprite icon)
		{
			_data.icon = icon;
			return this;
		}

		public ChallengeDataBuilder WithRequires(params ChallengeData[] requires)
		{
			_data.requires = requires;
			return this;
		}

		public ChallengeDataBuilder WithRequires(params string[] requires)
		{
			_data.requires = requires.Select(Mod.Get<ChallengeData>).ToArray();
			return this;
		}

		public ChallengeDataBuilder WithReward(UnlockData reward)
		{
			_data.reward = reward;
			return this;
		}

		public ChallengeDataBuilder WithReward(string reward)
		{
			_data.reward = Mod.Get<UnlockData>(reward);
			return this;
		}
	}
}
