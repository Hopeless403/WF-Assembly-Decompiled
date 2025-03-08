#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Destroy Card", menuName = "Card Scripts/Destroy Card")]
public class CardScriptDestroyCard : CardScript
{
	public override void Run(CardData target)
	{
		RemoveFromDeck(target);
		DestroyEntities(target);
	}

	public static void RemoveFromDeck(CardData target)
	{
		if (References.PlayerData.inventory.deck.RemoveWhere((CardData a) => target.id == a.id))
		{
			Debug.Log("[" + target.name + "] Removed From Player's deck");
		}
		else if (References.PlayerData.inventory.reserve.RemoveWhere((CardData a) => target.id == a.id))
		{
			Debug.Log("[" + target.name + "] Removed From Player's reserve");
		}
	}

	public static void DestroyEntities(CardData target)
	{
		Entity[] array = Object.FindObjectsOfType<Entity>();
		foreach (Entity entity in array)
		{
			if (entity.data == target)
			{
				entity.gameObject.AddComponent<CardDestroyedConsume>().sortingLayer = "ParticlesFront";
				entity.RemoveFromContainers();
				entity.display.GetCanvas().sortingLayerName = "Inspect";
			}
		}

		Object.FindObjectOfType<DeckDisplaySequence>()?.UpdatePositions();
	}
}
