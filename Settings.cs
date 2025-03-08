#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public static class Settings
{
	public static readonly ES3Settings settings = new ES3Settings(ES3.Directory.PersistentDataPath)
	{
		path = "settings.json",
		encryptionType = ES3.EncryptionType.None,
		compressionType = ES3.CompressionType.None,
		prettyPrint = true
	};

	public static void Save<T>(string key, T value)
	{
		try
		{
			ES3.Save(key, value, settings);
		}
		catch (FormatException message)
		{
			Debug.LogWarning(message);
			ES3.DeleteFile(settings);
			try
			{
				ES3.Save(key, value, settings);
			}
			catch (Exception arg)
			{
				Debug.LogError($"ES3 Failed to save Settings even after deleting file.\n{arg}");
			}
		}

		Events.InvokeSettingChanged(key, value);
		Debug.Log($"Setting Saved [{key} = {value}]");
	}

	public static T Load<T>(string key, T defaultValue)
	{
		T val = defaultValue;
		try
		{
			val = ES3.Load(key, defaultValue, settings);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
		}

		Debug.Log($"Setting Loaded [{key} = {val}]");
		return val;
	}

	public static bool Exists(string key)
	{
		try
		{
			return ES3.KeyExists(key, settings);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
		}

		return false;
	}
}
