#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardControllerCardOrganizer : CardController
{
	[Header("Plane to store positions in")]
	[SerializeField]
	public CardPlane cardPlane;

	[Header("Snap To Grid")]
	[SerializeField]
	public Vector2 snapOffset;

	[SerializeField]
	public Vector2 snapSize = new Vector2(3.8f, 5.5f);

	public override void Press()
	{
		if ((bool)pressEntity && TryDrag(pressEntity))
		{
			pressEntity.transform.SetAsLastSibling();
		}
	}

	public override void Release()
	{
		if (dragging != null)
		{
			Vector3 vector = (GetDragPosition() - dragging.transform.parent.position).WithZ(0f);
			if (Input.GetButton("Snap"))
			{
				vector = Snap(vector);
			}

			dragging.transform.localPosition = vector;
			cardPlane?.StorePosition(dragging);
			dragging.gameObject.layer = draggingLayerPre;
			dragging.dragging = false;
			TweenUnHover(dragging);
			DragEnd();
		}
	}

	public override void DragUpdatePosition()
	{
		Vector3 vector = (GetDragPosition() - dragging.transform.parent.position).WithZ(dragZ);
		if (Input.GetButton("Snap"))
		{
			vector = Snap(vector);
		}

		dragging.transform.localPosition = Delta.Lerp(dragging.transform.localPosition, vector, dragLerp, Time.deltaTime);
	}

	public Vector3 Snap(Vector3 position)
	{
		position.x = Mathf.Round((position.x + snapOffset.x) / snapSize.x) * snapSize.x - snapOffset.x;
		position.y = Mathf.Round((position.y + snapOffset.y) / snapSize.y) * snapSize.y - snapOffset.y;
		return position;
	}
}
