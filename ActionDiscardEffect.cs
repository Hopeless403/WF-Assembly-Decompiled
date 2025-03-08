#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class ActionDiscardEffect : PlayAction
{
	public readonly Entity target;

	public readonly int healAmount;

	public ActionDiscardEffect(Entity target, int healAmount)
	{
		this.target = target;
		this.healAmount = healAmount;
	}

	public override IEnumerator Run()
	{
		Routine.Clump clump = new Routine.Clump();
		if (target.data.hasHealth)
		{
			target.hp.current = Mathf.Min(target.hp.current + healAmount, target.hp.max);
			target.PromptUpdate();
			target.curveAnimator?.Ping();
			clump.Add(Sequences.Wait(0.6f));
			SfxSystem.OneShot("event:/sfx/status/heal");
		}

		clump.Add(RemoveStatuses());
		yield return clump.WaitForEnd();
	}

	public IEnumerator RemoveStatuses()
	{
		for (int i = target.statusEffects.Count - 1; i >= 0; i--)
		{
			StatusEffectData statusEffectData = target.statusEffects[i];
			if ((bool)statusEffectData && statusEffectData.removeOnDiscard)
			{
				yield return statusEffectData.Remove();
			}
		}
	}
}
