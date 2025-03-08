#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Dead;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject, ISaveable<InventorySaveData>
{
	public CardDataList deck = new CardDataList();

	public CardDataList reserve = new CardDataList();

	public List<CardUpgradeData> upgrades = new List<CardUpgradeData>();

	public SafeInt gold;

	public int goldOwed;

	public void AddGold(int amount)
	{
		gold += amount;
		goldOwed = Mathf.Max(0, goldOwed - amount);
	}

	public Inventory Clone()
	{
		Inventory inventory = this.InstantiateKeepName();
		inventory.deck.Clear();
		inventory.reserve.Clear();
		inventory.upgrades.Clear();
		foreach (CardData item in deck)
		{
			inventory.deck.Add(item.Clone());
		}

		foreach (CardData item2 in reserve)
		{
			inventory.reserve.Add(item2.Clone());
		}

		foreach (CardUpgradeData upgrade in upgrades)
		{
			inventory.upgrades.Add(upgrade.Clone());
		}

		inventory.gold = new SafeInt(gold.Value);
		return inventory;
	}

	public InventorySaveData Save()
	{
		return new InventorySaveData(deck.SaveArray<CardData, CardSaveData>(), reserve.SaveArray<CardData, CardSaveData>(), upgrades.SaveArray<CardUpgradeData, CardUpgradeSaveData>(), gold.Value + goldOwed);
	}
}
