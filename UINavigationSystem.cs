#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UINavigationSystem : MonoBehaviourSingleton<UINavigationSystem>
{
	[Header("Current Status")]
	public UINavigationItem currentNavigationItem;

	public List<UINavigationItem> AvailableNavigationItems = new List<UINavigationItem>();

	public List<UINavigationItem> PossibleNavigationItems = new List<UINavigationItem>();

	public List<UINavigationLayer> NavigationLayers = new List<UINavigationLayer>();

	public static UINavigationLayer ActiveNavigationLayer;

	public UINavigationLayer lastActiveNavigationLayer;

	[Header("Settings Status")]
	public float navigationControllerDeadzone = 0.05f;

	[ReadOnly]
	public MoveDirection lastMove;

	public float navigationGridThreshold = 1.5f;

	public float minorNavigationGridThreshold = 0.25f;

	public float maxAnyItemCheckBeforeLoopMultiplier = 2.5f;

	public float anglePriority = 1f;

	public float disPriority = 1f;

	public float maxNavAngle = 45f;

	public UnityEvent OnNavigate;

	[Header("References")]
	public Cursor3d virtualCursor;

	[SerializeField]
	public CustomEventSystem eventSystem;

	[SerializeField]
	public GameObject eventSystemObj;

	public bool didNavigate;

	public Vector3 lastNavigationItemPos;

	public bool currentNavigationItemChanged = true;

	public void Start()
	{
		NavigationState.Reset();
	}

	public void OnEnable()
	{
		Events.OnTransitionEnd += Events_OnTransitionEnd;
		Events.OnUINavigationReset += Events_OnUINavigationReset;
		Events.OnEntityFlipComplete += Events_OnEntityFlipComplete;
	}

	public void OnDisable()
	{
		Events.OnTransitionEnd -= Events_OnTransitionEnd;
		Events.OnUINavigationReset -= Events_OnUINavigationReset;
		Events.OnEntityFlipComplete -= Events_OnEntityFlipComplete;
	}

	public void Events_OnEntityFlipComplete(Entity entity)
	{
		if ((bool)entity && (bool)currentNavigationItem && entity.gameObject == currentNavigationItem.gameObject)
		{
			UINavigationDefaultSystem.SetStartingItem();
		}
	}

	public void Events_OnUINavigationReset()
	{
		ResetState();
	}

	public void Events_OnTransitionEnd(TransitionType transition)
	{
		UINavigationDefaultSystem.SetStartingItem(useClosest: false, ignoreHistory: true);
	}

	public void RemoveActiveLayer()
	{
		UnregisterNavigationLayer(ActiveNavigationLayer);
	}

	public void RegisterSelectable(UINavigationItem uiNavigationItem)
	{
		if (!AvailableNavigationItems.Contains(uiNavigationItem) && AvailableNavigationItems.Count((UINavigationItem x) => x.gameObject == uiNavigationItem.gameObject) == 0)
		{
			AvailableNavigationItems.Add(uiNavigationItem);
			if ((bool)MonoBehaviourSingleton<UINavigationSystem>.instance && (bool)MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem && (MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem.selectionPriority == UINavigationItem.SelectionPriority.Lowest || (uiNavigationItem.selectionPriority == UINavigationItem.SelectionPriority.Highest && MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem.selectionPriority != UINavigationItem.SelectionPriority.Highest)))
			{
				UINavigationDefaultSystem.SetStartingItem();
			}
		}
	}

	public void UnregisterSelectable(UINavigationItem uiNavigationItem)
	{
		if (AvailableNavigationItems.Contains(uiNavigationItem))
		{
			AvailableNavigationItems.Remove(uiNavigationItem);
		}
	}

	public void RegisterNavigationLayer(UINavigationLayer navigationLayer)
	{
		NavigationLayers.Add(navigationLayer);
		if (navigationLayer.isOverrideLayer)
		{
			ActiveNavigationLayer = navigationLayer;
		}
	}

	public void UnregisterNavigationLayer(UINavigationLayer navigationLayer)
	{
		if (!NavigationLayers.Contains(navigationLayer))
		{
			return;
		}

		foreach (UINavigationItem item in AvailableNavigationItems.Where((UINavigationItem item) => item.navigationLayer == navigationLayer))
		{
			item.CheckForNavigationLayer(isFirstTime: true);
		}

		NavigationLayers.Remove(navigationLayer);
		UINavigationHistory.GoBackALayer();
		if (navigationLayer.isOverrideLayer)
		{
			ActiveNavigationLayer = NavigationLayers.LastOrDefault((UINavigationLayer x) => x.isOverrideLayer);
		}
	}

	public void Update()
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse && (bool)currentNavigationItem)
		{
			SetCurrentNavigationItem(null);
			return;
		}

		UINavigationLayer activeNavigationLayer = ActiveNavigationLayer;
		if ((bool)currentNavigationItem && (!AvailableNavigationItems.Contains(currentNavigationItem) || !currentNavigationItem.CheckLayer()))
		{
			SetCurrentNavigationItem(null);
		}

		if (!currentNavigationItem && AvailableNavigationItems.Any((UINavigationItem a) => (bool)a && a.navigationLayer == activeNavigationLayer))
		{
			UINavigationDefaultSystem.SetStartingItem();
		}
		else if ((bool)currentNavigationItem)
		{
			CheckForNavigation();
			if (Vector3.Distance(currentNavigationItem.Position, lastNavigationItemPos) > Mathf.Epsilon)
			{
				SetCursor();
			}
		}

		if (activeNavigationLayer != lastActiveNavigationLayer)
		{
			UINavigationHistory.AddLayer(activeNavigationLayer);
		}

		foreach (UINavigationHistory.Layer layer in UINavigationHistory.layers)
		{
			if (layer.navigationLayer == activeNavigationLayer)
			{
				if (layer.navigationItemHistory.Count <= 0 || layer.navigationItemHistory.Last() != currentNavigationItem)
				{
					layer.navigationItemHistory.Add(currentNavigationItem);
				}

				break;
			}
		}

		lastActiveNavigationLayer = activeNavigationLayer;
	}

	public void SetCursor()
	{
		VirtualPointer.Show();
		virtualCursor.SetPosition(currentNavigationItem.Position);
		lastNavigationItemPos = (currentNavigationItem ? currentNavigationItem.Position : lastNavigationItemPos);
	}

	public void ResetState()
	{
		UINavigationDefaultSystem.SetStartingItem();
	}

	public void CheckForNavigation()
	{
		if (!virtualCursor.showVirtualPointerState || Console.active)
		{
			return;
		}

		if (InputSystem.CheckLongHold())
		{
			SetCurrentNavigationItem(GetSelectable(lastMove));
		}

		int num = (InputSystem.IsButtonPressed("Move Vertical") ? 1 : (InputSystem.IsButtonPressed("Move Vertical", positive: false) ? (-1) : 0));
		int num2 = (InputSystem.IsButtonPressed("Move Horizontal") ? 1 : (InputSystem.IsButtonPressed("Move Horizontal", positive: false) ? (-1) : 0));
		if ((float)Mathf.Abs(num) > navigationControllerDeadzone || (float)Mathf.Abs(num2) > navigationControllerDeadzone)
		{
			if (!didNavigate)
			{
				if (Mathf.Abs(num) > Mathf.Abs(num2))
				{
					if ((float)num > navigationControllerDeadzone)
					{
						SetCurrentNavigationItem(GetSelectable(MoveDirection.Up));
					}

					if ((float)num < 0f - navigationControllerDeadzone)
					{
						SetCurrentNavigationItem(GetSelectable(MoveDirection.Down));
					}
				}
				else
				{
					if ((float)num2 < 0f - navigationControllerDeadzone)
					{
						SetCurrentNavigationItem(GetSelectable(MoveDirection.Left));
					}

					if ((float)num2 > navigationControllerDeadzone)
					{
						SetCurrentNavigationItem(GetSelectable(MoveDirection.Right));
					}
				}

				didNavigate = true;
			}
		}
		else
		{
			didNavigate = false;
		}

		if (didNavigate)
		{
			currentNavigationItemChanged = true;
			Events.InvokeUINavigation();
		}
	}

	public UINavigationItem GetSelectable(MoveDirection moveDirection)
	{
		lastMove = moveDirection;
		if (currentNavigationItem.overrideInputs)
		{
			switch (moveDirection)
			{
				case MoveDirection.Left:
				if ((bool)currentNavigationItem.inputLeft)
				{
					return currentNavigationItem.inputLeft;
					}
	
					break;
				case MoveDirection.Right:
				if ((bool)currentNavigationItem.inputRight)
				{
					return currentNavigationItem.inputRight;
					}
	
					break;
				case MoveDirection.Up:
				if ((bool)currentNavigationItem.inputUp)
				{
					return currentNavigationItem.inputUp;
					}
	
					break;
				case MoveDirection.Down:
				if ((bool)currentNavigationItem.inputDown)
				{
					return currentNavigationItem.inputDown;
					}
	
					break;
			}
		}

		if (currentNavigationItem.overrideHorizontal)
		{
			switch (moveDirection)
			{
				case MoveDirection.Left:
					currentNavigationItem.OnHorizontalOverride?.Invoke(-1f);
					return currentNavigationItem;
				case MoveDirection.Right:
					currentNavigationItem.OnHorizontalOverride?.Invoke(1f);
					return currentNavigationItem;
			}
		}

		if (currentNavigationItem.overrideVertical)
		{
			switch (moveDirection)
			{
				case MoveDirection.Up:
					currentNavigationItem.OnVerticalOverride?.Invoke(1f);
					return currentNavigationItem;
				case MoveDirection.Down:
					currentNavigationItem.OnVerticalOverride?.Invoke(-1f);
					return currentNavigationItem;
			}
		}

		PossibleNavigationItems = AvailableNavigationItems.Where((UINavigationItem x) => (bool)x && x != currentNavigationItem && x.CheckLayer()).ToList();
		Vector3 position = currentNavigationItem.Position;
		UINavigationItem selectable = GetSelectable(moveDirection, position);
		if (!selectable)
		{
			return currentNavigationItem;
		}

		return selectable;
	}

	public UINavigationItem GetSelectable(MoveDirection moveDirection, Vector3 currentPosition)
	{
		UINavigationItem uINavigationItem = null;
		switch (moveDirection)
		{
			case MoveDirection.Left:
				uINavigationItem = GetSelectable(currentPosition, Vector3.left, (UINavigationItem a) => currentPosition.x - a.Position.x > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(a.Position.y - currentPosition.y) < navigationGridThreshold * maxAnyItemCheckBeforeLoopMultiplier && Mathf.Abs(Vector3.Angle(Vector3.left, currentPosition.GetDirTowardsPoint(a.Position))) < maxNavAngle);
			if (!uINavigationItem && currentNavigationItem.canLoop)
			{
				uINavigationItem = GetSelectableLoop(currentPosition, Vector3.left, (UINavigationItem a) => Mathf.Abs(currentPosition.x - a.Position.x) > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(currentPosition.y - a.Position.y) < navigationGridThreshold, (UINavigationItem a) => a.Position.x - currentPosition.x > minorNavigationGridThreshold);
				}
	
				break;
			case MoveDirection.Right:
				uINavigationItem = GetSelectable(currentPosition, Vector3.right, (UINavigationItem a) => a.Position.x - currentPosition.x > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(a.Position.y - currentPosition.y) < navigationGridThreshold * maxAnyItemCheckBeforeLoopMultiplier && Mathf.Abs(Vector3.Angle(Vector3.right, currentPosition.GetDirTowardsPoint(a.Position))) < maxNavAngle);
			if (!uINavigationItem && currentNavigationItem.canLoop)
			{
				uINavigationItem = GetSelectableLoop(currentPosition, Vector3.right, (UINavigationItem a) => Mathf.Abs(currentPosition.x - a.Position.x) > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(currentPosition.y - a.Position.y) < navigationGridThreshold, (UINavigationItem a) => currentPosition.x - a.Position.x > minorNavigationGridThreshold);
				}
	
				break;
			case MoveDirection.Down:
				uINavigationItem = GetSelectable(currentPosition, Vector3.down, (UINavigationItem a) => currentPosition.y - a.Position.y > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(a.Position.x - currentPosition.x) < navigationGridThreshold * maxAnyItemCheckBeforeLoopMultiplier && Mathf.Abs(Vector3.Angle(Vector3.down, currentPosition.GetDirTowardsPoint(a.Position))) < maxNavAngle);
			if (!uINavigationItem && currentNavigationItem.canLoop)
			{
				uINavigationItem = GetSelectableLoop(currentPosition, Vector3.down, (UINavigationItem a) => Mathf.Abs(currentPosition.y - a.Position.y) > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(currentPosition.x - a.Position.x) < navigationGridThreshold, (UINavigationItem a) => a.Position.y - currentPosition.y > minorNavigationGridThreshold);
				}
	
				break;
			case MoveDirection.Up:
				uINavigationItem = GetSelectable(currentPosition, Vector3.up, (UINavigationItem a) => a.Position.y - currentPosition.y > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(a.Position.x - currentPosition.x) < navigationGridThreshold * maxAnyItemCheckBeforeLoopMultiplier && Mathf.Abs(Vector3.Angle(Vector3.up, currentPosition.GetDirTowardsPoint(a.Position))) < maxNavAngle);
			if (!uINavigationItem && currentNavigationItem.canLoop)
			{
				uINavigationItem = GetSelectableLoop(currentPosition, Vector3.up, (UINavigationItem a) => Mathf.Abs(currentPosition.y - a.Position.y) > minorNavigationGridThreshold, (UINavigationItem a) => Mathf.Abs(currentPosition.x - a.Position.x) < navigationGridThreshold, (UINavigationItem a) => currentPosition.y - a.Position.y > minorNavigationGridThreshold);
				}
	
				break;
		}

		return uINavigationItem;
	}

	public UINavigationItem GetSelectable(Vector3 currentPosition, Vector3 direction, Predicate<UINavigationItem> directionCheck, Predicate<UINavigationItem> alignmentCheck)
	{
		List<UINavigationItem> source = PossibleNavigationItems.Where((UINavigationItem a) => directionCheck(a) && alignmentCheck(a)).ToList();
		if (source.Any())
		{
			return source.OrderBy((UINavigationItem a) => Vector3.Distance(a.Position, currentPosition).RemapProportion(0f, navigationGridThreshold, 0f, disPriority) + Mathf.Abs(Vector3.Angle(direction, currentPosition.GetDirTowardsPoint(a.Position))).RemapProportion(0f, maxNavAngle, 0f, anglePriority)).First();
		}

		return null;
	}

	public UINavigationItem GetSelectableLoop(Vector3 currentPosition, Vector3 direction, Predicate<UINavigationItem> directionCheck, Predicate<UINavigationItem> alignmentCheck, Predicate<UINavigationItem> fallbackCheck)
	{
		List<UINavigationItem> list = PossibleNavigationItems.Where((UINavigationItem a) => directionCheck(a) && alignmentCheck(a)).ToList();
		if (!list.Any())
		{
			list = PossibleNavigationItems.Where((UINavigationItem a) => fallbackCheck(a)).ToList();
		}

		if (list.Count > 0)
		{
			UINavigationItem uINavigationItem = list.OrderBy((UINavigationItem a) => a.Position.DistanceTo(currentPosition)).Last();
			if (list.Count == 1)
			{
				return uINavigationItem;
			}

			Vector3 furthestPosition = (uINavigationItem.Position - direction * navigationGridThreshold).WithZ(currentPosition.z);
			return list.OrderBy((UINavigationItem a) => Vector3.Distance(a.Position, furthestPosition).RemapProportion(0f, navigationGridThreshold, 0f, disPriority) + Mathf.Abs(Vector3.Angle(direction, furthestPosition.GetDirTowardsPoint(a.Position))).RemapProportion(0f, maxNavAngle, 0f, anglePriority)).First();
		}

		return null;
	}

	public void SetCurrentNavigationItem(UINavigationItem navItem)
	{
		if ((bool)currentNavigationItem)
		{
			if (currentNavigationItem.Equals(navItem))
			{
				return;
			}

			eventSystem.Unhover(currentNavigationItem.clickHandler);
		}

		if ((bool)navItem && AvailableNavigationItems.Any((UINavigationItem a) => a == navItem))
		{
			currentNavigationItem = navItem;
			UINavigationLayer activeNavigationLayer = ActiveNavigationLayer;
			if (!activeNavigationLayer || activeNavigationLayer.forceHover)
			{
				eventSystem.Hover(navItem.clickHandler);
			}

			UINavigationHistory.AddItem(navItem);
		}
		else
		{
			currentNavigationItem = null;
		}
	}
}
