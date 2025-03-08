#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AddSecretCardsSystem : GameSystem
{
	[SerializeField]
	public GameModifierData[] requiredModifiers;

	[SerializeField]
	public string[] cardsToAdd;

	[SerializeField]
	public string[] possibleNodeTypes;

	[SerializeField]
	public int[] tiers = new int[2] { 2, 4 };

	public bool HasRequiredModifiers()
	{
		if (Campaign.Data.GameMode.mainGameMode)
		{
			return StormBellManager.TrueFinalBossPointThresholdReached();
		}

		return false;
	}

	public void OnEnable()
	{
		Events.OnCampaignGenerated += Add;
		Events.OnEntityEnterBackpack += EntityEnterBackpack;
		if (Campaign.Data.Modifiers == null || !HasRequiredModifiers())
		{
			base.enabled = false;
		}
	}

	public void OnDisable()
	{
		Events.OnCampaignGenerated -= Add;
		Events.OnEntityEnterBackpack -= EntityEnterBackpack;
	}

	public async Task Add()
	{
		HashSet<CampaignNode> hashSet = new HashSet<CampaignNode>();
		foreach (CampaignNode node2 in References.Campaign.nodes)
		{
			if (tiers.Contains(node2.tier) && possibleNodeTypes.Contains(node2.type.name))
			{
				hashSet.Add(node2);
			}
		}

		if (hashSet.Count < cardsToAdd.Length)
		{
			return;
		}

		List<CampaignNode> list = hashSet.InRandomOrder().OrderByDescending(OrderNodesBySingular).ToList();
		string[] array = cardsToAdd;
		foreach (string cardName in array)
		{
			CampaignNode node = list[0];
			list.RemoveAt(0);
			AddCardToNode(node, cardName);
			list.RemoveAll((CampaignNode a) => a.tier == node.tier);
		}
	}

	public static int OrderNodesBySingular(CampaignNode a)
	{
		if (a.dataLinkedTo == -1)
		{
			List<int> linkedToThis = a.linkedToThis;
			if ((linkedToThis == null || linkedToThis.Count <= 0) && (a.connections.Count > 1 || a.connectedTo > 1))
			{
				return 1;
			}
		}

		return -1;
	}

	public void EntityEnterBackpack(Entity entity)
	{
		if (cardsToAdd.Any((string a) => entity.name == a))
		{
			Campaign.FindCharacterNode(References.Player).glow = false;
		}
	}

	public static void AddCardToNode(CampaignNode node, string cardName)
	{
		node.glow = true;
		CampaignNodeType type = node.type;
		if (!(type is CampaignNodeTypeItem) && !(type is CampaignNodeTypeCurseItems))
		{
			if (!(type is CampaignNodeTypeShop))
			{
				if (type is CampaignNodeTypeCharmShop)
				{
					EventRoutineCharmShop.Data data = node.data.Get<EventRoutineCharmShop.Data>("data");
					float num = UnityEngine.Random.Range(0.8f, 1.2f);
					CardData cardData = AddressableLoader.Get<CardData>("CardData", cardName);
					int index = data.cards.RandomIndex();
					data.cards[index] = new EventRoutineCharmShop.UpgradedCard
					{
						cardDataName = cardName,
						upgradeNames = Array.Empty<string>(),
						price = Mathf.RoundToInt((float)cardData.value * num),
						priceFactor = 1f,
						purchased = false
					};
				}

				return;
			}

			ShopRoutine.Data data2 = node.data.Get<ShopRoutine.Data>("shopData");
			float priceFactor = UnityEngine.Random.Range(0.8f, 1.2f);
			CardData cardData2 = AddressableLoader.Get<CardData>("CardData", cardName);
			{
				foreach (int item in data2.items.GetIndices().InRandomOrder())
				{
					if (data2.items[item].category == "Items" && (data2.upgrades == null || data2.upgrades.Count <= item || data2.upgrades[item] == null || data2.upgrades[item].Length == 0))
					{
						float priceFactor2 = data2.items[item].priceFactor;
						data2.items[item] = new ShopRoutine.Item("Items", cardData2, 0, priceFactor)
						{
							priceFactor = priceFactor2
						};
						break;
					}
				}

				return;
			}
		}

		SaveCollection<string> saveCollection = node.data.Get<SaveCollection<string>>("cards");
		bool flag = false;
		foreach (int item2 in saveCollection.collection.GetIndices().InRandomOrder())
		{
			if (!node.data.ContainsKey($"upgrades{item2}"))
			{
				saveCollection[item2] = cardName;
				flag = true;
				break;
			}
		}

		if (!flag)
		{
			int num2 = UnityEngine.Random.Range(0, saveCollection.Count - 1);
			saveCollection[num2] = cardName;
			node.data.Remove($"upgrades{num2}");
		}
	}
}
