namespace Deadpan.Enums.Engine.Components.Modding
{
	public class BuildingPlotTypeBuilder : DataFileBuilder<BuildingPlotType, BuildingPlotTypeBuilder>
	{
		public BuildingPlotTypeBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public BuildingPlotTypeBuilder()
		{
		}

		public BuildingPlotTypeBuilder WithIllegalBuildings(params BuildingType[] illegalBuildings)
		{
			_data.illegalBuildings = illegalBuildings;
			return this;
		}
	}
}
