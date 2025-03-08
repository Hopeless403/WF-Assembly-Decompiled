#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.IO;
using FMODUnity;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ErrorHandlerSystem : GameSystem
{
	[SerializeField]
	public bool displayErrors = true;

	[SerializeField]
	[ShowIf("displayErrors")]
	public GameObject errorDisplay;

	[SerializeField]
	[ShowIf("displayErrors")]
	public TMP_InputField errorText;

	[SerializeField]
	[ShowIf("displayErrors")]
	public bool freezeTimeScale = true;

	[SerializeField]
	[ShowIf("displayErrors")]
	public EventReference sfxEvent;

	[SerializeField]
	public bool showPersistentMessage = true;

	[SerializeField]
	[ShowIf("showPersistentMessage")]
	public GameObject persistentMessage;

	public const string format = "\n\n[{0}] {1}\n{2}";

	public float timeScalePre = 1f;

	public static int errorCount;

	public static string path => Application.persistentDataPath + "/Errors.log";

	public void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	public void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	public void HandleLog(string log, string stacktrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			errorCount++;
			using (StreamWriter streamWriter = new StreamWriter(path, append: true))
			{
				streamWriter.WriteLine("\n\n[{0}] {1}\n{2}", DateTime.Now, log, stacktrace);
			}
			if (displayErrors)
			{
				ShowError(log + "\n" + stacktrace);
			}
			else if (showPersistentMessage)
			{
				ShowPersistentMessage();
			}
		}
	}

	public void ShowError(string text)
	{
		errorDisplay.SetActive(value: true);
		errorText.text = text;
		if (freezeTimeScale)
		{
			timeScalePre = Time.timeScale;
			Time.timeScale = 0f;
		}

		SfxSystem.OneShot(sfxEvent.Guid);
	}

	public void HideError()
	{
		errorDisplay.SetActive(value: false);
		Time.timeScale = timeScalePre;
		if (showPersistentMessage && errorCount > 0)
		{
			ShowPersistentMessage();
		}
	}

	public void ExitGame()
	{
		GameManager.Quit();
	}

	public void ShowPersistentMessage()
	{
		persistentMessage.SetActive(value: true);
	}

	public void HidePersistentMessage()
	{
		persistentMessage.SetActive(value: false);
	}
}
