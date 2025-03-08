#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ControllerInputSwitcher : BaseInputSwitcher
{
	public override bool CheckSwitchTo()
	{
		if (!canSwitchTo || Console.active)
		{
			return false;
		}

		if (!(Mathf.Abs(InputSystem.GetAxisDelta("Move Vertical")) > 0f))
		{
			return Mathf.Abs(InputSystem.GetAxisDelta("Move Horizontal")) > 0f;
		}

		return true;
	}

	public override void SwitchTo()
	{
		base.gameObject.SetActive(value: true);
		MonoBehaviourSingleton<Cursor3d>.instance.usingMouse = false;
		MonoBehaviourSingleton<Cursor3d>.instance.usingTouch = false;
		VirtualPointer.Show();
		CustomCursor.UpdateState();
		UINavigationDefaultSystem.SetStartingItem();
		ControllerButtonSystem.SetControllerStyle();
		InputSystem.mainPlayer.controllers.Mouse.enabled = false;
		RewiredControllerManager.instance.AssignNextPlayer(InputSystem.mainPlayer.id);
		InputSystem.AllowDynamicSelectRelease = true;
	}
}
