#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class ShopRoutine : EventRoutine
{
	[Serializable]
	public class Data : ICloneable
	{
		public List<Item> items = new List<Item>();

		public List<string[]> upgrades = new List<string[]>();

		public List<string> charms = new List<string>();

		public int charmPrice = 50;

		public int charmPriceAdd = 20;

		public bool hasCrown = true;

		public string crownType;

		public int crownPrice = 120;

		public int crownPriceAdd = 30;

		public int openCount;

		public object Clone()
		{
			return new Data
			{
				items = items.Select((Item a) => (Item)a.Clone()).ToList(),
				upgrades = upgrades.Clone(),
				charms = charms.Clone(),
				charmPrice = charmPrice,
				charmPriceAdd = charmPriceAdd,
				hasCrown = hasCrown,
				crownType = crownType,
				crownPrice = crownPrice,
				crownPriceAdd = crownPriceAdd,
				openCount = openCount
			};
		}
	}

	[Serializable]
	public class Item : ICloneable
	{
		public string category;

		public string cardDataName;

		public int price;

		public float priceFactor = 1f;

		public bool purchased;

		public Item()
		{
		}

		public Item(string category, CardData cardData, int priceOffset, float priceFactor = 1f)
		{
			this.category = category;
			cardDataName = cardData.name;
			price = Mathf.RoundToInt((float)cardData.value * priceFactor) + priceOffset;
		}

		public object Clone()
		{
			return new Item
			{
				category = category,
				cardDataName = cardDataName,
				price = price,
				priceFactor = priceFactor,
				purchased = purchased
			};
		}
	}

	[Serializable]
	public struct Container
	{
		public string category;

		public CardContainer container;

		public float cardScale;

		public ShopPrice.Position pricePosition;
	}

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public float cardContainerRandomAngle = 2f;

	[SerializeField]
	public ShopPriceManager priceManager;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString enterKey;

	[SerializeField]
	public string brokenVaseCardName = "BrokenVase";

	[Header("Card Containers")]
	[SerializeField]
	public Container[] containers;

	[Header("Charms")]
	[SerializeField]
	public CharmMachine charmMachine;

	[SerializeField]
	public OpenCharmBlockSequence openSequence;

	[SerializeField]
	public Transform charmBlock;

	[Header("Crown")]
	[SerializeField]
	public CrownHolderShop crownHolder;

	[SerializeField]
	public GainCrownSequence gainCrownSequence;

	[Header("Speech Bubs")]
	[SerializeField]
	public Talker talker;

	[SerializeField]
	public Vector2 sayDelay = new Vector2(0.8f, 1f);

	[SerializeField]
	public float speechBubDelay = 1f;

	[Header("Music")]
	[SerializeField]
	public EventReference music;

	[SerializeField]
	public SfxLoop ovenCrackleLoop;

	public bool open;

	public bool promptOpen;

	public bool promptClose;

	public bool promptEnd;

	public bool promptBuyCharm;

	public bool promptBuyCrown;

	public bool endWhenClosed = true;

	public List<Entity> entities;

	public ShopItem currentHover;

	public float speechBubTimer;

	public int secretCount;

	public ShopItem secretClick;

	public int secretClickCount;

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

	public override IEnumerator Run()
	{
		secretCount = UnityEngine.Random.Range(7, 10);
		CinemaBarSystem.Top.SetPrompt(enterKey.GetLocalizedString(), "Select");
		MusicSystem.StopMusic();
		MusicSystem.StartMusic(music);
		MusicSystem.SetParam("shop_enter", 0f);
		cardController.owner = base.player;
		cardSelector.character = base.player;
		while (!promptEnd)
		{
			if (promptBuyCharm)
			{
				yield return BuyCharmRoutine();
			}
			else if (promptBuyCrown)
			{
				yield return BuyCrownRoutine();
			}

			else if (!open && promptOpen)
			{
				yield return OpenRoutine();
				ovenCrackleLoop.Play();
			}

			else if (open && promptClose)
			{
				yield return CloseRoutine();
				if (endWhenClosed)
				{
					End();
				}
			}
			else
			{
				yield return null;
			}
		}

		promptEnd = false;
		ovenCrackleLoop.Stop();
		cardController.enabled = false;
		cardSelector.enabled = false;
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
			speechBubTimer = speechBubDelay;
		}
	}

	public void Open(BaseEventData eventData)
	{
		if ((!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left) && !open && !promptOpen)
		{
			promptOpen = true;
		}
	}

	public void Close()
	{
		if (open && !promptClose)
		{
			promptClose = true;
		}
	}

	public IEnumerator OpenRoutine()
	{
		SfxSystem.OneShot("event:/sfx/location/shop/visit");
		animator.SetBool("Zoom", value: true);
		promptOpen = false;
		open = true;
		int num = (int)base.data.GetValueOrDefault("openCount", 0);
		if (num <= 0)
		{
			float num2 = sayDelay.Random() - 0.5f;
			if (base.data.Get<Data>("shopData").items.Count((Item a) => a.cardDataName == brokenVaseCardName) > 0)
			{
				talker.Say("broken vase", num2);
				talker.Say("broken vase price", num2 + 1f);
			}
			else
			{
				talker.Say("greet1", num2);
				talker.Say("greet2", num2 + 1f);
			}

			speechBubTimer = speechBubDelay;
			Container[] array = containers;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (Entity item in array[i].container)
				{
					Events.InvokeEntityOffered(item);
				}
			}
		}

		base.data["openCount"] = num + 1;
		yield return new WaitForSeconds(0.5f);
		yield return AdjustMusicToInside(0.5f);
	}

	public IEnumerator CloseRoutine()
	{
		animator.SetBool("Zoom", value: false);
		promptClose = false;
		open = false;
		yield return null;
	}

	public static IEnumerator AdjustMusicToInside(float totalTime)
	{
		float v = 0f;
		float add = 1f / totalTime;
		while (v < 1f)
		{
			v += Time.deltaTime * add;
			MusicSystem.SetParam("shop_enter", v);
			AmbienceSystem.SetParam("shop_enter", v);
			yield return null;
		}
	}

	public void End()
	{
		promptEnd = true;
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
		int num = base.player.data.inventory.gold.Value - price;
		if (num >= 0)
		{
			Bub("thanks");
			SfxSystem.OneShot("event:/sfx/location/shop/buying");
			priceManager.Remove(component);
			base.player.SpendGold(price);
			cardSelector.TakeCard(entity);
			base.data.Get<Data>("shopData").items.Find((Item a) => a.cardDataName == entity.data.name).purchased = true;
			Events.InvokeShopItemPurchase(component);
		}
		else if (SecretCheck(num, component))
		{
			TryBuy(entity);
			Events.InvokeShopItemHaggled(component);
		}
		else
		{
			Bub("not enough gold");
		}
	}

	public void TryBuyCharm(BaseEventData eventData)
	{
		if (Deckpack.IsOpen || (eventData is PointerEventData pointerEventData && pointerEventData.button != 0))
		{
			return;
		}

		Data data = base.data.Get<Data>("shopData");
		if (data.charms.Count <= 0)
		{
			return;
		}

		int num = base.player.data.inventory.gold.Value - data.charmPrice;
		if (num >= 0)
		{
			if (charmMachine.CanRun())
			{
				Bub("thanks");
				promptBuyCharm = true;
			}

			return;
		}

		ShopItem component = charmMachine.GetComponent<ShopItem>();
		if (SecretCheck(num, component))
		{
			TryBuyCharm(eventData);
			Events.InvokeShopItemHaggled(component);
		}
		else
		{
			Bub("not enough gold");
		}
	}

	public void TryBuyCrown(BaseEventData eventData)
	{
		if (Deckpack.IsOpen || (eventData is PointerEventData pointerEventData && pointerEventData.button != 0))
		{
			return;
		}

		Data data = base.data.Get<Data>("shopData");
		if (!data.hasCrown)
		{
			return;
		}

		int num = base.player.data.inventory.gold.Value - GetCrownPrice(data);
		if (num >= 0)
		{
			if (crownHolder.CanTake())
			{
				Bub("thanks");
				promptBuyCrown = true;
			}

			return;
		}

		ShopItem component = crownHolder.GetComponent<ShopItem>();
		if (SecretCheck(num, component))
		{
			TryBuyCrown(eventData);
			Events.InvokeShopItemHaggled(component);
		}
		else
		{
			Bub("not enough gold");
		}
	}

	public bool SecretCheck(int goldDiff, ShopItem item)
	{
		if (goldDiff == -1)
		{
			if (secretClick != item)
			{
				secretClick = item;
				secretClickCount = 1;
			}
			else if (++secretClickCount >= secretCount)
			{
				speechBubTimer = 0f;
				int num = item.GetPrice() - 1;
				Bub("secret", num);
				base.player.data.inventory.gold.Value++;
				secretClick = null;
				return true;
			}
		}

		return false;
	}

	public IEnumerator BuyCharmRoutine()
	{
		promptBuyCharm = false;
		SfxSystem.OneShot("event:/sfx/location/shop/buying");
		Data shopData = base.data.Get<Data>("shopData");
		cardController.enabled = false;
		crownHolder.enabled = false;
		charmMachine.enabled = false;
		base.player.SpendGold(shopData.charmPrice);
		string c = shopData.charms[0];
		ShopItem item = charmMachine.GetComponent<ShopItem>();
		priceManager.Remove(item);
		yield return charmMachine.Run();
		shopData.charms.RemoveAt(0);
		shopData.charmPrice += shopData.charmPriceAdd;
		CardUpgradeData charmData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", c).Clone();
		openSequence.SetCharm(charmData, charmBlock);
		openSequence.SetCharacter(base.player);
		yield return openSequence.Run();
		if (shopData.charms.Count > 0)
		{
			charmMachine.enabled = true;
			CreateCharmPrice();
		}

		cardController.enabled = true;
		crownHolder.enabled = true;
		Events.InvokeShopItemPurchase(item);
	}

	public IEnumerator BuyCrownRoutine()
	{
		promptBuyCrown = false;
		Data data = base.data.Get<Data>("shopData");
		SfxSystem.OneShot("event:/sfx/location/shop/buying");
		base.player.SpendGold(GetCrownPrice(data));
		crownHolder.TakeCrown();
		data.hasCrown = false;
		ShopItem item = crownHolder.GetComponent<ShopItem>();
		priceManager.Remove(item);
		gainCrownSequence.SetData(crownHolder.GetCrownData().Clone());
		yield return gainCrownSequence.Run();
		Events.InvokeShopItemPurchase(item);
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

	public override IEnumerator Populate()
	{
		yield return UnPopulate();
		Data shopData = base.data.Get<Data>("shopData");
		entities = new List<Entity>();
		Container[] array = containers;
		for (int i = 0; i < array.Length; i++)
		{
			Container c = array[i];
			List<Item> list = shopData.items.FindAll((Item a) => a.category == c.category);
			c.container.SetSize(list.Count, c.cardScale);
			CardContainer container = c.container;
			container.owner = base.player;
			container.transform.localEulerAngles = container.transform.localEulerAngles.WithZ(PettyRandom.Range(0f - cardContainerRandomAngle, cardContainerRandomAngle));
			foreach (Item item2 in list)
			{
				CardData cardDataClone = AddressableLoader.GetCardDataClone(item2.cardDataName);
				int num = shopData.items.IndexOf(item2);
				if (shopData.upgrades.Count > num)
				{
					string[] array2 = shopData.upgrades[num];
					if (array2 != null)
					{
						string[] array3 = array2;
						foreach (string assetName in array3)
						{
							AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", assetName).Clone().Assign(cardDataClone);
						}
					}
				}

				Card card = CardManager.Get(cardDataClone, cardController, base.player, inPlay: false, isPlayerCard: true);
				yield return card.UpdateData();
				container.Add(card.entity);
				entities.Add(card.entity);
				ShopItem target = card.entity.gameObject.AddComponent<ShopItem>();
				priceManager.Add(target, c.pricePosition).SetPrice(item2.price, item2.priceFactor);
			}

			foreach (Entity item3 in container)
			{
				Transform obj = item3.transform;
				obj.localPosition = container.GetChildPosition(item3);
				obj.localScale = container.GetChildScale(item3);
				obj.localEulerAngles = container.GetChildRotation(item3);
			}
		}

		foreach (Item item in shopData.items)
		{
			if (item.purchased)
			{
				Entity entity = entities.Find((Entity a) => a.data.name == item.cardDataName);
				if ((bool)entity)
				{
					priceManager.Remove(entity.GetComponent<ShopItem>());
					entity.RemoveFromContainers();
					CardManager.ReturnToPool(entity);
				}
			}
		}

		if ((bool)charmMachine && shopData.charms.Count <= 0)
		{
			charmMachine.enabled = false;
		}
		else
		{
			CreateCharmPrice();
		}

		if ((bool)crownHolder && !shopData.hasCrown)
		{
			crownHolder.TakeCrown();
			yield break;
		}

		string assetName2 = (shopData.crownType.IsNullOrWhitespace() ? "Crown" : shopData.crownType);
		CardUpgradeData cardUpgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", assetName2);
		if ((bool)cardUpgradeData)
		{
			crownHolder.SetCrownData(cardUpgradeData);
		}

		CreateCrownPrice();
	}

	public void CreateCharmPrice()
	{
		Data data = base.data.Get<Data>("shopData");
		if (charmMachine != null && data.charms.Count > 0)
		{
			ShopPrice shopPrice = priceManager.Add(charmMachine.GetComponent<ShopItem>(), ShopPrice.Position.Bottom);
			shopPrice.SetPrice(data.charmPrice);
			shopPrice.SetOffset(new Vector3(0f, -0.33f, 0f));
			shopPrice.scaleWithTarget = 0f;
			shopPrice.scaleOffsetWithTarget = 0f;
		}
	}

	public void CreateCrownPrice()
	{
		Data data = base.data.Get<Data>("shopData");
		if (crownHolder != null && data.hasCrown)
		{
			ShopPrice shopPrice = priceManager.Add(crownHolder.GetComponent<ShopItem>(), ShopPrice.Position.Bottom);
			shopPrice.SetPrice(GetCrownPrice(data));
			shopPrice.SetOffset(new Vector3(0f, -0.3f, 0f));
			shopPrice.scaleWithTarget = 0f;
			shopPrice.scaleOffsetWithTarget = 0f;
		}
	}

	public static int GetCrownPrice(Data shopData)
	{
		return shopData.crownPrice + shopData.crownPriceAdd * StatsSystem.Get().Get("shopItemsBought", "crown", 0);
	}

	public IEnumerator UnPopulate()
	{
		List<Entity> list = new List<Entity>();
		Container[] array = containers;
		for (int i = 0; i < array.Length; i++)
		{
			Container container = array[i];
			foreach (Entity item in container.container)
			{
				list.Add(item);
			}

			container.container.Clear();
		}

		foreach (Entity item2 in list)
		{
			CardManager.ReturnToPool(item2);
		}

		priceManager.Clear();
		yield return null;
	}
}
