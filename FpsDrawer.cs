#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class FpsDrawer : MonoBehaviour
{
	public TextMeshProUGUI fpsText;

	[SerializeField]
	public bool @default = true;

	public float deltaTime;

	public void Awake()
	{
		if (!Settings.Load("ShowFps", @default))
		{
			base.gameObject.SetActive(value: false);
		}

		Events.OnSettingChanged += SettingChanged;
	}

	public void OnDestroy()
	{
		Events.OnSettingChanged -= SettingChanged;
	}

	public void SettingChanged(string key, object value)
	{
		if (key == "ShowFps" && value is bool)
		{
			bool active = (bool)value;
			base.gameObject.SetActive(active);
		}
	}

	public void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		float f = 1f / deltaTime;
		fpsText.text = $"{Mathf.Ceil(f)}\n{Random.seed}";
	}
}
