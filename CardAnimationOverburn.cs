#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Overburn", menuName = "Card Animation/Overburn")]
public class CardAnimationOverburn : CardAnimation
{
	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Entity target)
		{
			yield return new WaitForSeconds(startDelay);
			CurveAnimator curveAnimator = target.curveAnimator;
			if ((object)curveAnimator != null)
			{
				curveAnimator.Scale(Vector3.one * 0.85f, Curves.Get("Buildup"), 0.67f);
				yield return new WaitForSeconds(0.67f);
			}
		}
	}
}
