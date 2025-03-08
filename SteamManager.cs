#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using Steamworks.Ugc;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
	public const int appId = 1811990;

	public static bool init { get; set; }

	public async void Awake()
	{
		try
		{
			SteamClient.Init(1811990u);
			if (SteamClient.RestartAppIfNecessary(1811990u))
			{
				GameManager.Quit();
			}
			else
			{
				init = true;
			}
		}
		catch (Exception arg)
		{
			Debug.LogError($"Steam failed to initialize! ({arg})");
			Debug.Log($"Steam failed to initialize! ({arg})");
			GameManager.Quit();
		}

		List<string> steamFiles = new List<string>();
		int i = 1;
		while (true)
		{
			ResultPage? resultPage = await Query.All.WhereUserSubscribed().GetPageAsync(i);
			if (!resultPage.HasValue || !(resultPage?.Entries).Any())
			{
				break;
			}

			foreach (Item pageEntry in resultPage?.Entries)
			{
				string directory = pageEntry.Directory;
				if (string.IsNullOrEmpty(directory) || pageEntry.IsDownloadPending || pageEntry.NeedsUpdate || (!string.IsNullOrEmpty(directory) && Directory.GetLastWriteTimeUtc(directory) < pageEntry.Updated))
				{
					await pageEntry.DownloadAsync();
					directory = pageEntry.Directory;
				}

				steamFiles.Add(directory);
			}

			i++;
		}

		foreach (string item in steamFiles)
		{
			Bootstrap.LoadModAtPath(item);
		}
	}

	public void OnEnable()
	{
		if (init)
		{
			Debug.Log($"Steam Initialized {SteamClient.Name} ({SteamClient.SteamId})");
		}
	}

	public void OnDisable()
	{
		SteamClient.Shutdown();
	}
}
