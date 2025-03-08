#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticleSystem : GameSystem
{
	[SerializeField]
	public ParticleSystem fxPrefab;

	[SerializeField]
	public Transform group;

	public readonly List<ParticleSystem> pool = new List<ParticleSystem>();

	public void Update()
	{
		if (InputSystem.IsSelectPressed())
		{
			Pop();
		}
	}

	public void Pop()
	{
		ParticleSystem particleSystem = Get();
		particleSystem.transform.position = Cursor3d.PositionWithZ;
		particleSystem.Play(withChildren: true);
		StartCoroutine(ReturnToPoolWhenFinished(particleSystem));
	}

	public IEnumerator ReturnToPoolWhenFinished(ParticleSystem ps)
	{
		while (ps.isPlaying)
		{
			yield return null;
		}

		ps.gameObject.SetActive(value: false);
		pool.Add(ps);
	}

	public ParticleSystem Get()
	{
		if (pool.Count > 0)
		{
			ParticleSystem particleSystem = pool[0];
			pool.RemoveAt(0);
			particleSystem.gameObject.SetActive(value: true);
			return particleSystem;
		}

		return Object.Instantiate(fxPrefab, group);
	}
}
