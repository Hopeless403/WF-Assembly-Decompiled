#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;

public static class ISaveableExt
{
	public static D[] SaveArray<S, D>(this IEnumerable<S> list) where S : ISaveable<D>
	{
		return list.Select((S a) => a.Save()).ToArray();
	}

	public static D[] SaveArray<S, D>(this S[] array) where S : ISaveable<D>
	{
		return array.Select((S a) => a.Save()).ToArray();
	}
}
