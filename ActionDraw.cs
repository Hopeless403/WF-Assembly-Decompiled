#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionDraw : PlayAction
{
	public readonly Character character;

	public int count;

	public float pauseBetween;

	public ActionDraw(Character character, int count = 1, float pauseBetween = 0.1f)
	{
		this.character = character;
		this.count = count;
		this.pauseBetween = pauseBetween;
	}

	public override IEnumerator Run()
	{
		if (count <= 0 || !character.drawContainer || !character.handContainer || !character.discardContainer)
		{
			yield break;
		}

		Events.InvokeCardDraw(count);
		while (count > 0)
		{
			yield return Sequences.Wait(pauseBetween);
			Entity top = character.drawContainer.GetTop();
			if (!top)
			{
				Events.InvokeCardDrawEnd();
				yield return Sequences.ShuffleTo(character.discardContainer, character.drawContainer);
				top = character.drawContainer.GetTop();
				Events.InvokeCardDraw(count);
			}

			if ((bool)top)
			{
				yield return Sequences.CardMove(top, new CardContainer[1] { character.handContainer });
				character.handContainer.TweenChildPositions();
			}

			count--;
		}

		Events.InvokeCardDrawEnd();
		ActionQueue.Stack(new ActionRevealAll(character.handContainer));
	}
}
