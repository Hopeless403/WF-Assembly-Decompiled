namespace NexPlugin
{
	public class StationURL
	{
		public enum URLType
		{
			unknown,
			prudp,
			prudps,
			udp
		}

		public enum Flags
		{
			BehindNAT = 1,
			Public = 2,
			DetectedByNatCheck = 4,
			DetectedByNgs = 8
		}
	}
}
