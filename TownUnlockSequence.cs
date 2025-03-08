#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TownUnlockSequence : MonoBehaviour
{
	[SerializeField]
	public GainUnlockSequence unlockSequence;

	[SerializeField]
	public Fader fader;

	[SerializeField]
	public float pauseBeforeUnlock = 0.1f;

	[SerializeField]
	public float pauseAfterUnlock = 0.1f;

	public const float fadeOutDur = 0.1f;

	public IEnumerator Start()
	{
		yield return AddressableLoader.LoadGroup("UnlockData");
		List<UnlockData> unlocks = AddressableLoader.GetGroup<UnlockData>("UnlockData");
		List<string> unlocksToGain = SaveSystem.LoadProgressData("townNew", new List<string>());
		foreach (string unlockName in unlocksToGain)
		{
			UnlockData unlockData = unlocks.FirstOrDefault((UnlockData a) => a.name == unlockName);
			if (unlockData != null)
			{
				yield return new WaitForSeconds(pauseBeforeUnlock);
				Events.InvokeTownUnlock(unlockData);
				unlockSequence.SetUp(unlockData);
				yield return new WaitUntil(() => !unlockSequence.gameObject.activeSelf);
				yield return new WaitForSeconds(pauseAfterUnlock);
			}
		}

		unlocksToGain.Clear();
		SaveSystem.SaveProgressData("townNew", unlocksToGain);
		fader.Out(0.1f, LeanTweenType.linear);
		yield return new WaitForSeconds(0.1f);
		new Routine(SceneManager.Unload("TownUnlocks"));
	}
}
