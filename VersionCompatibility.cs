#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class VersionCompatibility : MonoBehaviour
{
	public readonly struct Profile
	{
		public readonly string name;

		public readonly int versionFrom;

		public readonly int versionTo;

		public readonly int incompatibleWithFrom;

		public readonly int incompatibleWithTo;

		public readonly Action action;

		public Profile(string name, string version, string incompatibleWith, Action action)
		{
			this.name = name;
			ProcessVersionString(version, out versionFrom, out versionTo);
			ProcessVersionString(incompatibleWith, out incompatibleWithFrom, out incompatibleWithTo);
			this.action = action;
		}

		public static void ProcessVersionString(string @in, out int lower, out int higher)
		{
			string[] array = @in.Split(new char[1] { '-' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				int.TryParse(array[0], out lower);
				int.TryParse(array[1], out higher);
			}
			else
			{
				int.TryParse(@in.Replace("+", ""), out lower);
				higher = (@in.EndsWith("+") ? 99999 : lower);
			}
		}

		public static bool InRange(int x, int a, int b)
		{
			if (a <= x)
			{
				return x <= b;
			}

			return false;
		}

		public void Check(string currentVersion, string previousVersion)
		{
			if (int.TryParse(previousVersion, out var result) && InRange(result, incompatibleWithFrom, incompatibleWithTo) && int.TryParse(currentVersion, out var result2) && InRange(result2, versionFrom, versionTo))
			{
				Debug.Log("Version [" + currentVersion + "] incompatible with [" + previousVersion + "] running [" + name + "] Script");
				action();
			}
		}
	}

	public static bool versionsGot;

	public static string previousVersion;

	public static string currentVersion;

	public void OnEnable()
	{
		Events.OnGameStart += GameStart;
	}

	public void OnDisable()
	{
		Events.OnGameStart -= GameStart;
	}

	public static void GetVersions()
	{
		if (!versionsGot)
		{
			currentVersion = Config.data.version;
			previousVersion = SaveSystem.LoadProgressData("version", "0");
		}
	}

	public static void GameStart()
	{
		GetVersions();
		if (currentVersion != previousVersion)
		{
			Debug.Log("Checking Version Compatibility [" + previousVersion + "] → [" + currentVersion + "]");
			foreach (Profile item in new List<Profile>
			{
				new Profile("Reset Progress", "63+", "0-62", ResetProgress),
				new Profile("Reset Town Progress", "88+", "0-87", ResetTownProgress),
				new Profile("Delete Campaign Save", "135+", "0-134", DeleteCampaignSave),
				new Profile("Reset Progress", "153+", "0-152", DeleteDefaultProfile),
				new Profile("Create Beta Profile", "175", "0-174", CreateBetaProfile),
				new Profile("Copy Beta Profile", "177+", "175-176", CopyBetaProfile),
				new Profile("Copy Beta Profile", "187+", "179-186", CopyBetaProfile)
			})

			{
				item.Check(currentVersion, previousVersion);
			}
			SaveSystem.SaveProgressData("version", currentVersion);
		}

		ProgressSaveData progressSaveData = SaveSystem.LoadProgressData<ProgressSaveData>("progress");
		if (progressSaveData != null)
		{
			Debug.Log("VersionCompatibility → Converting ProgressSaveData");
			ConvertProgressSaveData(progressSaveData);
		}
	}

	public static void CreateBetaProfile()
	{
		string folderName = SaveSystem.folderName;
		string newDirectoryPath = SaveSystem.profileFolder + "/Beta";
		ES3.CopyDirectory(folderName, newDirectoryPath, SaveSystem.settings, SaveSystem.settings);
	}

	public static void CopyBetaProfile()
	{
		if (ES3.DirectoryExists(SaveSystem.profileFolder + "/Beta") && ES3.FileExists(SaveSystem.profileFolder + "/Beta/Save.sav") && SaveSystem.gotSaveTimestamp)
		{
			DateTime timestamp = ES3.GetTimestamp(SaveSystem.profileFolder + "/Beta/Save.sav", SaveSystem.settings);
			if (timestamp > SaveSystem.saveTimestamp)
			{
				Debug.Log($"Beta Save Timestamp: {timestamp}, Default: {SaveSystem.saveTimestamp}, Copying Beta Save to Default");
				CopyFileFromBetaProfile("Save.sav");
				CopyFileFromBetaProfile("Stats.sav");
				CopyFileFromBetaProfile("Campaign.sav");
				CopyFileFromBetaProfile("CampaignDaily.sav");
				CopyFileFromBetaProfile("CampaignTutorial.sav");
				CopyFileFromBetaProfile("History.sav");
			}
			else
			{
				Debug.Log($"Beta Save Timestamp: {timestamp}, Default: {SaveSystem.saveTimestamp}, Default is more recent, deleting Beta profile");
			}

			ES3.DeleteDirectory(SaveSystem.profileFolder + "/Beta");
		}
	}

	public static void CopyFileFromBetaProfile(string fileName)
	{
		CopyFileData(SaveSystem.profileFolder + "/Beta/" + fileName, SaveSystem.folderName + "/" + fileName, SaveSystem.settings);
	}

	public static void CopyFileData(string fromPath, string toPath, ES3Settings settings)
	{
		if (ES3.FileExists(fromPath, settings))
		{
			ES3.SaveRaw(ES3.LoadRawString(fromPath, settings), toPath, settings);
		}
	}

	public static void DeleteDefaultProfile()
	{
		SaveSystem.DeleteProfile("Default");
	}

	public static void ResetProgress()
	{
		if (SaveSystem.ProgressExists())
		{
			ProgressSaveData progressSaveData = SaveSystem.LoadProgressData<ProgressSaveData>("progress");
			SaveSystem.DeleteProgress();
			if (progressSaveData != null)
			{
				SaveSystem.SaveProgressData("progress", progressSaveData);
			}
		}
	}

	public static void ResetTownProgress()
	{
		if (SaveSystem.ProgressExists())
		{
			SaveSystem.DeleteProgressData("townProgress");
			SaveSystem.DeleteProgressData("unlocked");
			SaveSystem.DeleteProgressData("townNew");
			SaveSystem.DeleteProgressData("finalBoss");
			SaveSystem.DeleteProgressData("finalBossEnemies");
			SaveSystem.DeleteProgressData("petHutUnlocks");
			SaveSystem.DeleteProgressData("inventorHutUnlocks");
			SaveSystem.DeleteProgressData("hotSpringUnlocks");
			SaveSystem.DeleteProgressData("completedChallenges");
			SaveSystem.DeleteProgressData("challengeProgress");
		}
	}

	public static void ConvertProgressSaveData(ProgressSaveData data)
	{
		SaveSystem.SaveProgressData("nextSeed", data.nextSeed);
		SaveSystem.SaveProgressData("tutorialProgress", data.tutorialProgress);
		SaveSystem.SaveProgressData("tutorialCharmDone", data.tutorialCharmDone);
		SaveSystem.SaveProgressData("tutorialInjuryDone", data.tutorialInjuryDone);
		SaveSystem.DeleteProgressData("progress");
	}

	public static void DeleteCampaignSave()
	{
		SaveSystem.DeleteCampaign(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"));
	}
}
