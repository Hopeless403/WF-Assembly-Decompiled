#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SplashScreenSequence : MonoBehaviour
{
	[Serializable]
	public struct Phase
	{
		public GameObject gameObject;

		public float canSkipTime;

		public float autoSkipTime;

		public EventReference sfxEvent;
	}

	[SerializeField]
	public Phase[] phases;

	public int phase;

	public EventInstance sfxInstance;

	public IEnumerator Run()
	{
		PauseMenu.Block();
		while (phase < phases.Length)
		{
			phases[phase].gameObject.SetActive(value: true);
			if (!phases[phase].sfxEvent.IsNull)
			{
				sfxInstance = SfxSystem.OneShot(phases[phase].sfxEvent);
			}

			float time = 0f;
			while (time < phases[phase].autoSkipTime)
			{
				yield return null;
				time += Time.deltaTime;
				if (time > phases[phase].canSkipTime && InputSystem.Enabled && AnyButtonPressed())
				{
					break;
				}
			}

			phase++;
			if (sfxInstance.isValid())
			{
				sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		PauseMenu.Unblock();
	}

	public static bool AnyButtonPressed()
	{
		if (!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
		{
			return InputSystem.IsSelectPressed();
		}

		return true;
	}
}
