#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using Deadpan.Enums.Engine.Components.Modding;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
	[SerializeField]
	public string[] loadFirst = new string[2] { "Camera", "Global" };

	[SerializeField]
	public string[] thenLoad = new string[4] { "Input", "Systems", "Saving", "PauseScreen" };

	[SerializeField]
	public string startSceneKey = "MainMenu";

	[SerializeField]
	public string culture = "en-GB";

	[SerializeField]
	public UnityEngine.Animator progressAnimator;

	[SerializeField]
	public Image progressFill;

	[SerializeField]
	public float pauseBefore = 0.5f;

	[SerializeField]
	public float pauseAfter = 0.5f;

	[SerializeField]
	public bool unloadSceneAfter = true;

	[SerializeField]
	public float fillLerp = 0.1f;

	[SerializeField]
	public SplashScreenSequence splashScreen;

	[SerializeField]
	public float minTime;

	public float targetFill;

	public float fillAdd;

	public static bool done;

	public static int Count;

	public static readonly SortedSet<WildfrostMod> Mods = new SortedSet<WildfrostMod>();

	public void OnEnable()
	{
		Count++;
	}

	public void OnDisable()
	{
		Count--;
	}

	public IEnumerator Start()
	{
		if (done)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		done = true;
		Scene thisScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		Routine.Clump clump = new Routine.Clump();
		clump.Add(Load());
		if (minTime > 0f)
		{
			clump.Add(Sequences.Wait(minTime));
		}

		yield return clump.WaitForEnd();
		while (AudioSettingsSystem.Loading)
		{
			yield return null;
		}

		if ((bool)splashScreen)
		{
			yield return splashScreen.Run();
		}

		ModsSetup();
		SceneManager.JobStart();
		yield return Transition.To(startSceneKey);
		if (unloadSceneAfter)
		{
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(thisScene).completed += delegate
			{
				SceneManager.JobEnd();
			};
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
			SceneManager.JobEnd();
		}
	}

	public IEnumerator Load()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
		fillAdd = 1f / (float)(loadFirst.Length + thenLoad.Length);
		yield return new WaitForSeconds(pauseBefore);
		if ((bool)progressAnimator)
		{
			progressAnimator.SetBool("Increasing", value: true);
		}

		yield return Load(loadFirst);
		yield return Load(thenLoad);
		if ((bool)progressAnimator)
		{
			progressAnimator.SetBool("Increasing", value: false);
			progressAnimator.SetTrigger("Ping");
		}

		yield return new WaitForSeconds(pauseAfter);
	}

	public void Update()
	{
		if ((bool)progressFill)
		{
			progressFill.fillAmount = Delta.Lerp(progressFill.fillAmount, targetFill, fillLerp, Time.deltaTime);
		}
	}

	public IEnumerator Load(IEnumerable<string> sceneKeys)
	{
		Routine.Clump clump = new Routine.Clump();
		foreach (string sceneKey in sceneKeys)
		{
			clump.Add(SceneManager.Load(sceneKey, SceneType.Persistent, delegate
			{
				targetFill += fillAdd;
			}));
		}

		yield return clump.WaitForEnd();
	}

	public static bool IsModDirectory(string dir, out string[] dlls)
	{
		dlls = Directory.GetFiles(dir, "*.dll");
		return dlls.Length != 0;
	}

	public static void LoadModAtPath(string path)
	{
		if (!IsModDirectory(path, out var dlls))
		{
			return;
		}

		Assembly assembly = null;
		string[] array = dlls;
		for (int i = 0; i < array.Length; i++)
		{
			Assembly assembly2 = Assembly.LoadFrom(array[i]);
			try
			{
				Type[] types = assembly2.GetTypes();
				for (int j = 0; j < types.Length; j++)
				{
					if (types[j].BaseType == typeof(WildfrostMod))
					{
						Debug.Log("Found valid mod type");
						assembly = assembly2;
						break;
					}
				}
			}
			catch (TypeLoadException)
			{
			}
		}

		if (assembly == null)
		{
			Debug.LogWarning("Empty mod? " + path);
		}
		else
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.BaseType == typeof(WildfrostMod) && type != typeof(InternalMod))
				{
					WildfrostMod item = Activator.CreateInstance(type, path) as WildfrostMod;
					Mods.Add(item);
					break;
				}
			}

			Debug.LogWarning("Properly loaded mod and added instance " + assembly);
		}

		Debug.LogWarning("Found mod directory " + path);
	}

	public void ModsSetup()
	{
		foreach (Type allDataType in WildfrostMod.AllDataTypes)
		{
			typeof(AddressableLoader).GetMethod("GetGroup", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(allDataType).Invoke(this, new object[1] { allDataType.ToString() });
		}

		TMP_Text.OnSpriteAssetRequest += delegate(int hash, string s)
		{
			foreach (WildfrostMod mod in Mods)
			{
				if (s == mod.GUID)
				{
					return mod.HasLoaded ? mod.SpriteAsset : null;
				}
			}

			return null;
		};
		string path = Path.Combine(Application.streamingAssetsPath, "Mods");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		string[] directories = Directory.GetDirectories(path);
		for (int i = 0; i < directories.Length; i++)
		{
			LoadModAtPath(directories[i]);
		}

		WildfrostMod[] lastMods = WildfrostMod.GetLastMods();
		for (int i = 0; i < lastMods.Length; i++)
		{
			lastMods[i]?.ModLoad();
		}
	}
}
