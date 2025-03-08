#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectStartingPet : MonoBehaviour, IRerollable
{
	[SerializeField]
	public SelectLeader leaderSelect;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer leaderContainer;

	[SerializeField]
	public CardContainer group;

	[SerializeField]
	public InspectNewUnitSequence selectionSequence;

	[SerializeField]
	public GameObject text;

	[SerializeField]
	public Vector3 startPos = new Vector3(0f, -7f, 0f);

	[SerializeField]
	public GameObject winStreakDisplay;

	[SerializeField]
	public TitleSetter titleSetter;

	public string[] petData;

	public readonly List<Entity> pets = new List<Entity>();

	public Entity leader;

	public CardContainer leaderPreContainer;

	public int leaderPreContainerIndex;

	public bool CanRun
	{
		get
		{
			if (petData != null)
			{
				return petData.Length > 1;
			}

			return false;
		}
	}

	public bool running { get; set; }

	public int selectedPetIndex { get; set; } = -1;


	public IEnumerator SetUp()
	{
		if (!Campaign.Data.GameMode.takeStartingPet)
		{
			yield break;
		}

		petData = MetaprogressionSystem.GetUnlockedPets();
		Routine.Clump clump = new Routine.Clump();
		string[] array = petData;
		foreach (string text in array)
		{
			if (!text.IsNullOrWhitespace())
			{
				clump.Add(CreateCard(text));
			}
		}

		yield return clump.WaitForEnd();
	}

	public IEnumerator CreateCard(string cardDataName)
	{
		Card card = CardManager.Get(AddressableLoader.Get<CardData>("CardData", cardDataName).Clone(), cardController, null, inPlay: false, isPlayerCard: true);
		group.Insert(0, card.entity);
		pets.Add(card.entity);
		card.transform.localScale = group.GetChildScale(card.entity);
		card.transform.localPosition = startPos;
		card.hover.SetHoverable(value: false);
		yield return card.UpdateData();
	}

	public void Run(Entity leader)
	{
		if (!running)
		{
			StartCoroutine(Routine(leader));
		}
	}

	public void Stop()
	{
		running = false;
	}

	public void Cancel()
	{
		Stop();
		selectedPetIndex = -1;
		leader.RemoveFromContainers();
		leader.wobbler.WobbleRandom();
		leaderPreContainer.Insert(leaderPreContainerIndex, leader);
		leaderPreContainer.TweenChildPositions();
		if ((bool)text)
		{
			text.SetActive(value: false);
		}

		foreach (Entity item in pets.OrderBy((Entity a) => UnityEngine.Random.Range(0f, 1f)))
		{
			LeanTween.moveLocal(item.gameObject, startPos, 0.1f).setEaseInQuad().setDelay(UnityEngine.Random.Range(0f, 0.1f));
			item.display.hover.SetHoverable(value: false);
		}

		winStreakDisplay.SetActive(value: true);
	}

	public IEnumerator Routine(Entity leader)
	{
		if (running)
		{
			yield break;
		}

		running = true;
		this.leader = leader;
		titleSetter.Set();
		winStreakDisplay.SetActive(value: false);
		leaderPreContainer = leader.actualContainers[0];
		leaderPreContainerIndex = leaderPreContainer.IndexOf(leader);
		leader.RemoveFromContainers();
		leader.wobbler.WobbleRandom();
		leaderContainer.Add(leader);
		leaderContainer.TweenChildPositions();
		leaderSelect.Hide();
		foreach (Entity item in pets.OrderBy((Entity a) => UnityEngine.Random.Range(0f, 1f)))
		{
			group.TweenChildPosition(item);
			item.display.hover.SetHoverable(value: true);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.1f));
		}

		if ((bool)text)
		{
			text.SetActive(value: true);
			text.transform.localScale = Vector3.zero;
			LeanTween.scale(text, Vector3.one, 1f).setEaseOutElastic();
		}

		yield return new WaitUntil(() => !running);
	}

	public bool Reroll()
	{
		throw new NotImplementedException();
	}

	public void Select(Entity entity)
	{
		if (running)
		{
			int num = pets.IndexOf(entity);
			if (num >= 0)
			{
				selectedPetIndex = num;
				selectionSequence.SetUnit(entity);
				selectionSequence.Begin();
				cardController.enabled = false;
				cardController.UnHover();
			}
		}
	}

	public void Gain(PlayerData playerData)
	{
		if (selectedPetIndex < 0 && pets.Count > 0)
		{
			selectedPetIndex = 0;
		}

		Entity entity = ((selectedPetIndex >= 0) ? pets[selectedPetIndex] : null);
		if ((bool)entity)
		{
			Events.InvokeEntityChosen(entity);
			playerData.inventory.deck.Insert(0, entity.data);
			MetaprogressionSystem.Set("selectedPet", selectedPetIndex);
		}
	}
}
