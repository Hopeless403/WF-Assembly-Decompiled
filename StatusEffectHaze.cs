#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Haze", fileName = "Haze")]
public class StatusEffectHaze : StatusEffectData
{
	public override void Init()
	{
		base.PreTrigger += EntityPreTrigger;
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		if (trigger.entity == target)
		{
			return trigger.type != "haze";
		}

		return false;
	}

	public IEnumerator EntityPreTrigger(Trigger trigger)
	{
		if (TryTargetRandomAlly(trigger))
		{
			trigger.type = "haze";
		}
		else
		{
			trigger.nullified = true;
			if (NoTargetTextSystem.Exists())
			{
				yield return NoTargetTextSystem.Run(target, NoTargetType.NoAllyToAttack);
			}
		}

		yield return RemoveStacks(1, removeTemporary: false);
	}

	public bool TryTargetRandomAlly(Trigger trigger)
	{
		bool result = false;
		Entity randomAlly = GetRandomAlly();
		if ((bool)randomAlly)
		{
			CardContainer targetContainer = randomAlly.containers.RandomItem();
			Entity[] subsequentTargets = target.targetMode.GetSubsequentTargets(trigger.entity, randomAlly, targetContainer);
			trigger.targets = subsequentTargets;
			result = true;
		}

		return result;
	}

	public Entity GetRandomAlly()
	{
		List<Entity> allAllies = target.GetAllAllies();
		if (allAllies.Count > 0)
		{
			return allAllies.RandomItem();
		}

		return null;
	}
}
