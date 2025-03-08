#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
	public const float CARD_WIDTH = 3f;

	public const float CARD_HEIGHT = 4.5f;

	public static readonly Vector2 CARD_SIZE = new Vector2(3f, 4.5f);

	public const float LARGE_UI = 0f;

	[SerializeField]
	public int targetFrameRate = -1;

	[SerializeField]
	public int editorTargetFrameRate = 60;

	public static int tasksInProgress = 0;

	public static bool init;

	public static bool End;

	public static bool paused;

	public static readonly CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-GB");

	public static bool Busy
	{
		get
		{
			if (tasksInProgress <= 0)
			{
				return !init;
			}

			return true;
		}
	}

	public static bool Ready => init;

	public IEnumerator Start()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo;
		Application.targetFrameRate = targetFrameRate;
		UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		Debug.Log("RELEASE = TRUE");
		yield return null;
		yield return new WaitUntil(() => Bootstrap.Count <= 0);
		init = true;
		Events.InvokeGameStart();
	}

	public void OnApplicationQuit()
	{
		Debug.Log(">>>> GAME END <<<<");
		End = true;
		Events.InvokeGameEnd();
	}

	public static void Quit()
	{
		Application.Quit();
	}
}
