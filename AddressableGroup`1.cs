#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class AddressableGroup<T> : IEnumerable<T>, IEnumerable, IDisposable where T : UnityEngine.Object
{
	public readonly List<AsyncOperationHandle<T>> handles;

	public bool subbed;

	public AddressableGroup(params AssetReferenceT<T>[] assetRefs)
	{
		handles = new List<AsyncOperationHandle<T>>();
		foreach (AssetReferenceT<T> assetReferenceT in assetRefs)
		{
			handles.Add(assetReferenceT.LoadAssetAsync());
		}
	}

	public void WaitForCompletion()
	{
		foreach (AsyncOperationHandle<T> handle in handles)
		{
			handle.WaitForCompletion();
		}
	}

	public void Dispose()
	{
		Release();
		if (subbed)
		{
			Events.OnSceneChanged -= SceneChanged;
		}
	}

	public void DisposeOnSceneChange()
	{
		Events.OnSceneChanged += SceneChanged;
		subbed = true;
	}

	public void SceneChanged(Scene scene)
	{
		Dispose();
	}

	public void Release()
	{
		foreach (AsyncOperationHandle<T> handle in handles)
		{
			Addressables.Release(handle);
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return handles.Select(delegate(AsyncOperationHandle<T> a)
		{
			a.WaitForCompletion();
			return a.Result;
		}).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return handles.GetEnumerator();
	}
}
