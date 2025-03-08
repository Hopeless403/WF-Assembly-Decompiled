#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TargetingArrowSystem : GameSystem
{
	public TargetingDisplay offensiveArrow;

	public TargetingDisplay supportiveArrow;

	public TargetingDisplay targetMulti;

	public Entity target;

	public Entity hover;

	public CardContainer snapToContainer;

	public bool active;

	public Vector3 toPosition;

	[SerializeField]
	[Range(0f, 1f)]
	public float lerp = 0.4f;

	public TargetingDisplay currentArrow;

	public bool offensive;

	public Vector3 toPositionTarget
	{
		get
		{
			if ((bool)hover)
			{
				return hover.transform.position;
			}

			if ((bool)snapToContainer)
			{
				return snapToContainer.transform.position;
			}

			return MonoBehaviourSingleton<Cursor3d>.instance.transform.position;
		}
	}

	public static bool CorrectType(Card.PlayType playType)
	{
		return playType == Card.PlayType.Play;
	}

	public void OnEnable()
	{
		Events.OnEntityDrag += EntityDrag;
		Events.OnEntityRelease += EntityRelease;
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
		Events.OnContainerHover += ContainerHover;
		Events.OnContainerUnHover += ContainerUnHover;
		Events.OnSlotHover += SlotHover;
		Events.OnSlotUnHover += SlotUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityDrag -= EntityDrag;
		Events.OnEntityRelease -= EntityRelease;
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
		Events.OnContainerHover -= ContainerHover;
		Events.OnContainerUnHover -= ContainerUnHover;
		Events.OnSlotHover -= SlotHover;
		Events.OnSlotUnHover -= SlotUnHover;
	}

	public void LateUpdate()
	{
		if (!active && (bool)target)
		{
			Show();
		}

		if (active && !target)
		{
			Hide();
		}

		if (active)
		{
			UpdateArrow();
			toPosition = Delta.Lerp(toPosition, toPositionTarget, lerp, Time.deltaTime);
		}
	}

	public void UpdateArrow()
	{
		Vector3 position = target.transform.position;
		Vector3 end = toPosition;
		currentArrow.UpdatePosition(position, end);
	}

	public void Show()
	{
		active = true;
		currentArrow = ((!target.NeedsTarget) ? targetMulti : (offensive ? offensiveArrow : supportiveArrow));
		currentArrow.gameObject.SetActive(value: true);
		currentArrow.ResetStyle();
		if (TouchInputModule.active)
		{
			toPosition = target.transform.position;
		}
		else
		{
			toPosition = toPositionTarget;
		}

		currentArrow.Show(this);
	}

	public void Hide()
	{
		active = false;
		currentArrow.gameObject.SetActive(value: false);
		currentArrow.Hide();
	}

	public void EntityDrag(Entity entity)
	{
		if (entity.inPlay && (bool)entity && (bool)entity.data && CorrectType(entity.data.playType))
		{
			target = entity;
			offensive = target.IsOffensive();
		}
	}

	public void EntityRelease(Entity entity)
	{
		if (target == entity)
		{
			target = null;
		}
	}

	public void EntityHover(Entity entity)
	{
		if (active && (bool)target && !target.targetMode.TargetRow && !target.data.playOnSlot)
		{
			hover = entity;
			currentArrow.EntityHover(entity);
		}
	}

	public void EntityUnHover(Entity entity)
	{
		if (hover == entity)
		{
			hover = null;
			if (active && (bool)currentArrow)
			{
				currentArrow.ResetStyle();
			}
		}
	}

	public void ContainerHover(CardContainer container)
	{
		if (active && target != null)
		{
			currentArrow.ContainerHover(container, this);
		}
	}

	public void ContainerUnHover(CardContainer container)
	{
		if (snapToContainer == container)
		{
			snapToContainer = null;
			if (active && (bool)currentArrow)
			{
				currentArrow.ResetStyle();
			}
		}
	}

	public void SlotHover(CardSlot slot)
	{
		if (active && (bool)target)
		{
			currentArrow.SlotHover(slot, this);
		}
	}

	public void SlotUnHover(CardSlot slot)
	{
		if (snapToContainer == slot)
		{
			snapToContainer = null;
			if (active && (bool)currentArrow)
			{
				currentArrow.ResetStyle();
			}
		}
	}
}
