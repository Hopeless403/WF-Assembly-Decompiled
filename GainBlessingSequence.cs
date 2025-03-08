#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GainBlessingSequence : UISequence
{
	[SerializeField]
	public AssetReferenceGameObject blessingPrefab;

	[SerializeField]
	public Transform blessingGroup;

	[SerializeField]
	public int options = 3;

	[SerializeField]
	public BlessingData[] blessingPool;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public EventReference selectSfxEvent;

	public readonly List<BlessingData> blessings = new List<BlessingData>();

	public override IEnumerator Run()
	{
		List<BlessingData> list = GetBlessings();
		if (list.Count > 0)
		{
			CinemaBarSystem.In();
			CinemaBarSystem.Top.SetPrompt(titleKey.GetLocalizedString(), "Select");
			yield return CreateBlessings(list);
			while (!promptEnd)
			{
				yield return null;
			}

			CinemaBarSystem.Out();
		}
	}

	public List<BlessingData> GetBlessings()
	{
		List<BlessingData> list = new List<BlessingData>();
		BlessingData[] array = blessingPool;
		foreach (BlessingData blessingData in array)
		{
			bool flag = false;
			if (Campaign.Data.Modifiers != null)
			{
				foreach (GameModifierData modifier in Campaign.Data.Modifiers)
				{
					if (blessingData.modifierToAdd.blockedBy.Contains(modifier))
					{
						flag = true;
						break;
					}
				}
			}

			if (!flag)
			{
				list.Add(blessingData);
				if (list.Count >= options)
				{
					break;
				}
			}
		}

		return list;
	}

	public IEnumerator CreateBlessings(IEnumerable<BlessingData> blessingsToCreate)
	{
		foreach (BlessingData item in blessingsToCreate)
		{
			yield return CreateBlessing(item);
		}
	}

	public IEnumerator CreateBlessing(BlessingData blessingData)
	{
		AsyncOperationHandle<GameObject> handle = blessingPrefab.InstantiateAsync(blessingGroup);
		yield return handle;
		handle.Result.GetComponent<BlessingSelect>().SetUp(blessingData, this);
	}

	public void SelectBlessing(BlessingData blessingData)
	{
		if (!promptEnd)
		{
			blessingData.Select();
			promptEnd = true;
			SfxSystem.OneShot(selectSfxEvent);
		}
	}
}
