#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizeActionString : MonoBehaviour
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString mouseString;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString joystickString;

	[SerializeField]
	public UnityEvent<string> onUpdate;

	[SerializeField]
	public bool preferTextActions;

	public void OnEnable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged += LocaleChanged;
		Events.OnButtonStyleChanged += ButtonStyleChanged;
		UpdateText();
	}

	public void OnDisable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged -= LocaleChanged;
		Events.OnButtonStyleChanged -= ButtonStyleChanged;
	}

	public void UpdateText()
	{
		string arg = ControllerButtonSystem.ProcessActionTags(MonoBehaviourSingleton<Cursor3d>.instance.usingMouse ? mouseString : joystickString, preferTextActions);
		onUpdate?.Invoke(arg);
	}

	public void LocaleChanged(Locale locale)
	{
		UpdateText();
	}

	public void ButtonStyleChanged()
	{
		UpdateText();
	}
}
