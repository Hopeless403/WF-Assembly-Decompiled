#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Dead;
using UnityEngine;

[Serializable]
public class InventorySaveData : ILoadable<Inventory>
{
	public CardSaveData[] deck;

	public CardSaveData[] reserve;

	public CardUpgradeSaveData[] upgrades;

	public int gold;

	public InventorySaveData()
	{
	}

	public InventorySaveData(CardSaveData[] deck, CardSaveData[] reserve, CardUpgradeSaveData[] upgrades, int gold)
	{
		this.deck = deck;
		this.reserve = reserve;
		this.upgrades = upgrades;
		this.gold = gold;
	}

	public Inventory Load()
	{
		Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
		CardSaveData[] array = deck;
		foreach (CardSaveData cardSaveData in array)
		{
			inventory.deck.Add(cardSaveData.Load());
		}

		array = reserve;
		foreach (CardSaveData cardSaveData2 in array)
		{
			inventory.reserve.Add(cardSaveData2.Load());
		}

		CardUpgradeSaveData[] array2 = upgrades;
		foreach (CardUpgradeSaveData cardUpgradeSaveData in array2)
		{
			inventory.upgrades.Add(cardUpgradeSaveData.Load());
		}

		inventory.gold = new SafeInt(gold);
		return inventory;
	}
}
