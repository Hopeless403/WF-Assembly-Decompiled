#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class RedrawBellStartChargedModifierSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnRedrawBellRevealed += RedrawBellRevealed;
	}

	public void OnDisable()
	{
		Events.OnRedrawBellRevealed -= RedrawBellRevealed;
	}

	public static void RedrawBellRevealed(RedrawBellSystem redrawBellSystem)
	{
		redrawBellSystem.SetCounter(0);
	}
}
