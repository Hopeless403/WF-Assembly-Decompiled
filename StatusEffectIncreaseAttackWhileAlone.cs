#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Increase Attack While Alone", fileName = "Increase Attack While Alone")]
public class StatusEffectIncreaseAttackWhileAlone : StatusEffectData
{
	[SerializeField]
	public bool active;

	[SerializeField]
	public int currentBonus;

	public override bool HasStackRoutine => active;

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (entity == target && active && !CanTrigger())
		{
			Deactivate();
		}
		else if ((entity.preContainers.ContainsAny(target.containers) || target.containers.ContainsAny(entity.containers)) && CanTrigger())
		{
			Check();
		}

		return false;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		Check();
		return false;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (CanTrigger())
		{
			Check();
		}

		return false;
	}

	public override bool RunEffectBonusChangedEvent()
	{
		if (active)
		{
			Deactivate();
			Activate();
		}

		return false;
	}

	public void Check()
	{
		int num = CountOthersInMyRows();
		if (!active)
		{
			if (num <= 0)
			{
				Activate();
			}
		}
		else if (num > 0)
		{
			Deactivate();
		}
	}

	public override bool RunStackEvent(int stacks)
	{
		if (active)
		{
			currentBonus += stacks;
			target.tempDamage += stacks;
			target.PromptUpdate();
		}

		return false;
	}

	public int CountOthersInMyRows()
	{
		int num = 0;
		CardContainer[] containers = target.containers;
		for (int i = 0; i < containers.Length; i++)
		{
			foreach (Entity item in containers[i])
			{
				if (item.IsAliveAndExists() && item != target)
				{
					num++;
				}
			}
		}

		return num;
	}

	public void Activate()
	{
		active = true;
		currentBonus = GetAmount();
		target.tempDamage += currentBonus;
		target.PromptUpdate();
	}

	public void Deactivate()
	{
		active = false;
		target.tempDamage -= currentBonus;
		target.PromptUpdate();
	}

	public override bool CanTrigger()
	{
		if (base.CanTrigger())
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}
}
