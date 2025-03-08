#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardSelector : MonoBehaviour
{
	public Character character;

	public UnityEventEntity selectEvent;

	public void TakeCard(Entity entity)
	{
		if ((bool)character && (bool)entity.data)
		{
			Debug.Log("CardSelector → adding [" + entity.data.name + "] to " + character.name + "'s deck");
			character.data.inventory.deck.Add(entity.data);
			MoveCardToDeck(entity);
			selectEvent.Invoke(entity);
		}
	}

	public void TakeFirstCard(CardContainer cardContainer)
	{
		if (cardContainer.Count > 0)
		{
			TakeCard(cardContainer.GetTop());
		}
	}

	public void MoveCardToDeck(Entity entity)
	{
		Events.InvokeEntityEnterBackpack(entity);
		entity.transform.parent = character.entity.display.transform;
		entity.display?.hover?.Disable();
		new Routine(AssetLoader.Lookup<CardAnimation>("CardAnimations", "FlyToBackpack").Routine(entity));
	}
}
