#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class ActionEarlyDeploy : PlayAction
{
	public readonly Transform transform;

	public readonly IEnumerator earlyDeployRoutine;

	public ActionEarlyDeploy(Transform transform, IEnumerator earlyDeployRoutine)
	{
		this.transform = transform;
		this.earlyDeployRoutine = earlyDeployRoutine;
	}

	public override IEnumerator Run()
	{
		SfxSystem.OneShot("event:/sfx/inventory/wave_counter_ring");
		transform.localEulerAngles = new Vector3(0f, 0f, 20f);
		LeanTween.cancel(transform.gameObject);
		LeanTween.rotateLocal(transform.gameObject, Vector3.zero, 1.5f).setEaseOutElastic();
		yield return earlyDeployRoutine;
	}
}
