#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationStateAssignUpgrade : INavigationState
{
	public readonly List<UINavigationItem> disabled = new List<UINavigationItem>();

	public readonly List<Entity> eligibleCards;

	public NavigationStateAssignUpgrade(List<Entity> eligibleCards)
	{
		this.eligibleCards = eligibleCards;
	}

	public void Begin()
	{
		UpgradeDisplay[] array = Object.FindObjectsOfType<UpgradeDisplay>();
		foreach (UpgradeDisplay upgradeDisplay in array)
		{
			Disable(upgradeDisplay.navigationItem);
		}

		DeckDisplay deckDisplay = Object.FindObjectOfType<DeckDisplay>();
		if ((object)deckDisplay != null)
		{
			Disable(deckDisplay.backButtonNavigationItem);
		}

		Dictionary<UINavigationItem, UINavigationItem.SelectionPriority> dictionary = new Dictionary<UINavigationItem, UINavigationItem.SelectionPriority>();
		foreach (UINavigationItem item in eligibleCards.Select((Entity a) => a.uINavigationItem))
		{
			dictionary[item] = item.selectionPriority;
			item.selectionPriority = UINavigationItem.SelectionPriority.Mega;
		}

		UINavigationDefaultSystem.SetStartingItem(useClosest: false);
		foreach (KeyValuePair<UINavigationItem, UINavigationItem.SelectionPriority> item2 in dictionary)
		{
			item2.Key.selectionPriority = item2.Value;
		}
	}

	public void End()
	{
		foreach (UINavigationItem item in disabled.Where((UINavigationItem a) => a != null))
		{
			item.enabled = true;
		}

		disabled.Clear();
	}

	public void Disable(UINavigationItem item)
	{
		if (!(item == null))
		{
			item.enabled = false;
			disabled.Add(item);
		}
	}
}
