#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Health Lost", fileName = "Apply X When Health Lost")]
public class StatusEffectApplyXWhenHealthLost : StatusEffectApplyX
{
	[SerializeField]
	public bool hasThreshold;

	public bool active;

	public int currentHealth;

	public override void Init()
	{
		Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
	}

	public void OnDestroy()
	{
		Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
	}

	public override bool RunBeginEvent()
	{
		active = true;
		currentHealth = target.hp.current;
		return false;
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		if (active && target.hp.current != currentHealth && entity == target)
		{
			int num = target.hp.current - currentHealth;
			currentHealth = target.hp.current;
			if (num < 0 && target.enabled && !target.silenced && CheckThreshold() && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target))))
			{
				ActionQueue.Stack(new ActionSequence(HealthLost(-num))
				{
					note = base.name,
					priority = eventPriority
				}, fixedPosition: true);
			}
		}
	}

	public bool CheckThreshold()
	{
		if (hasThreshold)
		{
			return target.hp.current <= target.hp.max - GetAmount();
		}

		return true;
	}

	public IEnumerator HealthLost(int amount)
	{
		if ((bool)this && target.IsAliveAndExists())
		{
			yield return Run(GetTargets(), amount);
		}
	}
}
