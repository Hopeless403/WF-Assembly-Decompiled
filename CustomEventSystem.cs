#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventSystem : MonoBehaviour
{
	[SerializeField]
	public GameObject current;

	[SerializeField]
	public GameObject press;

	public readonly PointerEventData buttonData = new PointerEventData(null);

	public void Update()
	{
		if (!press)
		{
			if (InputSystem.IsSelectPressed() && (bool)current)
			{
				Press(current);
			}
		}
		else if (!InputSystem.IsSelectHeld())
		{
			Release(press);
		}
	}

	public void Hover(GameObject obj)
	{
		current = obj;
		ExecuteEvents.ExecuteHierarchy(obj, buttonData, ExecuteEvents.pointerEnterHandler);
	}

	public void Unhover(GameObject obj)
	{
		if ((bool)current && current.Equals(obj))
		{
			current = null;
		}

		ExecuteEvents.ExecuteHierarchy(obj, buttonData, ExecuteEvents.pointerExitHandler);
	}

	public void Press(GameObject obj)
	{
		press = obj;
		ExecuteEvents.ExecuteHierarchy(obj, buttonData, ExecuteEvents.pointerDownHandler);
	}

	public void Release(GameObject obj)
	{
		ExecuteEvents.ExecuteHierarchy(obj, buttonData, ExecuteEvents.pointerUpHandler);
		if ((bool)obj && (bool)current && obj.Equals(current))
		{
			ExecuteEvents.ExecuteHierarchy(obj, buttonData, ExecuteEvents.pointerClickHandler);
		}

		press = null;
	}
}
