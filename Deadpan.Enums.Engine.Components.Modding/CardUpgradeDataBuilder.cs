using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class CardUpgradeDataBuilder : DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder>
	{
		private ChallengeData UnlockChallenge;

		public CardUpgradeDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public CardUpgradeDataBuilder()
		{
		}

		public CardUpgradeDataBuilder CreateCharm(string name)
		{
			return Create(name).WithType(CardUpgradeData.Type.Charm).AddPool();
		}

		public CardUpgradeDataBuilder WithPools(params RewardPool[] pools)
		{
			base.AfterBuildEvent += delegate(CardUpgradeData data)
			{
				data.WithPools(pools);
			};
			return this;
		}

		public CardUpgradeDataBuilder WithPools(params string[] pools)
		{
			base.AfterBuildEvent += delegate(CardUpgradeData data)
			{
				data.WithPools(pools.Select(Extensions.GetRewardPool).ToArray());
			};
			return this;
		}

		public CardUpgradeDataBuilder AddPool(RewardPool pool)
		{
			base.AfterBuildEvent += delegate(CardUpgradeData data)
			{
				data.AddPool(pool);
			};
			return this;
		}

		public CardUpgradeDataBuilder AddPool(string pool = "GeneralCharmPool")
		{
			base.AfterBuildEvent += delegate(CardUpgradeData data)
			{
				data.AddPool(Extensions.GetRewardPool(pool));
			};
			return this;
		}

		public CardUpgradeDataBuilder WithTier(int tier)
		{
			_data.tier = tier;
			return this;
		}

		public CardUpgradeDataBuilder WithImage(Sprite img)
		{
			_data.image = img;
			return this;
		}

		public CardUpgradeDataBuilder WithType(CardUpgradeData.Type type)
		{
			_data.type = type;
			return this;
		}

		public CardUpgradeDataBuilder SetAttackEffects(params CardData.StatusEffectStacks[] efs)
		{
			_data.attackEffects = efs;
			return this;
		}

		public CardUpgradeDataBuilder SetEffects(params CardData.StatusEffectStacks[] efs)
		{
			_data.effects = efs;
			return this;
		}

		public CardUpgradeDataBuilder SetTraits(params CardData.TraitStacks[] efs)
		{
			_data.giveTraits = efs;
			return this;
		}

		public CardUpgradeDataBuilder SetScripts(params CardScript[] efs)
		{
			_data.scripts = efs;
			return this;
		}

		public CardUpgradeDataBuilder SetConstraints(params TargetConstraint[] efs)
		{
			_data.targetConstraints = efs;
			return this;
		}

		public CardUpgradeDataBuilder SetBecomesTarget(bool val)
		{
			_data.becomesTargetedCard = val;
			return this;
		}

		public CardUpgradeDataBuilder SetCanBeRemoved(bool val)
		{
			_data.canBeRemoved = val;
			return this;
		}

		public CardUpgradeDataBuilder ChangeDamage(int val)
		{
			_data.damage = val;
			return this;
		}

		public CardUpgradeDataBuilder ChangeHP(int val)
		{
			_data.hp = val;
			return this;
		}

		public CardUpgradeDataBuilder ChangeCounter(int val)
		{
			_data.counter = val;
			return this;
		}

		public CardUpgradeDataBuilder ChangeUses(int val)
		{
			_data.uses = val;
			return this;
		}

		public CardUpgradeDataBuilder ChangeEffectBonus(int val)
		{
			_data.effectBonus = val;
			return this;
		}

		public CardUpgradeDataBuilder WithSetDamage(bool val)
		{
			_data.setDamage = val;
			return this;
		}

		public CardUpgradeDataBuilder WithSetHP(bool val)
		{
			_data.setHp = val;
			return this;
		}

		public CardUpgradeDataBuilder WithSetCounter(bool val)
		{
			_data.setCounter = val;
			return this;
		}

		public CardUpgradeDataBuilder WithSetUses(bool val)
		{
			_data.setUses = val;
			return this;
		}

		public CardUpgradeDataBuilder IsCharm(ChallengeData challenge, bool value = true)
		{
			UnlockChallenge = challenge;
			if (value)
			{
				_data.AddToCharms(challenge.reward);
				BuildingSequenceWithUnlocks<ChallengeShrineSequence>.OnStart += UnlockSequenceOnOnStart;
			}
			else
			{
				_data.RemoveFromCharms(challenge.reward);
				BuildingSequenceWithUnlocks<ChallengeShrineSequence>.OnStart -= UnlockSequenceOnOnStart;
			}
			return this;
		}

		private void UnlockSequenceOnOnStart<T>(BuildingSequenceWithUnlocks<T> obj) where T : BuildingSequenceWithUnlocks<T>
		{
			obj.AddChallengeStone(UnlockChallenge);
		}

		public CardUpgradeDataBuilder WithImage(string img)
		{
			_data.image = Mod.ImagePath(img).ToSprite();
			return this;
		}

		public CardUpgradeDataBuilder WithTitle(UnityEngine.Localization.LocalizedString title)
		{
			_data.titleKey = title;
			return this;
		}

		public CardUpgradeDataBuilder WithText(UnityEngine.Localization.LocalizedString text)
		{
			_data.textKey = text;
			return this;
		}

		public CardUpgradeDataBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Upgrades", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_title", title);
			_data.titleKey = collection.GetString(_data.name + "_title");
			return this;
		}

		public CardUpgradeDataBuilder WithText(string text, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Upgrades", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_text", text);
			_data.textKey = collection.GetString(_data.name + "_text");
			return this;
		}
	}
}
