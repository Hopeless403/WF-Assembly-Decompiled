#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific Effects/Temporary Trait", fileName = "Temporary Trait")]
public class StatusEffectTemporaryTrait : StatusEffectData
{
	[SerializeField]
	public TraitData trait;

	public List<Entity.TraitStacks> silenced;

	public Entity.TraitStacks added;

	public int addedAmount;

	public TraitData Trait => trait;

	public override bool HasStackRoutine => true;

	public override bool HasEndRoutine => true;

	public override IEnumerator StackRoutine(int stacks)
	{
		added = target.GainTrait(trait, stacks, temporary: true);
		yield return target.UpdateTraits();
		addedAmount += stacks;
		target.display.promptUpdateDescription = true;
		target.PromptUpdate();
	}

	public override IEnumerator EndRoutine()
	{
		if ((bool)target)
		{
			if (added != null)
			{
				added.count -= addedAmount;
				added.tempCount -= addedAmount;
			}

			addedAmount = 0;
			yield return target.UpdateTraits(added);
			target.display.promptUpdateDescription = true;
			target.PromptUpdate();
		}
	}
}
