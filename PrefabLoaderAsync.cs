#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PrefabLoaderAsync : MonoBehaviour
{
	[SerializeField]
	public bool onAwake;

	[ShowIf("onAwake")]
	[SerializeField]
	public AssetReferenceGameObject prefabRef;

	[ShowIf("onAwake")]
	[SerializeField]
	public UnityEvent<GameObject> onComplete;

	public AsyncOperationHandle<GameObject> handle;

	public bool busy;

	public void Awake()
	{
		if (onAwake)
		{
			StartCoroutine(Load(prefabRef));
		}
	}

	public IEnumerator Load(AssetReferenceGameObject prefabRef)
	{
		while (busy)
		{
			yield return null;
		}

		busy = true;
		if (handle.IsValid())
		{
			Addressables.Release(handle);
		}

		handle = prefabRef.InstantiateAsync(base.transform);
		yield return handle;
		onComplete?.Invoke(handle.Result);
		busy = false;
	}

	public void OnDestroy()
	{
		if (handle.IsValid())
		{
			Addressables.Release(handle);
		}
	}
}
