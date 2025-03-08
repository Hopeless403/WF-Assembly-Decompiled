#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class ActionRedraw : PlayAction
{
	public readonly Character character;

	public readonly int drawCount;

	public override bool IsRoutine => false;

	public ActionRedraw(Character character, int drawCount = -1)
	{
		this.character = character;
		this.drawCount = drawCount;
	}

	public override void Process()
	{
		if ((bool)character)
		{
			DiscardAll();
			if (drawCount < 0)
			{
				ActionQueue.Add(new ActionDrawHand(character));
			}
			else if (drawCount > 0)
			{
				ActionQueue.Add(new ActionDraw(character, drawCount));
			}
		}
	}

	public void DiscardAll()
	{
		foreach (Entity item in character.handContainer)
		{
			item.display.hover.SetHoverable(value: false);
			ActionQueue.Stack(new ActionMove(item, character.discardContainer));
		}
	}
}
