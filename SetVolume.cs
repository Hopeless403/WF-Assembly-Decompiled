#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class SetVolume : MonoBehaviour
{
	[SerializeField]
	public string busName = "Master";

	[SerializeField]
	public Setting<float> setting;

	public void OnEnable()
	{
		if (setting != null)
		{
			setting.SetValue(AudioSettingsSystem.GetVolume(busName));
		}
	}

	public void Set(float value)
	{
		AudioSettingsSystem.Volume(busName, value);
	}
}
