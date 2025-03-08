#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Rewired;
using UnityEngine;

public class InputSystem : GameSystem
{
	public static InputSystem instance;

	[Header("Hold Direction Settings")]
	public float holdDirectionStartTime = 0.1f;

	public float holdDirectionFlowTime = 0.05f;

	public static Player mainPlayer;

	public new static bool enabled = true;

	public static bool isLongHeld;

	public static float holdDirectionTime;

	public static bool wasSelectHeldLong;

	public static bool wasSelectHeldLong2;

	public static bool AllowDynamicSelectRelease = true;

	public static int _reset;

	public static bool Enabled
	{
		get
		{
			if (enabled)
			{
				return !Transition.Running;
			}

			return false;
		}
	}

	public static Vector3 MousePosition { get; set; }

	public static bool reset
	{
		get
		{
			return _reset > 0;
		}
		set
		{
			_reset = (value ? 2 : 0);
		}
	}

	public void Awake()
	{
		instance = this;
		mainPlayer = ReInput.players.GetPlayer(0);
	}

	public void LateUpdate()
	{
		wasSelectHeldLong = wasSelectHeldLong2;
		wasSelectHeldLong2 = mainPlayer.GetButtonTimedPress("Select", 0.1f);
		if (Input.touchCount > 0)
		{
			MousePosition = Input.GetTouch(0).position;
		}
		else
		{
			MousePosition = Input.mousePosition;
		}

		_reset--;
	}

	public new static void Enable()
	{
		enabled = true;
	}

	public new static void Disable()
	{
		enabled = false;
	}

	public static bool IsButtonPressed(string input, bool positive = true)
	{
		if (!(Enabled && positive))
		{
			return mainPlayer.GetNegativeButtonDown(input);
		}

		return mainPlayer.GetButtonDown(input);
	}

	public static bool IsButtonHeld(string input, bool positive = true)
	{
		if (!(Enabled && positive))
		{
			return mainPlayer.GetNegativeButton(input);
		}

		return mainPlayer.GetButton(input);
	}

	public static bool IsButtonLongHeld(string input, bool positive = true)
	{
		if (!(Enabled && positive))
		{
			return mainPlayer.GetNegativeButtonLongPress(input);
		}

		return mainPlayer.GetButtonLongPress(input);
	}

	public static bool IsButtonReleased(string input, bool positive = true)
	{
		if (!(Enabled && positive))
		{
			return mainPlayer.GetNegativeButtonUp(input);
		}

		return mainPlayer.GetButtonUp(input);
	}

	public static bool WasButtonPressed(string input, bool positive = true)
	{
		if (!(Enabled && positive))
		{
			return mainPlayer.GetNegativeButtonPrev(input);
		}

		return mainPlayer.GetButtonPrev(input);
	}

	public static bool WasButtonReleased(string input, bool positive = true)
	{
		if (!(Enabled && positive))
		{
			return !mainPlayer.GetNegativeButtonPrev(input);
		}

		return !mainPlayer.GetButtonPrev(input);
	}

	public static bool IsSelectPressed()
	{
		return IsButtonPressed("Select");
	}

	public static bool IsSelectHeld()
	{
		return IsButtonHeld("Select");
	}

	public static bool IsSelectReleased()
	{
		return IsButtonReleased("Select");
	}

	public static bool IsDynamicSelectReleased(bool allowSelectAgainToRelease)
	{
		if (!Enabled)
		{
			return false;
		}

		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse && wasSelectHeldLong && !IsSelectHeld())
		{
			return true;
		}

		if (allowSelectAgainToRelease)
		{
			return IsSelectPressed();
		}

		return !IsSelectHeld();
	}

	public static bool CheckLongHold()
	{
		if (IsButtonLongHeld("Move Vertical") || IsButtonLongHeld("Move Vertical", positive: false) || IsButtonLongHeld("Move Horizontal") || IsButtonLongHeld("Move Horizontal", positive: false))
		{
			if (!isLongHeld)
			{
				holdDirectionTime = instance.holdDirectionStartTime;
			}

			isLongHeld = true;
		}

		if (isLongHeld)
		{
			if (!RewiredControllerManager.instance.IsButtonReleased() && RewiredControllerManager.instance.IsButtonHeld())
			{
				holdDirectionTime -= Time.unscaledDeltaTime;
				if (holdDirectionTime <= 0f)
				{
					holdDirectionTime = instance.holdDirectionFlowTime;
					return true;
				}

				return false;
			}

			isLongHeld = false;
		}

		return isLongHeld;
	}

	public static float GetAxis(string actionName)
	{
		if (Enabled)
		{
			return mainPlayer.GetAxis(actionName);
		}

		return 0f;
	}

	public static float GetAxisDelta(string actionName)
	{
		if (Enabled)
		{
			return mainPlayer.GetAxisDelta(actionName);
		}

		return 0f;
	}
}
