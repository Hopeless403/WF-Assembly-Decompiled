#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

public class EventRoutineCharmShop : EventRoutine
{
	[Serializable]
	public class Data
	{
		public List<UpgradedCard> cards;

		public List<CharmShopItemData> items;
	}

	[Serializable]
	public class UpgradedCard
	{
		public string cardDataName;

		public string[] upgradeNames;

		public int price;

		public float priceFactor;

		public bool purchased;
	}

	[Serializable]
	public class CharmShopItemData
	{
		public string upgradeDataName;

		public int price;

		public float priceFactor;

		public bool purchased;

		public CharmShopItemData()
		{
		}

		public CharmShopItemData(string upgradeDataName, int price, float priceFactor = 1f)
		{
			this.upgradeDataName = upgradeDataName;
			this.price = Mathf.RoundToInt((float)price * priceFactor);
			this.priceFactor = priceFactor;
		}
	}

	[SerializeField]
	public ShopPriceManager priceManager;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public CardCharmHolder[] holders;

	[SerializeField]
	public GainCharmSequence gainCharmSequence;

	[SerializeField]
	public Talker talker;

	public const float speechBubDelay = 1f;

	public float speechBubTimer;

	public ShopItem currentHover;

	public readonly List<ShopItem> items = new List<ShopItem>();

	public bool promptEnd;

	public void PromptEnd()
	{
		promptEnd = true;
	}

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
	}

	public void EntityHover(Entity entity)
	{
		ShopItem component = entity.GetComponent<ShopItem>();
		if ((object)component != null)
		{
			Hover(component);
		}
	}

	public void EntityUnHover(Entity entity)
	{
		ShopItem component = entity.GetComponent<ShopItem>();
		if ((object)component != null)
		{
			UnHover(component);
		}
	}

	public void Update()
	{
		if (speechBubTimer > 0f)
		{
			speechBubTimer -= Time.deltaTime;
		}
	}

	public override IEnumerator Populate()
	{
		Data shopData = base.data.Get<Data>("data");
		cardContainer.SetSize(shopData.cards.Count, 0.8f);
		foreach (UpgradedCard card3 in shopData.cards)
		{
			CardData cardData = AddressableLoader.Get<CardData>("CardData", card3.cardDataName).Clone();
			if (card3.upgradeNames != null)
			{
				string[] upgradeNames = card3.upgradeNames;
				foreach (string assetName in upgradeNames)
				{
					CardUpgradeData cardUpgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", assetName);
					if ((bool)cardUpgradeData)
					{
						cardUpgradeData.Clone().Assign(cardData);
					}
				}
			}

			Card card2 = CardManager.Get(cardData, cardController, base.player, inPlay: false, isPlayerCard: true);
			cardContainer.Add(card2.entity);
			ShopItem target = card2.entity.gameObject.AddComponent<ShopItem>();
			priceManager.Add(target, ShopPrice.Position.Bottom).SetPrice(card3.price, card3.priceFactor);
			yield return card2.UpdateData();
		}

		cardContainer.SetChildPositions();
		foreach (CharmShopItemData item in shopData.items)
		{
			CardUpgradeData upgradeDataClone = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", item.upgradeDataName).Clone();
			CreateUpgrade(upgradeDataClone, item.price, item.priceFactor);
		}

		foreach (UpgradedCard card in shopData.cards)
		{
			if (card.purchased)
			{
				Entity entity = cardContainer.First((Entity a) => a.data.name == card.cardDataName);
				if ((bool)entity)
				{
					priceManager.Remove(entity.GetComponent<ShopItem>());
					entity.RemoveFromContainers();
					CardManager.ReturnToPool(entity);
				}
			}
		}

		for (int j = 0; j < items.Count; j++)
		{
			if (shopData.items[j].purchased)
			{
				items[j].gameObject.Destroy();
			}
		}
	}

	public override IEnumerator Run()
	{
		int num = base.data.Get("enterCount", 0) + 1;
		base.data["enterCount"] = num;
		if (num == 1)
		{
			talker.Say("greet", PettyRandom.Range(0.5f, 1f));
			foreach (Entity item in cardContainer)
			{
				Events.InvokeEntityOffered(item);
			}
		}

		cardController.owner = base.player;
		cardContainer.owner = base.player;
		cardSelector.character = base.player;
		while (!promptEnd)
		{
			yield return null;
		}

		if (base.data.Get<Data>("data").items.Count((CharmShopItemData i) => !i.purchased) <= 0)
		{
			node.SetCleared();
		}

		cardController.enabled = false;
		cardSelector.enabled = false;
		foreach (ShopItem item2 in items)
		{
			if ((bool)item2)
			{
				CardCharmInteraction component = item2.GetComponent<CardCharmInteraction>();
				if ((object)component != null)
				{
					component.canHover = false;
					component.canDrag = false;
				}
			}
		}

		if (base.player.entity.display is CharacterDisplay characterDisplay && (bool)characterDisplay.goldDisplay)
		{
			characterDisplay.goldDisplay.HideChange();
		}
	}

	public void Bub(string speechType, params object[] inserts)
	{
		if (speechBubTimer <= 0f)
		{
			talker.Say(speechType, 0f, inserts);
			speechBubTimer = 1f;
		}
	}

	public void TryBuy(Entity entity)
	{
		if (Deckpack.IsOpen)
		{
			return;
		}

		ShopItem component = entity.GetComponent<ShopItem>();
		if ((object)component == null)
		{
			return;
		}

		int price = component.GetPrice();
		if (base.player.data.inventory.gold.Value - price >= 0)
		{
			SfxSystem.OneShot("event:/sfx/location/shop/buying");
			priceManager.Remove(component);
			base.player.SpendGold(price);
			cardSelector.TakeCard(entity);
			base.data.Get<Data>("data").cards.Find((UpgradedCard a) => a.cardDataName == entity.data.name).purchased = true;
			Events.InvokeShopItemPurchase(component);
			talker.Say("thanks", 0f);
		}
		else
		{
			Bub("no");
		}
	}

	public void TryBuy(ShopItem item)
	{
		if (Deckpack.IsOpen)
		{
			return;
		}

		int price = item.GetPrice();
		if (base.player.data.inventory.gold.Value - price >= 0)
		{
			int num = items.IndexOf(item);
			SfxSystem.OneShot("event:/sfx/location/shop/buying");
			base.player.SpendGold(price);
			priceManager.Remove(item);
			UpgradeDisplay componentInChildren = holders[num].GetComponentInChildren<UpgradeDisplay>();
			if ((object)componentInChildren != null)
			{
				componentInChildren.gameObject.Destroy();
				base.data.Get<Data>("data").items[num].purchased = true;
				gainCharmSequence.SetCharm(componentInChildren.data);
				gainCharmSequence.SetCharacter(base.player);
				StartCoroutine(gainCharmSequence.Run());
				Events.InvokeShopItemPurchase(item);
				talker.Say("thanks", 0f);
			}
		}
		else
		{
			Bub("no");
		}
	}

	public void Hover(ShopItem item)
	{
		if ((bool)currentHover)
		{
			UnHover(currentHover);
		}

		currentHover = item;
		if (base.player.entity.display is CharacterDisplay characterDisplay && (bool)characterDisplay.goldDisplay && (bool)priceManager.Get(item))
		{
			int price = item.GetPrice();
			if (price <= base.player.data.inventory.gold)
			{
				characterDisplay.goldDisplay.ShowChange(-price);
			}
		}

		Events.InvokeShopItemHover(item);
	}

	public void UnHover(ShopItem item)
	{
		if (currentHover == item)
		{
			currentHover = null;
			if (base.player.entity.display is CharacterDisplay characterDisplay && (bool)characterDisplay.goldDisplay)
			{
				characterDisplay.goldDisplay.HideChange();
			}

			Events.InvokeShopItemUnHover(item);
		}
	}

	public void CreateUpgrade(CardUpgradeData upgradeDataClone, int price, float priceFactor)
	{
		CardCharmHolder cardCharmHolder = holders.FirstOrDefault((CardCharmHolder a) => a.transform.childCount == 0);
		if ((bool)cardCharmHolder)
		{
			UpgradeDisplay upgradeDisplay = cardCharmHolder.Create(upgradeDataClone);
			ShopItem shopItem = upgradeDisplay.gameObject.AddComponent<ShopItem>();
			items.Add(shopItem);
			ShopPrice shopPrice = priceManager.Add(shopItem, ShopPrice.Position.Bottom);
			shopPrice.SetPrice(price, priceFactor);
			shopPrice.SetOffset(new Vector3(0f, -1.5f, 0f));
			shopPrice.scaleWithTarget = 0f;
			shopPrice.scaleOffsetWithTarget = 0f;
			CardCharmInteraction component = upgradeDisplay.GetComponent<CardCharmInteraction>();
			component.popUpOffset = new Vector2(1f, -0.25f);
			component.onHover.AddListener(delegate
			{
				Hover(shopItem);
			});
			component.onUnHover.AddListener(delegate
			{
				UnHover(shopItem);
			});
			component.onDrag.AddListener(delegate
			{
				TryBuy(shopItem);
			});
		}
	}
}
