#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BattleSaveData
{
	[Serializable]
	public class ContainerGroup
	{
		public Container[] containers;

		public ContainerGroup()
		{
		}

		public ContainerGroup(IEnumerable<CardContainer> containers)
		{
			this.containers = containers.Select((CardContainer a) => new Container(a)).ToArray();
		}
	}

	[Serializable]
	public class Container
	{
		public BattleEntityData[] cards;

		public Container()
		{
		}

		public Container(CardContainer container)
		{
			if (container is CardSlotLane cardSlotLane)
			{
				cards = cardSlotLane.slots.Select(delegate(CardSlot a)
				{
					Entity top = a.GetTop();
					return ((object)top == null) ? null : new BattleEntityData(top);
				}).ToArray();
			}
			else
			{
				cards = container.Select((Entity a) => new BattleEntityData(a)).ToArray();
			}
		}
	}

	[Serializable]
	public class Status
	{
		public string name;

		public int count;

		public ulong targetId;

		public bool hasApplier;

		public ulong applierId;

		public Status()
		{
		}

		public Status(StatusEffectData data)
		{
			name = data.name;
			count = data.count - data.temporary;
			targetId = data.target.data.id;
			hasApplier = data.applier;
			if (hasApplier)
			{
				applierId = data.applier.data.id;
			}
		}
	}

	public int campaignNodeId;

	public int turnCount;

	public int redrawBellCount;

	public ContainerGroup playerRows;

	public ContainerGroup enemyRows;

	public Container playerHand;

	public Container playerDraw;

	public Container playerDiscard;

	public Container playerReserve;

	public Container enemyReserve;

	public Status[] statuses;

	public BattleEntityData[] destroyed;

	public BattleWaveData enemyWaves;

	public BattleMusicSaveData battleMusicState;

	public Dictionary<string, object> storeStatusData = new Dictionary<string, object>();

	public int gold;

	public bool HasMissingCardData()
	{
		List<Container> list = new List<Container>();
		list.AddRange(playerRows.containers);
		list.AddRange(enemyRows.containers);
		list.Add(playerReserve);
		list.Add(enemyReserve);
		list.Add(playerDraw);
		list.Add(playerDiscard);
		list.Add(playerHand);
		foreach (Container item in list)
		{
			BattleEntityData[] cards = item.cards;
			foreach (BattleEntityData battleEntityData in cards)
			{
				if (battleEntityData != null && (battleEntityData.cardSaveData == null || MissingCardSystem.IsMissing(battleEntityData.cardSaveData.name)))
				{
					Debug.LogError("BattleSaveData has missing CardData");
					return true;
				}
			}
		}

		return false;
	}
}
