#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoutineClunkShop : EventRoutine
{
	[Serializable]
	public class Data
	{
		public List<CardItem> cards = new List<CardItem>();

		public List<CharmItem> charms = new List<CharmItem>();
	}

	[Serializable]
	public abstract class Item
	{
		public int price;

		public float priceFactor = 1f;

		public bool purchased;

		public Item()
		{
		}
	}

	[Serializable]
	public class CardItem : Item
	{
		public string dataName;

		public CardItem()
		{
		}

		public CardItem(CardData cardData, float priceFactor = 1f)
		{
			dataName = cardData.name;
			price = Mathf.RoundToInt((float)cardData.value * priceFactor);
		}
	}

	[Serializable]
	public class CharmItem : Item
	{
		public string dataName;

		public CharmItem()
		{
		}

		public CharmItem(string upgradeDataName, int price, float priceFactor = 1f)
		{
			dataName = upgradeDataName;
			base.price = price;
			price = Mathf.RoundToInt((float)price * priceFactor);
		}
	}

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public ShopPriceManager priceManager;

	[SerializeField]
	public CardContainer[] cardContainers;

	[SerializeField]
	public CardCharmHolder[] charmHolders;

	[SerializeField]
	public GainCharmSequence gainCharmSequence;

	public ShopItem currentHover;

	public bool promptEnd;

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

	public override IEnumerator Run()
	{
		cardController.owner = base.player;
		cardSelector.character = base.player;
		cardController.enabled = true;
		cardSelector.enabled = true;
		while (!promptEnd)
		{
			yield return null;
		}

		promptEnd = false;
		cardController.enabled = false;
		cardSelector.enabled = false;
	}

	public override IEnumerator Populate()
	{
		CinemaBarSystem.Clear();
		UnPopulate();
		Data shopData = base.data.Get<Data>("shopData");
		int cardCount = shopData.cards.Count;
		for (int i = 0; i < cardCount; i++)
		{
			CardItem item = shopData.cards[i];
			if (!item.purchased)
			{
				CardContainer container = ((cardContainers.Length > i) ? cardContainers[i] : null);
				if (!container)
				{
					break;
				}

				container.SetSize(1, 0.67f);
				CardData cardData = AddressableLoader.Get<CardData>("CardData", item.dataName).Clone();
				Card card = CardManager.Get(cardData, cardController, base.player, inPlay: false, isPlayerCard: true);
				yield return card.UpdateData();
				container.Add(card.entity);
				container.SetChildPositions();
				ShopItem target = card.gameObject.AddComponent<ShopItem>();
				priceManager.Add(target, ShopPrice.Position.Bottom).SetPrice(item.price, item.priceFactor);
			}
		}

		int count = shopData.charms.Count;
		for (int j = 0; j < count; j++)
		{
			CharmItem charmItem = shopData.charms[j];
			if (!charmItem.purchased)
			{
				CardCharmHolder cardCharmHolder = ((charmHolders.Length > j) ? charmHolders[j] : null);
				if (cardCharmHolder == null)
				{
					break;
				}

				CardUpgradeData upgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", charmItem.dataName).Clone();
				UpgradeDisplay charm = cardCharmHolder.Create(upgradeData);
				cardCharmHolder.SetPositions();
				ShopItem shopItem = charm.gameObject.AddComponent<ShopItem>();
				CardCharmInteraction orAdd = charm.gameObject.GetOrAdd<CardCharmInteraction>();
				orAdd.canHover = true;
				orAdd.canDrag = true;
				orAdd.onHover.AddListener(delegate
				{
					Hover(shopItem);
				});
				orAdd.onUnHover.AddListener(delegate
				{
					UnHover(shopItem);
				});
				orAdd.onDrag.AddListener(delegate
				{
					TryBuyCharm(charm as CardCharm);
				});
				orAdd.popUpOffset = new Vector2(0.8f, -0.3f);
				priceManager.Add(shopItem, ShopPrice.Position.Top).SetPrice(charmItem.price, charmItem.priceFactor);
			}
		}
	}

	public void UnPopulate()
	{
		CardContainer[] array = cardContainers;
		foreach (CardContainer obj in array)
		{
			obj.DestroyAll();
			obj.Clear();
		}

		CardCharmHolder[] array2 = charmHolders;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Clear();
		}

		priceManager.Clear();
	}

	public bool TryBuy(ShopItem item)
	{
		bool result = false;
		int price = item.GetPrice();
		if (base.player.data.inventory.gold >= price)
		{
			SfxSystem.OneShot("event:/sfx/location/shop/buying");
			priceManager.Remove(item);
			base.player.SpendGold(price);
			Events.InvokeShopItemPurchase(item);
			result = true;
		}

		return result;
	}

	public void TryBuy(Entity entity)
	{
		ShopItem component = entity.GetComponent<ShopItem>();
		if ((object)component != null && TryBuy(component))
		{
			base.data.Get<Data>("shopData").cards.Find((CardItem a) => a.dataName == entity.data.name).purchased = true;
			cardSelector.TakeCard(entity);
		}
	}

	public void TryBuyCharm(CardCharm charm)
	{
		ShopItem component = charm.GetComponent<ShopItem>();
		if ((object)component != null && TryBuy(component))
		{
			base.data.Get<Data>("shopData").charms.Find((CharmItem a) => a.dataName == charm.data.name).purchased = true;
			gainCharmSequence.SetCharacter(base.player);
			gainCharmSequence.SetCharm(charm.data);
			gainCharmSequence.Begin();
			charm.gameObject.Destroy();
		}
	}

	public void Hover(ShopItem item)
	{
		if (base.player.entity.display is CharacterDisplay characterDisplay && characterDisplay.goldDisplay != null && priceManager.Get(item) != null)
		{
			int price = item.GetPrice();
			if (price <= base.player.data.inventory.gold)
			{
				currentHover = item;
				characterDisplay.goldDisplay.ShowChange(-price);
			}
		}

		Events.InvokeShopItemHover(item);
	}

	public void UnHover(ShopItem item)
	{
		if (currentHover == item)
		{
			if (base.player.entity.display is CharacterDisplay characterDisplay && characterDisplay.goldDisplay != null)
			{
				characterDisplay.goldDisplay.HideChange();
			}

			currentHover = null;
			Events.InvokeShopItemUnHover(item);
		}
	}

	public void End()
	{
		promptEnd = true;
		cardController.enabled = false;
	}
}
