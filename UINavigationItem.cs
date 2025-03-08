#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UINavigationItem : MonoBehaviour
{
	public enum SelectionPriority
	{
		Mega = 10,
		Highest = 2,
		High = 1,
		Medium = 0,
		Low = -1,
		Lowest = -2
	}

	public UINavigationLayer navigationLayer;

	public Vector3 offset;

	public SelectionPriority selectionPriority;

	public GameObject clickHandler;

	public bool isHighlighted;

	public bool canLoop;

	public bool ignoreLayers;

	public bool isSelectable = true;

	public bool overrideInputs;

	[ShowIf("overrideInputs")]
	public UINavigationItem inputLeft;

	[ShowIf("overrideInputs")]
	public UINavigationItem inputRight;

	[ShowIf("overrideInputs")]
	public UINavigationItem inputUp;

	[ShowIf("overrideInputs")]
	public UINavigationItem inputDown;

	[HideIf("overrideInputs")]
	public bool overrideHorizontal;

	[ShowIf("overrideHorizontal")]
	[HideIf("overrideInputs")]
	public UnityEvent<float> OnHorizontalOverride;

	[HideIf("overrideInputs")]
	public bool overrideVertical;

	[ShowIf("overrideVertical")]
	[HideIf("overrideInputs")]
	public UnityEvent<float> OnVerticalOverride;

	public bool hasLayerBeenChecked;

	public Camera _cam;

	public Vector3 Position => base.transform.position + offset;

	public Camera cam => _cam ?? (_cam = Camera.main);

	public void OnEnable()
	{
		CheckForReferences(!hasLayerBeenChecked);
		MonoBehaviourSingleton<UINavigationSystem>.instance.RegisterSelectable(this);
	}

	public void OnDisable()
	{
		OnRemoved();
	}

	public void OnDestroy()
	{
		OnRemoved();
	}

	public void OnRemoved()
	{
		MonoBehaviourSingleton<UINavigationSystem>.instance.UnregisterSelectable(this);
	}

	public virtual void Reset()
	{
		if (GetComponents<UINavigationItem>().Length > 1)
		{
			Object.Destroy(this);
		}

		CheckForReferences(isFirstTime: true);
	}

	public void OnTransformParentChanged()
	{
		navigationLayer = null;
		CheckForNavigationLayer(!hasLayerBeenChecked);
	}

	public void CheckForReferences(bool isFirstTime)
	{
		if (navigationLayer == null)
		{
			CheckForNavigationLayer(isFirstTime);
		}

		if (clickHandler == null)
		{
			CheckForSelectable();
		}
	}

	public void CheckForNavigationLayer(bool isFirstTime)
	{
		if (isFirstTime)
		{
			navigationLayer = null;
		}

		CheckForNavigationLayer(base.transform, isFirstTime);
		hasLayerBeenChecked = true;
	}

	public void CheckForNavigationLayer(Transform checkTransform, bool isFirstTime)
	{
		if ((bool)checkTransform.GetComponent<UINavigationLayer>() && checkTransform.GetComponent<UINavigationLayer>().isOverrideLayer && (isFirstTime || checkTransform.GetComponent<UINavigationLayer>().allowLayerToBeAppliedAtRuntime))
		{
			navigationLayer = checkTransform.GetComponent<UINavigationLayer>();
		}

		if (!navigationLayer && (bool)checkTransform.transform.parent)
		{
			CheckForNavigationLayer(checkTransform.transform.parent, isFirstTime);
		}
	}

	public void CheckForSelectable()
	{
		CheckForSelectableDown(base.transform);
		if (clickHandler == null)
		{
			CheckForSelectableUp(base.transform.parent);
		}
	}

	public void CheckForSelectableDown(Transform checkTransform)
	{
		clickHandler = ((checkTransform.GetComponent<IPointerDownHandler>() != null) ? checkTransform.gameObject : null);
		if (!(clickHandler == null))
		{
			return;
		}

		for (int i = 0; i < checkTransform.childCount; i++)
		{
			CheckForSelectableDown(checkTransform.GetChild(i));
			if (clickHandler != null)
			{
				break;
			}
		}
	}

	public void CheckForSelectableUp(Transform checkTransform)
	{
		if (checkTransform != null)
		{
			clickHandler = ((checkTransform.GetComponent<IPointerDownHandler>() != null) ? checkTransform.gameObject : null);
			if (clickHandler == null && (bool)checkTransform.transform.parent)
			{
				CheckForSelectableDown(checkTransform.transform.parent);
			}
		}
	}

	public bool CheckLayer()
	{
		UINavigationLayer activeNavigationLayer = UINavigationSystem.ActiveNavigationLayer;
		if (ignoreLayers || navigationLayer == activeNavigationLayer)
		{
			if (!activeNavigationLayer || !activeNavigationLayer.allowOutsideVisibleSelection)
			{
				return cam.IsInCameraView(Position);
			}

			return true;
		}

		return false;
	}
}
