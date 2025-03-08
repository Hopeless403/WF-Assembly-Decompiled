#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class EndTransition : MonoBehaviour
{
	[SerializeField]
	public bool destroyObject;

	public IEnumerator Start()
	{
		yield return new WaitUntil(() => SceneManager.HasNoActiveJobs);
		Transition.End();
		if (destroyObject)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			Object.Destroy(this);
		}
	}
}
