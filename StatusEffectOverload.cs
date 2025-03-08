#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Overload", fileName = "Overload")]
public class StatusEffectOverload : StatusEffectData
{
	[SerializeField]
	public CardAnimation buildupAnimation;

	public bool overloading;

	public override void Init()
	{
		base.OnStack += Stack;
		Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
	}

	public void OnDestroy()
	{
		Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		if (entity == target && target.enabled)
		{
			Check();
		}
	}

	public IEnumerator Stack(int stacks)
	{
		Check();
		yield return null;
	}

	public void Check()
	{
		if (count >= target.hp.current && !overloading)
		{
			ActionQueue.Stack(new ActionSequence(DealDamage())
			{
				fixedPosition = true,
				priority = eventPriority,
				note = "Overload"
			});
			ActionQueue.Stack(new ActionSequence(Clear())
			{
				fixedPosition = true,
				priority = eventPriority,
				note = "Clear Overload"
			});
			overloading = true;
		}
	}

	public IEnumerator DealDamage()
	{
		if (!this || !target || !target.alive)
		{
			yield break;
		}

		HashSet<Entity> targets = new HashSet<Entity>();
		CardContainer[] containers = target.containers;
		foreach (CardContainer collection in containers)
		{
			targets.AddRange(collection);
		}

		if ((bool)buildupAnimation)
		{
			yield return buildupAnimation.Routine(target);
		}

		Entity damager = GetDamager();
		Routine.Clump clump = new Routine.Clump();
		foreach (Entity item in targets)
		{
			Hit hit = new Hit(damager, item, count)
			{
				damageType = "overload"
			};
			clump.Add(hit.Process());
		}

		clump.Add(Sequences.Wait(0.5f));
		yield return clump.WaitForEnd();
	}

	public IEnumerator Clear()
	{
		if ((bool)this && (bool)target && target.alive)
		{
			yield return Remove();
			overloading = false;
		}
	}
}
