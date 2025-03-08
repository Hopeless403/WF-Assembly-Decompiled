#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
	public static readonly List<GameObject> unhoverCache = new List<GameObject>();

	public static readonly Vector2 offscreen = new Vector2(-1000f, -1000f);

	public override void OnEnable()
	{
		base.OnEnable();
		Events.OnCardControllerEnabled += CardControllerEnabled;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Events.OnCardControllerEnabled -= CardControllerEnabled;
	}

	public void CardControllerEnabled(CardController controller)
	{
		ForceSetMousePosition(offscreen);
	}

	public void ForceSetMousePosition(Vector2 position)
	{
		MouseState mousePointerEventDataWithPosition = GetMousePointerEventDataWithPosition(position);
		MouseButtonEventData eventData = mousePointerEventDataWithPosition.GetButtonState(PointerEventData.InputButton.Left).eventData;
		ProcessMousePress(eventData);
		ProcessMove(eventData.buttonData);
		ProcessDrag(eventData.buttonData);
		ProcessMousePress(mousePointerEventDataWithPosition.GetButtonState(PointerEventData.InputButton.Right).eventData);
		ProcessDrag(mousePointerEventDataWithPosition.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
		ProcessMousePress(mousePointerEventDataWithPosition.GetButtonState(PointerEventData.InputButton.Middle).eventData);
		ProcessDrag(mousePointerEventDataWithPosition.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
		if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
		{
			ExecuteEvents.ExecuteHierarchy(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
		}
	}

	public MouseState GetMousePointerEventDataWithPosition(Vector2 forcePosition)
	{
		PointerEventData data;
		bool pointerData = GetPointerData(-1, out data, create: true);
		data.Reset();
		if (pointerData)
		{
			data.position = forcePosition;
		}

		if (Cursor.lockState == CursorLockMode.Locked)
		{
			data.position = new Vector2(-1f, -1f);
			data.delta = Vector2.zero;
		}
		else
		{
			data.delta = forcePosition - data.position;
			data.position = forcePosition;
		}

		data.scrollDelta = base.input.mouseScrollDelta;
		data.button = PointerEventData.InputButton.Left;
		base.eventSystem.RaycastAll(data, m_RaycastResultCache);
		RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(m_RaycastResultCache);
		data.pointerCurrentRaycast = pointerCurrentRaycast;
		m_RaycastResultCache.Clear();
		GetPointerData(-2, out var data2, create: true);
		data2.Reset();
		CopyFromTo(data, data2);
		data2.button = PointerEventData.InputButton.Right;
		GetPointerData(-3, out var data3, create: true);
		data3.Reset();
		CopyFromTo(data, data3);
		data3.button = PointerEventData.InputButton.Middle;
		MouseState mouseState = new MouseState();
		mouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), data);
		mouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), data2);
		mouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), data3);
		return mouseState;
	}

	public override void ProcessMove(PointerEventData pointerEvent)
	{
		GameObject newEnterTarget = ((Cursor.lockState == CursorLockMode.Locked) ? null : pointerEvent.pointerCurrentRaycast.gameObject);
		CustomHandlePointerExitAndEnter(pointerEvent, newEnterTarget);
	}

	public static void CustomHandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget)
	{
		if (newEnterTarget == null || currentPointerData.pointerEnter == null)
		{
			int count = currentPointerData.hovered.Count;
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = currentPointerData.hovered[i];
				ExecuteEvents.Execute(gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
				unhoverCache.Add(gameObject);
			}

			currentPointerData.hovered.Clear();
			if (newEnterTarget == null)
			{
				currentPointerData.pointerEnter = null;
				return;
			}
		}

		if (currentPointerData.pointerEnter == newEnterTarget && (bool)newEnterTarget)
		{
			return;
		}

		GameObject gameObject2 = BaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
		if (currentPointerData.pointerEnter != null)
		{
			Transform parent = currentPointerData.pointerEnter.transform;
			while (parent != null && (!(gameObject2 != null) || !(gameObject2.transform == parent)))
			{
				ExecuteEvents.Execute(parent.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
				currentPointerData.hovered.Remove(parent.gameObject);
				unhoverCache.Add(parent.gameObject);
				parent = parent.parent;
			}
		}

		currentPointerData.pointerEnter = newEnterTarget;
		if (newEnterTarget != null)
		{
			Transform parent2 = newEnterTarget.transform;
			while (parent2 != null && parent2.gameObject != gameObject2)
			{
				ExecuteEvents.Execute(parent2.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
				currentPointerData.hovered.Add(parent2.gameObject);
				parent2 = parent2.parent;
			}
		}

		foreach (GameObject item in unhoverCache)
		{
			if ((bool)item)
			{
				item.GetComponent<IPointerAfterExitHandler>()?.OnPointerAfterExit(currentPointerData);
			}
		}

		unhoverCache.Clear();
	}
}
