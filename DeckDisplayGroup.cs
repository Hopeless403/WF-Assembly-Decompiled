#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using TMPro;
using UnityEngine;

public class DeckDisplayGroup : MonoBehaviour
{
	public TextMeshProUGUI titleElement;

	public CardContainerGrid[] grids;

	[SerializeField]
	public CardController cardController;

	public IEnumerator CreateCard(CardData data)
	{
		Card card = CardManager.Get(data, cardController, null, inPlay: false, isPlayerCard: true);
		AddCard(card);
		yield return card.UpdateData();
	}

	public CardContainerGrid GetGrid(Card card)
	{
		return GetGrid(card.entity.data);
	}

	public CardContainerGrid GetGrid(CardData cardData)
	{
		if (grids.Length <= 1 || !(cardData.cardType.tag == "Friendly"))
		{
			return grids[0];
		}

		return grids[1];
	}

	public void AddCard(Card card)
	{
		CardContainerGrid grid = GetGrid(card);
		grid.Add(card.entity);
		card.entity.owner = grid.owner;
	}

	public void InsertCard(int index, Card card)
	{
		CardContainerGrid grid = GetGrid(card);
		grid.Insert(index, card.entity);
		card.entity.owner = grid.owner;
	}

	public void RemoveCard(Card card)
	{
		GetGrid(card).Remove(card.entity);
	}

	public void UpdatePositions()
	{
		CardContainerGrid[] array = grids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetChildPositions();
		}
	}

	public void Clear()
	{
		CardContainerGrid[] array = grids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DestroyAll();
		}
	}
}
