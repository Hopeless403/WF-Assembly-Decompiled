#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCardAnimation : MonoBehaviour
{
	public virtual IEnumerator Run(Entity entity, params CardData.StatusEffectStacks[] withEffects)
	{
		yield return GainEffects(entity, withEffects);
		base.gameObject.Destroy();
	}

	public IEnumerator DestroyOnEnd(ParticleSystem ps)
	{
		yield return new WaitUntil(() => !this || !ps || !ps.isPlaying);
		if ((bool)this && (bool)base.gameObject)
		{
			base.gameObject.Destroy();
		}
	}

	public static IEnumerator GainEffects(Entity entity, IEnumerable<CardData.StatusEffectStacks> withEffects)
	{
		Routine.Clump clump = new Routine.Clump();
		foreach (CardData.StatusEffectStacks withEffect in withEffects)
		{
			clump.Add(StatusEffectSystem.Apply(entity, null, withEffect.data, withEffect.count));
		}

		yield return clump.WaitForEnd();
	}
}
