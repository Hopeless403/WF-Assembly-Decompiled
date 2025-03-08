#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Destroy Cards In Hand And Apply X For Each", fileName = "Instant Destroy Cards In Hand And Apply X For Each")]
public class StatusEffectInstantDestroyCardsInHandAndApplyXForEach : StatusEffectInstant
{
	[SerializeField]
	public TargetConstraint[] destroyConstraints;

	[SerializeField]
	public StatusEffectInstant destroyCardEffect;

	[SerializeField]
	public StatusEffectData effectToApply;

	public int destroyed;

	public override IEnumerator Process()
	{
		Character player = References.Player;
		int a = GetAmount();
		yield return DestroyCardsSequence(player.handContainer);
		for (int i = 0; i < destroyed; i++)
		{
			yield return StatusEffectSystem.Apply(target, target, effectToApply, a);
		}

		yield return base.Process();
	}

	public IEnumerator DestroyCardsSequence(CardContainer container)
	{
		bool pingDone = false;
		List<Entity> list = new List<Entity>(container);
		foreach (Entity item in list)
		{
			if (CheckConstraints(item))
			{
				if (!pingDone)
				{
					target.curveAnimator.Ping();
					pingDone = true;
				}

				destroyed++;
				yield return StatusEffectSystem.Apply(item, target, destroyCardEffect, 1, temporary: true);
			}
		}
	}

	public bool CheckConstraints(Entity card)
	{
		TargetConstraint[] array = destroyConstraints;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Check(card))
			{
				return false;
			}
		}

		return true;
	}
}
