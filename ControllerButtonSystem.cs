#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using Rewired;
using UnityEngine;
using UnityEngine.Localization;

public class ControllerButtonSystem : GameSystem
{
	public static ControllerButtonSystem instance;

	public static JoystickButtonStyle style;

	[SerializeField]
	public JoystickButtonStyle defaultControllerStyle;

	[SerializeField]
	public JoystickButtonStyle mouseStyle;

	[SerializeField]
	public JoystickButtonStyle touchStyle;

	[SerializeField]
	public JoystickButtonStyle[] styles;

	public void Awake()
	{
		instance = this;
	}

	public void OnEnable()
	{
		Events.OnControllerSwitched += ControllerSwitched;
	}

	public void OnDisable()
	{
		Events.OnControllerSwitched -= ControllerSwitched;
	}

	public static void ControllerSwitched()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			SetControllerStyle();
		}
	}

	public static void SetMouseStyle()
	{
		style = instance.mouseStyle;
		Events.InvokeButtonStyleChanged();
	}

	public static void SetTouchStyle()
	{
		style = instance.touchStyle;
		Events.InvokeButtonStyleChanged();
	}

	public static void SetControllerStyle()
	{
		Player playerController = RewiredControllerManager.GetPlayerController(0);
		Controller controller = null;
		double num = -1.0;
		foreach (Joystick joystick in playerController.controllers.Joysticks)
		{
			double lastTimeActive = joystick.GetLastTimeActive();
			if (lastTimeActive > num)
			{
				num = lastTimeActive;
				controller = joystick;
			}
		}

		if (controller != null)
		{
			style = instance.styles.FirstOrDefault((JoystickButtonStyle a) => a.guids.Contains(controller.hardwareTypeGuid));
			if ((object)style == null)
			{
				style = instance.defaultControllerStyle;
			}

			Debug.LogWarning("ControllerButtonStyle Set: [" + style.name + "]");
			Events.InvokeButtonStyleChanged();
		}
	}

	public static Sprite Get(string action)
	{
		if (action.IsNullOrWhitespace())
		{
			return null;
		}

		Player playerController = RewiredControllerManager.GetPlayerController(0);
		return style.GetElement(playerController, action).buttonSprite;
	}

	public static JoystickButtonStyle.ElementButton GetElement(Player player, string actionName)
	{
		return style.GetElement(player, actionName);
	}

	public static string ProcessActionTags(UnityEngine.Localization.LocalizedString key, bool preferTextActions = true)
	{
		return ProcessActionTags(key.GetLocalizedString(), preferTextActions);
	}

	public static string ProcessActionTags(string text, bool preferTextActions)
	{
		Player playerController = RewiredControllerManager.GetPlayerController(0);
		int num = 0;
		do
		{
			num = text.IndexOf("<action=", num);
			if (num < 0)
			{
				break;
			}

			int num2 = text.IndexOf('>', num) - num + 1;
			string text2 = text.Substring(num + 8, num2 - 9);
			Debug.Log("Action: " + text2);
			text = text.Remove(num, num2);
			JoystickButtonStyle.ElementButton element = style.GetElement(playerController, text2);
			if (element != null)
			{
				if (element.textKey.IsEmpty || !preferTextActions)
				{
					string text3 = element.buttonSprite.name;
					Debug.Log("Button Sprite Index for [" + text2 + "]: " + text3);
					text = text.Insert(num, "<sprite name=" + text3 + ">");
				}
				else
				{
					string text4 = element.text;
					Debug.Log("Button Key Name for [" + text2 + "]: " + text4);
					text = text.Insert(num, text4);
				}
			}
			else
			{
				Debug.Log("No element using that action! (or action doesn't exist...)");
			}
		}
		while (num >= 0);
		return text;
	}
}
