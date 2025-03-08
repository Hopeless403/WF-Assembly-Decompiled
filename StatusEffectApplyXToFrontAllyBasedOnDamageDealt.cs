#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X To Front Ally Based On Damage Dealt", fileName = "Apply X To Front Ally Based On Damage Dealt")]
public class StatusEffectApplyXToFrontAllyBasedOnDamageDealt : StatusEffectApplyX
{
	public Dictionary<Entity, int[]> entitiesHitInRows = new Dictionary<Entity, int[]>();

	public Dictionary<int, int> damageDealtInRows = new Dictionary<int, int>();

	public override void Init()
	{
		base.OnCardPlayed += Check;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.attacker == target && hit.target != null && hit.Offensive)
		{
			entitiesHitInRows.Add(hit.target, Battle.instance.GetRowIndices(hit.target));
		}

		return false;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (hit.attacker == target && entitiesHitInRows.ContainsKey(hit.target) && hit.damageDealt > 0)
		{
			int[] array = entitiesHitInRows[hit.target];
			int damageDealt = hit.damageDealt;
			int[] array2 = array;
			foreach (int key in array2)
			{
				if (damageDealtInRows.ContainsKey(key))
				{
					damageDealtInRows[key] += damageDealt;
				}
				else
				{
					damageDealtInRows[key] = damageDealt;
				}
			}
		}

		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target)
		{
			if (damageDealtInRows.Count <= 0)
			{
				return entitiesHitInRows.Count > 0;
			}

			return true;
		}

		return false;
	}

	public IEnumerator Check(Entity entity, Entity[] targets)
	{
		Dictionary<Entity, int> toAffect = new Dictionary<Entity, int>();
		foreach (int key in damageDealtInRows.Keys)
		{
			CardContainer row = Battle.instance.GetRow(target.owner, key);
			if (row != null)
			{
				Entity top = row.GetTop();
				if (top != null)
				{
					toAffect[top] = damageDealtInRows[key];
				}
			}
		}

		if (toAffect.Count > 0)
		{
			yield return Sequences.WaitForAnimationEnd(target);
			target.curveAnimator.Ping();
			yield return Sequences.Wait(0.1f);
			Routine.Clump clump = new Routine.Clump();
			foreach (Entity key2 in toAffect.Keys)
			{
				int num = toAffect[key2];
				Hit hit = new Hit(target, key2, 0);
				hit.AddStatusEffect(effectToApply, num);
				clump.Add(hit.Process());
				if (doPing)
				{
					key2.curveAnimator.Ping();
				}
			}

			yield return clump.WaitForEnd();
		}

		yield return Sequences.Wait(0.1f);
		damageDealtInRows.Clear();
		entitiesHitInRows.Clear();
	}
}
