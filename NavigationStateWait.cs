#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class NavigationStateWait : INavigationState
{
	public readonly bool disableInput;

	public NavigationStateWait(bool disableInput = false)
	{
		this.disableInput = disableInput;
	}

	public void Begin()
	{
		VirtualPointer.Hide();
		if (disableInput)
		{
			InputSystem.Disable();
		}
	}

	public void End()
	{
		VirtualPointer.Show();
		if (disableInput)
		{
			InputSystem.Enable();
		}
	}
}
