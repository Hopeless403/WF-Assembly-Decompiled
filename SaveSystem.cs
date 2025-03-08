#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.IO;
using System.Security.Cryptography;
using NaughtyAttributes;
using UnityEngine;

public class SaveSystem : GameSystem
{
	public class Saver
	{
		public readonly string baseFileName;

		public readonly ES3Settings settings;

		public Saver(string baseFileName, ES3Settings settings = null)
		{
			this.baseFileName = baseFileName;
			this.settings = settings ?? ES3Settings.defaultSettings;
		}

		public void SaveValue<TValue>(string key, TValue value, string folderName, string fileName = "")
		{
			StopWatch.Start();
			string text = string.Format(baseFileName, fileName);
			Debug.Log("Saving [" + text + "] (" + key + ")...");
			string filePath = folderName + "/" + text;
			try
			{
				ES3.Save(key, value, filePath, settings);
			}
			catch (InvalidOperationException message)
			{
				Debug.LogWarning(message);
			}

			catch (IOException message2)
			{
				Debug.LogWarning(message2);
			}

			Debug.Log($"Saving Done! ({StopWatch.Stop()}ms)");
		}

		public TValue LoadValue<TValue>(string key, string folderName, TValue defaultValue, string fileName = "")
		{
			StopWatch.Start();
			string text = string.Format(baseFileName, fileName);
			Debug.Log("Loading [" + text + "] (" + key + ")...");
			string text2 = folderName + "/" + text;
			TValue result = defaultValue;
			try
			{
				result = ES3.Load(key, text2, defaultValue, settings);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				Debug.LogWarning(text2 + " data appears to be corrupt, attempting to restore backup");
				ES3.RestoreBackup(text2, settings);
				try
				{
					result = ES3.Load(key, text2, defaultValue, settings);
				}
				catch (Exception)
				{
					Debug.LogWarning("Failed to restore backup :( returning default value...");
				}
			}

			Debug.Log($"Loading Done! ({StopWatch.Stop()}ms)");
			return result;
		}

		public bool FileExists(string folderName, string fileName = "")
		{
			string text = string.Format(baseFileName, fileName);
			return ES3.FileExists(folderName + "/" + text, settings);
		}

		public bool KeyExists(string key, string folderName, string fileName = "")
		{
			string text = string.Format(baseFileName, fileName);
			string text2 = folderName + "/" + text;
			try
			{
				return ES3.KeyExists(key, text2, settings);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				Debug.LogWarning(text2 + " data appears to be corrupt, attempting to restore backup");
				ES3.RestoreBackup(text2, settings);
				try
				{
					return ES3.KeyExists(key, text2, settings);
				}
				catch (Exception)
				{
					Debug.LogWarning("Failed to restore backup :( returning default value...");
					return false;
				}
			}
		}

		public void Delete(string folderName, string fileName = "")
		{
			string text = string.Format(baseFileName, fileName);
			ES3.DeleteFile(folderName + "/" + text, settings);
		}

		public void DeleteKey(string key, string folderName, string fileName = "")
		{
			string text = string.Format(baseFileName, fileName);
			ES3.DeleteKey(key, folderName + "/" + text, settings);
		}

		public void CheckBackup(string folderName, string fileName = "")
		{
			string text = string.Format(baseFileName, fileName);
			string text2 = folderName + "/" + text;
			Debug.Log("Checking " + text + " for corrupt data...");
			try
			{
				ES3.KeyExists("a", text2, settings);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				Debug.LogWarning(text2 + " data appears to be corrupt, attempting to restore backup");
				ES3.RestoreBackup(text2, settings);
				try
				{
					ES3.KeyExists("a", text2, settings);
					Debug.Log("Successfully retrieved " + text + " backup");
				}
				catch (Exception)
				{
					Debug.LogWarning("Failed to restore backup :(");
				}
			}
		}
	}

	public static SaveSystem instance;

	public bool encode;

	[HideIf("encode")]
	public bool beautify;

	public bool promptSave;

	public bool busy;

	public static readonly string profileFolder = "Profiles";

	public static string folderName = "SaveData";

	public static Saver progressSaver;

	public static Saver campaignSaver;

	public static Saver battleSaver;

	public static Saver statsSaver;

	public static Saver historySaver;

	public static ES3Settings settings;

	public static bool gotSaveTimestamp;

	public static DateTime saveTimestamp;

	public static bool Enabled
	{
		get
		{
			if (instance != null)
			{
				return instance.enabled;
			}

			return false;
		}
	}

	public static string Profile { get; set; } = "";


	public void OnEnable()
	{
		Events.InvokeSaveSystemEnabled();
	}

	public void OnDisable()
	{
		Events.InvokeSaveSystemDisabled();
	}

	public void Awake()
	{
		instance = this;
	}

	public void Start()
	{
		settings = new ES3Settings(ES3.Directory.PersistentDataPath)
		{
			encryptionType = ES3.EncryptionType.AES,
			compressionType = ES3.CompressionType.Gzip,
			prettyPrint = false
		};
		SetProfile(GetProfile(), save: false);
		if (ES3.FileExists(folderName + "/Save.sav"))
		{
			gotSaveTimestamp = true;
			saveTimestamp = ES3.GetTimestamp(folderName + "/Save.sav");
		}

		progressSaver = new Saver("Save{0}.sav", settings);
		campaignSaver = new Saver("Campaign{0}.sav", settings);
		battleSaver = new Saver("Battle{0}.sav", settings);
		statsSaver = new Saver("Stats{0}.sav", settings);
		historySaver = new Saver("History{0}.sav", settings);
		EncryptSaveData();
		Events.OnSaveSystemProfileChanged += EncryptSaveData;
		progressSaver.CheckBackup(folderName);
		campaignSaver.CheckBackup(folderName);
		campaignSaver.CheckBackup(folderName, "Demo");
		campaignSaver.CheckBackup(folderName, "PressDemo");
		battleSaver.CheckBackup(folderName);
		statsSaver.CheckBackup(folderName);
		historySaver.CheckBackup(folderName);
	}

	public void OnDestroy()
	{
		Events.OnSaveSystemProfileChanged -= EncryptSaveData;
	}

	public static void SaveProgressData<T>(string key, T value)
	{
		if (Enabled)
		{
			progressSaver.SaveValue(key, value, folderName);
		}
	}

	public static void SaveCampaignData<T>(GameMode gameMode, string key, T value)
	{
		if (Enabled)
		{
			campaignSaver.SaveValue(key, value, folderName, gameMode.saveFileName);
		}
	}

	public static void SaveStatsData<T>(string key, T value)
	{
		if (Enabled)
		{
			statsSaver.SaveValue(key, value, folderName);
		}
	}

	public static void SaveHistoryData<T>(string key, T value)
	{
		if (Enabled)
		{
			historySaver.SaveValue(key, value, folderName);
		}
	}

	public static T LoadProgressData<T>(string key) where T : class
	{
		return LoadProgressData<T>(key, null);
	}

	public static T LoadCampaignData<T>(GameMode gameMode, string key) where T : class
	{
		return LoadCampaignData<T>(gameMode, key, null);
	}

	public static T LoadStatsData<T>(string key) where T : class
	{
		return LoadStatsData<T>(key, null);
	}

	public static T LoadHistoryData<T>(string key) where T : class
	{
		return LoadHistoryData<T>(key, null);
	}

	public static T LoadProgressData<T>(string key, T defaultValue)
	{
		if (!Enabled)
		{
			return defaultValue;
		}

		return progressSaver.LoadValue(key, folderName, defaultValue);
	}

	public static T LoadCampaignData<T>(GameMode gameMode, string key, T defaultValue)
	{
		if (!Enabled)
		{
			return defaultValue;
		}

		return campaignSaver.LoadValue(key, folderName, defaultValue, gameMode.saveFileName);
	}

	public static T LoadStatsData<T>(string key, T defaultValue)
	{
		if (!Enabled)
		{
			return defaultValue;
		}

		return statsSaver.LoadValue(key, folderName, defaultValue);
	}

	public static T LoadHistoryData<T>(string key, T defaultValue)
	{
		if (!Enabled)
		{
			return defaultValue;
		}

		return historySaver.LoadValue(key, folderName, defaultValue);
	}

	public static bool ProgressExists()
	{
		if (Enabled)
		{
			return progressSaver.FileExists(folderName);
		}

		return false;
	}

	public static bool CampaignExists(GameMode gameMode)
	{
		if (Enabled)
		{
			return campaignSaver.FileExists(folderName, gameMode.saveFileName);
		}

		return false;
	}

	public static bool StatsExists()
	{
		if (Enabled)
		{
			return statsSaver.FileExists(folderName);
		}

		return false;
	}

	public static bool HistoryExists()
	{
		if (Enabled)
		{
			return historySaver.FileExists(folderName);
		}

		return false;
	}

	public static bool ProgressDataExists(string key)
	{
		if (Enabled)
		{
			return progressSaver.KeyExists(key, folderName);
		}

		return false;
	}

	public static bool CampaignDataExists(GameMode gameMode, string key)
	{
		if (Enabled)
		{
			return campaignSaver.KeyExists(key, folderName, gameMode.saveFileName);
		}

		return false;
	}

	public static bool StatsDataExists(string key)
	{
		if (Enabled)
		{
			return statsSaver.KeyExists(key, folderName);
		}

		return false;
	}

	public static bool HistoryDataExists(string key)
	{
		if (Enabled)
		{
			return historySaver.KeyExists(key, folderName);
		}

		return false;
	}

	public static void DeleteProgress()
	{
		if (Enabled)
		{
			progressSaver.Delete(folderName);
		}
	}

	public static void DeleteCampaign(GameMode gameMode)
	{
		if (Enabled)
		{
			campaignSaver.Delete(folderName, gameMode.saveFileName);
		}
	}

	public static void DeleteProfile(string profileName)
	{
		if (Enabled)
		{
			ES3.DeleteDirectory(folderName);
		}
	}

	public static void DeleteStats()
	{
		if (Enabled)
		{
			statsSaver.Delete(folderName);
		}
	}

	public static void DeleteHistory()
	{
		if (Enabled)
		{
			historySaver.Delete(folderName);
		}
	}

	public static void DeleteProgressData(string key)
	{
		if (Enabled)
		{
			progressSaver.DeleteKey(key, folderName);
		}
	}

	public static string GetProfile()
	{
		try
		{
			return ES3.Load("profile", "data.sav", "Default");
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
			return "Default";
		}
	}

	public static void SetProfile(string name, bool save = true)
	{
		Debug.Log("Save Profile Set: " + name);
		Profile = name;
		folderName = profileFolder + "/" + name;
		if (Enabled)
		{
			Events.InvokeSaveSystemProfileChanged();
		}

		if (save)
		{
			try
			{
				ES3.Save("profile", name, "data.sav");
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				ES3.DeleteFile("data.sav");
				ES3.Save("profile", name, "data.sav");
			}
		}
	}

	public static void EncryptSaveData()
	{
		ES3Settings defaultSettings = ES3Settings.defaultSettings;
		ES3Settings newSettings = settings;
		ConvertSaveFile(progressSaver, folderName, "", defaultSettings, newSettings);
		ConvertSaveFile(campaignSaver, folderName, "", defaultSettings, newSettings);
		ConvertSaveFile(campaignSaver, folderName, "Demo", defaultSettings, newSettings);
		ConvertSaveFile(statsSaver, folderName, "", defaultSettings, newSettings);
	}

	public static void ConvertSaveFile(Saver saver, string folder, string fileName, ES3Settings oldSettings, ES3Settings newSettings)
	{
		string text = string.Format(saver.baseFileName, fileName);
		string filePath = folderName + "/" + text;
		try
		{
			ES3.KeyExists("a", filePath, newSettings);
			Debug.Log("[" + text + "] save file does not need converting");
		}
		catch (Exception ex) when (ex is FormatException || ex is ArgumentException || ex is CryptographicException)
		{
			Debug.LogWarning(ex);
			Debug.LogWarning("[" + text + "] save file needs converting");
			try
			{
				string filePath2 = folder + "/" + text;
				ES3.SaveRaw(ES3.LoadRawString(filePath2, oldSettings), filePath2, newSettings);
				Debug.Log("Successfully converted [" + text + "] to new file format");
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				Debug.LogWarning("Failed to convert [" + text + "] to new file format");
			}
		}
	}
}
