#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using FMODUnity;
using UnityEngine;

public class JournalAddNameSequence : MonoBehaviour
{
	[SerializeField]
	public LeaderNameHistoryDisplay display;

	[SerializeField]
	public TweenUI endTween;

	[SerializeField]
	public EventReference sfxEvent;

	public static IEnumerator LoadAndRun(CardData leader, bool unloadAfter)
	{
		InputSystem.Disable();
		yield return SceneManager.Load("JournalNameHistory", SceneType.Temporary);
		JournalAddNameSequence addNameSequence = Object.FindObjectOfType<JournalAddNameSequence>();
		yield return addNameSequence.Run(leader);
		if (unloadAfter)
		{
			new Routine(addNameSequence.End());
		}

		InputSystem.Enable();
	}

	public IEnumerator Run(CardData leader)
	{
		yield return new WaitForSecondsRealtime(0.5f);
		JournalNameHistory.FadePrevious();
		JournalNameHistory.AddName(leader.title);
		display.Repopulate();
		SfxSystem.OneShot(sfxEvent);
		yield return new WaitForSecondsRealtime(1f);
	}

	public IEnumerator End()
	{
		endTween.Fire();
		yield return new WaitForSecondsRealtime(endTween.GetDuration());
		new Routine(SceneManager.Unload("JournalNameHistory"));
	}
}
