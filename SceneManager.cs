#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager
{
	public static readonly Dictionary<string, Scene> Loaded = new Dictionary<string, Scene>();

	public static readonly List<string> Temporary = new List<string>();

	public static readonly List<string> Loading = new List<string>();

	public static int jobsActive;

	public static string ActiveSceneKey { get; set; }

	public static string ActiveSceneName { get; set; }

	public static bool HasNoActiveJobs => jobsActive <= 0;

	public static Scene GetActive()
	{
		return Loaded[ActiveSceneKey];
	}

	public static IEnumerator Load(string sceneKey, SceneType type, Action<Scene> onComplete = null)
	{
		JobStart();
		sceneKey = sceneKey.Replace("Scenes/", "").Replace(".unity", "");
		Debug.Log("SceneManager → Loading Scene \"" + sceneKey + "\"");
		if (Loading.Contains(sceneKey))
		{
			yield return new WaitUntil(() => !Loading.Contains(sceneKey));
		}

		if (!IsLoaded(sceneKey))
		{
			yield return Load(sceneKey, type, activateOnLoad: true);
			Events.InvokeSceneLoaded(Loaded[sceneKey]);
		}
		else
		{
			Debug.Log("\"" + sceneKey + "\" already loaded");
		}

		if (type == SceneType.Active)
		{
			yield return SetActive(sceneKey);
		}

		onComplete?.Invoke(Loaded[sceneKey]);
		JobEnd();
	}

	public static IEnumerator Load(string sceneKey, SceneType type, bool activateOnLoad)
	{
		Loading.Add(sceneKey);
		AsyncOperation handle = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
		while (!handle.isDone)
		{
			yield return null;
		}

		Loaded[sceneKey] = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneKey);
		Loading.Remove(sceneKey);
		Debug.Log("\"" + sceneKey + "\" Loaded");
		if (type == SceneType.Temporary)
		{
			Temporary.Add(sceneKey);
		}
	}

	public static IEnumerator SetActive(string sceneKey)
	{
		Scene scene = Loaded[sceneKey];
		ActiveSceneName = scene.name;
		ActiveSceneKey = sceneKey;
		Events.InvokeSceneChanged(scene);
		yield return UnloadTemporary();
	}

	public static IEnumerator Unload(string sceneKey)
	{
		if (sceneKey.IsNullOrWhitespace())
		{
			yield break;
		}

		Debug.Log("SceneManager → Unloading Scene \"" + sceneKey + "\"");
		if (IsLoaded(sceneKey))
		{
			Scene scene = Loaded[sceneKey];
			Events.InvokeSceneUnload(scene);
			AsyncOperation handle = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
			while (!handle.isDone)
			{
				yield return null;
			}

			Debug.Log("Scene successfully unloaded");
			Loaded.Remove(sceneKey);
			Temporary.Remove(sceneKey);
		}
		else
		{
			Debug.Log("Scene is already unloaded...");
		}
	}

	public static IEnumerator UnloadActive()
	{
		Routine.Clump clump = new Routine.Clump();
		if (!ActiveSceneKey.IsNullOrWhitespace())
		{
			clump.Add(Unload(ActiveSceneKey));
		}

		clump.Add(UnloadTemporary());
		yield return clump.WaitForEnd();
	}

	public static IEnumerator UnloadTemporary()
	{
		Routine.Clump clump = new Routine.Clump();
		foreach (string item in Temporary)
		{
			clump.Add(Unload(item));
		}

		yield return clump.WaitForEnd();
	}

	public static bool IsLoaded(string sceneKey)
	{
		return Loaded.ContainsKey(sceneKey);
	}

	public static IEnumerator WaitUntilUnloaded(string sceneKey)
	{
		yield return new WaitUntil(() => !Loaded.ContainsKey(sceneKey));
	}

	public static void JobStart()
	{
		jobsActive++;
	}

	public static void JobEnd()
	{
		jobsActive--;
	}
}
