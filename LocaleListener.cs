#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleListener : MonoBehaviour
{
	[SerializeField]
	public UnityEvent<Locale> OnLocaleChanged;

	public void OnEnable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged += LocaleChanged;
	}

	public void OnDisable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged -= LocaleChanged;
	}

	public void LocaleChanged(Locale locale)
	{
		OnLocaleChanged?.Invoke(locale);
	}
}
