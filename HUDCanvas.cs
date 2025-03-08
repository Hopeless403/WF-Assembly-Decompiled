#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HUDCanvas : MonoBehaviour
{
	public CanvasGroup _canvasGroup;

	public CanvasGroup canvasGroup => _canvasGroup ?? (_canvasGroup = GetComponent<CanvasGroup>());

	public void Awake()
	{
		canvasGroup.alpha = Settings.Load("HudAlpha", 1f);
		Events.OnSettingChanged += SettingChanged;
	}

	public void OnDestroy()
	{
		Events.OnSettingChanged -= SettingChanged;
	}

	public void SettingChanged(string key, object value)
	{
		if (key == "HudAlpha" && value is float)
		{
			float alpha = (float)value;
			canvasGroup.alpha = alpha;
		}
	}
}
