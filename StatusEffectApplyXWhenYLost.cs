#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Y Lost", fileName = "Apply X When Y Lost")]
public class StatusEffectApplyXWhenYLost : StatusEffectApplyX
{
	[SerializeField]
	public string statusType = "block";

	[SerializeField]
	public bool whenAllLost;

	public bool active;

	public int currentAmount;

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
		currentAmount = GetCurrentAmount();
		return false;
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		int num = GetCurrentAmount();
		if (active && num != currentAmount && entity == target)
		{
			int num2 = num - currentAmount;
			currentAmount = num;
			if (num2 < 0 && (!whenAllLost || currentAmount == 0) && target.enabled && !target.silenced && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target))))
			{
				ActionQueue.Stack(new ActionSequence(Lost(-num2))
				{
					note = base.name,
					priority = eventPriority
				}, fixedPosition: true);
			}
		}
	}

	public IEnumerator Lost(int amount)
	{
		if ((bool)this && target.IsAliveAndExists())
		{
			yield return Run(GetTargets(), amount);
		}
	}

	public int GetCurrentAmount()
	{
		return target.FindStatus(statusType)?.count ?? 0;
	}
}
