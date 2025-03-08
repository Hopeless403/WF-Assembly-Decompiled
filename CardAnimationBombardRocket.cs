#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BombardRocket", menuName = "Card Animation/Bombard Rocket")]
public class CardAnimationBombardRocket : CardAnimation
{
	[Header("Rocket")]
	[SerializeField]
	public BombardRocket rocketPrefab;

	[SerializeField]
	public float rocketDuration = 0.67f;

	[SerializeField]
	public Vector3 startPosOffset = new Vector3(0f, 10f, 0f);

	[SerializeField]
	public Vector3 endPosOffset = new Vector3(0f, 0.1f, 0f);

	[SerializeField]
	public AnimationCurve rocketMoveCurve;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Vector3)
		{
			Vector3 vector = (Vector3)data;
			Vector3 to = vector + endPosOffset;
			Vector3 position = vector + startPosOffset;
			BombardRocket rocket = Object.Instantiate(rocketPrefab, position, Quaternion.identity);
			LeanTween.move(rocket.gameObject, to, rocketDuration).setEase(rocketMoveCurve);
			Events.InvokeBombardRocketFall(rocket);
			yield return new WaitForSeconds(rocketDuration);
			Events.InvokeBombardRocketExplode(rocket);
			rocket.Explode();
		}
	}
}
