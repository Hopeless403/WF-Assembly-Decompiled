#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.IO;
using UnityEngine;

public class LogSystem : GameSystem
{
	public const int backups = 9;

	public const string format = "[{0}] {1}\n";

	public static string toLog = "";

	public static int logDelay = 0;

	public const int logDelayMax = 10;

	public static string directory => Application.persistentDataPath ?? "";

	public static string path => directory + "/Log.log";

	public void OnEnable()
	{
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		CreateBackups();
		Application.logMessageReceived += Log;
		LogSystemInformation();
	}

	public void OnDisable()
	{
		Application.logMessageReceived -= Log;
	}

	public void OnDestroy()
	{
		if (!toLog.IsNullOrEmpty())
		{
			Write(toLog);
		}
	}

	public void Update()
	{
		if (logDelay < 0)
		{
			if (!toLog.IsNullOrEmpty())
			{
				Write(toLog);
				toLog = "";
				logDelay = 10;
			}
		}
		else
		{
			logDelay--;
		}
	}

	public static void Write(string str)
	{
		using (StreamWriter streamWriter = new StreamWriter(path, append: true))
		{
			streamWriter.Write(str);
		}
	}

	public static void Log(string log, string stacktrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			log = "\n\n" + log + "\n" + stacktrace;
		}

		Log(log);
	}

	public static void Log(string log)
	{
		toLog += $"[{DateTime.Now}] {log}\n";
	}

	public static void CreateBackups()
	{
		for (int num = 8; num >= 0; num--)
		{
			string fileName = GetFileName(num);
			if (File.Exists(fileName))
			{
				string fileName2 = GetFileName(num + 1);
				File.Copy(fileName, fileName2, overwrite: true);
				if (num == 0)
				{
					File.Delete(fileName);
				}
			}
		}
	}

	public static string GetFileName(int backupNumber)
	{
		if (backupNumber <= 0)
		{
			return path;
		}

		return $"{path}.{backupNumber}";
	}

	public static void LogSystemInformation()
	{
		Log("\n" + SystemInfo.deviceModel + $"\n{SystemInfo.deviceType} ({SystemInfo.deviceName}) [{SystemInfo.deviceUniqueIdentifier}]" + "\n" + SystemInfo.operatingSystem + $"\n{SystemInfo.processorType} ({SystemInfo.processorCount} Cores, {SystemInfo.processorFrequency}hz)" + "\n" + SystemInfo.graphicsDeviceVendor + " | " + SystemInfo.graphicsDeviceName + " (" + SystemInfo.graphicsDeviceVersion + ")" + $"\nShader Level: {SystemInfo.graphicsShaderLevel}" + $"\nGraphics Memory: {SystemInfo.graphicsMemorySize}" + $"\nSystem Memory: {SystemInfo.systemMemorySize}");
	}
}
