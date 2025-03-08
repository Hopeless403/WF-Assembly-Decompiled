namespace Deadpan.Enums.Engine.Components.Modding
{
	internal class InternalMod : WildfrostMod
	{
		public override string GUID => "wildfrost";

		public override string[] Depends => new string[0];

		public override string Title => "wildfrost";

		public override string Description => "wildfrost";

		internal InternalMod(string modDirectory)
			: base(modDirectory)
		{
		}
	}
}
