#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class JournalVoidNameSequence : MonoBehaviour
{
	[SerializeField]
	public LeaderNameHistoryDisplay display;

	[SerializeField]
	public TweenUI endTween;

	public static IEnumerator LoadAndRun(bool unloadAfter)
	{
		yield return SceneManager.Load("JournalNameHistory", SceneType.Temporary);
		JournalVoidNameSequence voidNameSequence = Object.FindObjectOfType<JournalVoidNameSequence>();
		yield return voidNameSequence.PlayerKilled();
		if (unloadAfter)
		{
			yield return voidNameSequence.End();
		}
	}

	public IEnumerator PlayerKilled()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		JournalNameHistory.MostRecentNameKilled();
		display.Repopulate();
		yield return new WaitForSecondsRealtime(1f);
	}

	public IEnumerator End()
	{
		endTween.Fire();
		yield return new WaitForSecondsRealtime(endTween.GetDuration());
		new Routine(SceneManager.Unload("JournalNameHistory"));
	}
}
