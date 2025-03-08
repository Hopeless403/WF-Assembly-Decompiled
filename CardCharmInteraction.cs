#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UpgradeDisplay))]
public class CardCharmInteraction : MonoBehaviourRect, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	public GameObject image;

	public CardCharmDragHandler dragHandler;

	public bool canHover;

	public bool canDrag;

	public bool hover;

	public bool preHover;

	public bool press;

	public bool drag;

	public Vector2 popUpOffset = new Vector2(0f, -1f);

	public UnityEvent<UpgradeDisplay> onHover;

	public UnityEvent<UpgradeDisplay> onUnHover;

	public UnityEvent<UpgradeDisplay> onDrag;

	public UnityEvent<UpgradeDisplay> onDragEnd;

	public UpgradeDisplay _upgradeDisplay;

	public UpgradeDisplay upgradeDisplay => _upgradeDisplay ?? (_upgradeDisplay = GetComponent<UpgradeDisplay>());

	public bool DragHandlerDragging
	{
		get
		{
			if ((bool)dragHandler)
			{
				return dragHandler.IsDragging;
			}

			return false;
		}
	}

	public void LateUpdate()
	{
		if (!press)
		{
			if (hover && InputSystem.IsSelectPressed())
			{
				press = true;
				if (preHover)
				{
					Press();
				}
			}
		}
		else if (MonoBehaviourSingleton<Cursor3d>.instance.usingTouch ? InputSystem.IsSelectReleased() : InputSystem.IsDynamicSelectReleased(drag))
		{
			Release();
			press = false;
			if (MonoBehaviourSingleton<Cursor3d>.instance.usingTouch)
			{
				StartCoroutine(UpdateInputSystem());
			}
		}

		preHover = hover;
	}

	public IEnumerator UpdateInputSystem()
	{
		yield return null;
		if (upgradeDisplay is CardCharm cardCharm)
		{
			cardCharm.StopWobble();
		}

		yield return null;
		Events.InvokeUpdateInputSystem(forceTouch: true);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Hover();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (hover && press && !drag && MonoBehaviourSingleton<Cursor3d>.instance.usingTouch && (bool)dragHandler)
		{
			StartDrag();
		}
		else
		{
			UnHover();
		}
	}

	public void Press()
	{
		if (canDrag && hover)
		{
			StartDrag();
		}
	}

	public void Release()
	{
		if (drag)
		{
			StopDrag();
		}
	}

	public void Hover()
	{
		if (canHover && !drag && !DragHandlerDragging)
		{
			if (!hover)
			{
				LeanTween.cancel(image);
				LeanTween.scale(image, Vector3.one * 1.1f, 0.33f).setEase(LeanTweenType.easeOutBack);
			}

			hover = true;
			onHover?.Invoke(upgradeDisplay);
			Events.InvokeUpgradeHover(upgradeDisplay);
			PopUpDescription();
		}
	}

	public void UnHover()
	{
		if (canHover && hover)
		{
			LeanTween.cancel(image);
			LeanTween.scale(image, Vector3.one, 0.2f).setEase(LeanTweenType.easeOutQuart);
			hover = false;
			onUnHover?.Invoke(upgradeDisplay);
			HideDescription();
		}
	}

	public void StartDrag()
	{
		Debug.Log("Dragging Card Charm [" + base.name + "]");
		drag = true;
		onDrag?.Invoke(upgradeDisplay);
		UnHover();
		upgradeDisplay.CanRaycast = false;
	}

	public void StopDrag()
	{
		Debug.Log("Dropping Card Charm [" + base.name + "]");
		onDragEnd?.Invoke(upgradeDisplay);
		drag = false;
		upgradeDisplay.CanRaycast = true;
	}

	public void CancelDrag()
	{
		Debug.Log("Cancelling Card Charm Drag [" + base.name + "]");
		drag = false;
		upgradeDisplay.CanRaycast = true;
	}

	public void PopUpDescription()
	{
		CardPopUp.AssignTo(base.rectTransform, popUpOffset.x, popUpOffset.y);
		CardPopUp.AddPanel(upgradeDisplay.data.name, upgradeDisplay.data.title, upgradeDisplay.data.text);
	}

	public void HideDescription()
	{
		CardPopUp.RemovePanel(upgradeDisplay.data.name);
	}
}
