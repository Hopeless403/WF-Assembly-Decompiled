#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSystem : GameSystem
{
	[Serializable]
	public class Music
	{
		public string sceneName;

		public EventReference eventId;
	}

	public static MusicSystem instance;

	public Music[] music;

	public Music[] eventMusic;

	public static EventInstance current;

	public static LTDescr fadePitchTween;

	public static float pitch = 1f;

	public void OnEnable()
	{
		instance = this;
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
		StopMusic();
	}

	public void SceneChanged(Scene from, Scene to)
	{
		SceneChanged(to);
	}

	public void SceneChanged(Scene scene)
	{
		StopMusic();
		Music music = this.music.FirstOrDefault((Music a) => a.sceneName == scene.name);
		if (music != null)
		{
			StartMusic(music.eventId.Guid);
		}
	}

	public static void StartMusic(EventReference eventId)
	{
		StartMusic(eventId.Guid);
	}

	public static void StartMusic(GUID eventGUID)
	{
		try
		{
			current = RuntimeManager.CreateInstance(eventGUID);
			current.start();
		}
		catch (EventNotFoundException message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	public static void SetParam(string name, float value)
	{
		if (IsRunning(current))
		{
			current.setParameterByName(name, value);
		}
	}

	public static void StopMusic(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (IsRunning(current))
		{
			current.stop(stopMode);
			current.release();
		}
	}

	public static void FadePitchTo(float value, float time)
	{
		fadePitchTween = LeanTween.value(instance.gameObject, pitch, value, time).setOnUpdate(delegate(float a)
		{
			pitch = a;
			if (IsRunning(current))
			{
				current.setPitch(pitch);
			}

		});
	}

	public static bool IsRunning(EventInstance instance)
	{
		if (instance.isValid())
		{
			instance.getPlaybackState(out var state);
			if (state != PLAYBACK_STATE.STOPPED)
			{
				return true;
			}
		}

		return false;
	}
}
