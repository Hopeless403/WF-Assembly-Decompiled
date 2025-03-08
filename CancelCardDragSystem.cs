#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CancelCardDragSystem : GameSystem
{
	[SerializeField]
	public string input = "Back";

	public void Update()
	{
		if (InputSystem.Enabled && !InputSystem.reset && !InputSystem.IsButtonPressed("Back"))
		{
			return;
		}

		CardController[] array = Object.FindObjectsOfType<CardController>();
		foreach (CardController cardController in array)
		{
			if ((bool)cardController.dragging)
			{
				cardController.DragCancel();
			}
		}
	}
}
