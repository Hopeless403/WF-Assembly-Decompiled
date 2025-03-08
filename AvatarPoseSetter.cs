#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class AvatarPoseSetter : MonoBehaviour
{
	[SerializeField]
	public bool onAwake;

	[SerializeField]
	[HideIf("onAwake")]
	public bool onEnable = true;

	[SerializeField]
	public AvatarPoser poser;

	[SerializeField]
	public string[] poseOptions = new string[2] { "", "Greet" };

	[SerializeField]
	public Vector2 delay = new Vector2(0.5f, 1.5f);

	public void Awake()
	{
		if (onAwake)
		{
			StartCoroutine(Run());
		}
	}

	public void OnEnable()
	{
		if (!onAwake && onEnable)
		{
			StartCoroutine(Run());
		}
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public IEnumerator Run()
	{
		yield return new WaitForSeconds(delay.PettyRandom());
		if ((bool)poser && poseOptions != null && poseOptions.Length != 0)
		{
			poser.Set(poseOptions.RandomItem());
		}
	}
}
