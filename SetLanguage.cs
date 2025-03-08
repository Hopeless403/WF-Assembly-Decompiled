#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SetLanguage : MonoBehaviour
{
	[SerializeField]
	public TMP_Dropdown dropdown;

	[SerializeField]
	public Setting<int> setting;

	public bool init;

	public IEnumerator Start()
	{
		yield return LocalizationSettings.InitializationOperation;
		dropdown.options = LocalizationSettings.AvailableLocales.Locales.Select((Locale locale) => new TMP_Dropdown.OptionData(locale.name)).ToList();
		init = true;
		OnEnable();
	}

	public void OnEnable()
	{
		if (init && setting != null)
		{
			int value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
			setting.SetValue(value);
		}
	}

	public void Set(int value)
	{
		Locale locale = LocalizationSettings.AvailableLocales.Locales[value];
		Settings.Save("Language", locale.Identifier.Code);
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
	}
}
