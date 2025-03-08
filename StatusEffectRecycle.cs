#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Recycle", fileName = "Recycle")]
public class StatusEffectRecycle : StatusEffectData
{
	public string cardToRecycle = "Junk";

	public bool running;

	public readonly List<Entity> toDestroy = new List<Entity>();

	public override void Init()
	{
		Events.OnCheckAction += CheckAction;
		base.PreTrigger += EntityPreTrigger;
	}

	public void OnDestroy()
	{
		Events.OnCheckAction -= CheckAction;
	}

	public void CheckAction(ref PlayAction action, ref bool allow)
	{
		if (running || !target.enabled || target.silenced || !allow || !(action is ActionTrigger actionTrigger) || !(actionTrigger.entity == target))
		{
			return;
		}

		int amount = GetAmount();
		Events.CheckRecycleAmount(target, ref amount);
		if (amount > 0 && !GetTargets(amount))
		{
			allow = false;
			if (NoTargetTextSystem.Exists())
			{
				new Routine(NoTargetTextSystem.Run(target, NoTargetType.RequiresJunk, amount));
			}
		}
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		return toDestroy.Count > 0;
	}

	public IEnumerator EntityPreTrigger(Trigger trigger)
	{
		running = true;
		foreach (Entity item in toDestroy)
		{
			target.curveAnimator.Ping();
			yield return item.Kill();
		}

		toDestroy.Clear();
		running = false;
	}

	public bool GetTargets(int requiredAmount)
	{
		bool flag = false;
		toDestroy.Clear();
		foreach (Entity item in References.Player.handContainer)
		{
			if (item.data.name == cardToRecycle)
			{
				toDestroy.Add(item);
				if (--requiredAmount <= 0)
				{
					flag = true;
					break;
				}
			}
		}

		if (!flag)
		{
			toDestroy.Clear();
		}

		return flag;
	}

	public bool IsEnoughJunkInHand()
	{
		int num = GetAmount();
		foreach (Entity item in References.Player.handContainer)
		{
			if (item.data.name == cardToRecycle && --num <= 0)
			{
				return true;
			}
		}

		return false;
	}
}
