namespace ES3Types
{
	public class ES3UserType_SplatterDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SplatterDataArray()
			: base(typeof(SplatterPersistenceSystem.SplatterData[]), ES3UserType_SplatterData.Instance)
		{
			Instance = this;
		}
	}
}
