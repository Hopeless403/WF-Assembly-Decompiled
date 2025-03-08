using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetaJester.EnumGenerator
{
	public class EnumCreator : MonoBehaviour
	{
		[Serializable]
		public struct EnumValRef
		{
			public string enumName;

			public string enumVal;

			public int enumIntVal;
		}

		public bool isPerScene = true;

		public static char whiteSpaceReplacement = '_';

		public string namespaceName = "ExampleTeam";

		[Tooltip("Must start with Assets/")]
		public string filePathOverride = "Assets/";

		public EnumInfo[] enumInfo;

		public GameObject[] enumContainers;

		public List<EnumValRef> createdValues = new List<EnumValRef>();

		public static T StringToEnum<T>(string value, T defaultValue) where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			string text = value.Replace(' ', whiteSpaceReplacement);
			foreach (T value2 in Enum.GetValues(typeof(T)))
			{
				if (value2.ToString().ToLower().Equals(text.Trim().ToLower()))
				{
					return value2;
				}
			}
			return defaultValue;
		}

		public static string EnumToString<T>(T value)
		{
			return Enum.GetName(typeof(T), value).Replace(whiteSpaceReplacement, ' ');
		}
	}
}
