#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Punch", menuName = "Card Animation/Punch")]
public class CardAnimationPunch : CardAnimation
{
	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public float duration = 1f;

	[SerializeField]
	public float animationDistance = 2.5f;

	[SerializeField]
	public float hitPos = 0.22f;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Trigger trigger)
		{
			yield return new WaitForSeconds(startDelay);
			Vector3 zero = Vector3.zero;
			Hit[] hits = trigger.hits;
			foreach (Hit hit in hits)
			{
				zero += hit.target.transform.position;
			}

			if (trigger.hits.Length != 0)
			{
				zero /= (float)trigger.hits.Length;
			}

			Vector3 offset = (zero - trigger.entity.transform.position).normalized * animationDistance;
			offset.z = -1f;
			trigger.entity.curveAnimator.Move(offset, curve, duration);
			yield return Sequences.Wait(duration * hitPos);
		}
	}
}
