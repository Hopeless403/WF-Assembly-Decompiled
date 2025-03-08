#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class DateTextSetter : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textElement;

	public void OnEnable()
	{
		SetText(0);
	}

	public void SetText(int dayOffset)
	{
		CultureInfo cultureInfo = LocalizationSettings.SelectedLocale.Identifier.CultureInfo;
		DateTime dateTime = DailyFetcher.GetDateTime().AddDays(dayOffset);
		textElement.text = dateTime.ToString("D", cultureInfo);
	}
}
