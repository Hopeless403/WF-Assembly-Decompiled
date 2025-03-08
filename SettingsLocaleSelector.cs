#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[Serializable]
public class SettingsLocaleSelector : IStartupLocaleSelector
{
	[SerializeField]
	public string settingsKey = "Language";

	public Locale GetStartupLocale(ILocalesProvider availableLocales)
	{
		if (Settings.Exists(settingsKey))
		{
			string text = Settings.Load(settingsKey, "");
			if (!text.IsNullOrWhitespace())
			{
				return availableLocales.GetLocale(text);
			}
		}

		return null;
	}
}
