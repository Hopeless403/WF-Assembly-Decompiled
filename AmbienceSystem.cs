#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbienceSystem : GameSystem
{
	[SerializeField]
	public string[] validScenes = new string[2] { "Battle", "Event" };

	public static EventInstance current;

	public void OnEnable()
	{
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
		Stop();
	}

	public void SceneChanged(Scene scene)
	{
		Stop();
		if (validScenes.Contains(scene.name))
		{
			Play(References.GetCurrentArea().ambienceEvent);
		}
	}

	public static void Play(EventReference eventId)
	{
		Play(eventId.Guid);
	}

	public static void Play(GUID eventGUID)
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

	public static void Stop(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (IsRunning(current))
		{
			current.stop(stopMode);
			current.release();
		}
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
