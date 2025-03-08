#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCardUnlockSequence : BuildingSequenceWithUnlocks<BuildingCardUnlockSequence>
{
	[SerializeField]
	public CardController controller;

	[SerializeField]
	public Transform owner;

	[SerializeField]
	public CardUnlockSequence cardUnlockSequence;

	[SerializeField]
	public string metaprogressionKey = "companions";

	[SerializeField]
	public string firstGreetKey;

	public void Start()
	{
		_OnStart();
	}

	public override IEnumerator Sequence()
	{
		int num = Mathf.Min(locks.Length, building.type.unlocks.Length);
		for (int i = 0; i < num; i++)
		{
			if (building.type.unlocks[i].IsActive)
			{
				locks[i].SetActive(value: false);
			}
		}

		yield return CreateCards();
		if (building.HasUncheckedUnlocks)
		{
			foreach (string uncheckedUnlock in building.uncheckedUnlocks)
			{
				yield return Unlock(uncheckedUnlock);
			}

			Entity entity = cardSlots.LastOrDefault((CardContainer a) => !a.Empty)?.GetTop();
			if ((bool)entity)
			{
				TalkerNewCard(entity.data);
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
		int num = building.type.unlocks.Select((UnlockData a) => a.name).ToList().IndexOf(unlockDataName);
		if (num < 0)
		{
			throw new IndexOutOfRangeException("[" + unlockDataName + "] does not exist in [" + building.name + "] unlocks list!");
		}

		string assetName = MetaprogressionSystem.Get<List<string>>(metaprogressionKey)[num];
		List<string> list = SaveSystem.LoadProgressData(building.type.unlockedCheckedKey, new List<string>());
		list.Add(unlockDataName);
		SaveSystem.SaveProgressData(building.type.unlockedCheckedKey, list);
		CardData data = AddressableLoader.Get<CardData>("CardData", assetName);
		Card card = CardManager.Get(data, controller, null, inPlay: false, isPlayerCard: true);
		yield return card.UpdateData();
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
		int num = building.checkedUnlocks?.Count ?? 0;
		List<string> list = MetaprogressionSystem.Get<List<string>>(metaprogressionKey);
		for (int i = 0; i < num && i < list.Count; i++)
		{
			string assetName = list[i];
			Card card = CardManager.Get(AddressableLoader.Get<CardData>("CardData", assetName), controller, null, inPlay: false, isPlayerCard: true);
			cards.Add(card.entity);
			clump.Add(card.UpdateData());
		}

		yield return clump.WaitForEnd();
		foreach (Entity item in cards)
		{
			CardContainer cardContainer = cardSlots.FirstOrDefault((CardContainer a) => a.Empty);
			if ((object)cardContainer != null)
			{
				item.flipper.FlipUpInstant();
				item.enabled = true;
				cardContainer.Add(item);
				cardContainer.SetChildPositions();
				Image component = cardContainer.GetComponent<Image>();
				if ((object)component != null)
				{
					component.enabled = false;
				}

				continue;
			}

			break;
		}

		onSetUpComplete?.Invoke();
	}
}
