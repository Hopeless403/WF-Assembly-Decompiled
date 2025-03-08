#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputModule : BaseInputModule
{
	[SerializeField]
	public new BaseInput input;

	public PointerEventData pointer;

	public static bool touching;

	public static bool active;

	public static int blockScroll;

	public static readonly Vector2 offscreen = new Vector2(-1000f, -1000f);

	public Vector2 MousePosition { get; set; }

	public Vector2 LastMousePosition { get; set; }

	public Vector2 MouseMove { get; set; }

	public GameObject Hover { get; set; }

	public GameObject Press { get; set; }

	public static float ScrollX { get; set; }

	public static float ScrollY { get; set; }

	public override void OnEnable()
	{
		base.OnEnable();
		active = true;
		pointer = new PointerEventData(base.eventSystem);
		Events.OnUpdateInputSystem += ForceUpdate;
		Events.OnCardControllerEnabled += CardControllerEnabled;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		active = false;
		Events.OnUpdateInputSystem -= ForceUpdate;
		Events.OnCardControllerEnabled -= CardControllerEnabled;
		touching = false;
		ScrollX = 0f;
		ScrollY = 0f;
	}

	public static void BlockScroll()
	{
		blockScroll++;
	}

	public static void UnblockScroll()
	{
		blockScroll--;
	}

	public override void Process()
	{
		Touch? touch = GetTouch();
		if (touch.HasValue)
		{
			Touch valueOrDefault = touch.GetValueOrDefault();
			ProcessTouch(valueOrDefault);
			touching = true;
			if (blockScroll > 0)
			{
				ScrollX = 0f;
				ScrollY = 0f;
			}
		}
		else
		{
			touching = false;
			ScrollX = 0f;
			ScrollY = 0f;
		}
	}

	public void ForceUpdate(bool forceTouch)
	{
		if (forceTouch)
		{
			Touch touch = default(Touch);
			touch.position = MousePosition;
			touch.deltaTime = Time.deltaTime;
			touch.phase = TouchPhase.Stationary;
			Touch touch2 = touch;
			GameObject hover = Hover;
			PopulateTouchPointerData(touch2, out var _, out var _);
			Hover = pointer.pointerCurrentRaycast.gameObject;
			ProcessTouchMove(hover, Hover);
		}
		else
		{
			Process();
		}
	}

	public override void UpdateModule()
	{
		Process();
	}

	public void CardControllerEnabled(CardController controller)
	{
		Touch simulatedTouch = GetSimulatedTouch(offscreen, pressed: false, released: false);
		ProcessTouch(simulatedTouch);
	}

	public Touch? GetTouch()
	{
		if (input.touchCount > 0)
		{
			Touch touch = input.GetTouch(0);
			LastMousePosition = MousePosition;
			MousePosition = touch.position;
			MouseMove = touch.deltaPosition;
			return touch;
		}

		return null;
	}

	public Touch GetSimulatedTouch(Vector2 position, bool pressed, bool released)
	{
		LastMousePosition = MousePosition;
		MousePosition = position;
		MouseMove = MousePosition - LastMousePosition;
		Touch touch = default(Touch);
		touch.position = MousePosition;
		touch.rawPosition = MousePosition;
		touch.deltaPosition = MouseMove;
		touch.phase = TouchPhase.Stationary;
		touch.pressure = 1f;
		touch.maximumPossiblePressure = 1f;
		touch.type = TouchType.Direct;
		touch.tapCount = 1;
		Touch result = touch;
		if (pressed)
		{
			result.phase = TouchPhase.Began;
		}
		else if (released)
		{
			result.phase = TouchPhase.Ended;
		}

		else if (result.deltaPosition.sqrMagnitude > 0f)
		{
			result.phase = TouchPhase.Moved;
		}

		return result;
	}

	public void PopulateTouchPointerData(Touch touch, out bool pressed, out bool released)
	{
		pointer.Reset();
		pressed = !touching;
		TouchPhase phase = touch.phase;
		released = phase == TouchPhase.Canceled || phase == TouchPhase.Ended;
		pointer.position = touch.position;
		pointer.delta = (pressed ? Vector2.zero : touch.deltaPosition);
		pointer.button = PointerEventData.InputButton.Left;
		if (touch.phase == TouchPhase.Canceled)
		{
			pointer.pointerCurrentRaycast = default(RaycastResult);
			return;
		}

		base.eventSystem.RaycastAll(pointer, m_RaycastResultCache);
		pointer.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
		m_RaycastResultCache.Clear();
	}

	public void ProcessTouch(Touch touch)
	{
		GameObject hover = Hover;
		PopulateTouchPointerData(touch, out var pressed, out var released);
		Hover = pointer.pointerCurrentRaycast.gameObject;
		ProcessTouchMove(hover, Hover);
		if (!pressed && !released)
		{
			ScrollX = touch.deltaPosition.x;
			ScrollY = touch.deltaPosition.y;
		}
		else
		{
			ScrollX = 0f;
			ScrollY = 0f;
		}

		if (released)
		{
			ProcessTouchRelease();
		}
		else if (pressed)
		{
			ProcessTouchPress(hover);
		}

		if (!released && (bool)pointer.pointerDrag)
		{
			ProcessTouchDrag();
		}
	}

	public void ProcessTouchPress(GameObject preHover)
	{
		if (!Hover)
		{
			return;
		}

		TouchHandler component = Hover.GetComponent<TouchHandler>();
		bool alreadyHovered = preHover == Hover;
		if ((bool)component)
		{
			if (component.CanTouchPress(alreadyHovered))
			{
				Press = Hover;
				component.HandleTouchPress(pointer, alreadyHovered);
			}

			return;
		}

		pointer.pressPosition = pointer.position;
		pointer.pointerPressRaycast = pointer.pointerCurrentRaycast;
		Press = Hover;
		ExecuteEvents.ExecuteHierarchy(Press, pointer, ExecuteEvents.pointerDownHandler);
		pointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(Hover);
		if ((bool)pointer.pointerDrag)
		{
			ExecuteEvents.Execute(pointer.pointerDrag, pointer, ExecuteEvents.initializePotentialDrag);
		}
	}

	public void ProcessTouchRelease()
	{
		if ((bool)Press)
		{
			ExecuteEvents.ExecuteHierarchy(Press, pointer, ExecuteEvents.pointerUpHandler);
			if (Hover == Press)
			{
				ExecuteEvents.ExecuteHierarchy(Press, pointer, ExecuteEvents.pointerClickHandler);
			}

			if ((bool)pointer.pointerDrag && pointer.dragging)
			{
				ExecuteEvents.ExecuteHierarchy(Hover, pointer, ExecuteEvents.dropHandler);
				ExecuteEvents.ExecuteHierarchy(pointer.pointerDrag, pointer, ExecuteEvents.endDragHandler);
				pointer.dragging = false;
			}

			Press = null;
		}
	}

	public void ProcessTouchMove(GameObject preHover, GameObject hover)
	{
		if (preHover != hover)
		{
			bool num = preHover;
			if (num)
			{
				ExecuteEvents.ExecuteHierarchy(preHover, pointer, ExecuteEvents.pointerExitHandler);
			}

			if ((bool)hover)
			{
				ExecuteEvents.ExecuteHierarchy(hover, pointer, ExecuteEvents.pointerEnterHandler);
			}

			if (num)
			{
				preHover.GetComponent<IPointerAfterExitHandler>()?.OnPointerAfterExit(pointer);
			}
		}
	}

	public void ProcessTouchDrag()
	{
		if (pointer.IsPointerMoving())
		{
			if (!pointer.dragging && ShouldStartDrag(pointer.pressPosition, pointer.position, base.eventSystem.pixelDragThreshold, pointer.useDragThreshold))
			{
				ExecuteEvents.Execute(pointer.pointerDrag, pointer, ExecuteEvents.beginDragHandler);
				pointer.dragging = true;
			}

			if (pointer.dragging)
			{
				ExecuteEvents.Execute(pointer.pointerDrag, pointer, ExecuteEvents.dragHandler);
			}
		}
	}

	public static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
	{
		if (!useDragThreshold)
		{
			return true;
		}

		return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
	}

	public new static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
	{
		foreach (RaycastResult candidate in candidates)
		{
			if ((bool)candidate.gameObject)
			{
				return candidate;
			}
		}

		return default(RaycastResult);
	}
}
