#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{
	public class Group
	{
		public readonly Dictionary<string, DataFile> lookup = new Dictionary<string, DataFile>();

		public readonly List<DataFile> list = new List<DataFile>();

		public T Get<T>(string name) where T : DataFile
		{
			if (!lookup.ContainsKey(name))
			{
				return null;
			}

			return lookup[name] as T;
		}

		public List<T> GetList<T>() where T : DataFile
		{
			return list.Cast<T>().ToList();
		}

		public void Add<T>(T obj) where T : DataFile
		{
			list.Add(obj);
			lookup[GetName(obj)] = obj;
		}

		public void Remove<T>(T obj) where T : DataFile
		{
			list.Remove(obj);
			lookup.Remove(GetName(obj));
		}

		public void AddRange<T>(IEnumerable<T> obj) where T : DataFile
		{
			foreach (T item in obj)
			{
				Add(item);
			}
		}

		public Group(IEnumerable<DataFile> items)
		{
			foreach (DataFile item in items)
			{
				list.Add(item);
				lookup[GetName(item)] = item;
			}
		}

		public static string GetName(DataFile asset)
		{
			if (asset is KeywordData keywordData)
			{
				return keywordData.name.ToLower();
			}

			return asset.name;
		}
	}

	[SerializeField]
	public bool initOnStart;

	public static readonly Dictionary<string, Group> groups = new Dictionary<string, Group>();

	public IEnumerator Start()
	{
		if (initOnStart)
		{
			Debug.Log("Addressables Init");
			AsyncOperationHandle<IResourceLocator> asyncOperationHandle = Addressables.InitializeAsync();
			yield return asyncOperationHandle;
			Debug.Log("Addressables Init Done");
		}
	}

	public static bool IsGroupLoaded(string name)
	{
		return groups.ContainsKey(name);
	}

	public static void ForceLoadGroup(string name)
	{
		if (StartLoadGroup(name, out var handle))
		{
			handle.WaitForCompletion();
			StoreGroup(name, handle.Result);
		}
	}

	public static async Task PreLoadGroup(string name)
	{
		if (!IsGroupLoaded(name))
		{
			StartLoadGroup(name, out var handle);
			await handle.Task;
		}
	}

	public static IEnumerator LoadGroup(string name)
	{
		if (StartLoadGroup(name, out var handle))
		{
			yield return handle;
			StoreGroup(name, handle.Result);
		}
	}

	public static bool StartLoadGroup(string name, out AsyncOperationHandle<IList<DataFile>> handle)
	{
		if (IsGroupLoaded(name))
		{
			Debug.Log("Group [" + name + "] is already loaded!");
			handle = default(AsyncOperationHandle<IList<DataFile>>);
			return false;
		}

		handle = Addressables.LoadAssetsAsync<DataFile>(name, null);
		return true;
	}

	public static void StoreGroup<T>(string name, ICollection<T> data) where T : DataFile
	{
		if (data == null)
		{
			data = new List<T>();
		}

		Group value = new Group(data);
		groups[name] = value;
		Debug.Log($"Group [{name}] loaded! ({data.Count} items)");
	}

	public static List<T> GetGroup<T>(string name) where T : DataFile
	{
		if (!IsGroupLoaded(name))
		{
			ForceLoadGroup(name);
		}

		return groups[name].GetList<T>();
	}

	public static void AddToGroup<T>(string name, T value) where T : DataFile
	{
		if (!IsGroupLoaded(name))
		{
			ForceLoadGroup(name);
		}

		groups[name].Add(value);
	}

	public static void RemoveFromGroup<T>(string name, T value) where T : DataFile
	{
		if (!IsGroupLoaded(name))
		{
			ForceLoadGroup(name);
		}

		groups[name].Remove(value);
	}

	public static void AddRangeToGroup<T>(string name, IEnumerable<T> value) where T : DataFile
	{
		if (!IsGroupLoaded(name))
		{
			ForceLoadGroup(name);
		}

		groups[name].AddRange(value);
	}

	public static T Get<T>(string groupName, string assetName) where T : DataFile
	{
		if (!IsGroupLoaded(groupName))
		{
			ForceLoadGroup(groupName);
		}

		try
		{
			return groups[groupName].Get<T>(assetName);
		}
		catch (Exception ex)
		{
			throw new Exception("[" + assetName + "] does not exist!\n\n" + ex.Message, ex.InnerException);
		}
	}

	public static CardData GetCardDataClone(string cardDataName)
	{
		CardData cardData = Get<CardData>("CardData", cardDataName);
		if (!cardData)
		{
			return MissingCardSystem.GetClone(cardDataName);
		}

		return cardData.Clone();
	}

	public static T Get<T>(string assetName) where T : DataFile
	{
		AsyncOperationHandle<T> asyncOperationHandle = Addressables.LoadAssetAsync<T>(assetName);
		asyncOperationHandle.WaitForCompletion();
		return asyncOperationHandle.Result;
	}

	public static GameObject Get(string assetName)
	{
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(assetName);
		asyncOperationHandle.WaitForCompletion();
		return asyncOperationHandle.Result;
	}

	public static bool TryGet<T>(string groupName, string assetName, out T result) where T : DataFile
	{
		if (!IsGroupLoaded(groupName))
		{
			ForceLoadGroup(groupName);
		}

		result = groups[groupName].Get<T>(assetName);
		return (UnityEngine.Object)result != (UnityEngine.Object)null;
	}

	public static AsyncOperationHandle<GameObject> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		return Addressables.InstantiateAsync(key, position, rotation, parent);
	}

	public static AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetRef, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		return assetRef.InstantiateAsync(position, rotation, parent);
	}
}
