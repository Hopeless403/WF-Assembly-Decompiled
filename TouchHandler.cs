#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour
{
	[SerializeField]
	public bool hoverBeforePress = true;

	public bool CanTouchPress(bool alreadyHovered)
	{
		return !hoverBeforePress || alreadyHovered;
	}

	public void HandleTouchPress(PointerEventData pointer, bool alreadyHovered)
	{
		pointer.pressPosition = pointer.position;
		pointer.pointerPressRaycast = pointer.pointerCurrentRaycast;
		ExecuteEvents.ExecuteHierarchy(base.gameObject, pointer, ExecuteEvents.pointerDownHandler);
		pointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(base.gameObject);
		if ((bool)pointer.pointerDrag)
		{
			ExecuteEvents.Execute(pointer.pointerDrag, pointer, ExecuteEvents.initializePotentialDrag);
		}
	}
}
