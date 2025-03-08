#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class SfxOneshot : MonoBehaviour
{
	[SerializeField]
	public EventReference eventRef;

	[SerializeField]
	[HideIf("onEnable")]
	public bool onAwake;

	[SerializeField]
	[HideIf("onAwake")]
	public bool onEnable = true;

	[SerializeField]
	public float pitch = 1f;

	[SerializeField]
	public float delay;

	public void Awake()
	{
		if (onAwake && !onEnable)
		{
			if (delay > 0f)
			{
				StartCoroutine(FireAfterDelay());
			}
			else
			{
				Fire();
			}
		}
	}

	public void OnEnable()
	{
		if (onEnable)
		{
			if (delay > 0f)
			{
				StartCoroutine(FireAfterDelay());
			}
			else
			{
				Fire();
			}
		}
	}

	public void Fire()
	{
		SfxSystem.OneShot(eventRef).setPitch(pitch);
	}

	public IEnumerator FireAfterDelay()
	{
		yield return new WaitForSeconds(delay);
		Fire();
	}
}
