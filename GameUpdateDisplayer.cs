#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class GameUpdateDisplayer : MonoBehaviour
{
	[Serializable]
	public struct Update
	{
		public string buildNumber;

		public UnityEngine.Localization.LocalizedString titleRef;

		public UnityEngine.Localization.LocalizedString bodyRef;

		public float panelHeight;
	}

	[SerializeField]
	public GameObject display;

	[SerializeField]
	public SmoothScrollRect scrollRect;

	[SerializeField]
	public LocalizeStringEvent titleEvent;

	[SerializeField]
	public LocalizeStringEvent bodyEvent;

	[SerializeField]
	public Update[] updates;

	[Header("Beta Version")]
	[SerializeField]
	public Update beta;

	public void OnEnable()
	{
		display.SetActive(value: false);
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
	}

	public void SceneChanged(Scene scene)
	{
		if (scene.name == "MainMenu")
		{
			Check();
		}
	}

	public void Check()
	{
		VersionCompatibility.GetVersions();
		foreach (Update item in updates.Reverse())
		{
			if (VersionCompatibility.currentVersion == item.buildNumber && int.TryParse(VersionCompatibility.currentVersion, out var result))
			{
				if (int.TryParse(VersionCompatibility.previousVersion, out var result2) && result2 > 0 && result2 < result)
				{
					StartCoroutine(ShowRoutine(item));
				}
				else
				{
					Hide();
				}

				return;
			}
		}

		Hide();
	}

	public IEnumerator ShowRoutine(Update update)
	{
		display.SetActive(value: true);
		titleEvent.StringReference = update.titleRef;
		bodyEvent.StringReference = update.bodyRef;
		if (scrollRect.transform is RectTransform rectTransform)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.WithY(update.panelHeight);
		}

		yield return new WaitForSeconds(0.35f);
		scrollRect.ScrollToTop();
	}

	public void Hide()
	{
		display.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}
}
