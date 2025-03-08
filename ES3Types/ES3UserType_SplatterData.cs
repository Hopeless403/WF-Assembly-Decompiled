using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "sprite", "color", "offset", "scale", "angle" })]
	public class ES3UserType_SplatterData : ES3ObjectType
	{
		public static ES3Type Instance;

		public ES3UserType_SplatterData()
			: base(typeof(SplatterPersistenceSystem.SplatterData))
		{
			Instance = this;
			priority = 1;
		}

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			SplatterPersistenceSystem.SplatterData objectContainingField = (SplatterPersistenceSystem.SplatterData)obj;
			writer.WritePrivateFieldByRef("sprite", objectContainingField);
			writer.WritePrivateField("color", objectContainingField);
			writer.WritePrivateField("offset", objectContainingField);
			writer.WritePrivateField("scale", objectContainingField);
			writer.WritePrivateField("angle", objectContainingField);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			SplatterPersistenceSystem.SplatterData objectContainingField = (SplatterPersistenceSystem.SplatterData)obj;
			IEnumerator enumerator = reader.Properties.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					switch ((string)enumerator.Current)
					{
					case "sprite":
						reader.SetPrivateField("sprite", reader.Read<Sprite>(), objectContainingField);
						break;
					case "color":
						reader.SetPrivateField("color", reader.Read<Color>(), objectContainingField);
						break;
					case "offset":
						reader.SetPrivateField("offset", reader.Read<Vector3>(), objectContainingField);
						break;
					case "scale":
						reader.SetPrivateField("scale", reader.Read<Vector3>(), objectContainingField);
						break;
					case "angle":
						reader.SetPrivateField("angle", reader.Read<float>(), objectContainingField);
						break;
					default:
						reader.Skip();
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			SplatterPersistenceSystem.SplatterData splatterData = new SplatterPersistenceSystem.SplatterData();
			ReadObject<T>(reader, splatterData);
			return splatterData;
		}
	}
}
