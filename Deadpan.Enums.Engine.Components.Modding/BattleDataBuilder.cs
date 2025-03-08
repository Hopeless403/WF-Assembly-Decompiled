using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class BattleDataBuilder : DataFileBuilder<BattleData, BattleDataBuilder>
	{
		public BattleDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public BattleDataBuilder()
		{
		}

		public BattleDataBuilder WithTitle(string title)
		{
			_data.title = title;
			return this;
		}

		public BattleDataBuilder WithPointFactor(float factor = 1f)
		{
			_data.pointFactor = factor;
			return this;
		}

		public BattleDataBuilder WithWaveCounter(int waveCounter = 4)
		{
			_data.waveCounter = waveCounter;
			return this;
		}

		public BattleDataBuilder WithPools(params BattleWavePoolData[] pools)
		{
			_data.pools = pools;
			return this;
		}

		public BattleDataBuilder WithBonusUnitPool(params CardData[] pools)
		{
			_data.bonusUnitPool = pools;
			return this;
		}

		public BattleDataBuilder WithBonusUnitRange(Vector2Int v)
		{
			_data.bonusUnitRange = v;
			return this;
		}

		public BattleDataBuilder WithGoldGiverPool(params CardData[] pools)
		{
			_data.goldGiverPool = pools;
			return this;
		}

		public BattleDataBuilder WithGoldGivers(int amount = 1)
		{
			_data.goldGivers = amount;
			return this;
		}

		public BattleDataBuilder WithGenerationScript(BattleGenerationScript s)
		{
			_data.generationScript = s;
			return this;
		}

		public BattleDataBuilder WithSetUpScript(Script s)
		{
			_data.setUpScript = s;
			return this;
		}

		public BattleDataBuilder WithSprite(Sprite sprite)
		{
			_data.sprite = sprite;
			return this;
		}

		public BattleDataBuilder WithSprite(string sprite)
		{
			_data.sprite = Mod.GetImageSprite(sprite);
			return this;
		}

		public BattleDataBuilder WithName(string name, SystemLanguage lang = SystemLanguage.English)
		{
			StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(lang));
			collection.SetString(_data.name + "_ref", name);
			_data.nameRef = collection.GetString(_data.name + "_ref");
			return this;
		}
	}
}
