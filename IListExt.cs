#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public static class IListExt
{
	public static SaveCollection<string> ToSaveCollectionOfNames<T>(this IList<T> list) where T : Object
	{
		return new SaveCollection<string>(list.ToArrayOfNames());
	}

	public static void DestroyAllAndClear(this IList<GameObject> list)
	{
		foreach (GameObject item in list)
		{
			item.Destroy();
		}

		list.Clear();
	}
}
