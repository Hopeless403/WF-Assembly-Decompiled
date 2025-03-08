namespace Deadpan.Enums.Engine.Components.Modding
{
	public class TraitDataBuilder : DataFileBuilder<TraitData, TraitDataBuilder>
	{
		public TraitDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public TraitDataBuilder()
		{
		}

		public TraitDataBuilder WithKeyword(KeywordData data)
		{
			_data.keyword = data;
			return this;
		}

		public TraitDataBuilder WithEffects(params StatusEffectData[] effects)
		{
			_data.effects = effects;
			return this;
		}

		public TraitDataBuilder WithOverrides(params TraitData[] traits)
		{
			_data.overrides = traits;
			return this;
		}

		public TraitDataBuilder WithIsReaction(bool isReaction)
		{
			_data.isReaction = isReaction;
			return this;
		}
	}
}
