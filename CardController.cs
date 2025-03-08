#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class CardController : MonoBehaviour
{
	public Character owner;

	[ReadOnly]
	public GameObject hover;

	[ReadOnly]
	public Entity hoverEntity;

	[ReadOnly]
	public Entity dragging;

	[ReadOnly]
	public CardContainer hoverContainer;

	[ReadOnly]
	public CardSlot hoverSlot;

	public bool canHoverWhileDragging = true;

	[Range(0f, 1f)]
	public float dragLerp = 0.25f;

	public float hoverZ = -0.2f;

	public float dragZ = -1f;

	[Header("Hover Tween")]
	public float cardHoverScale = 1.33f;

	public LeanTweenType cardHoverEase = LeanTweenType.easeOutBack;

	public float cardHoverEaseDur = 0.33f;

	public LeanTweenType cardUnHoverEase = LeanTweenType.easeOutBack;

	public float cardUnHoverEaseDur = 0.33f;

	[Header("Draw Order")]
	public int hoverDrawOrder = 100;

	public int dragDrawOrder = 200;

	[Header("Interactables")]
	public bool interactWithInPlay = true;

	public bool interactWithNotInPlay = true;

	public bool canPress = true;

	public bool canPressAndHoverInSameFrame;

	public Vector3 draggingPositionPre;

	public int draggingLayerPre;

	public Entity pressEntity;

	public bool press;

	public virtual bool AllowDynamicSelectRelease => InputSystem.AllowDynamicSelectRelease;

	public Vector3 CardHoverScale => new Vector3(cardHoverScale * 1f, cardHoverScale * 1f, 1f);

	public static CardController Find(GameObject gameObject)
	{
		return gameObject.GetComponentInParent<CardController>();
	}

	public virtual void Update()
	{
		if (!press)
		{
			if (canPress && InputSystem.IsSelectPressed())
			{
				press = true;
				if (canPressAndHoverInSameFrame || !hoverEntity || hoverEntity.display.hover.WasHovering)
				{
					pressEntity = hoverEntity;
					Press();
					if ((bool)pressEntity)
					{
						Events.InvokeEntitySelect(pressEntity);
					}
				}
			}
		}
		else if (InputSystem.IsDynamicSelectReleased(AllowDynamicSelectRelease && (bool)dragging))
		{
			Release();
			pressEntity = null;
			press = false;
		}

		if ((bool)dragging)
		{
			DragUpdate();
		}
	}

	public void Enable()
	{
		base.enabled = true;
	}

	public void OnEnable()
	{
		Events.InvokeCardControllerEnabled(this);
	}

	public void Disable()
	{
		base.enabled = false;
		Release();
		UnHover();
	}

	public void OnDisable()
	{
		Events.InvokeCardControllerDisabled(this);
	}

	public virtual void Press()
	{
	}

	public virtual void Release()
	{
	}

	public bool TryDrag(Entity entity)
	{
		if (Events.CheckEntityDrag(entity))
		{
			Drag(entity);
			return true;
		}

		return false;
	}

	public void Drag(Entity entity)
	{
		dragging = entity;
		draggingLayerPre = dragging.gameObject.layer;
		dragging.gameObject.layer = LayerMask.NameToLayer("Default");
		entity.dragging = true;
		draggingPositionPre = dragging.transform.position;
		if (dragDrawOrder != 0)
		{
			entity.DrawOrder = dragDrawOrder;
		}

		Events.InvokeEntityDrag(entity);
	}

	public virtual void DragEnd()
	{
		Events.InvokeEntityRelease(dragging);
		dragging.gameObject.layer = draggingLayerPre;
		dragging.dragging = false;
		dragging = null;
		press = false;
	}

	public virtual void DragUpdate()
	{
		DragUpdatePosition();
		Wobbler wobbler = dragging.wobbler;
		if ((bool)wobbler)
		{
			Vector3 position = dragging.transform.position;
			Vector3 movement = position - draggingPositionPre;
			wobbler.Wobble(movement);
			draggingPositionPre = position;
		}
	}

	public virtual void DragCancel()
	{
		DragEnd();
		UnHover(dragging);
	}

	public virtual bool CanUseOn(Entity entity, Entity target)
	{
		if ((bool)entity && (bool)target && entity.data.playType == Card.PlayType.Play && !entity.targetMode.TargetRow && ((entity.data.canPlayOnBoard && Battle.IsOnBoard(target)) || (entity.data.canPlayOnHand && target.containers.Contains(entity.owner?.handContainer))) && ((entity.data.canPlayOnFriendly && entity.owner == target.owner) || (entity.data.canPlayOnEnemy && entity.owner != target.owner)))
		{
			return entity.CanPlayOn(target);
		}

		return false;
	}

	public Vector3 GetDragPosition()
	{
		return (Cursor3d.Position - (dragging.offset.position - dragging.transform.position)).WithZ(dragZ);
	}

	public virtual void DragUpdatePosition()
	{
		dragging.transform.position = Delta.Lerp(dragging.transform.position, GetDragPosition(), dragLerp, Time.deltaTime);
	}

	public void Hover(Entity entity)
	{
		if ((!dragging || (canHoverWhileDragging && CanUseOn(dragging, entity))) && (!entity.StillExists() || (entity.inPlay && interactWithInPlay) || (!entity.inPlay && interactWithNotInPlay)))
		{
			if ((bool)hoverEntity && dragging != hoverEntity)
			{
				UnHover(hoverEntity);
			}

			if ((bool)entity && entity != dragging)
			{
				hoverEntity = entity;
				TweenHover(entity);
				Events.InvokeEntityHover(entity);
			}
		}
	}

	public void UnHover(Entity entity)
	{
		if (!dragging || canHoverWhileDragging)
		{
			if (hoverEntity == entity)
			{
				hoverEntity = null;
				Events.InvokeEntityUnHover(entity);
			}

			if (dragging != entity)
			{
				TweenUnHover(entity);
			}
		}
	}

	public void UnHover()
	{
		if ((bool)hoverEntity)
		{
			UnHover(hoverEntity);
		}
	}

	public void HoverContainer(CardContainer container)
	{
		UnHoverContainer();
		hoverContainer = container;
		Events.InvokeContainerHover(container);
	}

	public void UnHoverContainer()
	{
		if ((bool)hoverContainer)
		{
			Events.InvokeContainerUnHover(hoverContainer);
			hoverContainer = null;
		}
	}

	public void HoverSlot(CardSlot slot)
	{
		if (!dragging || dragging.CanPlayOn(slot))
		{
			UnHoverSlot();
			hoverSlot = slot;
			Events.InvokeSlotHover(slot);
		}
	}

	public void UnHoverSlot()
	{
		if ((bool)hoverSlot)
		{
			Events.InvokeSlotUnHover(hoverSlot);
			hoverSlot = null;
		}
	}

	public void TweenHover(Entity entity, bool doScale = true, bool doMove = true, bool doRotate = true, bool doDrawOrder = true)
	{
		GameObject gameObject = entity.offset.gameObject;
		LeanTween.cancel(gameObject);
		if (doScale)
		{
			LeanTween.scale(gameObject, CardHoverScale, cardHoverEaseDur).setEase(cardHoverEase);
		}

		if (doMove)
		{
			float num = 0f;
			Vector3 v = new Vector3(0f, num, 0f);
			if (entity.containers.Length != 0)
			{
				CardContainer[] containers = entity.containers;
				foreach (CardContainer cardContainer in containers)
				{
					num -= cardContainer.GetChildPosition(entity).y / entity.transform.localScale.y;
					v += cardContainer.childHoverOffset;
				}

				v /= (float)entity.actualContainers.Count;
			}

			LeanTween.moveLocal(gameObject, v.WithZ(hoverZ), cardHoverEaseDur * 1.5f).setEase(LeanTweenType.easeOutQuart);
		}

		if (doRotate)
		{
			LeanTween.rotateZ(gameObject, 0f, cardHoverEaseDur * 1.5f).setEase(LeanTweenType.easeOutQuart);
		}

		if (doDrawOrder && hoverDrawOrder != 0)
		{
			entity.DrawOrder = hoverDrawOrder;
		}

		if ((bool)entity.display && (bool)entity.display.hover)
		{
			entity.display.hover.hovering = true;
		}
	}

	public void TweenUnHover(Entity entity, bool retainScale = false, bool retainPosition = false, bool retainRotation = false, bool retainDrawOrder = false)
	{
		GameObject gameObject = entity.offset.gameObject;
		LeanTween.cancel(gameObject);
		if (!retainScale)
		{
			LeanTween.scale(gameObject, Vector3.one, cardUnHoverEaseDur).setEase(cardUnHoverEase);
		}

		if (!retainPosition)
		{
			LeanTween.moveLocal(gameObject, Vector3.zero, cardUnHoverEaseDur).setEase(cardUnHoverEase);
		}

		if (!retainRotation)
		{
			LeanTween.rotateLocal(gameObject, Vector3.zero, cardUnHoverEaseDur).setEase(cardUnHoverEase);
		}

		if (!retainDrawOrder)
		{
			entity.ResetDrawOrder();
		}

		if ((bool)entity.display && (bool)entity.display.hover)
		{
			entity.display.hover.hovering = false;
		}
	}
}
