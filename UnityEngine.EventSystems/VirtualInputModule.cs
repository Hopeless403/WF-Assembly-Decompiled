using System;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Virtual Input Module")]
	public class VirtualInputModule : PointerInputModule
	{
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			Mouse,
			Buttons
		}

		private float m_PrevActionTime;

		private Vector2 m_LastMoveVector;

		private int m_ConsecutiveMoveCount;

		private Vector2 m_LastMousePosition;

		private Vector2 m_MousePosition;

		[SerializeField]
		private RectTransform m_VirtualCursor;

		private Camera _cam;

		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		[SerializeField]
		private string m_SubmitButton = "Submit";

		[SerializeField]
		private string m_CancelButton = "Cancel";

		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		[SerializeField]
		private float m_RepeatDelay = 0.5f;

		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		private bool m_ForceModuleActive;

		private PointerEventData.FramePressState oldButtonState = PointerEventData.FramePressState.NotChanged;

		private readonly MouseState m_MouseState = new MouseState();

		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public InputMode inputMode => InputMode.Mouse;

		private Camera m_canvasCamera => _cam ?? (_cam = Camera.main);

		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return m_ForceModuleActive;
			}
			set
			{
				m_ForceModuleActive = value;
			}
		}

		public bool forceModuleActive
		{
			get
			{
				return m_ForceModuleActive;
			}
			set
			{
				m_ForceModuleActive = value;
			}
		}

		public float inputActionsPerSecond
		{
			get
			{
				return m_InputActionsPerSecond;
			}
			set
			{
				m_InputActionsPerSecond = value;
			}
		}

		public float repeatDelay
		{
			get
			{
				return m_RepeatDelay;
			}
			set
			{
				m_RepeatDelay = value;
			}
		}

		public string horizontalAxis
		{
			get
			{
				return m_HorizontalAxis;
			}
			set
			{
				m_HorizontalAxis = value;
			}
		}

		public string verticalAxis
		{
			get
			{
				return m_VerticalAxis;
			}
			set
			{
				m_VerticalAxis = value;
			}
		}

		public string submitButton
		{
			get
			{
				return m_SubmitButton;
			}
			set
			{
				m_SubmitButton = value;
			}
		}

		public string cancelButton
		{
			get
			{
				return m_CancelButton;
			}
			set
			{
				m_CancelButton = value;
			}
		}

		protected VirtualInputModule()
		{
		}

		public override void UpdateModule()
		{
			m_LastMousePosition = m_MousePosition;
			m_MousePosition = m_VirtualCursor.anchoredPosition;
		}

		public override bool IsModuleSupported()
		{
			if (!m_ForceModuleActive)
			{
				return Input.mousePresent;
			}
			return true;
		}

		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			bool flag = m_ForceModuleActive;
			flag |= !Mathf.Approximately(Mathf.Abs(RewiredControllerManager.instance.GetAnalog("Move Horizontal")), 0f);
			flag |= !Mathf.Approximately(Mathf.Abs(RewiredControllerManager.instance.GetAnalog("Move Vertical")), 0f);
			flag |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0f;
			flag |= RewiredControllerManager.instance.IsButtonPressed("Select");
			if (Console.active)
			{
				flag = false;
			}
			return flag;
		}

		public override void ActivateModule()
		{
			base.ActivateModule();
			m_MousePosition = m_VirtualCursor.anchoredPosition;
			m_LastMousePosition = m_VirtualCursor.anchoredPosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, GetBaseEventData());
		}

		public override void DeactivateModule()
		{
			base.DeactivateModule();
			ClearSelection();
		}

		public override void Process()
		{
			bool flag = SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					SendSubmitEventToSelectedObject();
				}
			}
			ProcessMouseEvent();
			ProcessRewiredEvent();
		}

		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			return GetBaseEventData().used;
		}

		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = RewiredControllerManager.instance.GetAnalog("Move Vertical");
			zero.y = RewiredControllerManager.instance.GetAnalog("Move Horizontal");
			if (Mathf.Abs(RewiredControllerManager.instance.GetAnalog("Move Horizontal")) >= 0.05f)
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Mathf.Abs(RewiredControllerManager.instance.GetAnalog("Move Vertical")) >= 0.05f)
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Mathf.Abs(RewiredControllerManager.instance.GetAnalog("Move Vertical")) >= 0.05f || Mathf.Abs(RewiredControllerManager.instance.GetAnalog("Move Horizontal")) >= 0.05f;
			bool flag2 = Vector2.Dot(rawMoveVector, m_LastMoveVector) > 0f;
			if (!flag)
			{
				flag = ((!flag2 || m_ConsecutiveMoveCount != 1) ? (unscaledTime > m_PrevActionTime + 1f / m_InputActionsPerSecond) : (unscaledTime > m_PrevActionTime + m_RepeatDelay));
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				m_ConsecutiveMoveCount = 0;
			}
			m_ConsecutiveMoveCount++;
			m_PrevActionTime = unscaledTime;
			m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		protected void ProcessRewiredEvent()
		{
			MouseButtonEventData eventData = GetMousePointerEventData(0).GetButtonState(PointerEventData.InputButton.Left).eventData;
			PointerEventData.FramePressState framePressState = PointerEventData.FramePressState.NotChanged;
			if (RewiredControllerManager.instance.IsButtonPressed("Select"))
			{
				framePressState = PointerEventData.FramePressState.Pressed;
			}
			if (RewiredControllerManager.instance.IsButtonReleased("Select"))
			{
				framePressState = PointerEventData.FramePressState.Released;
			}
			if (framePressState == oldButtonState)
			{
				framePressState = PointerEventData.FramePressState.NotChanged;
			}
			oldButtonState = framePressState;
			if (MonoBehaviourSingleton<UINavigationSystem>.instance != null && MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem != null)
			{
				_ = MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem.clickHandler;
			}
			MouseButtonEventData mouseButtonEventData = new MouseButtonEventData
			{
				buttonState = framePressState,
				buttonData = eventData.buttonData
			};
			ProcessMousePress(mouseButtonEventData);
			if (!Mathf.Approximately(mouseButtonEventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy(ExecuteEvents.GetEventHandler<IScrollHandler>(mouseButtonEventData.buttonData.pointerCurrentRaycast.gameObject), mouseButtonEventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		protected void ProcessMouseEvent()
		{
			ProcessMouseEvent(0);
		}

		protected void ProcessMouseEvent(int id)
		{
			MouseButtonEventData eventData = GetMousePointerEventData(id).GetButtonState(PointerEventData.InputButton.Left).eventData;
			if (MonoBehaviourSingleton<UINavigationSystem>.instance != null && MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem != null)
			{
				eventData.buttonData.pointerCurrentRaycast = new RaycastResult
				{
					gameObject = MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem.clickHandler
				};
			}
			ProcessMove(eventData.buttonData);
			ProcessDrag(eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
			}
		}

		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		protected void ProcessMousePress(MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				buttonData.position = m_VirtualCursor.anchoredPosition;
				DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						int clickCount = buttonData.clickCount + 1;
						buttonData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				GameObject gameObject3 = ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.pointerUpHandler);
				if (gameObject3 == null)
				{
					gameObject3 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				buttonData.pointerPress = gameObject3;
				ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					HandlePointerExitAndEnter(buttonData, null);
					HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		protected override MouseState GetMousePointerEventData(int id)
		{
			PointerEventData data;
			bool pointerData = GetPointerData(-1, out data, true);
			data.Reset();
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(m_canvasCamera, m_VirtualCursor.position);
			if (pointerData)
			{
				data.position = vector;
			}
			Vector2 vector2 = vector;
			data.delta = vector2 - data.position;
			data.position = vector2;
			data.scrollDelta = Input.mouseScrollDelta;
			data.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(data, m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(m_RaycastResultCache);
			data.pointerCurrentRaycast = pointerCurrentRaycast;
			m_RaycastResultCache.Clear();
			GetPointerData(-2, out var data2, true);
			CopyFromTo(data, data2);
			data2.button = PointerEventData.InputButton.Right;
			GetPointerData(-3, out var data3, true);
			CopyFromTo(data, data3);
			data3.button = PointerEventData.InputButton.Middle;
			m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), data);
			m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), data2);
			m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), data3);
			return m_MouseState;
		}
	}
}
