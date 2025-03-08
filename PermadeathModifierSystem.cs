#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PermadeathModifierSystem : GameSystem
{
	public static readonly string[] LegalTypes = new string[1] { "Friendly" };

	public void OnEnable()
	{
		Events.OnEntityKilled += EntityKilled;
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
	}

	public static void EntityKilled(Entity entity, DeathType deathType)
	{
		if (IsPlayerCard(entity) && IsLegalCardType(entity))
		{
			RemoveFromDeck(entity, References.PlayerData.inventory);
		}
	}

	public static bool IsPlayerCard(Entity entity)
	{
		if (entity.owner == References.Player)
		{
			return References.PlayerData.inventory.deck.Contains(entity.data);
		}

		return false;
	}

	public static bool IsLegalCardType(Entity entity)
	{
		return LegalTypes.Contains(entity.data.cardType.name);
	}

	public static void RemoveFromDeck(Entity entity, Inventory inventory)
	{
		Debug.Log($"Permadeath System → deleting [{entity.data}]");
		inventory.deck.Remove(entity.data);
	}
}
