#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Lose Trait", fileName = "Lose Trait")]
public class StatusEffectInstantLoseTrait : StatusEffectInstant
{
	public TraitData traitToLose;

	public override IEnumerator Process()
	{
		Entity.TraitStacks traitStacks = target.traits.FirstOrDefault((Entity.TraitStacks t) => t.data == traitToLose);
		if (traitStacks != null)
		{
			traitStacks.count = 0;
			yield return target.UpdateTraits(traitStacks);
			target.display.promptUpdateDescription = true;
			target.PromptUpdate();
		}

		List<StatusEffectData> list = target.statusEffects.Where((StatusEffectData a) => a is StatusEffectTemporaryTrait statusEffectTemporaryTrait && statusEffectTemporaryTrait.Trait == traitToLose).ToList();
		foreach (StatusEffectData item in list)
		{
			yield return item.Remove();
		}

		yield return base.Process();
	}
}
