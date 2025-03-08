#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Ping", menuName = "Card Animation/Ping")]
public class CardAnimationPing : CardAnimation
{
	[SerializeField]
	public bool waitForEnd;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (!(data is Entity target))
		{
			yield break;
		}

		yield return new WaitForSeconds(startDelay);
		CurveAnimator curveAnimator = target.curveAnimator;
		if ((object)curveAnimator != null)
		{
			curveAnimator.Ping();
			if (waitForEnd)
			{
				yield return new WaitForSeconds(curveAnimator.pingDuration);
			}
		}
	}
}
