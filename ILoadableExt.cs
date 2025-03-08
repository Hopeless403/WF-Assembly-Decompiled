#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public static class ILoadableExt
{
	public static T[] LoadArray<T, TSaveData>(this TSaveData[] data) where TSaveData : ILoadable<T>
	{
		return data.LoadList<T, TSaveData>().ToArray();
	}

	public static List<T> LoadList<T, TSaveData>(this TSaveData[] data) where TSaveData : ILoadable<T>
	{
		List<T> list = new List<T>();
		for (int i = 0; i < data.Length; i++)
		{
			TSaveData val = data[i];
			list.AddIfNotNull(val.Load());
		}

		return list;
	}
}
