#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Miniboss Intro", menuName = "Card Animation/Miniboss Intro")]
public class CardAnimationMinibossIntro : CardAnimation
{
	[SerializeField]
	public float rumbleAmount = 1f;

	[SerializeField]
	public float wobbleAmount = 1f;

	[SerializeField]
	public CurveProfile scaleTween;

	[SerializeField]
	public Vector3 scaleTo = new Vector3(1f, 1f, 1f);

	[SerializeField]
	public CurveProfile rotateTween;

	[SerializeField]
	public Vector3 rotateAmount = new Vector3(1f, 1f, 5f);

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Entity target)
		{
			yield return new WaitForSeconds(startDelay);
			float num = Mathf.Max(scaleTween.duration, rotateTween.duration);
			Events.InvokeScreenRumble(0f, rumbleAmount, 0f, num * 0.5f, num * 0.5f, num * 0.5f);
			target.wobbler?.WobbleRandom(wobbleAmount);
			LeanTween.scale(target.gameObject, scaleTo, scaleTween.duration).setEase(scaleTween.curve);
			LeanTween.rotateLocal(target.gameObject, rotateAmount, rotateTween.duration).setEase(rotateTween.curve);
			yield return new WaitForSeconds(num);
		}
	}
}
