#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trigger
{
	public Entity entity;

	public Entity triggeredBy;

	public Entity[] targets;

	public bool nullified;

	public Hit[] hits;

	public bool countsAsTrigger = true;

	public string type = "basic";

	public bool triggerAgainst;

	public Entity triggerAgainstTarget;

	public CardContainer triggerAgainstContainer;

	public Trigger(Entity entity, Entity triggeredBy, string type, Entity[] targets)
	{
		this.entity = entity;
		this.triggeredBy = triggeredBy;
		this.type = type;
		this.targets = targets;
	}

	public IEnumerator Process()
	{
		string attackerString = this.entity.name;
		string targetsString = ((targets == null || targets.Length == 0) ? "null" : "");
		if (targets != null)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				if (i > 0)
				{
					targetsString += ", ";
				}

				Entity entity = targets[i];
				targetsString += (entity ? entity.name : "null");
			}
		}

		Debug.Log("CardPlayAgainst [" + attackerString + " vs " + targetsString + "]");
		yield return PreProcess();
		if (this.entity.IsAliveAndExists())
		{
			if (hits.Length != 0)
			{
				yield return Animate();
			}

			yield return ProcessHits();
			yield return PostProcess();
		}

		Debug.Log("CardPlayAgainst [" + attackerString + " vs " + targetsString + "] DONE");
	}

	public virtual IEnumerator PreProcess()
	{
		yield return StatusEffectSystem.PreCardPlayedEvent(entity, targets);
		if (!entity.IsAliveAndExists())
		{
			yield break;
		}

		if (hits == null)
		{
			Trigger trigger = this;
			Entity[] array = targets;
			trigger.hits = new Hit[(array != null) ? array.Length : 0];
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					Hit hit = new Hit(entity, targets[i]);
					hit.AddAttackerStatuses();
					hit.trigger = this;
					hits[i] = hit;
				}
			}
		}

		Hit[] array2 = hits;
		foreach (Hit hit2 in array2)
		{
			yield return StatusEffectSystem.PreAttackEvent(hit2);
		}
	}

	public virtual IEnumerator Animate()
	{
		if (entity.HasAttackIcon())
		{
			CardAnimation cardAnimation = AssetLoader.Lookup<CardAnimation>("CardAnimations", "Punch");
			yield return cardAnimation.Routine(this);
		}
		else
		{
			CardAnimation cardAnimation2 = AssetLoader.Lookup<CardAnimation>("CardAnimations", "Supportive");
			yield return cardAnimation2.Routine(this);
		}
	}

	public virtual IEnumerator ProcessHits()
	{
		List<Entity> list = new List<Entity>();
		Routine.Clump clump = new Routine.Clump();
		Hit[] array = hits;
		foreach (Hit hit in array)
		{
			clump.Add(ProcessHit(hit));
			list.Add(hit.target);
		}

		yield return clump.WaitForEnd();
	}

	public virtual IEnumerator PostProcess()
	{
		if (countsAsTrigger)
		{
			yield return StatusEffectSystem.CardPlayedEvent(entity, hits.Select((Hit hit) => hit.target).ToArray());
		}
	}

	public static IEnumerator ProcessHit(Hit hit)
	{
		yield return hit.Process();
		yield return StatusEffectSystem.PostAttackEvent(hit);
	}

	public static CardContainer GetTargetRow(Entity attacker, Entity target)
	{
		CardContainer cardContainer = null;
		int[] rowIndices = GetRowIndices(attacker);
		int[] rowIndices2 = GetRowIndices(target);
		if (rowIndices == null || rowIndices2 == null)
		{
			return null;
		}

		foreach (int item in rowIndices.Intersect(rowIndices2))
		{
			cardContainer = References.Battle.GetRow(target.owner, item);
		}

		if (!cardContainer)
		{
			cardContainer = ((target.containers.Length != 0) ? target.containers[0] : ((target.preContainers.Length != 0) ? target.preContainers[0] : null));
		}

		return cardContainer;
	}

	public static int[] GetRowIndices(Entity entity)
	{
		if (!entity.alive || !Battle.IsOnBoard(entity.containers))
		{
			if (!Battle.IsOnBoard(entity.preContainers))
			{
				return null;
			}

			return References.Battle.GetRowIndices(entity.preContainers);
		}

		return References.Battle.GetRowIndices(entity);
	}
}
