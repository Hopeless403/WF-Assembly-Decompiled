#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationStateCard : INavigationState
{
	public readonly List<UINavigationItem> disabled = new List<UINavigationItem>();

	public readonly Entity entity;

	public NavigationStateCard(Entity entity)
	{
		this.entity = entity;
	}

	public void Begin()
	{
		List<CardContainer> list = new List<CardContainer>();
		foreach (CardContainer item in from c in Object.FindObjectsOfType<CardContainer>()
			where (bool)c.nav && c.nav.enabled

			select c)
		{
			if (entity.CanPlayOn(item))
			{
				list.Add(item);
			}
			else
			{
				Disable(item.nav);
			}
		}
		foreach (Entity card in References.Battle.cards)
		{
			if ((bool)card.uINavigationItem && card.uINavigationItem.enabled && (entity.data.playType != Card.PlayType.Play || !entity.CanPlayOn(card)))
			{
				Disable(card.uINavigationItem);
			}
		}

		Disable(RedrawBellSystem.nav);
		Disable(WaveDeploySystem.nav);
		if (References.Battle.playerCardController is CardControllerBattle cardControllerBattle)
		{
			UINavigationItem useOnHandAnchor = cardControllerBattle.useOnHandAnchor;
			if ((object)useOnHandAnchor != null && entity.NeedsTarget)
			{
				Disable(useOnHandAnchor);
			}
		}

		Dictionary<UINavigationItem, UINavigationItem.SelectionPriority> dictionary = new Dictionary<UINavigationItem, UINavigationItem.SelectionPriority>();
		foreach (CardContainer item2 in list)
		{
			if (item2 is CardSlot && item2.Empty)
			{
				UINavigationItem nav = item2.nav;
				if ((object)nav != null && nav.enabled)
				{
					dictionary[nav] = nav.selectionPriority;
					nav.selectionPriority = UINavigationItem.SelectionPriority.Mega;
				}
			}
		}

		UINavigationDefaultSystem.SetDefaultTarget(entity);
		foreach (KeyValuePair<UINavigationItem, UINavigationItem.SelectionPriority> item3 in dictionary)
		{
			item3.Key.selectionPriority = item3.Value;
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
