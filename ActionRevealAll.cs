#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class ActionRevealAll : PlayAction
{
	public readonly CardContainer[] containers;

	public override bool IsRoutine => false;

	public ActionRevealAll(params CardContainer[] containers)
	{
		this.containers = containers;
	}

	public override void Process()
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		float num = 0.167f;
		int num2 = 1;
		CardContainer[] array = containers;
		foreach (CardContainer cardContainer in array)
		{
			if (cardContainer == null)
			{
				continue;
			}

			foreach (Entity item in cardContainer)
			{
				if (!(item == null))
				{
					if (item.flipper.flipped)
					{
						ActionQueue.Insert(num2++, new ActionReveal(item, num));
					}

					if (!item.enabled)
					{
						hashSet.Add(item);
					}
				}
			}
		}

		foreach (Entity item2 in hashSet)
		{
			ActionQueue.Insert(num2++, new ActionRunEnableEvent(item2));
		}
	}
}
