#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;

public class NavigationStateBattle : INavigationState
{
	public readonly List<UINavigationItem> disabled = new List<UINavigationItem>();

	public void Begin()
	{
		foreach (CardSlotLane allRow in References.Battle.allRows)
		{
			Disable(allRow.nav);
			foreach (CardSlot slot in allRow.slots)
			{
				Disable(slot.nav);
			}
		}

		if (References.Battle.playerCardController is CardControllerBattle cardControllerBattle)
		{
			Disable(cardControllerBattle.useOnHandAnchor);
		}
	}

	public void End()
	{
		foreach (UINavigationItem item in disabled.Where((UINavigationItem a) => a))
		{
			item.enabled = true;
		}

		disabled.Clear();
	}

	public void Disable(UINavigationItem item)
	{
		if ((bool)item)
		{
			item.enabled = false;
			disabled.Add(item);
		}
	}
}
