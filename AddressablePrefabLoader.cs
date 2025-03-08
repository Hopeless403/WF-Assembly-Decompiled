#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class AddressablePrefabLoader : AddressableAssetLoader<GameObject>
{
	[SerializeField]
	public AssetReferenceGameObject prefabRef;

	[SerializeField]
	public bool setChildIndex;

	[SerializeField]
	[ShowIf("setChildIndex")]
	public int childIndex;

	[SerializeField]
	public UnityEvent<GameObject> onLoad;

	public override void Load()
	{
		if (loaded)
		{
			return;
		}

		operation = prefabRef.InstantiateAsync(base.transform);
		if (instant)
		{
			operation.WaitForCompletion();
			Loaded();
		}
		else
		{
			operation.Completed += delegate
			{
				Loaded();
			};
		}

		loaded = true;
	}

	public void Loaded()
	{
		if (setChildIndex)
		{
			operation.Result.transform.SetSiblingIndex(childIndex);
		}

		onLoad?.Invoke(operation.Result);
	}
}
