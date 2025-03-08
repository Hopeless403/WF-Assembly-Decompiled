#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class MouseInputSwitcher : BaseInputSwitcher
{
	public override bool CheckSwitchTo()
	{
		if (canSwitchTo)
		{
			return Input.GetMouseButtonDown(0);
		}

		return false;
	}

	public override void SwitchTo()
	{
		base.gameObject.SetActive(value: true);
		MonoBehaviourSingleton<Cursor3d>.instance.usingMouse = true;
		MonoBehaviourSingleton<Cursor3d>.instance.usingTouch = false;
		VirtualPointer.Hide();
		CustomCursor.UpdateState();
		MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(null);
		ControllerButtonSystem.SetMouseStyle();
		InputSystem.mainPlayer.controllers.Mouse.enabled = true;
		InputSystem.AllowDynamicSelectRelease = true;
	}
}
