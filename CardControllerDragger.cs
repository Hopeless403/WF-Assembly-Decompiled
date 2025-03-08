#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardControllerDragger : CardController
{
	[Header("Dragging")]
	public bool canDrag = true;

	public UnityEventEntity onDrag;

	public UnityEventEntity onRelease;

	public UnityEventEntity onCancel;

	public override void Press()
	{
		if (canDrag && (bool)pressEntity && pressEntity.owner == owner && TryDrag(pressEntity))
		{
			onDrag?.Invoke(pressEntity);
		}
	}

	public override void DragCancel()
	{
		if ((bool)dragging)
		{
			onCancel?.Invoke(dragging);
			DragEnd();
		}
	}

	public override void Release()
	{
		if ((bool)dragging)
		{
			onRelease?.Invoke(dragging);
			DragEnd();
		}
	}

	public override void DragEnd()
	{
		dragging.TweenToContainer();
		TweenUnHover(dragging);
		base.DragEnd();
	}
}
