namespace Deadpan.Enums.Engine.Components.Modding
{
	public class EyeDataBuilder : DataFileBuilder<EyeData, EyeDataBuilder>
	{
		public EyeDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public EyeDataBuilder()
		{
		}

		public EyeDataBuilder WithCardData(string cardData)
		{
			_data.cardData = cardData;
			return this;
		}

		public EyeDataBuilder WithCardData(CardData cardData)
		{
			_data.cardData = cardData.name;
			return this;
		}

		public EyeDataBuilder WithEyes(params EyeData.Eye[] eyes)
		{
			_data.eyes = eyes;
			return this;
		}
	}
}
