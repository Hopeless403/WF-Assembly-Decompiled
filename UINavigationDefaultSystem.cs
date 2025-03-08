#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UINavigationDefaultSystem
{
	public static void SetStartingItem(bool useClosest = true, bool ignoreHistory = false)
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			return;
		}

		Debug.Log("UINavigationDefaultSystem → Finding Default Item");
		if (!MonoBehaviourSingleton<UINavigationSystem>.instance.AvailableNavigationItems.Any())
		{
			Debug.Log("UINavigationDefaultSystem → no available navigation items");
			return;
		}

		List<UINavigationItem> list = MonoBehaviourSingleton<UINavigationSystem>.instance.AvailableNavigationItems.Where((UINavigationItem item) => item != null && item.CheckLayer()).ToList();
		if (list.Count <= 0)
		{
			Debug.Log("UINavigationDefaultSystem → no possible items on current navigation layer");
			return;
		}

		if (list.Count == 1)
		{
			MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(list[0]);
		}
		else
		{
			list = (useClosest ? ((!ignoreHistory) ? (from a in list
				orderby Vector3.Distance(MonoBehaviourSingleton<Cursor3d>.instance.transform.position, a.Position) - (float)a.selectionPriority, (int)((!UINavigationHistory.items.Contains(a)) ? a.selectionPriority : (UINavigationHistory.items.IndexOf(a) + a.selectionPriority)) descending
				select a).ToList() : (from a in list
				orderby Vector3.Distance(MonoBehaviourSingleton<Cursor3d>.instance.transform.position, a.Position) - (float)a.selectionPriority, (int)a.selectionPriority descending
				select a).ToList()) : ((!ignoreHistory) ? (from a in list
				orderby (int)((!UINavigationHistory.items.Contains(a)) ? a.selectionPriority : (UINavigationHistory.items.IndexOf(a) + a.selectionPriority)) descending, Vector3.Distance(MonoBehaviourSingleton<Cursor3d>.instance.transform.position, a.Position)
				select a).ToList() : (from a in list
				orderby (int)a.selectionPriority descending, Vector3.Distance(MonoBehaviourSingleton<Cursor3d>.instance.transform.position, a.Position)
				select a).ToList()));
			MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(list.First());
		}

		Debug.Log($"UINavigationDefaultSystem → Default Item Set: {MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem}");
	}

	public static void SetDefaultTarget(Entity entity)
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse || !MonoBehaviourSingleton<UINavigationSystem>.instance.AvailableNavigationItems.Any())
		{
			return;
		}

		CardData.PlayPosition playPosition = GetPlayPosition(entity);
		List<UINavigationItem> list = (from item in GetDefaultTargets(entity, playPosition).Intersect(MonoBehaviourSingleton<UINavigationSystem>.instance.AvailableNavigationItems)
			where item != null && item.CheckLayer()
			select item).ToList();
		if (list.Count <= 0)
		{
			SetStartingItem();
			return;
		}

		if (list.Count == 1)
		{
			MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(list[0]);
			return;
		}

		list = list.OrderBy((UINavigationItem a) => Vector3.Distance(MonoBehaviourSingleton<Cursor3d>.instance.transform.position, a.Position) - (float)a.selectionPriority - (float)UINavigationHistory.GetItemIndex(a)).ToList();
		MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(list.First());
	}

	public static CardData.PlayPosition GetPlayPosition(Entity entity)
	{
		if (entity.data.playType == Card.PlayType.Place)
		{
			return CardData.PlayPosition.FriendlySlot;
		}

		if (entity.data.defaultPlayPosition != 0)
		{
			return entity.data.defaultPlayPosition;
		}

		if (!entity.NeedsTarget)
		{
			return CardData.PlayPosition.None;
		}

		if (entity.data.playOnSlot)
		{
			if (!entity.data.canPlayOnFriendly)
			{
				if (!entity.data.canPlayOnEnemy)
				{
					return CardData.PlayPosition.None;
				}

				return CardData.PlayPosition.EnemySlot;
			}

			return CardData.PlayPosition.FriendlySlot;
		}

		if (entity.IsOffensive())
		{
			if (!entity.targetMode.TargetRow)
			{
				return CardData.PlayPosition.Enemy;
			}

			return CardData.PlayPosition.EnemyRow;
		}

		if (!entity.targetMode.TargetRow)
		{
			return CardData.PlayPosition.Friendly;
		}

		return CardData.PlayPosition.FriendlyRow;
	}

	public static IEnumerable<UINavigationItem> GetDefaultTargets(Entity entity, CardData.PlayPosition playPosition)
	{
		switch (playPosition)
		{
			case CardData.PlayPosition.Friendly:
				return (from a in Battle.GetCardsOnBoard(entity.owner)
					select a.uINavigationItem).ToList();
			case CardData.PlayPosition.Enemy:
				return (from a in Battle.GetCardsOnBoard(Battle.GetOpponent(entity.owner))
					select a.uINavigationItem).ToList();
			case CardData.PlayPosition.FriendlyRow:
				return (from a in References.Battle.GetRows(entity.owner)
					select a.nav).ToList();
			case CardData.PlayPosition.EnemyRow:
				return (from a in References.Battle.GetRows(Battle.GetOpponent(entity.owner))
					select a.nav).ToList();
			case CardData.PlayPosition.FriendlySlot:
				return Battle.IsOnBoard(entity) ? entity.actualContainers.Select((CardContainer a) => a.nav).ToList() : (from a in References.Battle.GetSlots(entity.owner)
					select a.nav).ToList();
			case CardData.PlayPosition.EnemySlot:
				return (from a in References.Battle.GetSlots(Battle.GetOpponent(entity.owner))
					select a.nav).ToList();
			default:
				return new List<UINavigationItem>();
		}
	}
}
