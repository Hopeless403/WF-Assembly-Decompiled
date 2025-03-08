#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class TargetingArrowHeadRow : MonoBehaviour
{
	[SerializeField]
	public TargetingArrowSystem targetArrowSystem;

	[SerializeField]
	public Transform[] targetTransforms;

	[SerializeField]
	public SpriteRenderer[] targets;

	[SerializeField]
	public Sprite canTarget;

	[SerializeField]
	public Sprite cannotTarget;

	public List<CardSlot> slots;

	public void OnEnable()
	{
		if (!(targetArrowSystem.snapToContainer is CardSlotLane cardSlotLane))
		{
			return;
		}

		slots = cardSlotLane.slots;
		if (targetArrowSystem.target.data.playOnSlot)
		{
			for (int i = 0; i < targets.Length && i < slots.Count; i++)
			{
				SpriteRenderer obj = targets[i];
				CardSlot container = slots[i];
				obj.sprite = (targetArrowSystem.target.CanPlayOn(container, ignoreRowCheck: true) ? canTarget : cannotTarget);
			}
		}
		else
		{
			for (int j = 0; j < targets.Length && j < slots.Count; j++)
			{
				SpriteRenderer obj2 = targets[j];
				CardSlot cardSlot = slots[j];
				obj2.sprite = ((!cardSlot.Empty && targetArrowSystem.target.CanPlayOn(cardSlot.GetTop(), ignoreRowCheck: true)) ? canTarget : cannotTarget);
			}
		}
	}

	public void LateUpdate()
	{
		for (int i = 0; i < targetTransforms.Length && i < slots.Count; i++)
		{
			Transform obj = targetTransforms[i];
			CardSlot cardSlot = slots[i];
			obj.position = cardSlot.holder.position;
		}
	}
}
