#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;

public static class CharacterExt
{
	public static void OrderNextCards(this Character character, string[] nextCardNames)
	{
		CardContainer drawContainer = character.drawContainer;
		List<Entity> list = new List<Entity>();
		foreach (string cardName in nextCardNames)
		{
			Entity entity = drawContainer.FirstOrDefault((Entity a) => a.name == cardName);
			if (entity != null)
			{
				list.Insert(0, entity);
				drawContainer.Remove(entity);
			}
		}

		foreach (Entity item in list)
		{
			drawContainer.Add(item);
		}

		drawContainer.TweenChildPositions();
	}
}
