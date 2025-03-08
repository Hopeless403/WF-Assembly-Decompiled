#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class CursorDisplay : MonoBehaviourRect
{
	[SerializeField]
	public TouchInputModule inputModule;

	[SerializeField]
	public RectTransform pre;

	[SerializeField]
	public TMP_Text text;

	public void LateUpdate()
	{
		Vector2 mousePosition = inputModule.MousePosition;
		Vector2 lastMousePosition = inputModule.LastMousePosition;
		base.rectTransform.position = mousePosition;
		if ((bool)text)
		{
			Vector2 mouseMove = inputModule.MouseMove;
			text.text = $"({Mathf.RoundToInt(mousePosition.x)}, {Mathf.RoundToInt(mousePosition.y)})\n" + $"({Mathf.RoundToInt(lastMousePosition.x)}, {Mathf.RoundToInt(lastMousePosition.y)})\n" + $"({Mathf.RoundToInt(mouseMove.x)}, {Mathf.RoundToInt(mouseMove.y)})\n" + $"Hovering: {inputModule.Hover}\n" + $"Pressing: {inputModule.Press}";
		}

		if ((bool)pre)
		{
			pre.position = lastMousePosition;
		}
	}
}
