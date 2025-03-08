#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class BombardRocket : MonoBehaviour
{
	[SerializeField]
	public ParticleSystem rocketTrail;

	[SerializeField]
	public GameObject rocket;

	[SerializeField]
	public ParticleSystem explosion;

	[SerializeField]
	public float explosionShakeAmount = 2f;

	public void Explode()
	{
		StartCoroutine(ExplodeRoutine());
	}

	public IEnumerator ExplodeRoutine()
	{
		rocketTrail.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
		rocket.Destroy();
		explosion.Play();
		Events.InvokeScreenShake(explosionShakeAmount, 180f);
		yield return new WaitForSeconds(4f);
		base.gameObject.Destroy();
	}
}
