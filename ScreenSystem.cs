#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

public class ScreenSystem : GameSystem
{
	public static ScreenSystem instance;

	public int windowedWidth = 1920;

	public int windowedHeight = 1080;

	public FullScreenMode windowedMode = FullScreenMode.Windowed;

	public FullScreenMode fullMode = FullScreenMode.FullScreenWindow;

	public int _displayIndex;

	public int current;

	public int vsync;

	public int targetFramerate;

	public int fullScreenWidth => display.systemWidth;

	public int fullScreenHeight => display.systemHeight;

	public int displayIndex
	{
		get
		{
			if (_displayIndex >= Display.displays.Length)
			{
				_displayIndex = Display.displays.Length - 1;
			}

			return _displayIndex;
		}
	}

	public Display display => Display.displays[displayIndex];

	public static bool IsWindowed
	{
		get
		{
			if (Screen.fullScreenMode != FullScreenMode.Windowed)
			{
				return Screen.fullScreenMode == FullScreenMode.MaximizedWindow;
			}

			return true;
		}
	}

	public void OnEnable()
	{
		instance = this;
		switch (Screen.fullScreenMode)
		{
			case FullScreenMode.MaximizedWindow:
			case FullScreenMode.Windowed:
				current = 0;
				break;
			case FullScreenMode.ExclusiveFullScreen:
				current = 1;
				break;
			case FullScreenMode.FullScreenWindow:
				current = 2;
				break;
		}

		Events.OnSettingChanged += SettingChanged;
		int num = Settings.Load("DisplayMode", 2);
		if (num != current)
		{
			Set(num);
		}

		vsync = Settings.Load("Vsync", 1);
		SetVsync(vsync);
		targetFramerate = Settings.Load("TargetFramerate", 2);
		SetTargetFramerate(targetFramerate);
	}

	public void OnDisable()
	{
		Events.OnSettingChanged -= SettingChanged;
	}

	public void SettingChanged(string key, object value)
	{
		if (value is int)
		{
			int mode = (int)value;
			switch (key)
			{
				case "DisplayMode":
					Set(mode);
					break;
				case "TargetFramerate":
					SetTargetFramerate(mode);
					break;
				case "Vsync":
					SetVsync(mode);
					break;
			}
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.LeftAlt))
		{
			if (IsWindowed)
			{
				Settings.Save("DisplayMode", (fullMode == FullScreenMode.ExclusiveFullScreen) ? 1 : 2);
			}
			else
			{
				Settings.Save("DisplayMode", 0);
			}

			SetSettingInt setSettingInt = Object.FindObjectsOfType<SetSettingInt>().FirstOrDefault((SetSettingInt a) => a.Key == "DisplayMode");
			if ((object)setSettingInt != null)
			{
				setSettingInt.enabled = false;
				setSettingInt.enabled = true;
			}
		}
	}

	public void Set(int mode)
	{
		current = mode;
		switch (mode.Mod(3))
		{
			case 0:
				SetWindowed();
				break;
			case 1:
				SetFull();
				break;
			case 2:
				SetBorderless();
				break;
		}
	}

	public void SetWindowed(int forceWidth = 0, int forceHeight = 0)
	{
		Debug.Log("Screen Mode: Windowed");
		if (!IsWindowed)
		{
			fullMode = Screen.fullScreenMode;
		}

		Screen.fullScreenMode = windowedMode;
		int width = ((forceWidth > 0) ? forceWidth : windowedWidth);
		int height = ((forceHeight > 0) ? forceHeight : windowedHeight);
		Screen.SetResolution(width, height, windowedMode);
	}

	public void SetFull(int forceWidth = 0, int forceHeight = 0)
	{
		Debug.Log("Screen Mode: Fullscreen");
		if (IsWindowed)
		{
			windowedWidth = Screen.width;
			windowedHeight = Screen.height;
			windowedMode = Screen.fullScreenMode;
		}

		Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
		int width = ((forceWidth > 0) ? forceWidth : fullScreenWidth);
		int height = ((forceHeight > 0) ? forceHeight : fullScreenHeight);
		Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
	}

	public void SetBorderless(int forceWidth = 0, int forceHeight = 0)
	{
		Debug.Log("Screen Mode: Borderless");
		if (IsWindowed)
		{
			windowedWidth = Screen.width;
			windowedHeight = Screen.height;
			windowedMode = Screen.fullScreenMode;
		}

		Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
		int width = ((forceWidth > 0) ? forceWidth : fullScreenWidth);
		int height = ((forceHeight > 0) ? forceHeight : fullScreenHeight);
		Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
	}

	public static void SetTargetFramerate(int mode)
	{
		mode = Mathf.Clamp(mode, 0, 4);
		int targetFrameRate;
		switch (mode)
		{
			case 0:
				targetFrameRate = -1;
				break;
			case 1:
				targetFrameRate = 30;
				break;
			case 2:
				targetFrameRate = 60;
				break;
			case 3:
				targetFrameRate = 120;
				break;
			case 4:
				targetFrameRate = 240;
				break;
			default:
				targetFrameRate = Application.targetFrameRate;
				break;
		}

		Application.targetFrameRate = targetFrameRate;
	}

	public static void SetVsync(int mode)
	{
		QualitySettings.vSyncCount = Mathf.Clamp(mode, 0, 1);
	}

	public static void SetResolutionFullscreen(int width, int height)
	{
		instance.SetFull(width, height);
	}

	public static void SetResolutionBorderless(int width, int height)
	{
		instance.SetBorderless(width, height);
	}

	public static void SetResolutionWindowed(int width, int height)
	{
		instance.SetWindowed(width, height);
	}
}
