#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class FlipCreateCardAnimation : CreateCardAnimation
{
	public static readonly Vector3 startPos = new Vector3(0f, -10f, 0f);

	public override IEnumerator Run(Entity entity, params CardData.StatusEffectStacks[] withEffects)
	{
		entity.transform.localScale = Vector3.zero;
		entity.transform.position = startPos;
		if (entity.display is Card card)
		{
			card.canvasGroup.alpha = 1f;
		}

		entity.flipper.FlipDownInstant();
		entity.curveAnimator.Ping();
		yield return CreateCardAnimation.GainEffects(entity, withEffects);
		entity.flipper.FlipUp();
		entity.wobbler.WobbleRandom();
		base.gameObject.Destroy();
	}
}
