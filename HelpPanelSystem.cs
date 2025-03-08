#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class HelpPanelSystem : GameSystem
{
	[Serializable]
	public struct ButtonProfile
	{
		[SerializeField]
		public ButtonType type;

		[SerializeField]
		public GameObject prefab;
	}

	public enum ButtonType
	{
		Positive,
		Negative
	}

	public static HelpPanelSystem _instance;

	public static GameObject _rtCamera;

	[SerializeField]
	public TMP_Text title;

	[SerializeField]
	public TMP_Text body;

	[SerializeField]
	public TMP_Text note;

	[SerializeField]
	public ImageSprite image;

	[SerializeField]
	public LayoutElement imageLayout;

	[Header("Button Options")]
	[SerializeField]
	public Transform buttonGroup;

	[SerializeField]
	public GameObject backButton;

	[SerializeField]
	public ButtonProfile[] buttonPrefabs;

	public static readonly Dictionary<ButtonType, ButtonProfile> buttonProfileLookup = new Dictionary<ButtonType, ButtonProfile>();

	[Header("Emotes")]
	[SerializeField]
	public Image emote;

	[SerializeField]
	public Prompt.Emote[] emoteTypes;

	[Header("SFX")]
	[SerializeField]
	public EventReference popUpSfx;

	public static bool Active;

	public static HelpPanelSystem instance => _instance ?? (_instance = UnityEngine.Object.FindObjectOfType<HelpPanelSystem>(includeInactive: true));

	public static GameObject rtCamera => _rtCamera ?? (_rtCamera = UnityEngine.Object.FindObjectsOfType<Camera>(includeInactive: true).First((Camera a) => a.name == "RenderTextureCamera").gameObject);

	public void Awake()
	{
		buttonProfileLookup.Clear();
		ButtonProfile[] array = buttonPrefabs;
		for (int i = 0; i < array.Length; i++)
		{
			ButtonProfile value = array[i];
			buttonProfileLookup[value.type] = value;
		}
	}

	public void OnEnable()
	{
		Active = true;
		rtCamera.SetActive(value: true);
	}

	public void OnDisable()
	{
		Active = false;
		rtCamera.SetActive(value: false);
		foreach (Transform item in buttonGroup)
		{
			item.gameObject.Destroy();
		}

		imageLayout.gameObject.SetActive(value: false);
	}

	public static void SetBackButtonActive(bool active)
	{
		instance.backButton.SetActive(active);
	}

	public static void Show(UnityEngine.Localization.LocalizedString key)
	{
		string[] array = key.GetLocalizedString().Split('|');
		instance.title.gameObject.SetActive(array.Length != 0);
		instance.body.gameObject.SetActive(array.Length > 1);
		instance.note.gameObject.SetActive(array.Length > 2);
		instance.title.text = ((array.Length != 0) ? array[0] : "");
		instance.body.text = ((array.Length > 1) ? array[1] : "");
		instance.note.text = ((array.Length > 2) ? array[2] : "");
		SfxSystem.OneShot(instance.popUpSfx);
		instance.gameObject.SetActive(value: true);
	}

	public static void SetEmote(Prompt.Emote.Type emoteType)
	{
		Prompt.Emote emote = instance.emoteTypes.FirstOrDefault((Prompt.Emote a) => a.type == emoteType);
		instance.emote.sprite = emote.sprite;
	}

	public static void AddButton(ButtonType type, UnityEngine.Localization.LocalizedString textKey, string hotKey, UnityAction onSelect)
	{
		if (buttonProfileLookup.TryGetValue(type, out var value))
		{
			GameObject obj = UnityEngine.Object.Instantiate(value.prefab, instance.buttonGroup);
			obj.GetComponent<RewiredHotKeyController>()?.SetActionName(hotKey);
			Button componentInChildren = obj.GetComponentInChildren<Button>();
			if (onSelect != null)
			{
				componentInChildren.onClick.AddListener(onSelect);
			}

			componentInChildren.onClick.AddListener(delegate
			{
				instance.gameObject.SetActive(value: false);
			});
			obj.GetComponentInChildren<LocalizeStringEvent>().StringReference = textKey;
		}
	}

	public static void SetImage(float width, float height, Sprite sprite)
	{
		instance.imageLayout.gameObject.SetActive(value: true);
		instance.image.SetSprite(sprite);
		instance.imageLayout.preferredWidth = width;
		instance.imageLayout.preferredHeight = height;
	}
}
