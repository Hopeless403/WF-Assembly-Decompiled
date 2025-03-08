#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Fill Board", fileName = "Fill Board")]
public class StatusEffectInstantFillBoard : StatusEffectInstant
{
	[SerializeField]
	public bool doPing = true;

	[SerializeField]
	public EventReference sfxEvent;

	[SerializeField]
	public CardData[] withCards;

	public readonly List<CardData> pool = new List<CardData>();

	public override IEnumerator Process()
	{
		if (doPing)
		{
			target.curveAnimator.Ping();
			SfxSystem.OneShot(sfxEvent);
			Events.InvokeScreenRumble(0f, 0.4f, 0f, 0.4f, 0.1f, 0.4f);
		}

		List<CardContainer> rows = References.Battle.GetRows(target.owner);
		List<CardSlot> list = new List<CardSlot>();
		foreach (CardContainer item in rows)
		{
			if (item is CardSlotLane cardSlotLane)
			{
				list.AddRange(cardSlotLane.slots.Where((CardSlot slot) => slot.Empty));
			}
		}

		foreach (CardSlot slot2 in list)
		{
			CardData data = Pull().Clone();
			Card card = CardManager.Get(data, References.Battle.playerCardController, target.owner, inPlay: true, target.owner.team == References.Player.team);
			yield return card.UpdateData();
			target.owner.reserveContainer.Add(card.entity);
			target.owner.reserveContainer.SetChildPosition(card.entity);
			ActionQueue.Stack(new ActionMove(card.entity, slot2), fixedPosition: true);
			ActionQueue.Stack(new ActionRunEnableEvent(card.entity), fixedPosition: true);
		}

		yield return base.Process();
	}

	public CardData Pull()
	{
		if (pool.Count <= 0)
		{
			pool.AddRange(withCards);
		}

		return pool.TakeRandom();
	}
}
