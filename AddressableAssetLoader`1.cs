#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class AddressableAssetLoader<T> : MonoBehaviour
{
	[SerializeField]
	public bool instant = true;

	[SerializeField]
	public bool onEnable = true;

	[SerializeField]
	public bool releaseOnDisable = true;

	public AsyncOperationHandle<T> operation;

	public bool loaded;

	public void OnEnable()
	{
		if (onEnable)
		{
			Load();
		}
	}

	public void OnDisable()
	{
		if (releaseOnDisable)
		{
			Release();
		}
	}

	public void OnDestroy()
	{
		if (!releaseOnDisable)
		{
			Release();
		}
	}

	public void Release()
	{
		if (operation.IsValid())
		{
			Addressables.Release(operation);
		}

		loaded = false;
	}

	public virtual void Load()
	{
	}

	public AddressableAssetLoader()
	{
	}
}
