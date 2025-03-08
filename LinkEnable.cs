#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class LinkEnable : MonoBehaviour
{
	public GameObject linkTo;

	public void OnEnable()
	{
		StartCoroutine(EnableAfterEndOfFrame());
	}

	public IEnumerator EnableAfterEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		linkTo.gameObject.SetActive(value: true);
	}

	public void OnDisable()
	{
		StopAllCoroutines();
		linkTo.gameObject.SetActive(value: false);
	}
}
