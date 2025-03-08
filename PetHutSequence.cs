#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

public class PetHutSequence : BuildingSequenceWithUnlocks<PetHutSequence>
{
	[SerializeField]
	public Transform rowLayout;

	[SerializeField]
	public Transform rowPrefab;

	[SerializeField]
	public int cardsPerRow = 4;

	[SerializeField]
	public CardContainer slotPrefab;

	[SerializeField]
	public ChallengeDisplayCreator challengeDisplayPrefab;

	[SerializeField]
	public ChallengeData[] challenges;

	[SerializeField]
	public Transform challengeDisplayParent;

	[SerializeField]
	public CardController controller;

	[SerializeField]
	public CardUnlockSequence cardUnlockSequence;

	[SerializeField]
	public string firstGreetKey = "petHutFirstGreet";

	public Entity lastUnlockedCard;

	public void Start()
	{
		_OnStart();
		cardSlots = cardSlots.RemoveFromArray(delegate(CardContainer container)
		{
			ChallengeDisplayCreator componentInChildren = container.transform.parent.gameObject.GetComponentInChildren<ChallengeDisplayCreator>();
			if (!componentInChildren)
			{
				return true;
			}

			if (!componentInChildren.challenge)
			{
				UnityEngine.Object.Destroy(container.transform.parent.gameObject);
				return false;
			}

			return true;
		});
	}

	public override IEnumerator Sequence()
	{
		yield return CreateCards();
		for (int i = 0; i < cardSlots.Length; i++)
		{
			CardContainer cardContainer = cardSlots[i];
			Transform transform = cardContainer.transform.Find("Lock");
			if ((object)transform != null && (i == 0 || i >= building.type.unlocks.Length || !building.type.unlocks[i] || building.type.unlocks[i].IsActive))
			{
				transform.gameObject.SetActive(value: false);
			}

			if (challenges.Length > i)
			{
				ChallengeData challengeData = challenges[i];
				if ((object)challengeData != null)
				{
					ChallengeDisplayCreator challengeDisplayCreator = UnityEngine.Object.Instantiate(challengeDisplayPrefab, challengeDisplayParent);
					challengeDisplayCreator.transform.position = cardContainer.transform.position;
					challengeDisplayCreator.challenge = challengeData;
					challengeDisplayCreator.Check();
				}
			}
		}

		if (building.HasUncheckedUnlocks)
		{
			foreach (string uncheckedUnlock in building.uncheckedUnlocks)
			{
				yield return Unlock(uncheckedUnlock);
			}

			if ((bool)lastUnlockedCard)
			{
				TalkerNewCard(lastUnlockedCard.data);
			}
		}
		else if (!firstGreetKey.IsNullOrEmpty() && !SaveSystem.LoadProgressData(firstGreetKey, defaultValue: false))
		{
			TalkerFirstGreet();
			SaveSystem.SaveProgressData(firstGreetKey, value: true);
		}
		else
		{
			TalkerGreet();
		}

		onSetUpComplete?.Invoke();
	}

	public IEnumerator Unlock(string unlockDataName)
	{
		int num = 1 + building.type.unlocks.Select((UnlockData a) => a.name).ToList().IndexOf(unlockDataName);
		if (num < 0)
		{
			throw new IndexOutOfRangeException("[" + unlockDataName + "] does not exist in [" + building.name + "] unlocks list!");
		}

		string assetName = MetaprogressionSystem.GetAllPets()[num];
		List<string> list = SaveSystem.LoadProgressData(building.type.unlockedCheckedKey, new List<string>());
		list.Add(unlockDataName);
		SaveSystem.SaveProgressData(building.type.unlockedCheckedKey, list);
		CardData data = AddressableLoader.Get<CardData>("CardData", assetName);
		Card card = CardManager.Get(data, controller, null, inPlay: false, isPlayerCard: true);
		yield return card.UpdateData();
		lastUnlockedCard = card.entity;
		CardContainer finalSlot = cardSlots.FirstOrDefault((CardContainer a) => a.Empty);
		yield return cardUnlockSequence.Run(card.entity, finalSlot);
		if ((bool)finalSlot)
		{
			Image component = finalSlot.GetComponent<Image>();
			if ((object)component != null)
			{
				component.enabled = true;
			}
		}
	}

	public IEnumerator CreateCards()
	{
		List<Entity> cards = new List<Entity>();
		Routine.Clump clump = new Routine.Clump();
		Dictionary<string, string> petDict = MetaprogressionSystem.GetPetDict();
		List<string> checkedUnlocks = building.checkedUnlocks;
		foreach (var (assetName, text3) in petDict)
		{
			if (text3 == null || (checkedUnlocks != null && checkedUnlocks.Contains(text3)))
			{
				Card card = CardManager.Get(AddressableLoader.Get<CardData>("CardData", assetName), controller, null, inPlay: false, isPlayerCard: true);
				cards.Add(card.entity);
				clump.Add(card.UpdateData());
			}
			else
			{
				cards.Add(null);
			}
		}

		yield return clump.WaitForEnd();
		Transform parent = CreateRow();
		int num = 0;
		for (int i = 0; i < cards.Count; i++)
		{
			CardContainer item = CreateSlot(parent);
			cardSlots = cardSlots.AddToArray(item);
			if (++num >= cardsPerRow)
			{
				parent = CreateRow();
				num = 0;
			}
		}

		yield return null;
		for (int j = 0; j < cards.Count; j++)
		{
			Entity entity = cards[j];
			if ((bool)entity)
			{
				CardContainer obj = cardSlots[j];
				entity.flipper.FlipUpInstant();
				entity.enabled = true;
				obj.Add(entity);
				obj.SetChildPositions();
				Image component = obj.GetComponent<Image>();
				if ((object)component != null)
				{
					component.enabled = false;
				}
			}
		}
	}

	public CardContainer CreateSlot(Transform parent)
	{
		return UnityEngine.Object.Instantiate(slotPrefab, parent);
	}

	public Transform CreateRow()
	{
		Transform transform = UnityEngine.Object.Instantiate(rowPrefab, rowLayout);
		transform.SetSiblingIndex(Mathf.Max(0, transform.parent.childCount - 2));
		return transform;
	}
}
