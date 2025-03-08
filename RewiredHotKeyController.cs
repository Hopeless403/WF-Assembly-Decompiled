#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewiredHotKeyController : MonoBehaviour
{
	[SerializeField]
	public string HotKeyString;

	public UINavigationLayer uINavigationLayer;

	public Button uiButton;

	public EventTrigger eventTrigger;

	public UnityEvent OnHotKeyPressed;

	public bool ignoreLayers;

	public bool ignoreActivateCooldown;

	public HotKeyDisplay display;

	[Header("Keyboard")]
	[SerializeField]
	public bool hasKeyboardInput;

	[SerializeField]
	[ShowIf("hasKeyboardInput")]
	public KeyCode keyboardAction;

	public bool press;

	public static int ActivateCooldown;

	public static RewiredHotKeyController ActivateCooldownInstance;

	public bool IsHotKeyHeld()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			return InputSystem.IsButtonHeld(HotKeyString);
		}

		if (hasKeyboardInput)
		{
			return Input.GetKey(keyboardAction);
		}

		return false;
	}

	public bool IsHotKeyPressed()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			return InputSystem.IsButtonPressed(HotKeyString);
		}

		if (hasKeyboardInput)
		{
			return Input.GetKeyDown(keyboardAction);
		}

		return false;
	}

	public void SetActionName(string value)
	{
		HotKeyString = value;
		if ((bool)display)
		{
			display.SetActionName(value);
		}
	}

	public void Update()
	{
		if (ActivateCooldown > 0 && ActivateCooldownInstance == this && --ActivateCooldown <= 0)
		{
			ActivateCooldownInstance = null;
		}

		if ((ignoreLayers || MonoBehaviourSingleton<UINavigationSystem>.instance.lastActiveNavigationLayer == uINavigationLayer) && (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse || hasKeyboardInput))
		{
			if (press)
			{
				if (!InputSystem.Enabled || InputSystem.reset)
				{
					press = false;
					Release();
				}
				else
				{
					if (IsHotKeyHeld())
					{
						return;
					}

					if (ActivateCooldown > 0 && (bool)ActivateCooldownInstance)
					{
						if (!ignoreActivateCooldown)
						{
							press = false;
						}
					}
					else
					{
						ActivateCooldown = 5;
						ActivateCooldownInstance = this;
					}

					Release();
				}
			}
			else if (IsHotKeyPressed())
			{
				Press();
			}
		}
		else if (press)
		{
			press = false;
			Release();
		}
	}

	public void OnDisable()
	{
		if (ActivateCooldown > 0 && ActivateCooldownInstance == this)
		{
			ActivateCooldown = 0;
			ActivateCooldownInstance = null;
		}
	}

	public void Press()
	{
		press = true;
		if ((bool)eventTrigger && eventTrigger.enabled)
		{
			eventTrigger.OnPointerDown(null);
		}
	}

	public void Release()
	{
		if (press)
		{
			Invoke();
		}

		if ((bool)eventTrigger)
		{
			if (press && eventTrigger.enabled)
			{
				eventTrigger.OnPointerClick(null);
			}

			eventTrigger.OnPointerUp(null);
		}

		press = false;
	}

	public void Invoke()
	{
		if (OnHotKeyPressed != null && OnHotKeyPressed.GetPersistentEventCount() > 0)
		{
			OnHotKeyPressed.Invoke();
		}
		else if ((bool)uiButton && uiButton.interactable)
		{
			uiButton.onClick?.Invoke();
		}
	}
}
