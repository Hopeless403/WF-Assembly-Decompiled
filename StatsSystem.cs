#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

public class StatsSystem : GameSystem
{
	public static StatsSystem instance;

	[SerializeField]
	public CampaignStats stats;

	public int goldThisBattle;

	public int sacrificedThisBattle;

	public bool kingMokoExists;

	public bool campaignEnded;

	public CampaignStats Stats => stats;

	public static CampaignStats Get()
	{
		return instance.Stats;
	}

	public static void Set(CampaignStats stats)
	{
		if (stats == null)
		{
			stats = new CampaignStats();
		}

		instance.stats = stats;
	}

	public void OnEnable()
	{
		instance = this;
		Events.OnCampaignEnd += CampaignEnd;
		Events.OnCampaignSaved += CampaignSaved;
		Events.OnEntityHit += EntityHit;
		Events.OnEntityPostHit += PostEntityHit;
		Events.OnStatusEffectApplied += StatusApplied;
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntityOffered += EntityOffered;
		Events.OnEntityChosen += EntityChosen;
		Events.OnEntityFlee += EntityFlee;
		Events.OnDiscard += EntityDiscarded;
		Events.OnEntitySummoned += EntitySummoned;
		Events.OnEntityTriggered += EntityTriggered;
		Events.OnCardInjured += CardInjured;
		Events.OnBattleStart += BattleStart;
		Events.OnBattleEnd += BattleEnd;
		Events.OnDropGold += DropGold;
		Events.OnSpendGold += SpendGold;
		Events.OnShopItemPurchase += ShopItemPurchase;
		Events.OnShopItemHaggled += ShopItemHaggled;
		Events.OnKillCombo += KillCombo;
		Events.OnRedrawBellHit += RedrawBellHit;
		Events.OnWaveDeployerEarlyDeploy += WaveDeployerEarlyDeploy;
		Events.OnBattleTurnStart += BattleTurnStart;
		Events.OnRename += Rename;
		Events.OnMuncherFeed += MuncherFeed;
		Events.OnUpgradeGained += UpgradeGained;
		Events.OnUpgradeAssign += UpgradeAssigned;
	}

	public void OnDisable()
	{
		Events.OnCampaignEnd -= CampaignEnd;
		Events.OnCampaignSaved -= CampaignSaved;
		Events.OnEntityHit -= EntityHit;
		Events.OnEntityPostHit -= PostEntityHit;
		Events.OnStatusEffectApplied -= StatusApplied;
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntityOffered -= EntityOffered;
		Events.OnEntityChosen -= EntityChosen;
		Events.OnEntityFlee -= EntityFlee;
		Events.OnDiscard -= EntityDiscarded;
		Events.OnEntitySummoned -= EntitySummoned;
		Events.OnEntityTriggered -= EntityTriggered;
		Events.OnCardInjured -= CardInjured;
		Events.OnBattleStart -= BattleStart;
		Events.OnBattleEnd -= BattleEnd;
		Events.OnDropGold -= DropGold;
		Events.OnSpendGold -= SpendGold;
		Events.OnShopItemPurchase -= ShopItemPurchase;
		Events.OnShopItemHaggled -= ShopItemHaggled;
		Events.OnKillCombo -= KillCombo;
		Events.OnRedrawBellHit -= RedrawBellHit;
		Events.OnWaveDeployerEarlyDeploy -= WaveDeployerEarlyDeploy;
		Events.OnBattleTurnStart -= BattleTurnStart;
		Events.OnRename -= Rename;
		Events.OnMuncherFeed -= MuncherFeed;
		Events.OnUpgradeGained -= UpgradeGained;
		Events.OnUpgradeAssign -= UpgradeAssigned;
	}

	public void Update()
	{
		if (!campaignEnded)
		{
			stats.time += Time.deltaTime;
			if (stats.time >= 3600f)
			{
				stats.hours++;
				stats.time -= 3600f;
			}
		}
	}

	public void CampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		campaignEnded = true;
	}

	public void CampaignSaved()
	{
		if (Campaign.Data.GameMode.doSave)
		{
			SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "stats", stats);
		}
	}

	public void EntityHit(Hit hit)
	{
		bool flag = hit.target.owner == References.Player;
		if (!hit.Offensive)
		{
			return;
		}

		if (hit.owner == References.Player)
		{
			if (hit.damage > 0)
			{
				Stats.Add("damageDealt", hit.damageType, hit.damage);
				Stats.Max("highestDamageDealt", hit.damageType, hit.damage);
				if (flag)
				{
					Stats.Add("friendlyDamageDealt", hit.target.data.name, hit.damage);
				}
			}

			Stats.Add("cardsHit", hit.target.data.name, 1);
		}

		if (flag)
		{
			Stats.Add("hitsTakenByCardType", hit.target.data.cardType.name, 1);
			if (hit.damage > 0)
			{
				Stats.Add("damageTaken", hit.damageType, hit.damage);
			}

			if (hit.damageBlocked > 0)
			{
				Stats.Add("damageBlocked", hit.damageType, hit.damageBlocked);
			}
		}
	}

	public void PostEntityHit(Hit hit)
	{
		if (!hit.dodged && hit.damageDealt < 0 && hit.target.owner == References.Player && hit.owner == References.Player)
		{
			Stats.Add("amountHealedTo", hit.target.data.name, -hit.damageDealt);
			if ((bool)hit.attacker && (bool)hit.attacker.data)
			{
				Stats.Add("amountHealedFrom", hit.attacker.data.name, -hit.damageDealt);
			}

			Stats.Max("highestHealth", hit.target.hp.current);
		}
	}

	public void StatusApplied(StatusEffectApply apply)
	{
		if (!apply.applier)
		{
			return;
		}

		string text = (apply.effectData ? apply.effectData.type : null);
		if (!text.IsNullOrWhitespace() && apply.applier.owner == References.Player)
		{
			Stats.Add("statusesApplied", text, apply.count);
			StatusEffectData statusEffectData = apply.target.FindStatus(apply.effectData);
			if ((bool)statusEffectData)
			{
				Stats.Max("highestStatusEffect", text, statusEffectData.count);
			}

			string type = apply.effectData.type;
			if (type == "max health up" || type == "max health up only")
			{
				Stats.Max("highestHealth", apply.target.hp.current);
			}
		}
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if (entity.owner == References.Player)
		{
			Stats.Add("friendliesDied", entity.data.name, 1);
			if (DeathSystem.KilledByOwnTeam(entity))
			{
				Stats.Add("friendliesSacrificed", entity.data.name, 1);
				sacrificedThisBattle++;
				Stats.Max("highestSacrificesInBattle", sacrificedThisBattle);
			}

			return;
		}

		Stats.Add("enemiesKilled", entity.data.name, 1);
		Hit lastHit = entity.lastHit;
		if (lastHit == null)
		{
			return;
		}

		Entity attacker = lastHit.attacker;
		if ((object)attacker == null)
		{
			return;
		}

		Character owner = attacker.owner;
		if ((object)owner == null)
		{
			return;
		}

		CardData data = attacker.data;
		if ((object)data == null)
		{
			return;
		}

		if (entity.data.name == "FinalBoss")
		{
			Stats.Add("finalBossKills", data.name, 1);
		}

		if (owner == References.Player)
		{
			Stats.Add("kills", data.name, 1);
			Stats.Add("enemiesKilledByCardType", data.cardType.name, 1);
			Stats.Add("enemiesKilledDamageType", entity.lastHit.damageType, 1);
			if (entity.lastHit.trigger != null)
			{
				Stats.Add("enemiesKilledTriggerType", entity.lastHit.trigger.type, 1);
			}

			if (entity.data.cardType.name == "Boss")
			{
				Stats.Add("bossesKilled", entity.data.name, 1);
			}

			if (entity.data.cardType.miniboss)
			{
				Stats.Add("minibossesKilledByCardType", data.cardType.name, 1);
			}

			if ((bool)entity.statusEffects.Find((StatusEffectData a) => a.type == "demonize"))
			{
				Stats.Add("demonizedEnemiesKilled", data.name, 1);
			}
		}
	}

	public void EntityOffered(Entity entity)
	{
		Stats.Add("cardsOffered", entity.data.name, 1);
	}

	public void EntityChosen(Entity entity)
	{
		Stats.Add("cardsChosen", entity.data.name, 1);
		Stats.Add("cardTypesChosen", entity.data.cardType.name, 1);
	}

	public void EntityFlee(Entity entity)
	{
		if (entity.owner != References.Player)
		{
			Stats.Add("enemiesEscaped", entity.data.name, 1);
		}
	}

	public void EntityDiscarded(Entity entity)
	{
		if (entity.owner == References.Player)
		{
			Stats.Add("friendliesRecalled", entity.data.name, 1);
		}
	}

	public void EntitySummoned(Entity entity, Entity summonedBy)
	{
		if (entity.data.cardType.unit)
		{
			Stats.Add("cardsSummoned", entity.data.name, 1);
		}
	}

	public void EntityTriggered(ref Trigger trigger)
	{
		if (trigger.entity.owner == References.Player && !trigger.type.IsNullOrWhitespace())
		{
			Stats.Add("totalTriggers", trigger.type, 1);
		}

		Stats.Add("cardsTriggered", trigger.entity.data.name, 1);
	}

	public void CardInjured(CardData cardData)
	{
		Stats.Add("friendliesInjured", cardData.name, 1);
	}

	public void BattleStart()
	{
		goldThisBattle = 0;
		sacrificedThisBattle = 0;
		List<Entity> cards = Battle.GetCards(References.Battle.enemy);
		kingMokoExists = cards.FirstOrDefault((Entity a) => a.data.cardType.miniboss && a.data.name == "MonkeyKing");
	}

	public void BattleEnd()
	{
		string battleName = GetBattleName();
		if (References.Battle.winner == References.Player)
		{
			Stats.Add("battlesWon", battleName, 1);
			if (Battle.GetCardsOnBoard(References.Battle.player).Count == 1)
			{
				Stats.Add("winBattleWithOnlyLeaderRemaining", 1);
			}

			if (kingMokoExists && Stats.Get("cardsTriggered", "MonkeyKing", 0) > 0)
			{
				Stats.Add("winBattleSurviveKingMokoAttack", 1);
			}

			if (References.Player.handContainer.Count == 0 && References.Player.discardContainer.Count == 0 && References.Player.drawContainer.Count == 0)
			{
				Stats.Add("winBattleWithNoCardsInDeck", 1);
			}
		}
		else
		{
			Stats.Add("battlesLost", battleName, 1);
		}

		Stats.Max("highestGoldFromBattle", battleName, goldThisBattle);
	}

	public void DropGold(int amount, string source, Character owner, Vector3 position)
	{
		Stats.Add("goldGained", source, amount);
		goldThisBattle += amount;
	}

	public void SpendGold(int amount)
	{
		Stats.Add("goldSpent", amount);
	}

	public void ShopItemPurchase(ShopItem item)
	{
		if (item.priceFactor < 1f)
		{
			Stats.Add("discountsBought", 1);
		}

		string key = null;
		if ((object)item.GetComponent<CharmMachine>() != null || (object)item.GetComponent<CardCharm>() != null)
		{
			key = "charm";
		}
		else if ((object)item.GetComponent<CrownHolderShop>() != null)
		{
			key = "crown";
		}

		else if ((object)item.GetComponent<Card>() != null)
		{
			key = "card";
		}

		int id = Campaign.FindCharacterNode(References.Player).id;
		if (Stats.Get("preShopId", -1) != id)
		{
			Stats.Set("preShopId", id);
			Stats.Delete("boughtInSingleShop");
			Stats.Set("boughtInSingleShop", key, 1);
		}
		else
		{
			Stats.Add("boughtInSingleShop", key, 1);
		}

		Stats.Add("shopItemsBought", key, 1);
	}

	public void ShopItemHaggled(ShopItem item)
	{
		Stats.Add("shopItemsHaggled", 1);
	}

	public void KillCombo(int combo)
	{
		Stats.Max("highestKillCombo", combo);
	}

	public void RedrawBellHit(RedrawBellSystem redrawBellSystem)
	{
		Stats.Add("redrawBellHits", 1);
	}

	public void WaveDeployerEarlyDeploy()
	{
		Stats.Add("enemyWaveBellHits", 1);
	}

	public void BattleTurnStart(int turnCount)
	{
		Stats.Add("turnsTaken", 1);
	}

	public void Rename(Entity entity, string newName)
	{
		Stats.Add("renames", newName, 1);
		Stats.Add("bestRename", newName, PettyRandom.Range(1, 1000));
	}

	public void MuncherFeed(Entity entity)
	{
		Stats.Add("cardsMunched", entity.data.name, 1);
		if (entity.data.traits == null)
		{
			return;
		}

		foreach (CardData.TraitStacks trait in entity.data.traits)
		{
			if (trait.data.name == "Consume")
			{
				Stats.Add("consumeCardsMunched", 1);
			}
		}
	}

	public void UpgradeGained(CardUpgradeData upgradeData)
	{
		switch (upgradeData.type)
		{
			case CardUpgradeData.Type.Charm:
				Stats.Add("charmsGained", upgradeData.name, 1);
				break;
			case CardUpgradeData.Type.Crown:
				Stats.Add("crownsGained", upgradeData.name, 1);
				break;
		}
	}

	public void UpgradeAssigned(Entity entity, CardUpgradeData upgradeData)
	{
		if (upgradeData.type == CardUpgradeData.Type.Charm)
		{
			Stats.Add("charmsAssigned", upgradeData.name, 1);
			Stats.Add("charmsAssignedTo", entity.data.name, 1);
		}
	}

	public static string GetBattleName()
	{
		if (Campaign.FindCharacterNode(References.Player).data.TryGetValue("battle", out var value) && value is string result)
		{
			return result;
		}

		return null;
	}
}
