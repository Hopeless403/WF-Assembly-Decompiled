#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;

public class Talker : MonoBehaviour
{
	public struct Store
	{
		public string speechType;

		public float delay;

		public object[] inserts;

		public Store(string speechType, float delay, object[] inserts)
		{
			this.speechType = speechType;
			this.delay = delay;
			this.inserts = inserts;
		}
	}

	[Serializable]
	public struct Speech
	{
		public string type;

		public string[] poseOptions;

		public UnityEngine.Localization.LocalizedString[] options;

		public EventReference sfxEvent;

		public List<UnityEngine.Localization.LocalizedString> optionPool;

		public string Pull()
		{
			if (optionPool == null)
			{
				optionPool = new List<UnityEngine.Localization.LocalizedString>();
			}

			if (optionPool.Count <= 0)
			{
				optionPool.AddRange(options);
			}

			int index = PettyRandom.Range(0, optionPool.Count - 1);
			UnityEngine.Localization.LocalizedString localizedString = optionPool[index];
			optionPool.RemoveAt(index);
			return localizedString.GetLocalizedString();
		}
	}

	[SerializeField]
	public UnityEngine.Localization.LocalizedString nameKey;

	[SerializeField]
	public Transform talkFrom;

	[SerializeField]
	public AvatarPoser poser;

	[SerializeField]
	public Speech[] speech;

	public readonly Dictionary<string, Speech> speechLookup = new Dictionary<string, Speech>();

	public readonly List<Store> stored = new List<Store>();

	public string GetName()
	{
		if (!nameKey.IsEmpty)
		{
			return nameKey.GetLocalizedString();
		}

		return "";
	}

	public void Awake()
	{
		Speech[] array = speech;
		for (int i = 0; i < array.Length; i++)
		{
			Speech value = array[i];
			speechLookup[value.type] = value;
		}
	}

	public void OnEnable()
	{
		foreach (Store item in stored)
		{
			Say(item.speechType, item.delay, item.inserts);
		}

		stored.Clear();
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public void Say(string speechType, float delay = 0f, params object[] inserts)
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy)
		{
			stored.Add(new Store(speechType, delay, inserts));
		}
		else
		{
			if (!Get(speechType, out var speech))
			{
				return;
			}

			string text = speech.Pull();
			EventReference? sfxEvent = speech.sfxEvent;
			if (inserts.Length != 0)
			{
				text = string.Format(text, inserts);
			}

			if (text.Contains('|'))
			{
				string[] array = text.Split('|');
				float num = 0f;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					StartCoroutine(SayAfterDelay(text2, sfxEvent, delay + num));
					if (speech.poseOptions.Length != 0 && (bool)poser)
					{
						StartCoroutine(PoseAfterDelay(speech.poseOptions.RandomItem(), delay + num));
					}

					num += Mathf.Max(1.25f, SpeechBubbleSystem.GetDuration(text2) * 0.5f);
					sfxEvent = null;
				}
			}
			else
			{
				StartCoroutine(SayAfterDelay(text, sfxEvent, delay));
				if (speech.poseOptions.Length != 0 && (bool)poser)
				{
					StartCoroutine(PoseAfterDelay(speech.poseOptions.RandomItem(), delay));
				}
			}
		}
	}

	public bool Get(string type, out Speech speech)
	{
		bool flag = speechLookup.TryGetValue(type, out speech);
		if (!flag)
		{
			Speech[] array = this.speech.Where((Speech a) => a.type == type).ToArray();
			if (array.Length != 0)
			{
				flag = true;
				speech = array[0];
			}
		}

		return flag;
	}

	public IEnumerator SayAfterDelay(string text, EventReference? sfxEvent, float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}

		if (sfxEvent.HasValue && !sfxEvent.GetValueOrDefault().IsNull)
		{
			SfxSystem.OneShot(sfxEvent.Value);
		}

		SpeechBubbleSystem.Create(new SpeechBubbleData(talkFrom, GetName(), text));
	}

	public IEnumerator PoseAfterDelay(string pose, float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}

		poser.Set(pose);
	}
}
