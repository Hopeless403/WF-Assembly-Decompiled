#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class SetSettingInt : MonoBehaviour
{
	[SerializeField]
	public Setting<int> setting;

	[SerializeField]
	public string key = "Language";

	[SerializeField]
	public int defaultValue;

	public string Key => key;

	public void OnEnable()
	{
		if (setting != null)
		{
			setting.SetValue(Settings.Load(key, defaultValue));
		}
	}

	public void Set(int value)
	{
		Settings.Save(key, value);
	}
}
