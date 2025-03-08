#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class SceneUnloader : MonoBehaviour
{
	public bool active = true;

	public void Unload()
	{
		if (active)
		{
			active = false;
			new Routine(SceneManager.Unload(base.gameObject.scene.name));
		}
	}

	public void UnloadAfter(float delay)
	{
		StopAllCoroutines();
		StartCoroutine(UnloadAfterRoutine(delay));
	}

	public IEnumerator UnloadAfterRoutine(float delay)
	{
		yield return new WaitForSeconds(delay);
		Unload();
	}
}
