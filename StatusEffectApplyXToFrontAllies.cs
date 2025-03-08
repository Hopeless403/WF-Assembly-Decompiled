#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X To Front Allies", fileName = "Apply X To Front Allies")]
public class StatusEffectApplyXToFrontAllies : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnCardPlayed += Run;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		return entity == target;
	}

	public IEnumerator Run(Entity entity, Entity[] targets)
	{
		int a = GetAmount();
		List<Entity> toAffect = new List<Entity>();
		foreach (CardContainer row in Battle.instance.GetRows(target.owner))
		{
			toAffect.AddIfNotNull(row.GetTop());
		}

		if (toAffect.Count <= 0)
		{
			yield break;
		}

		target.curveAnimator.Ping();
		yield return Sequences.Wait(0.13f);
		Routine.Clump clump = new Routine.Clump();
		foreach (Entity item in toAffect)
		{
			Hit hit = new Hit(target, item, 0);
			hit.AddStatusEffect(effectToApply, a);
			clump.Add(hit.Process());
		}

		yield return clump.WaitForEnd();
		yield return Sequences.Wait(0.13f);
	}
}
