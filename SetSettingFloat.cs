#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class SetSettingFloat : MonoBehaviour
{
	[SerializeField]
	public Setting<float> setting;

	[SerializeField]
	public string key = "ScreenShake";

	[SerializeField]
	public float defaultValue = 1f;

	public void OnEnable()
	{
		if (setting != null)
		{
			setting.SetValue(Settings.Load(key, defaultValue));
		}
	}

	public void Set(float value)
	{
		Settings.Save(key, value);
	}
}
