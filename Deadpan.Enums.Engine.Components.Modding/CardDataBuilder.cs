using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class CardDataBuilder : DataFileBuilder<CardData, CardDataBuilder>
	{
		private ChallengeData UnlockChallenge;

		public CardDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public CardDataBuilder()
		{
		}

		public CardDataBuilder SetStats(int? health = null, int? damage = null, int counter = 0)
		{
			return SetHealth(health).SetDamage(damage).SetCounter(counter);
		}

		public CardDataBuilder SetCounter(int counter)
		{
			_data.counter = counter;
			return this;
		}

		public CardDataBuilder SetDamage(int? damage)
		{
			if (damage.HasValue)
			{
				_data.damage = damage.Value;
			}
			_data.hasAttack = damage.HasValue;
			return this;
		}

		public CardDataBuilder NeedsTarget(bool value = true)
		{
			_data.needsTarget = value;
			return this;
		}

		public CardDataBuilder SetHealth(int? health)
		{
			if (health.HasValue)
			{
				_data.hp = health.Value;
			}
			_data.hasHealth = health.HasValue;
			return this;
		}

		public CardDataBuilder SetSprites(Sprite mainSprite, Sprite backgroundSprite)
		{
			_data.mainSprite = mainSprite;
			_data.backgroundSprite = backgroundSprite;
			return this;
		}

		public CardDataBuilder SetSprites(string mainSprite, string backgroundSprite)
		{
			return SetSprites(Mod.ImagePath(mainSprite).ToSprite(), Mod.ImagePath(backgroundSprite).ToSprite());
		}

		public CardDataBuilder SetStartWithEffect(params CardData.StatusEffectStacks[] stacks)
		{
			_data.startWithEffects = stacks;
			return this;
		}

		public CardDataBuilder SetTraits(params CardData.TraitStacks[] stacks)
		{
			_data.traits = stacks.ToList();
			return this;
		}

		public CardDataBuilder WithDescription(string desc)
		{
			_data.desc = desc;
			return this;
		}

		public CardDataBuilder WithValue(int price)
		{
			_data.value = price;
			return this;
		}

		public CardDataBuilder WithTargetMode(TargetMode mode)
		{
			_data.targetMode = mode;
			return this;
		}

		public CardDataBuilder WithTargetMode(string mode = "TargetModeBasic")
		{
			_data.targetMode = Extensions.GetTargetMode(mode);
			return this;
		}

		public CardDataBuilder WithPlayType(Card.PlayType type)
		{
			_data.playType = type;
			return this;
		}

		public CardDataBuilder SetAttackEffect(params CardData.StatusEffectStacks[] stacks)
		{
			_data.attackEffects = stacks;
			return this;
		}

		public CardDataBuilder WithIdleAnimationProfile(CardAnimationProfile bp)
		{
			_data.idleAnimationProfile = bp;
			return this;
		}

		public CardDataBuilder WithIdleAnimationProfile(string bp = "SwayAnimationProfile")
		{
			return WithIdleAnimationProfile(Extensions.GetCardAnimationProfile(bp));
		}

		public CardDataBuilder WithBloodProfile(BloodProfile bp)
		{
			_data.bloodProfile = bp;
			return this;
		}

		public CardDataBuilder WithBloodProfile(string bp = "Blood Profile Normal")
		{
			return WithBloodProfile(Mod.GetAsset<BloodProfile>(bp));
		}

		public CardDataBuilder CanPlayOnBoard(bool value = true)
		{
			_data.canPlayOnBoard = value;
			return this;
		}

		public CardDataBuilder CanPlayOnEnemy(bool value = true)
		{
			_data.canPlayOnEnemy = value;
			return this;
		}

		public CardDataBuilder CanPlayOnFriendly(bool value = true)
		{
			_data.canPlayOnFriendly = value;
			return this;
		}

		public CardDataBuilder CanPlayOnHand(bool value = true)
		{
			_data.canPlayOnHand = value;
			return this;
		}

		public CardDataBuilder CanBeHit(bool value = true)
		{
			_data.canBeHit = value;
			return this;
		}

		public CardDataBuilder CanShoveToOtherRow(bool value = true)
		{
			_data.canShoveToOtherRow = value;
			return this;
		}

		public CardDataBuilder AsUnit(string targetMode = "TargetModeBasic", string idleAnim = "SwayAnimationProfile", string bloodProfile = "Blood Profile Normal")
		{
			_data.canPlayOnEnemy = true;
			_data.canBeHit = true;
			_data.playType = Card.PlayType.Place;
			_data.canPlayOnBoard = true;
			_data.cardType = Mod.Get<CardType>("Friendly");
			return SetStats(0, 0).WithTargetMode(targetMode).WithBloodProfile(bloodProfile).WithIdleAnimationProfile(idleAnim);
		}

		public CardDataBuilder AsItem(string targetMode = "TargetModeBasic", string idleAnim = "SwayAnimationProfile")
		{
			_data.canPlayOnEnemy = true;
			_data.canBeHit = false;
			_data.playType = Card.PlayType.Play;
			_data.canPlayOnBoard = true;
			_data.cardType = Mod.Get<CardType>("Item");
			return FreeModify(delegate(CardData a)
			{
				a.uses = 1;
			}).WithTargetMode(targetMode).WithIdleAnimationProfile(idleAnim).CanPlayOnHand(false);
		}

		public CardDataBuilder IsCompanion(ChallengeData challenge, bool value = true)
		{
			UnlockChallenge = challenge;
			if (value)
			{
				_data.AddToCompanions();
				BuildingSequenceWithUnlocks<BuildingCardUnlockSequence>.OnStart += UnlockSequenceOnOnStart;
			}
			else
			{
				_data.RemoveFromCompanions();
				BuildingSequenceWithUnlocks<BuildingCardUnlockSequence>.OnStart -= UnlockSequenceOnOnStart;
			}
			return this;
		}

		public CardDataBuilder IsItem(ChallengeData challenge, bool value = true)
		{
			UnlockChallenge = challenge;
			if (value)
			{
				_data.AddToItems();
				BuildingSequenceWithUnlocks<InventorHutSequence>.OnStart += UnlockSequenceOnOnStart;
			}
			else
			{
				_data.RemoveFromItems();
				BuildingSequenceWithUnlocks<InventorHutSequence>.OnStart -= UnlockSequenceOnOnStart;
			}
			return this;
		}

		public CardDataBuilder IsPet(string challenge, bool value = true)
		{
			return IsPet(Mod.Get<ChallengeData>(challenge), value);
		}

		public CardDataBuilder IsPet(ChallengeData challenge, bool value = true)
		{
			UnlockChallenge = challenge;
			if (value)
			{
				_data.AddToPets((challenge == null) ? null : challenge.name);
				BuildingSequenceWithUnlocks<PetHutSequence>.OnStart += UnlockSequenceOnOnStart;
			}
			else
			{
				_data.RemoveFromPets();
				BuildingSequenceWithUnlocks<PetHutSequence>.OnStart -= UnlockSequenceOnOnStart;
			}
			return this;
		}

		private void UnlockSequenceOnOnStart<T>(BuildingSequenceWithUnlocks<T> obj) where T : BuildingSequenceWithUnlocks<T>
		{
			obj.AddSlot(UnlockChallenge);
		}

		public CardDataBuilder CreateItem(string name, string englishTitle, string targetMode = "TargetModeBasic", string idleAnim = "SwayAnimationProfile")
		{
			return Create(name).WithTitle(englishTitle).AsItem(targetMode, idleAnim);
		}

		[Obsolete("Reverse", true)]
		public CardDataBuilder CreateItem(string name, string englishTitle, string targetMode, string wtv, string idleAnim)
		{
			return Create(name).WithTitle(englishTitle).AsItem(targetMode, idleAnim);
		}

		public CardDataBuilder CreateUnit(string name, string englishTitle, string targetMode = "TargetModeBasic", string bloodProfile = "Blood Profile Normal", string idleAnim = "SwayAnimationProfile")
		{
			return Create(name).WithTitle(englishTitle).AsUnit(targetMode, idleAnim, bloodProfile);
		}

		public CardDataBuilder WithPools(params RewardPool[] pools)
		{
			base.AfterBuildEvent += delegate(CardData data)
			{
				data.WithPools(pools);
			};
			return this;
		}

		public CardDataBuilder WithPools(params string[] pools)
		{
			base.AfterBuildEvent += delegate(CardData data)
			{
				data.WithPools(pools.Select(Extensions.GetRewardPool).ToArray());
			};
			return this;
		}

		public CardDataBuilder AddPool(RewardPool pool)
		{
			base.AfterBuildEvent += delegate(CardData data)
			{
				data.AddPool(pool);
			};
			return this;
		}

		public CardDataBuilder AddPool(string pool = "GeneralUnitPool")
		{
			base.AfterBuildEvent += delegate(CardData data)
			{
				data.AddPool(Extensions.GetRewardPool(pool));
			};
			return this;
		}

		public CardDataBuilder WithCardType(CardType type)
		{
			_data.cardType = type;
			return this;
		}

		public CardDataBuilder WithCardType(string type = "Friendly")
		{
			_data.cardType = Mod.Get<CardType>(type);
			return this;
		}

		public CardDataBuilder WithTitle(UnityEngine.Localization.LocalizedString title)
		{
			_data.titleKey = title;
			return this;
		}

		public CardDataBuilder WithFlavour(UnityEngine.Localization.LocalizedString flavour)
		{
			_data.flavourKey = flavour;
			return this;
		}

		public CardDataBuilder WithText(UnityEngine.Localization.LocalizedString text)
		{
			_data.textKey = text;
			return this;
		}

		public CardDataBuilder WithTitle(string title, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_title", title);
			_data.titleKey = collection.GetString(_data.name + "_title");
			return this;
		}

		public CardDataBuilder WithFlavour(string flavour, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_flavour", flavour);
			_data.flavourKey = collection.GetString(_data.name + "_flavour");
			return this;
		}

		public CardDataBuilder WithText(string text, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_text", text);
			_data.textKey = collection.GetString(_data.name + "_text");
			return this;
		}

		~CardDataBuilder()
		{
			UnityEngine.Object.Destroy(_data);
		}

		public CardDataBuilder Clone()
		{
			return new CardDataBuilder(Mod)
			{
				_data = _data.InstantiateKeepName()
			};
		}
	}
}
