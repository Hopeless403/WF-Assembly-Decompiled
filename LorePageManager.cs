#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LorePageManager : MonoBehaviour
{
	[SerializeField]
	public GameObject focusLayer;

	[SerializeField]
	public Transform focusLayerPageHolder;

	[SerializeField]
	public AssetReferenceT<JournalPageData>[] pages;

	[SerializeField]
	public AddressableReleaser assetReleaser;

	[SerializeField]
	public EventReference selectSfxEvent;

	[SerializeField]
	public EventReference deselectSfxEvent;

	[SerializeField]
	public EventReference newSfxEvent;

	[SerializeField]
	public SfxLoop newLoop;

	public readonly List<GameObject> pageObjects = new List<GameObject>();

	public LorePage current;

	public Canvas focus;

	public bool focusLayerActive;

	public List<string> inspected;

	public int newCount;

	public void OnEnable()
	{
		inspected = SaveSystem.LoadProgressData<List<string>>("lorePagesInspected");
		if (inspected == null)
		{
			inspected = new List<string>();
		}

		Populate();
	}

	public void OnDisable()
	{
		foreach (GameObject pageObject in pageObjects)
		{
			pageObject.Destroy();
		}

		pageObjects.Clear();
		DisableFocusLayer();
	}

	public void Populate()
	{
		newCount = 0;
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		AssetReferenceT<JournalPageData>[] array = pages;
		for (int i = 0; i < array.Length; i++)
		{
			AsyncOperationHandle<JournalPageData> asyncOperationHandle = array[i].LoadAssetAsync();
			JournalPageData journalPageData = asyncOperationHandle.WaitForCompletion();
			AsyncOperationHandle<GameObject> asyncOperationHandle2 = journalPageData.prefabRef.InstantiateAsync(base.transform);
			LorePage component = asyncOperationHandle2.WaitForCompletion().GetComponent<LorePage>();
			component.manager = this;
			pageObjects.Add(component.gameObject);
			if (unlockedList.Contains(journalPageData.unlock.name))
			{
				bool flag = !inspected.Contains(journalPageData.unlock.name);
				component.SetUnlocked(journalPageData, value: true);
				component.SetNew(flag);
				if (flag)
				{
					newCount++;
				}
			}

			assetReleaser.Add(asyncOperationHandle2);
			assetReleaser.Add(asyncOperationHandle);
		}

		if (newCount > 0)
		{
			SfxSystem.OneShot(newSfxEvent);
			newLoop.Play();
		}
	}

	public void Select(LorePage lorePage)
	{
		if (focusLayerActive || !lorePage.isUnlocked)
		{
			return;
		}

		current = lorePage;
		focus = lorePage.canvas;
		SfxSystem.OneShot(selectSfxEvent);
		if (lorePage.isNew)
		{
			lorePage.SetNew(value: false);
			inspected.Add(lorePage.pageData.unlock.name);
			SaveSystem.SaveProgressData("lorePagesInspected", inspected);
			if (--newCount <= 0)
			{
				newLoop.Stop();
			}
		}

		focusLayerActive = true;
		focusLayer.SetActive(value: true);
		focus.overrideSorting = true;
		focus.sortingLayerName = "PauseMenu";
		focus.sortingOrder = 11;
		focus.transform.SetParent(focusLayerPageHolder);
		LeanTween.cancel(focus.gameObject);
		LeanTween.moveLocal(focus.gameObject, Vector3.zero, 0.3f).setEaseOutQuint().setIgnoreTimeScale(useUnScaledTime: true);
		LeanTween.scale(focus.gameObject, Vector3.one, 0.3f).setEaseOutQuint().setIgnoreTimeScale(useUnScaledTime: true);
		LeanTween.rotateLocal(focus.gameObject, Vector3.zero, 0.6f).setEaseOutBack().setIgnoreTimeScale(useUnScaledTime: true);
	}

	public void DisableFocusLayer()
	{
		if (focusLayerActive)
		{
			SfxSystem.OneShot(deselectSfxEvent);
			focus.overrideSorting = false;
			focus.sortingOrder = 1;
			focus.transform.SetParent(current.button.transform);
			LeanTween.cancel(focus.gameObject);
			LeanTween.moveLocal(focus.gameObject, Vector3.zero, 0.1f).setEaseOutQuad().setIgnoreTimeScale(useUnScaledTime: true);
			LeanTween.scale(focus.gameObject, Vector3.one, 0.1f).setEaseOutQuad().setIgnoreTimeScale(useUnScaledTime: true);
			LeanTween.rotateLocal(focus.gameObject, Vector3.zero, 0.1f).setEaseOutQuad().setIgnoreTimeScale(useUnScaledTime: true);
			focusLayerActive = false;
			focusLayer.SetActive(value: false);
			current = null;
			focus = null;
		}
	}
}
