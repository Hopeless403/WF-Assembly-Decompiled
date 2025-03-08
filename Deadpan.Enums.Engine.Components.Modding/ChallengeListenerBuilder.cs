namespace Deadpan.Enums.Engine.Components.Modding
{
	public class ChallengeListenerBuilder : DataFileBuilder<ChallengeListener, ChallengeListenerBuilder>
	{
		public ChallengeListenerBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public ChallengeListenerBuilder()
		{
		}

		public ChallengeListenerBuilder WithKey(string key)
		{
			_data.key = key;
			_data.hasKey = true;
			return this;
		}

		public ChallengeListenerBuilder WithCheckType(ChallengeListener.CheckType type)
		{
			_data.checkType = type;
			return this;
		}

		public ChallengeListenerBuilder WithStat(string stat)
		{
			_data.stat = stat;
			return this;
		}

		public ChallengeListenerBuilder WithStat(int toReach)
		{
			_data.target = toReach;
			return this;
		}
	}
}
