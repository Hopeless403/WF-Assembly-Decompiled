#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionEndTurn : PlayAction
{
	public readonly Character character;

	public override bool IsRoutine => false;

	public ActionEndTurn(Character character)
	{
		this.character = character;
	}

	public override void Process()
	{
		character.endTurn = true;
	}
}
