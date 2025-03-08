#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BombardRocketShoot", menuName = "Card Animation/Bombard Rocket Shoot")]
public class CardAnimationBombardRocketShoot : CardAnimation
{
	[Header("Shoot Particles")]
	[SerializeField]
	public ParticleSystem shootFxPrefab;

	[SerializeField]
	public Vector3 shootAngle = new Vector3(0f, 0f, 135f);

	[SerializeField]
	public Vector3 shootFxOffset = new Vector3(0f, 1f, 0f);

	[SerializeField]
	public float shootScreenShake = 1f;

	[Header("Recoil Animation")]
	[SerializeField]
	public Vector3 recoilOffset = new Vector3(1f, -1f, 0f);

	[SerializeField]
	public AnimationCurve recoilCurve;

	[SerializeField]
	public float recoilDuration = 1f;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Entity entity)
		{
			ParticleSystem shootFx = Object.Instantiate(shootFxPrefab, entity.transform.position + shootFxOffset, Quaternion.Euler(shootAngle));
			Events.InvokeScreenShake(shootScreenShake, shootAngle.z + 180f);
			Events.InvokeBombardShoot(entity);
			entity.curveAnimator?.Move(recoilOffset, recoilCurve, 1f, 1f);
			yield return new WaitUntil(() => !shootFx);
		}
	}
}
