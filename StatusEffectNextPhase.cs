#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Misc/Next Phase", fileName = "Next Phase")]
public class StatusEffectNextPhase : StatusEffectData
{
	[SerializeField]
	public CardData nextPhase;

	[SerializeField]
	public CardData[] splitOptions;

	[SerializeField]
	public int splitCount;

	[SerializeField]
	public CardAnimation animation;

	public bool goToNextPhase;

	public bool activated;

	public override void Init()
	{
		Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
	}

	public void OnDestroy()
	{
		Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		if (!activated && target.hp.current <= 0 && entity == target)
		{
			TryActivate();
		}
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (!activated && hit.target == target && target.hp.current <= 0)
		{
			TryActivate();
		}

		return false;
	}

	public void TryActivate()
	{
		bool flag = true;
		foreach (StatusEffectData statusEffect in target.statusEffects)
		{
			if (statusEffect != this && statusEffect.preventDeath)
			{
				flag = false;
				break;
			}
		}

		if (!flag)
		{
			return;
		}

		activated = true;
		UnityEngine.Object.FindObjectOfType<ChangePhaseAnimationSystem>()?.Flash();
		if ((bool)nextPhase)
		{
			ActionQueue.Stack(new ActionChangePhase(target, nextPhase.Clone(), animation)
			{
				priority = 10
			}, fixedPosition: true);
			return;
		}

		if (splitCount > 0)
		{
			CardData[] array = splitOptions;
			if (array != null && array.Length > 0)
			{
				CardData[] newPhases = (from a in splitOptions.RandomItems(splitCount)
					select a.Clone()).ToArray();
				ActionQueue.Stack(new ActionChangePhase(target, newPhases, animation)
				{
					priority = 10
				}, fixedPosition: true);
				return;
			}
		}

		throw new ArgumentException("Next phase not given!");
	}
}
