#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionDrawHand : PlayAction
{
	public readonly Character character;

	public readonly float pauseBetween;

	public override bool IsRoutine => false;

	public ActionDrawHand(Character character, float pauseBetween = 0.1f)
	{
		this.character = character;
		this.pauseBetween = pauseBetween;
	}

	public override void Process()
	{
		int count = character.handContainer.max - character.handContainer.Count;
		ActionQueue.Stack(new ActionDraw(character, count, pauseBetween));
	}
}
