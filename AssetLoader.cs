#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
	[Serializable]
	public class Group
	{
		public string name;

		public UnityEngine.Object[] assets;

		public Dictionary<string, UnityEngine.Object> lookup;
	}

	public Group[] groups;

	public static Dictionary<string, Group> groupLookup;

	public void Awake()
	{
		Debug.Log("> AssetLoader Loading Resources...");
		StopWatch.Start();
		int num = 0;
		int num2 = 0;
		groupLookup = new Dictionary<string, Group>();
		Group[] array = groups;
		foreach (Group group in array)
		{
			groupLookup[group.name.ToLower()] = group;
			group.lookup = new Dictionary<string, UnityEngine.Object>();
			num++;
			UnityEngine.Object[] assets = group.assets;
			foreach (UnityEngine.Object @object in assets)
			{
				group.lookup[@object.name.ToLower()] = @object;
				num2++;
			}
		}

		Debug.Log($"> {num} Groups");
		Debug.Log($"> {num2} Assets");
		Debug.Log($"> DONE ({StopWatch.Stop()}ms)");
	}

	public static T Lookup<T>(string groupName, string assetName) where T : UnityEngine.Object
	{
		return GetGroup(groupName)?.lookup[assetName.ToLower()] as T;
	}

	public static Group GetGroup(string groupName)
	{
		return groupLookup[groupName.ToLower()];
	}

	public static IEnumerable<T> GetEnumerable<T>(string groupName)
	{
		return GetGroup(groupName)?.assets.Cast<T>();
	}

	public static List<T> GetList<T>(string groupName)
	{
		return GetEnumerable<T>(groupName)?.ToList();
	}

	public static T[] GetArray<T>(string groupName)
	{
		return GetEnumerable<T>(groupName)?.ToArray();
	}
}
