#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerAfterExitHandler
{
	[SerializeField]
	public bool IsMaster = true;

	[HideIf("IsMaster")]
	public CardHover master;

	[SerializeField]
	[ShowIf("IsMaster")]
	public GraphicRaycaster graphicRaycaster;

	[SerializeField]
	[ShowIf("IsMaster")]
	public Entity entity;

	[SerializeField]
	[ShowIf("IsMaster")]
	public bool disableWhileDragging = true;

	[SerializeField]
	[ShowIf("IsMaster")]
	public Flipper flipper;

	public UnityEvent onHover;

	public UnityEvent onUnHover;

	[Header("Pop up \"Keyword\" description(s) while mouse over")]
	[SerializeField]
	public CardPopUpTarget pop;

	[Header("Mouse Over This Element?")]
	[SerializeField]
	[ReadOnly]
	public bool mouseOver;

	[SerializeField]
	[ReadOnly]
	public int childMouseOverCount;

	public bool preMouseOver;

	[Header("Entity assigned as \"Hovering\"? (set by CardControllers)")]
	[ReadOnly]
	public bool hovering;

	[ShowIf("IsMaster")]
	public CardController controller;

	public bool hoverable = true;

	public bool dragging;

	public bool hasPop;

	public bool IsHovering
	{
		get
		{
			if (!IsMaster)
			{
				return master.hovering;
			}

			return hovering;
		}
	}

	public bool IsHoverable
	{
		get
		{
			if (!IsMaster)
			{
				return master.hoverable;
			}

			return hoverable;
		}
	}

	public bool IsMouseOver
	{
		get
		{
			if (!mouseOver)
			{
				return childMouseOverCount > 0;
			}

			return true;
		}
	}

	public bool CanHover
	{
		get
		{
			if (base.enabled && (bool)controller && controller.enabled && !flipper.flipped)
			{
				return IsHoverable;
			}

			return false;
		}
	}

	public bool WasHovering { get; set; }

	public void Awake()
	{
		hasPop = pop;
	}

	public void OnEnable()
	{
		hoverable = true;
		hovering = false;
		mouseOver = false;
		if (IsMaster)
		{
			graphicRaycaster.enabled = true;
		}

		Events.OnUpdateInputSystem += UpdateInputSystem;
		Events.OnCardControllerEnabled += CardControllerEnabled;
	}

	public void OnDisable()
	{
		if (mouseOver && !IsMaster)
		{
			mouseOver = false;
			master.childMouseOverCount = Mathf.Max(0, master.childMouseOverCount - 1);
		}

		if (hasPop && (bool)pop && pop.popped)
		{
			pop.UnPop();
		}

		Events.OnUpdateInputSystem -= UpdateInputSystem;
		Events.OnCardControllerEnabled -= CardControllerEnabled;
	}

	public void Update()
	{
		if (!hasPop || (mouseOver && IsHovering))
		{
			return;
		}

		if ((bool)pop)
		{
			if (pop.popped)
			{
				pop.UnPop();
			}
		}
		else
		{
			hasPop = false;
		}
	}

	public void LateUpdate()
	{
		WasHovering = IsHovering;
		if (IsMaster)
		{
			bool isMouseOver = IsMouseOver;
			if (isMouseOver && !preMouseOver)
			{
				Hover();
			}
			else if (!isMouseOver && preMouseOver)
			{
				UnHover();
			}

			preMouseOver = isMouseOver;
		}

		if (mouseOver && IsHovering && hasPop)
		{
			if ((bool)pop)
			{
				if (!pop.popped)
				{
					pop.Pop();
				}
			}
			else
			{
				hasPop = false;
			}
		}

		if (IsMaster && disableWhileDragging && entity.dragging != dragging)
		{
			dragging = entity.dragging;
			if (dragging)
			{
				Disable();
			}
			else
			{
				Enable();
			}
		}
	}

	public void UpdateInputSystem(bool forceTouch)
	{
		if (IsMaster && IsMouseOver)
		{
			preMouseOver = false;
		}
	}

	public void CardControllerEnabled(CardController controller)
	{
		if (mouseOver && controller == this.controller)
		{
			controller.Hover(IsMaster ? entity : master.entity);
		}
	}

	public void CheckHover()
	{
		if (IsMouseOver && !preMouseOver)
		{
			Hover();
			preMouseOver = true;
		}
	}

	public void CheckUnHover()
	{
		if (!IsMouseOver && preMouseOver)
		{
			UnHover();
			preMouseOver = false;
		}
	}

	public void Enable()
	{
		graphicRaycaster.enabled = true;
		Events.InvokeUpdateInputSystem(forceTouch: false);
	}

	public void Disable()
	{
		graphicRaycaster.enabled = false;
		ForceUnHover();
		Events.InvokeUpdateInputSystem(forceTouch: false);
	}

	public void SetHoverable(bool value)
	{
		hoverable = value;
		if (!hoverable)
		{
			ForceUnHover();
		}
	}

	public void Hover()
	{
		if (CanHover)
		{
			controller.Hover(entity);
			onHover?.Invoke();
		}
	}

	public void UnHover()
	{
		if (base.enabled && (bool)controller && controller.enabled && controller.hoverEntity == entity)
		{
			controller.UnHover(entity);
			onUnHover?.Invoke();
		}
	}

	public void ForceUnHover()
	{
		mouseOver = false;
		preMouseOver = false;
		UnHover();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!mouseOver && hoverable && (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse || !UINavigationSystem.ActiveNavigationLayer || UINavigationSystem.ActiveNavigationLayer.forceHover))
		{
			mouseOver = true;
			if (IsMaster)
			{
				CheckHover();
				return;
			}

			master.childMouseOverCount++;
			master.CheckHover();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (mouseOver)
		{
			mouseOver = false;
			if (!IsMaster)
			{
				master.childMouseOverCount = Mathf.Max(0, master.childMouseOverCount - 1);
			}
		}
	}

	public void OnPointerAfterExit(PointerEventData eventData)
	{
		if (IsMaster)
		{
			CheckUnHover();
		}
		else
		{
			master.CheckUnHover();
		}
	}
}
