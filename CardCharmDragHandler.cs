#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using Dead;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class CardCharmDragHandler : MonoBehaviour
{
	[SerializeField]
	public bool canDragMidBattle = true;

	[SerializeField]
	public EventReference denySfxEvent;

	[SerializeField]
	public CardContainer[] assignmentContainers;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public UpgradeHolder dragHolder;

	[SerializeField]
	public UnityEvent onAssign;

	[SerializeField]
	public AssignCharmSequence assignSequence;

	[SerializeField]
	public bool instantAssign = true;

	public UpgradeDisplay dragging;

	public UpgradeHolder preHolder;

	public int preIndex;

	public Entity hoverEntity;

	public List<Entity> eligibleCards;

	public List<Entity> ineligibleCards;

	public readonly Routine.Clump flipClump = new Routine.Clump();

	public bool IsDragging { get; set; }

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
		StopAllCoroutines();
	}

	public void LateUpdate()
	{
		if (IsDragging)
		{
			UpdatePosition();
			if (InputSystem.IsButtonPressed("Back"))
			{
				CancelDrag();
			}
		}
	}

	public void UpdatePosition()
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			base.transform.position = Cursor3d.Position;
			return;
		}

		UINavigationItem currentNavigationItem = MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem;
		if ((object)currentNavigationItem != null)
		{
			base.transform.position = currentNavigationItem.Position.WithZ(Cursor3d.Position.z);
		}
	}

	public void Drag(UpgradeDisplay upgrade)
	{
		if (!canDragMidBattle && (bool)References.Battle && !References.Battle.ended)
		{
			SfxSystem.OneShot(denySfxEvent);
			return;
		}

		TouchInputModule.BlockScroll();
		preHolder = upgrade.GetComponentInParent<UpgradeHolder>();
		if ((bool)preHolder)
		{
			preIndex = preHolder.IndexOf(upgrade);
			preHolder.Remove(upgrade);
			preHolder.SetPositions();
		}

		dragHolder.Add(upgrade);
		dragHolder.SetPositions();
		UpdatePosition();
		Events.InvokeUpgradePickup(upgrade);
		dragging = upgrade;
		IsDragging = true;
		if (eligibleCards == null)
		{
			eligibleCards = new List<Entity>();
		}

		if (ineligibleCards == null)
		{
			ineligibleCards = new List<Entity>();
		}

		CardContainer[] array = assignmentContainers;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Entity item in array[i])
			{
				if (dragging.data.CanAssign(item))
				{
					eligibleCards.Add(item);
					item.flipper.FlipUp();
				}
				else
				{
					ineligibleCards.Add(item);
					item.flipper.flipped = true;
				}
			}
		}

		StopAllCoroutines();
		StartCoroutine(FlipCardsDown(ineligibleCards));
		NavigationState.Start(new NavigationStateAssignUpgrade(eligibleCards));
		cardController.canPress = false;
	}

	public void Release(UpgradeDisplay upgrade)
	{
		if (dragging != upgrade)
		{
			return;
		}

		if ((bool)hoverEntity && eligibleCards != null && eligibleCards.Contains(hoverEntity))
		{
			dragHolder.Remove(dragging);
			new Routine(Assign(dragging, hoverEntity));
			return;
		}

		if ((bool)preHolder)
		{
			ReturnToHolder();
		}

		DragEnd();
	}

	public void DragEnd()
	{
		TouchInputModule.UnblockScroll();
		dragging = null;
		IsDragging = false;
		StopAllCoroutines();
		StartCoroutine(FlipCardsUp(ineligibleCards.ToArray()));
		eligibleCards = null;
		ineligibleCards = null;
		NavigationState.BackToPreviousState();
		cardController.canPress = true;
	}

	public void ReturnToHolder()
	{
		dragHolder.Remove(dragging);
		preHolder.Insert((preIndex >= 0) ? preIndex : 0, dragging);
		dragging.transform.localPosition = Vector3.zero;
		preHolder.SetPositions();
		Events.InvokeUpgradeDrop(dragging);
	}

	public void CancelDrag()
	{
		if (IsDragging)
		{
			CardCharmInteraction component = dragging.GetComponent<CardCharmInteraction>();
			if ((object)component != null)
			{
				component.CancelDrag();
				ReturnToHolder();
				DragEnd();
			}
		}
	}

	public IEnumerator FlipCardsDown(IEnumerable<Entity> cards)
	{
		flipClump.Clear();
		foreach (Entity card in cards)
		{
			flipClump.Add(FlipDown(card, PettyRandom.Range(0f, 0.2f)));
		}

		yield return flipClump.WaitForEnd();
	}

	public static IEnumerator FlipDown(Entity card, float delay)
	{
		yield return new WaitForSeconds(delay);
		card.flipper.FlipDown(force: true);
	}

	public IEnumerator FlipCardsUp(IEnumerable<Entity> cards)
	{
		flipClump.Clear();
		foreach (Entity card in cards)
		{
			flipClump.Add(FlipUp(card, PettyRandom.Range(0f, 0.2f)));
		}

		yield return flipClump.WaitForEnd();
	}

	public static IEnumerator FlipUp(Entity card, float delay)
	{
		yield return new WaitForSeconds(delay);
		card.flipper.FlipUp();
	}

	public IEnumerator Assign(UpgradeDisplay upgrade, Entity entity)
	{
		cardController.Disable();
		NavigationState.Start(new NavigationStateWait(disableInput: true));
		CardUpgradeData upgradeData = upgrade.data;
		upgrade.gameObject.Destroy();
		if (instantAssign || upgradeData.type != CardUpgradeData.Type.Charm)
		{
			yield return upgradeData.Assign(entity);
		}
		else
		{
			assignSequence.Assign(entity, upgradeData);
			yield return assignSequence.Run();
		}

		cardController.Enable();
		NavigationState.BackToPreviousState();
		DragEnd();
		if ((bool)cardController.owner)
		{
			cardController.owner.data.inventory.upgrades.Remove(upgradeData);
		}

		onAssign?.Invoke();
	}

	public void EntityHover(Entity entity)
	{
		if (eligibleCards != null && eligibleCards.Contains(entity))
		{
			hoverEntity = entity;
		}
	}

	public void EntityUnHover(Entity entity)
	{
		if (hoverEntity == entity)
		{
			hoverEntity = null;
		}
	}
}
