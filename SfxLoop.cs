#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class SfxLoop : MonoBehaviour
{
	[Serializable]
	public struct Param
	{
		public string name;

		public float value;
	}

	[SerializeField]
	public bool onEnable = true;

	[SerializeField]
	public bool useAreaAmbience;

	[SerializeField]
	[HideIf("useAreaAmbience")]
	public EventReference eventRef;

	[SerializeField]
	public Param[] @params;

	public EventInstance loop;

	public bool playing;

	public EventReference e
	{
		get
		{
			if (!useAreaAmbience)
			{
				return eventRef;
			}

			return References.GetCurrentArea().ambienceEvent;
		}
	}

	public void OnEnable()
	{
		if (onEnable)
		{
			Play();
		}
	}

	public void OnDisable()
	{
		Stop();
	}

	public void Play()
	{
		loop = SfxSystem.Loop(e);
		Param[] array = @params;
		for (int i = 0; i < array.Length; i++)
		{
			Param param = array[i];
			SetParam(param.name, param.value);
		}

		playing = true;
	}

	public void Stop()
	{
		if (playing)
		{
			SfxSystem.EndLoop(loop);
			playing = false;
		}
	}

	public void SetParam(string name, float value)
	{
		SfxSystem.SetParam(loop, name, value);
	}

	public void SetParam(float value)
	{
		if (@params.Length != 0)
		{
			SetParam(@params[0].name, value);
		}
	}
}
