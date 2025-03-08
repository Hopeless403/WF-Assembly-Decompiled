#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public static class DictionaryExt
{
	public static T[] GetSaveCollection<T>(this Dictionary<string, object> dict, string key)
	{
		return ((SaveCollection<T>)dict[key]).collection;
	}
}
