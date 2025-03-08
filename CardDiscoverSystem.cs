#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDiscoverSystem : GameSystem
{
	[Serializable]
	public struct PhaseChange
	{
		public string phasedCardName;

		public string originalCardName;
	}

	public static CardDiscoverSystem instance;

	[SerializeField]
	public PhaseChange[] phaseChanges;

	public List<string> discoveredCards;

	public List<string> discoveredCharms;

	public void OnEnable()
	{
		instance = this;
		discoveredCards = SaveSystem.LoadProgressData<List<string>>("cardsDiscovered");
		if (discoveredCards == null)
		{
			discoveredCards = new List<string>();
		}

		discoveredCharms = SaveSystem.LoadProgressData<List<string>>("charmsDiscovered");
		if (discoveredCharms == null)
		{
			discoveredCharms = new List<string>();
		}

		Events.OnPreCampaignPopulate += CampaignStart;
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntitySummoned += EntitySummoned;
		Events.OnEntityShowUnlocked += EntityShowUnlocked;
		Events.OnUpgradeGained += UpgradeGained;
		Events.OnEntityEnterBackpack += EntityEnterBackpack;
	}

	public void OnDisable()
	{
		Events.OnPreCampaignPopulate -= CampaignStart;
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntitySummoned -= EntitySummoned;
		Events.OnEntityShowUnlocked -= EntityShowUnlocked;
		Events.OnUpgradeGained -= UpgradeGained;
		Events.OnEntityEnterBackpack -= EntityEnterBackpack;
	}

	public void CampaignStart()
	{
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			DiscoverCard(item);
			foreach (CardUpgradeData upgrade in item.upgrades)
			{
				DiscoverCharm(upgrade.name);
			}
		}
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if ((bool)References.Battle && entity.owner.team == References.Battle.enemy.team)
		{
			DiscoverCard(entity.data);
			DiscoverCard(phaseChanges.FirstOrDefault((PhaseChange a) => a.phasedCardName == entity.data.name).originalCardName);
		}
	}

	public void EntitySummoned(Entity entity, Entity summonedBy)
	{
		DiscoverCard(entity.data);
	}

	public void EntityShowUnlocked(Entity entity)
	{
		DiscoverCard(entity.data);
	}

	public void UpgradeGained(CardUpgradeData upgradeData)
	{
		if (upgradeData.type == CardUpgradeData.Type.Charm)
		{
			DiscoverCharm(upgradeData.name);
		}
	}

	public void EntityEnterBackpack(Entity entity)
	{
		DiscoverCard(entity.data);
		foreach (CardUpgradeData upgrade in entity.data.upgrades)
		{
			if (upgrade.type == CardUpgradeData.Type.Charm)
			{
				DiscoverCharm(upgrade.name);
			}
		}
	}

	public void DiscoverCard(CardData cardData)
	{
		if (cardData.cardType.discoverInJournal)
		{
			DiscoverCard(cardData.name);
		}
	}

	public void DiscoverCard(string cardDataName)
	{
		if (!discoveredCards.Contains(cardDataName))
		{
			discoveredCards.Add(cardDataName);
			SaveSystem.SaveProgressData("cardsDiscovered", discoveredCards);
		}
	}

	public void DiscoverCharm(string charmName)
	{
		if (!discoveredCharms.Contains(charmName))
		{
			discoveredCharms.Add(charmName);
			SaveSystem.SaveProgressData("charmsDiscovered", discoveredCharms);
		}
	}

	public static void CheckDiscoverCharm(string charmName)
	{
		instance.DiscoverCharm(charmName);
	}
}
