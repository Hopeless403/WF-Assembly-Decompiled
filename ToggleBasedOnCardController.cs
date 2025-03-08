#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;

public class ToggleBasedOnCardController : MonoBehaviour
{
	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public MonoBehaviour[] components;

	[SerializeField]
	public UnityEvent onEnable;

	[SerializeField]
	public UnityEvent onDisable;

	public void AssignCardController(CardController controller)
	{
		cardController = controller;
		if (controller.enabled)
		{
			Enable();
		}
		else
		{
			Disable();
		}
	}

	public void OnEnable()
	{
		Events.OnCardControllerEnabled += CardControllerEnabled;
		Events.OnCardControllerDisabled += CardControllerDisabled;
	}

	public void OnDisable()
	{
		Events.OnCardControllerEnabled -= CardControllerEnabled;
		Events.OnCardControllerDisabled -= CardControllerDisabled;
	}

	public void CardControllerEnabled(CardController controller)
	{
		if (controller == cardController)
		{
			Enable();
		}
	}

	public void CardControllerDisabled(CardController controller)
	{
		if (controller == cardController)
		{
			Disable();
		}
	}

	public void Enable()
	{
		MonoBehaviour[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}

		onEnable?.Invoke();
	}

	public void Disable()
	{
		MonoBehaviour[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}

		onDisable?.Invoke();
	}
}
