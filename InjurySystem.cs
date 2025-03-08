#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InjurySystem : GameSystem
{
	[Serializable]
	public class SaveState
	{
		public int campaignNodeId;

		public List<ulong> injuredThisBattleIds;

		public SaveState()
		{
		}

		public SaveState(int campaignNodeId, IEnumerable<CardData> injuredThisBattle)
		{
			this.campaignNodeId = campaignNodeId;
			injuredThisBattleIds = injuredThisBattle.Select((CardData a) => a.id).ToList();
		}

		public List<CardData> Load()
		{
			List<CardData> list = new List<CardData>();
			foreach (CardData item in References.PlayerData.inventory.deck)
			{
				int num = injuredThisBattleIds.IndexOf(item.id);
				if (num >= 0)
				{
					injuredThisBattleIds.RemoveAt(num);
					list.Add(item);
				}
			}

			return list;
		}
	}

	[SerializeField]
	public CardType[] typesThatCanBeInjured;

	[SerializeField]
	public StatusEffectData injuryEffect;

	[SerializeField]
	public List<CardData> injuredThisBattle;

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
		Events.OnEntityKilled += EntityKilled;
		Events.OnBattleSaved += BattleSaved;
		Events.OnBattleLoaded += BattleLoaded;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
		Events.OnEntityKilled -= EntityKilled;
		Events.OnBattleSaved -= BattleSaved;
		Events.OnBattleLoaded -= BattleLoaded;
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		CardData originalCardData;
		if (IsPlayerCard(entity.data))
		{
			if (!AnyAliveClonesOfThisCard(entity.data))
			{
				Injure(entity.data);
			}
		}
		else if (IsCloneOfPlayerCard(entity.data, out originalCardData) && !IsCardAlive(originalCardData) && !AnyAliveClonesOfThisCard(originalCardData))
		{
			Injure(originalCardData);
		}
	}

	public void BattleStart()
	{
		if (injuredThisBattle == null)
		{
			injuredThisBattle = new List<CardData>();
		}

		injuredThisBattle.Clear();
	}

	public void BattleSaved()
	{
		if (Campaign.Data.GameMode.doSave)
		{
			CardData[] injuriesThisBattle = GetInjuriesThisBattle();
			if (injuriesThisBattle != null && injuriesThisBattle.Length > 0)
			{
				SaveState value = new SaveState(Campaign.FindCharacterNode(References.Player).id, injuriesThisBattle);
				SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "battleInjuredThisBattle", value);
			}
		}
	}

	public void BattleLoaded()
	{
		SaveState saveState = SaveSystem.LoadCampaignData<SaveState>(Campaign.Data.GameMode, "battleInjuredThisBattle");
		if (saveState == null || Campaign.FindCharacterNode(References.Player).id != saveState.campaignNodeId || saveState.injuredThisBattleIds == null)
		{
			return;
		}

		injuredThisBattle = saveState.Load();
		foreach (CardData item in injuredThisBattle)
		{
			CardData cardData = item;
			if (cardData.injuries == null)
			{
				cardData.injuries = new List<CardData.StatusEffectStacks>();
			}

			if (item.injuries.Count <= 0)
			{
				item.injuries.Add(new CardData.StatusEffectStacks(injuryEffect, 1));
			}
		}
	}

	public static bool IsPlayerCard(CardData cardData)
	{
		return References.PlayerData.inventory.deck.Any((CardData a) => a.id == cardData.id);
	}

	public static bool IsCloneOfPlayerCard(CardData cardData, out CardData originalCardData)
	{
		if (cardData.TryGetCustomData("splitOriginalId", out var value, 0uL))
		{
			foreach (CardData item in References.PlayerData.inventory.deck)
			{
				if (item.id == value)
				{
					originalCardData = item;
					return true;
				}
			}
		}

		originalCardData = null;
		return false;
	}

	public void Injure(CardData cardData)
	{
		if (CanInjure(cardData))
		{
			if (cardData.injuries == null)
			{
				cardData.injuries = new List<CardData.StatusEffectStacks>();
			}

			if (cardData.injuries.Count <= 0)
			{
				cardData.injuries.Add(new CardData.StatusEffectStacks(injuryEffect, 1));
			}

			if (injuredThisBattle == null)
			{
				injuredThisBattle = new List<CardData>();
			}

			injuredThisBattle.Add(cardData);
			Events.InvokeCardInjured(cardData);
		}
	}

	public bool CanInjure(CardData cardData)
	{
		if ((bool)cardData)
		{
			return CanInjure(cardData.cardType);
		}

		return false;
	}

	public bool CanInjure(CardType cardType)
	{
		return typesThatCanBeInjured.Contains(cardType);
	}

	public static CardData[] GetInjuriesThisBattle()
	{
		InjurySystem injurySystem = UnityEngine.Object.FindObjectOfType<InjurySystem>();
		if ((bool)injurySystem && injurySystem.injuredThisBattle != null)
		{
			return injurySystem.injuredThisBattle.Where(IsPlayerCard).ToArray();
		}

		return new CardData[0];
	}

	public static bool IsCardAlive(CardData cardData)
	{
		foreach (Entity card in References.Battle.cards)
		{
			if (card.data.id == cardData.id && card.IsAliveAndExists())
			{
				return true;
			}
		}

		return false;
	}

	public static bool AnyAliveClonesOfThisCard(CardData originalCardData)
	{
		bool result = false;
		foreach (Entity card in References.Battle.cards)
		{
			if (card.data.customData != null && card.data.customData.TryGetValue("splitOriginalId", out var value) && value is ulong num && num == originalCardData.id && card.IsAliveAndExists())
			{
				result = true;
				break;
			}
		}

		return result;
	}
}
