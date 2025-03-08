#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class RunMultipleSequences : MonoBehaviour
{
	[SerializeField]
	public UISequence[] sequences;

	[SerializeField]
	public bool unloadSceneAfter = true;

	public IEnumerator Start()
	{
		UISequence[] array = sequences;
		foreach (UISequence sequence in array)
		{
			sequence.gameObject.SetActive(value: true);
			yield return sequence.Run();
			sequence.gameObject.SetActive(value: false);
		}

		if (unloadSceneAfter)
		{
			new Routine(SceneManager.Unload(base.gameObject.scene.name));
		}
	}
}
