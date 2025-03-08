#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class DeckpackBlocker : MonoBehaviour
{
	public static uint open;

	public void OnEnable()
	{
		Block();
	}

	public void OnDisable()
	{
		Unblock();
	}

	public static void Block()
	{
		if (open++ == 0)
		{
			SetButtonsInteractable(interactable: false);
			InputSystem.reset = true;
		}
	}

	public static void Unblock()
	{
		if (--open == 0)
		{
			SetButtonsInteractable(interactable: true);
			InputSystem.reset = true;
		}
	}

	public static void SetButtonsInteractable(bool interactable)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Deckpack Interaction");
		for (int i = 0; i < array.Length; i++)
		{
			Selectable component = array[i].GetComponent<Selectable>();
			if ((object)component != null)
			{
				component.interactable = interactable;
			}
		}
	}
}
