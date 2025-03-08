#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMusic : MonoBehaviour
{
	[SerializeField]
	public bool playOnStart = true;

	[SerializeField]
	public EventReference musicEvent;

	public EventInstance current;

	public void OnEnable()
	{
		Events.OnBackToMainMenu += BackToMainMenu;
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnBackToMainMenu -= BackToMainMenu;
		Events.OnSceneChanged -= SceneChanged;
	}

	public void Start()
	{
		if (playOnStart && !(References.Battle.winner != References.Battle.player))
		{
			StartMusic(musicEvent.Guid);
		}
	}

	public void BackToMainMenu()
	{
		SetEndParam();
	}

	public void SceneChanged(Scene scene)
	{
		SetEndParam();
	}

	public void Play()
	{
		StartMusic(musicEvent.Guid);
	}

	public void StartMusic(GUID eventGUID)
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

	public void SetEndParam()
	{
		SetParam("final_win", 1f);
	}

	public void SetParam(string name, float value)
	{
		if (IsRunning())
		{
			current.setParameterByName(name, value);
		}
	}

	public bool IsRunning()
	{
		if (current.isValid())
		{
			current.getPlaybackState(out var state);
			if (state != PLAYBACK_STATE.STOPPED)
			{
				return true;
			}
		}

		return false;
	}
}
