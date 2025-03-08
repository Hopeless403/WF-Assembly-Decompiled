#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[Serializable]
public class CharacterSaveData : ILoadable<Character>
{
	public string name;

	public string title;

	public int team;

	public ClassSaveData classData;

	public InventorySaveData inventoryData;

	[Header("Stats")]
	public int handSize = 6;

	public int redrawBell = 4;

	public int companionLimit = 3;

	public float enemyGoldFactor = 1f;

	public float comboGoldFactor = 1f;

	public CharacterSaveData()
	{
	}

	public CharacterSaveData(Character character)
	{
		name = character.name;
		title = character.title;
		team = character.team;
		classData = character.data.classData.Save();
		inventoryData = character.data.inventory.Save();
		handSize = character.data.handSize;
		redrawBell = character.data.redrawBell;
		companionLimit = character.data.companionLimit;
		enemyGoldFactor = character.data.enemyGoldFactor;
		comboGoldFactor = character.data.comboGoldFactor;
	}

	public Character Load()
	{
		PlayerData playerData = LoadPlayerData();
		playerData.handSize = handSize;
		playerData.redrawBell = redrawBell;
		playerData.companionLimit = companionLimit;
		playerData.enemyGoldFactor = enemyGoldFactor;
		playerData.comboGoldFactor = comboGoldFactor;
		Character character = UnityEngine.Object.Instantiate(playerData.classData.characterPrefab);
		character.name = name;
		character.title = title;
		character.team = team;
		character.Assign(playerData);
		return character;
	}

	public PlayerData LoadPlayerData()
	{
		return new PlayerData(classData.Load(), inventoryData.Load());
	}
}
