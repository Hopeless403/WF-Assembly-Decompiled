#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRoutineGnomeShop : EventRoutine, IRerollable
{
	[Serializable]
	public class Data
	{
		public string[] pool;

		public List<string> cards;

		public int price;

		public int priceAdd;

		public int cyclesThroughPool;
	}

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public ShopPriceManager priceManager;

	[SerializeField]
	public ShopItem bell;

	[SerializeField]
	public float cardScale = 0.8f;

	public Entity currentCard;

	public bool promptReroll;

	public bool promptEnd;

	public override IEnumerator Populate()
	{
		Data data = base.data.Get<Data>("data");
		priceManager.Add(bell, ShopPrice.Position.Bottom).SetPrice(data.price);
		cardContainer.SetSize(1, cardScale);
		string nextCard = GetNextCard(data);
		yield return CreateCard(nextCard);
	}

	public override IEnumerator Run()
	{
		cardController.owner = base.player;
		cardContainer.owner = base.player;
		cardSelector.character = base.player;
		currentCard.flipper.FlipUp();
		while (!promptEnd)
		{
			if (promptReroll)
			{
				yield return RerollRoutine();
				promptReroll = false;
			}
			else
			{
				yield return null;
			}
		}
	}

	public void TakeCard()
	{
		cardSelector.TakeCard(currentCard);
		cardController.Disable();
		Events.InvokeEntityChosen(currentCard);
		priceManager.Remove(bell);
		Button component = bell.GetComponent<Button>();
		if ((object)component != null)
		{
			component.interactable = false;
		}

		promptEnd = true;
		node.SetCleared();
	}

	public string GetNextCard(Data shopData)
	{
		if (shopData.cards == null)
		{
			shopData.cards = new List<string>();
		}

		if (shopData.cards.Count <= 0)
		{
			UnityEngine.Random.InitState(node.seed);
			for (int i = 0; i < shopData.cyclesThroughPool; i++)
			{
				UnityEngine.Random.Range(0f, 1f);
			}

			string[] pool = shopData.pool;
			foreach (string item in pool)
			{
				shopData.cards.Insert(UnityEngine.Random.Range(0, shopData.cards.Count), item);
			}

			shopData.cyclesThroughPool++;
		}

		string result = shopData.cards[0];
		shopData.cards.RemoveAt(0);
		return result;
	}

	public IEnumerator CreateCard(string cardDataName)
	{
		Card card = CardManager.Get(AddressableLoader.Get<CardData>("CardData", cardDataName).Clone(), cardController, base.player, inPlay: false, isPlayerCard: true);
		currentCard = card.entity;
		card.transform.position = new Vector3(-999f, 0f, 0f);
		card.entity.flipper.FlipDownInstant();
		cardContainer.Add(card.entity);
		yield return card.UpdateData();
		cardContainer.SetChildPositions();
	}

	public void HitBell()
	{
		if (!promptEnd && !promptReroll)
		{
			int price = bell.GetPrice();
			if (base.player.data.inventory.gold.Value - price >= 0)
			{
				SfxSystem.OneShot("event:/sfx/location/shop/buying");
				base.player.SpendGold(price);
				Events.InvokeShopItemPurchase(bell);
				Reroll();
				priceManager.Remove(bell);
			}
		}
	}

	public bool Reroll()
	{
		promptReroll = true;
		return true;
	}

	public IEnumerator RerollRoutine()
	{
		cardController.Disable();
		InputSystem.Disable();
		currentCard.flipper.FlipDown();
		currentCard.RemoveFromContainers();
		Data shopData = base.data.Get<Data>("data");
		string nextCard = GetNextCard(shopData);
		Entity preCard = currentCard;
		yield return CreateCard(nextCard);
		CardManager.ReturnToPool(preCard);
		currentCard.flipper.FlipUp();
		shopData.price += shopData.priceAdd;
		priceManager.Add(bell, ShopPrice.Position.Bottom).SetPrice(shopData.price);
		InputSystem.Enable();
		cardController.Enable();
	}
}
