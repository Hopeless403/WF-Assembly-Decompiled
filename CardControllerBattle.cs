#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class CardControllerBattle : CardController
{
	public UINavigationItem useOnHandAnchor;

	public override void Press()
	{
		if ((bool)pressEntity && pressEntity.owner == owner)
		{
			Debug.Log("Pressing [" + pressEntity.name + "]");
			if (TryDrag(pressEntity))
			{
				UnHover(pressEntity);
				NavigationState.Start(new NavigationStateCard(pressEntity));
			}
		}
	}

	public override void DragUpdatePosition()
	{
		Vector3 vector = GetDragPosition();
		if ((bool)hoverContainer && hoverContainer.canBePlacedOn && hoverContainer == owner.discardContainer && dragging.CanRecall())
		{
			vector = hoverContainer.transform.position - Vector3.Scale(dragging.offset.localPosition, dragging.transform.localScale);
			dragging.transform.position = Delta.Lerp(dragging.transform.position, vector, dragLerp, Time.deltaTime);
		}
		else
		{
			if (!dragging.data)
			{
				return;
			}

			if (dragging.data.playType == Card.PlayType.Play)
			{
				if (!dragging.NeedsTarget)
				{
					if ((bool)hoverContainer && dragging.containers.Length != 0 && hoverContainer == dragging.containers[0])
					{
						Vector3 positionFromContainers = dragging.GetPositionFromContainers();
						vector = positionFromContainers + Vector3.ClampMagnitude(vector - positionFromContainers, 0.2f);
					}

					dragging.transform.position = Delta.Lerp(dragging.transform.position, vector, dragLerp, Time.deltaTime);
					return;
				}

				if (dragging.NeedsTarget)
				{
					Vector3 vector2 = dragging.GetPositionFromContainers();
					if ((!dragging.targetMode.TargetRow || !hoverContainer || !hoverContainer.canPlayOn) && (bool)hoverEntity && hoverEntity != dragging && hoverEntity.InHand())
					{
						Vector3 position = useOnHandAnchor.transform.position;
						vector2 = position.WithX((position.x + vector.x) / 2f);
					}

					if (dragging.data.playOnSlot && dragging.CanPlayOn(hoverSlot))
					{
						Entity top = hoverSlot.GetTop();
						if ((object)top != null && ShoveSystem.CanShove(top, dragging, out var shoveData))
						{
							ShoveSystem.ShowShove(hoverSlot, shoveData);
						}
					}

					Vector3 target = vector2 + Vector3.ClampMagnitude(vector - vector2, 0.2f);
					dragging.transform.position = Delta.Lerp(dragging.transform.position, target, dragLerp, Time.deltaTime);
					return;
				}
			}

			if (ShoveSystem.Active)
			{
				vector = ShoveSystem.Position;
			}
			else if (dragging.data.playType == Card.PlayType.Place && (bool)hoverSlot && ShoveSystem.Slot != hoverSlot && hoverSlot.canBePlacedOn && hoverSlot.owner == dragging.owner)
			{
				Dictionary<Entity, List<CardSlot>> shoveData2;
				if (hoverSlot.Count < hoverSlot.max || dragging.actualContainers.Contains(hoverSlot))
				{
					vector = hoverSlot.transform.position - Vector3.Scale(dragging.offset.localPosition, dragging.transform.localScale);
				}
				else if (ShoveSystem.CanShove(hoverSlot.GetTop(), dragging, out shoveData2))
				{
					vector = hoverSlot.transform.position - Vector3.Scale(dragging.offset.localPosition, dragging.transform.localScale);
					ShoveSystem.ShowShove(hoverSlot, shoveData2);
				}
			}

			dragging.transform.position = Delta.Lerp(dragging.transform.position, vector, dragLerp, Time.deltaTime);
		}
	}

	public override void DragCancel()
	{
		dragging.TweenToContainer();
		TweenUnHover(dragging);
		base.DragCancel();
	}

	public override void DragEnd()
	{
		base.DragEnd();
		NavigationState.BackToPreviousState();
	}

	public override void Release()
	{
		if (!dragging)
		{
			return;
		}

		bool retainPosition = false;
		bool retainRotation = false;
		bool retainScale = false;
		bool retainDrawOrder = false;
		if (base.enabled)
		{
			if (InputSwitcher.justSwitched)
			{
				dragging.TweenToContainer();
			}
			else if ((bool)hoverContainer && hoverContainer.canBePlacedOn && hoverContainer == owner.discardContainer && dragging.owner == owner)
			{
				if (dragging.CanRecall())
				{
					ActionMove action = new ActionMove(dragging, hoverContainer);
					if (Events.CheckAction(action))
					{
						Events.InvokeDiscard(dragging);
						if (Battle.IsOnBoard(dragging))
						{
							owner.freeAction = true;
						}

						ActionQueue.Add(action);
						ActionQueue.Add(new ActionEndTurn(owner));
						base.enabled = false;
						retainDrawOrder = true;
					}
				}

				hoverContainer.UnHover();
			}
			else
			{
				switch (dragging.data.playType)
				{
					case Card.PlayType.Place:
					if (!hoverSlot || dragging.actualContainers.Contains(hoverSlot) || !hoverSlot.canBePlacedOn || !(hoverSlot.owner == dragging.owner))
					{
						break;
						}
	
					if (hoverSlot.Count < hoverSlot.max)
					{
						ActionMove action6 = new ActionMove(dragging, hoverSlot);
						if (Events.CheckAction(action6))
						{
							bool flag = Battle.IsOnBoard(dragging) && Battle.IsOnBoard(hoverSlot.Group);
							Events.InvokeEntityPlace(dragging, new CardContainer[1] { hoverSlot }, flag);
							ActionQueue.Add(action6);
							ActionQueue.Add(new ActionEndTurn(owner));
							if (flag)
							{
								owner.freeAction = true;
							}

							base.enabled = false;
						}
						}
					else
					{
						if (!ShoveSystem.CanShove(hoverSlot.GetTop(), dragging, out var shoveData))
						{
							break;
						}

						ActionMove action7 = new ActionMove(dragging, hoverSlot);
						if (Events.CheckAction(action7))
						{
							bool flag2 = Battle.IsOnBoard(dragging) && Battle.IsOnBoard(hoverSlot.Group);
							ShoveSystem.Fix = true;
							Events.InvokeEntityPlace(dragging, new CardContainer[1] { hoverSlot }, flag2);
							ActionQueue.Add(new ActionShove(shoveData));
							ActionQueue.Add(action7);
							ActionQueue.Add(new ActionEndTurn(owner));
							if (flag2)
							{
								owner.freeAction = true;
							}

							base.enabled = false;
						}
						}
	
						break;
					case Card.PlayType.Play:
					if (!dragging.NeedsTarget)
					{
						if (!hoverContainer || !dragging.InContainer(hoverContainer))
						{
							ActionTrigger action2 = new ActionTrigger(dragging, owner.entity);
							if (Events.CheckAction(action2))
							{
								ActionQueue.Add(action2);
								ActionQueue.Add(new ActionReduceUses(dragging));
								ActionQueue.Add(new ActionResetOffset(dragging));
								ActionQueue.Add(new ActionEndTurn(owner));
								base.enabled = false;
								retainRotation = true;
								retainDrawOrder = true;
								dragging.RemoveFromContainers();
							}
						}
						}
					else if (dragging.data.playOnSlot)
					{
						CardContainer cardContainer = (dragging.targetMode.TargetRow ? hoverContainer : hoverSlot);
						if (!dragging.CanPlayOn(cardContainer))
						{
							break;
						}

						ActionTriggerAgainst action3 = new ActionTriggerAgainst(dragging, owner.entity, null, cardContainer);
						if (Events.CheckAction(action3))
						{
							if (ShoveSystem.Active)
							{
								ShoveSystem.Fix = true;
							}

							ActionQueue.Add(action3);
							ActionQueue.Add(new ActionReduceUses(dragging));
							ActionQueue.Add(new ActionResetOffset(dragging));
							ActionQueue.Add(new ActionEndTurn(owner));
							base.enabled = false;
							retainPosition = true;
							retainRotation = true;
							retainDrawOrder = true;
						}
						}
	
					else if (dragging.targetMode.TargetRow)
					{
						if (dragging.CanPlayOn(hoverContainer))
						{
							ActionTriggerAgainst action4 = new ActionTriggerAgainst(dragging, owner.entity, null, hoverContainer);
							if (Events.CheckAction(action4))
							{
								ActionQueue.Add(action4);
								ActionQueue.Add(new ActionReduceUses(dragging));
								ActionQueue.Add(new ActionResetOffset(dragging));
								ActionQueue.Add(new ActionEndTurn(owner));
								base.enabled = false;
								retainPosition = true;
								retainRotation = true;
								retainDrawOrder = true;
							}
						}
						}
	
					else if ((bool)hoverEntity && hoverEntity != dragging)
					{
						ActionTriggerAgainst action5 = new ActionTriggerAgainst(dragging, owner.entity, hoverEntity, null);
						if (Events.CheckAction(action5))
						{
							ActionQueue.Add(action5);
							ActionQueue.Add(new ActionReduceUses(dragging));
							ActionQueue.Add(new ActionResetOffset(dragging));
							ActionQueue.Add(new ActionEndTurn(owner));
							base.enabled = false;
							retainPosition = true;
							retainRotation = true;
							retainDrawOrder = true;
						}
						}
	
						break;
				}
			}

			if (ActionQueue.Empty)
			{
				dragging.TweenToContainer();
			}
		}

		TweenUnHover(dragging, retainScale, retainPosition, retainRotation, retainDrawOrder);
		DragEnd();
		UnHover();
	}
}
