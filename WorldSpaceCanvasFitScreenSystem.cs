#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSpaceCanvasFitScreenSystem : GameSystem
{
	public static WorldSpaceCanvasFitScreenSystem instance;

	public static bool exists;

	[Header("Screen Size")]
	[ReadOnly]
	public int screenWidth;

	[ReadOnly]
	public int screenHeight;

	[ReadOnly]
	public float aspectRatio;

	[Header("UI Size")]
	[ReadOnly]
	public Rect safeArea;

	public const float fixedHeight = 11.547f;

	public const float maxWidth = 26.943f;

	public const float minWidth = 17.32051f;

	public const float minAspect = 1.5f;

	public const float maxAspect = 2.33333325f;

	public static readonly List<WorldSpaceCanvasUpdater> canvases = new List<WorldSpaceCanvasUpdater>();

	public static readonly Vector3 camPosition = new Vector3(0f, 0f, -10f);

	public ScreenOrientation lastOrientation;

	public Rect lastSafeArea;

	public Camera cam { get; set; }

	public static float AspectRatio
	{
		get
		{
			if (!exists)
			{
				return 1.77777779f;
			}

			return Mathf.Min(2.33333325f, instance.aspectRatio);
		}
	}

	public void OnEnable()
	{
		instance = this;
		exists = true;
		Events.OnSceneChanged += SceneChanged;
		UpdateSize();
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
	}

	public void SceneChanged(Scene scene)
	{
		UpdateSize();
	}

	public void Update()
	{
		if (!cam)
		{
			cam = Camera.main;
			if ((bool)cam)
			{
				UpdateSize();
			}
		}
		else if (cam.scaledPixelWidth != screenWidth || cam.scaledPixelHeight != screenHeight)
		{
			UpdateSize();
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void UpdateSize()
	{
		lastOrientation = Screen.orientation;
		lastSafeArea = Screen.safeArea;
		Debug.Log($"[{this}] UPDATING ~World Space~ CANVAS SIZE");
		if (!cam)
		{
			cam = Camera.main;
		}

		screenWidth = cam.scaledPixelWidth;
		screenHeight = cam.scaledPixelHeight;
		aspectRatio = cam.aspect;
		Debug.Log($"New Size: ({screenWidth}, {screenHeight}) Aspect: {aspectRatio}");
		int num = screenWidth;
		Rect rect;
		if (aspectRatio > 2.33333325f)
		{
			num = Mathf.RoundToInt((float)screenHeight * 2.33333325f);
			Debug.Log($"OVER MAX ASPECT RATIO. Actual Size: ({num}, {screenHeight}) Aspect: {(float)num / (float)screenHeight}");
			rect = new Rect(Screen.safeArea.x, Screen.safeArea.y, Screen.safeArea.width, Screen.safeArea.height);
			if (rect.width / rect.height > 2.33333325f)
			{
				float num2 = rect.height * 2.33333325f;
				float num3 = rect.width - num2;
				rect.x = Mathf.Max(0f, rect.x - num3);
				rect.width = num2;
			}
		}
		else
		{
			rect = Screen.safeArea;
		}

		safeArea = new Rect
		{
			x = rect.x / (float)num,
			y = rect.y / (float)screenHeight,
			width = rect.width / (float)num,
			height = rect.height / (float)screenHeight
		};
		Debug.Log($"UI Safe Area: {rect}");
		foreach (WorldSpaceCanvasUpdater canvase in canvases)
		{
			canvase.UpdateSize();
		}
	}

	public static void Register(WorldSpaceCanvasUpdater canvas)
	{
		canvases.Add(canvas);
	}

	public static void Unregister(WorldSpaceCanvasUpdater canvas)
	{
		canvases.Remove(canvas);
	}
}
