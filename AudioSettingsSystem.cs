#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioSettingsSystem : GameSystem
{
	[Serializable]
	public class Bus
	{
		public string name;

		public string path;

		public string volumeSettingsKey;

		[Range(0f, 2f)]
		public float volume = 1f;

		[Range(0f, 2f)]
		public float pitch = 1f;

		public FMOD.Studio.Bus bus;

		public ChannelGroup channelGroup;

		public void Init()
		{
			bus = RuntimeManager.GetBus(path);
			bus.lockChannelGroup();
			volume = Settings.Load(volumeSettingsKey, 1f);
			UpdateVolume();
			UpdatePitch();
		}

		public void UpdateVolume()
		{
			bus.setVolume(volume);
		}

		public void UpdatePitch()
		{
			bus.getChannelGroup(out channelGroup);
			channelGroup.setPitch(pitch);
		}
	}

	public static bool Loading;

	[SerializeField]
	public Bus[] buses;

	[SerializeField]
	[Range(0f, 1f)]
	public float slowmoPitchMin = 0.67f;

	[SerializeField]
	[Range(0f, 1f)]
	public float slowmoPitchLerp = 0.25f;

	[SerializeField]
	public bool slowmoLerpUseDelta = true;

	[InfoBox("Master.strings and Master MUST be loaded first!", EInfoBoxType.Normal)]
	[SerializeField]
	public AssetReference[] banksToLoad;

	public Dictionary<string, Bus> busLookup;

	public float slowmoPitch = 1f;

	public float slowmoPitchTarget = 1f;

	public void Awake()
	{
		StartCoroutine(LoadBanks());
	}

	public void OnEnable()
	{
		Events.OnAudioVolumeChange += SetVolume;
		Events.OnAudioPitchChange += SetPitch;
		Events.OnTimeScaleChange += TimeScaleChange;
	}

	public void OnDisable()
	{
		Events.OnAudioVolumeChange -= SetVolume;
		Events.OnAudioPitchChange -= SetPitch;
		Events.OnTimeScaleChange -= TimeScaleChange;
	}

	public IEnumerator LoadBanks()
	{
		Loading = true;
		UnityEngine.Debug.Log("Audio Settings System → Loading Banks");
		AssetReference[] array = banksToLoad;
		foreach (AssetReference assetReference in array)
		{
			RuntimeManager.LoadBank(assetReference, loadSamples: true);
			UnityEngine.Debug.Log($"FMOD BANK {assetReference} LOADED");
		}

		while (!RuntimeManager.HaveAllBanksLoaded)
		{
			yield return null;
		}

		while (RuntimeManager.AnySampleDataLoading())
		{
			yield return null;
		}

		busLookup = new Dictionary<string, Bus>();
		Bus[] array2 = buses;
		foreach (Bus bus in array2)
		{
			bus.Init();
			busLookup.Add(bus.name, bus);
		}

		Loading = false;
	}

	public void Update()
	{
		if (slowmoPitch != slowmoPitchTarget)
		{
			if (slowmoLerpUseDelta)
			{
				slowmoPitch = Delta.Lerp(slowmoPitch, slowmoPitchTarget, slowmoPitchLerp, Time.deltaTime);
			}
			else
			{
				slowmoPitch = Mathf.Lerp(slowmoPitch, slowmoPitchTarget, slowmoPitchLerp);
			}

			if (Mathf.Abs(slowmoPitch - slowmoPitchTarget) < 0.01f)
			{
				slowmoPitch = slowmoPitchTarget;
			}

			SetPitch("Master", slowmoPitch);
		}
	}

	public void TimeScaleChange(float value)
	{
		slowmoPitchTarget = slowmoPitchMin + (1f - slowmoPitchMin) * value;
		Bus bus = busLookup["Master"];
		if (bus != null)
		{
			slowmoPitch = bus.pitch;
		}
	}

	public void SetVolume(string busName, float value)
	{
		Bus bus = busLookup[busName];
		if (bus != null)
		{
			bus.volume = value;
			bus.UpdateVolume();
			Settings.Save(bus.volumeSettingsKey, value);
		}
	}

	public static void Volume(string busName, float value)
	{
		AudioSettingsSystem audioSettingsSystem = UnityEngine.Object.FindObjectOfType<AudioSettingsSystem>();
		if ((object)audioSettingsSystem != null && audioSettingsSystem.enabled)
		{
			audioSettingsSystem.SetVolume(busName, Mathf.Clamp(value, 0f, 1f));
		}
	}

	public static float GetVolume(string busName)
	{
		AudioSettingsSystem audioSettingsSystem = UnityEngine.Object.FindObjectOfType<AudioSettingsSystem>();
		if ((object)audioSettingsSystem != null && audioSettingsSystem.enabled)
		{
			try
			{
				Bus bus = audioSettingsSystem.busLookup[busName];
				if (bus != null)
				{
					return bus.volume;
				}
			}
			catch (KeyNotFoundException ex)
			{
				throw new KeyNotFoundException("[" + busName + "] does not exist", ex.InnerException);
			}
		}

		return 0f;
	}

	public void SetPitch(string busName, float value)
	{
		try
		{
			Bus bus = busLookup[busName];
			if (bus != null)
			{
				bus.pitch = value;
				bus.UpdatePitch();
			}
		}
		catch (KeyNotFoundException ex)
		{
			throw new KeyNotFoundException("[" + busName + "] does not exist", ex.InnerException);
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void PromptUpdate()
	{
		Bus[] array = buses;
		foreach (Bus obj in array)
		{
			obj.UpdateVolume();
			obj.UpdatePitch();
		}
	}
}
