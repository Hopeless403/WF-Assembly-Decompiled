#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class SetOtherActive : MonoBehaviour
{
	[SerializeField]
	public GameObject other;

	[SerializeField]
	public bool setOnEnable = true;

	[SerializeField]
	public float delay;

	[SerializeField]
	public bool setOnDisable;

	public void OnEnable()
	{
		if (delay > 0f)
		{
			StartCoroutine(SetActiveAfter(other, setOnEnable, delay));
		}
		else
		{
			other.SetActive(setOnEnable);
		}
	}

	public static IEnumerator SetActiveAfter(GameObject obj, bool active, float delay)
	{
		yield return new WaitForSeconds(delay);
		obj.SetActive(active);
	}

	public void OnDisable()
	{
		StopAllCoroutines();
		other.SetActive(setOnDisable);
	}
}
