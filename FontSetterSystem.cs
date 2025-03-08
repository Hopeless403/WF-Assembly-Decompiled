#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class FontSetterSystem : GameSystem
{
	[Serializable]
	public struct LocaleFont
	{
		public string localeCode;

		public AssetReferenceT<TMP_FontAsset> fontRef;
	}

	[SerializeField]
	public AssetReferenceT<TMP_FontAsset> defaultFontRef;

	[SerializeField]
	public LocaleFont[] localeFonts;

	public static AssetReferenceT<TMP_FontAsset> defaultRef;

	public static AssetReferenceT<TMP_FontAsset> currentRef;

	public static TMP_FontAsset current;

	public static Dictionary<string, AssetReferenceT<TMP_FontAsset>> lookup;

	public static readonly List<FontSetter> fontSetters = new List<FontSetter>();

	public void Awake()
	{
		lookup = new Dictionary<string, AssetReferenceT<TMP_FontAsset>>();
		LocaleFont[] array = localeFonts;
		for (int i = 0; i < array.Length; i++)
		{
			LocaleFont localeFont = array[i];
			lookup[localeFont.localeCode] = localeFont.fontRef;
		}

		defaultRef = defaultFontRef;
		LocaleChanged(LocalizationSettings.SelectedLocale);
	}

	public void OnEnable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged += LocaleChanged;
	}

	public void OnDisable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged -= LocaleChanged;
	}

	public static void Register(FontSetter fontSetter)
	{
		fontSetters.Add(fontSetter);
		if (current != null)
		{
			fontSetter.SetFont(current);
		}
	}

	public static void Unregister(FontSetter fontSetter)
	{
		fontSetters.Remove(fontSetter);
	}

	public static void LocaleChanged(Locale locale)
	{
		if (current != null)
		{
			currentRef.ReleaseAsset();
		}

		currentRef = (lookup.ContainsKey(locale.Identifier.Code) ? lookup[locale.Identifier.Code] : defaultRef);
		current = currentRef.LoadAssetAsync().WaitForCompletion();
		UpdateFontSetters();
	}

	public static void UpdateFontSetters()
	{
		foreach (FontSetter fontSetter in fontSetters)
		{
			fontSetter.SetFont(current);
		}
	}
}
