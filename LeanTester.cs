#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class LeanTester : MonoBehaviour
{
	public float timeout = 15f;

	public void Start()
	{
		StartCoroutine(timeoutCheck());
	}

	public IEnumerator timeoutCheck()
	{
		float pauseEndTime = Time.realtimeSinceStartup + timeout;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}

		if (!LeanTest.testsFinished)
		{
			Debug.Log(LeanTest.formatB("Tests timed out!"));
			LeanTest.overview();
		}
	}
}
