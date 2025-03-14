#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Free Action", fileName = "Free Action")]
public class StatusEffectFreeAction : StatusEffectData
{
	[SerializeField]
	public ItemHolderPet petPrefab;

	public bool hasEffect;

	public override void Init()
	{
		base.OnCardPlayed += CardPlayed;
	}

	public override bool RunBeginEvent()
	{
		if (!target.inPlay || target.enabled)
		{
			hasEffect = true;
			if (!GameManager.paused && target.display is Card card)
			{
				card.itemHolderPet?.Create(petPrefab);
				Events.InvokeNoomlinShow(target);
			}
		}

		return false;
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (entity == target)
		{
			if (target.InHand())
			{
				RunBeginEvent();
			}
			else if (hasEffect && !Battle.IsOnBoard(entity.preContainers) && Battle.IsOnBoard(entity))
			{
				hasEffect = false;
				if (target.display is Card card)
				{
					card.itemHolderPet?.Used();
				}

				target.owner.freeAction = true;
			}
		}

		return false;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (entity == target)
		{
			RunBeginEvent();
		}

		return false;
	}

	public override bool RunDisableEvent(Entity entity)
	{
		if (entity == target)
		{
			RunEndEvent();
		}

		return false;
	}

	public override bool RunEndEvent()
	{
		hasEffect = false;
		if (target != null && target.display is Card card)
		{
			card.itemHolderPet?.DestroyCurrent();
		}

		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (hasEffect && entity == target)
		{
			return !target.silenced;
		}

		return false;
	}

	public IEnumerator CardPlayed(Entity entity, Entity[] targets)
	{
		hasEffect = false;
		if (target.display is Card card)
		{
			card.itemHolderPet?.Used();
			Events.InvokeNoomlinUsed(target);
			Mover mover = card.gameObject.AddComponent<Mover>();
			mover.velocity = new Vector3(PettyRandom.Range(0f, 1f).WithRandomSign(), -12f, 0f);
			mover.frictMult = 0.8f;
			target.wobbler?.WobbleRandom();
			yield return Sequences.Wait(0.6f);
		}

		target.owner.freeAction = true;
	}
}
