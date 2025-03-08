#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Trigger", fileName = "Trigger")]
public class StatusEffectInstantTrigger : StatusEffectInstant
{
	[SerializeField]
	public bool againstRandomEnemy;

	[SerializeField]
	public bool reduceUses;

	[SerializeField]
	public int priority = -1;

	public override IEnumerator Process()
	{
		if (againstRandomEnemy && target.NeedsTarget)
		{
			List<Entity> allEnemies = target.GetAllEnemies();
			if (allEnemies.Count > 0)
			{
				Entity entity = allEnemies.RandomItem();
				CardContainer targetContainer = entity.containers.RandomItem();
				ActionQueue.Stack(new ActionTriggerAgainst(target, applier, entity, targetContainer), fixedPosition: true);
			}
		}
		else
		{
			ActionQueue.Stack(new ActionTrigger(target, applier)
			{
				priority = priority
			}, fixedPosition: true);
		}

		if (reduceUses)
		{
			ActionQueue.Add(new ActionReduceUses(target));
		}

		yield return base.Process();
	}
}
