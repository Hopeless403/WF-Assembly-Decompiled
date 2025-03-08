#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Supportive", menuName = "Card Animation/Supportive")]
public class CardAnimationSupportive : CardAnimation
{
	[SerializeField]
	public float hitPos = 0.2f;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Trigger trigger)
		{
			yield return new WaitForSeconds(startDelay);
			float num = trigger.entity.curveAnimator.Ping();
			yield return Sequences.Wait(num * hitPos);
		}
	}
}
