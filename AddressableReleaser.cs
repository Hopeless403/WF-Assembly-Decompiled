#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableReleaser : MonoBehaviour
{
	[SerializeField]
	public bool releaseOnDisable = true;

	[SerializeField]
	[HideIf("releaseOnDisable")]
	public bool releaseOnDestroy;

	public readonly List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();

	[field: SerializeField]
	public int assetsToRelease { get; set; }

	public void Add(AsyncOperationHandle handle)
	{
		handles.Add(handle);
		assetsToRelease++;
	}

	public void OnDisable()
	{
		if (releaseOnDisable)
		{
			ReleaseAll();
		}
	}

	public void OnDestroy()
	{
		if (releaseOnDestroy)
		{
			ReleaseAll();
		}
	}

	public void ReleaseAll()
	{
		foreach (AsyncOperationHandle handle in handles)
		{
			Debug.Log($"→ Asset Releaser Releasing Asset: {handle.Result}");
			Addressables.Release(handle);
		}

		handles.Clear();
		assetsToRelease = 0;
	}
}
