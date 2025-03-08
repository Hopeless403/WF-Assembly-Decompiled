#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Names : MonoBehaviourSingleton<Names>
{
	[Serializable]
	public struct Asset
	{
		public string name;

		public LocaleTextAsset[] files;
	}

	[Serializable]
	public class LocaleTextAsset
	{
		public Locale locale;

		public TextAsset textAsset;
	}

	[SerializeField]
	public Asset[] assets;

	public static Dictionary<string, TextAsset> files;

	public static readonly Dictionary<string, Queue<string>> lookup = new Dictionary<string, Queue<string>>();

	public static void Reset()
	{
		lookup.Clear();
	}

	public void OnEnable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged += LocaleChanged;
	}

	public void OnDisable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged -= LocaleChanged;
	}

	public static void LocaleChanged(Locale locale)
	{
		files = null;
		Reset();
	}

	public static string Pull(string key)
	{
		if (!lookup.ContainsKey(key))
		{
			Load(key);
		}

		Queue<string> queue = lookup[key];
		if (queue == null || queue.Count == 0)
		{
			Load(key);
			queue = lookup[key];
		}

		return queue.Dequeue();
	}

	public static string Pull(string characterRace, string characterGender)
	{
		return Pull(characterRace + characterGender);
	}

	public static void Load(string key)
	{
		List<string> list = new List<string>();
		string[] array = Regex.Split(GetFiles(key).text, "\r\n|\n|\r");
		foreach (string text in array)
		{
			list.Add(text.Trim());
		}

		list.Shuffle();
		lookup[key] = new Queue<string>(list);
	}

	public static TextAsset GetFiles(string key)
	{
		if (files == null)
		{
			files = new Dictionary<string, TextAsset>();
			Asset[] array = MonoBehaviourSingleton<Names>.instance.assets;
			for (int i = 0; i < array.Length; i++)
			{
				Asset asset = array[i];
				LocaleTextAsset localeTextAsset = asset.files.FirstOrDefault((LocaleTextAsset a) => a.locale.name == LocalizationSettings.SelectedLocale.name) ?? asset.files.First();
				files[asset.name] = localeTextAsset.textAsset;
			}
		}

		return files[key];
	}
}
