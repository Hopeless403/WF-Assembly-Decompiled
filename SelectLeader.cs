#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

public class SelectLeader : MonoBehaviour, IRerollable
{
	public class Character
	{
		public PlayerData data;

		public Entity entity;

		public Character(PlayerData data, Entity entity)
		{
			this.data = data;
			this.entity = entity;
		}

		public void Clear()
		{
			if ((bool)entity)
			{
				entity.RemoveFromContainers();
				CardManager.ReturnToPool(entity);
				Object.Destroy(entity.data);
				Object.Destroy(data.inventory);
			}

			data = null;
		}

		public void AddLeaderToInventory()
		{
			Events.InvokeEntityChosen(entity);
			data.inventory.deck.Insert(0, entity.data);
		}
	}

	public class LeaderPool
	{
		public readonly ClassData classData;

		public readonly List<CardData> pool = new List<CardData>();

		public LeaderPool(ClassData classData)
		{
			this.classData = classData;
		}

		public CardData Pull()
		{
			if (pool.Count <= 0)
			{
				pool.AddRange(classData.leaders);
				pool.Shuffle();
			}

			CardData result = pool[0];
			pool.RemoveAt(0);
			return result;
		}
	}

	[SerializeField]
	public int options = 3;

	[SerializeField]
	public int differentTribes = 3;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer leaderCardContainer;

	[SerializeField]
	public InspectNewUnitSequence selectionSequence;

	[SerializeField]
	public TitleSetter titleSetter;

	[Header("Tribe Flags")]
	[SerializeField]
	public Transform flagGroup;

	[SerializeField]
	public TribeFlagDisplay flagBase;

	[SerializeField]
	public Vector3 flagOffset = new Vector3(0f, -4f);

	[SerializeField]
	public TribeDisplaySequence tribeDisplay;

	public readonly List<GameObject> flags = new List<GameObject>();

	public List<Character> characters;

	public Dictionary<ClassData, LeaderPool> leaderPools;

	public int seed;

	public Character current { get; set; }

	public bool generating { get; set; }

	public bool running { get; set; }

	public void Run(List<ClassData> tribes)
	{
		running = true;
		titleSetter.Set();
		leaderPools = new Dictionary<ClassData, LeaderPool>();
		foreach (ClassData tribe in tribes)
		{
			leaderPools[tribe] = new LeaderPool(tribe);
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		running = false;
	}

	public void Return()
	{
		running = true;
		base.gameObject.SetActive(value: true);
		LeanTween.cancel(base.gameObject);
		LeanTween.moveLocal(base.gameObject, Vector3.zero, 0.3f).setEaseOutQuint();
	}

	public void SetSeed(int seed)
	{
		this.seed = seed;
		UnityEngine.Random.InitState(seed);
		Names.Reset();
	}

	public IEnumerator GenerateLeaders(bool useSeed)
	{
		if (useSeed)
		{
			UnityEngine.Random.InitState(seed);
			Names.Reset();
		}

		generating = true;
		Clear();
		List<ClassData> availableTribes = leaderPools.Keys.ToList();
		availableTribes.Shuffle();
		List<ClassData> list = new List<ClassData>();
		for (int i = 0; i < Mathf.Min(options, differentTribes); i++)
		{
			list.Add(availableTribes[i % availableTribes.Count]);
		}

		while (list.Count < options)
		{
			list.Add(availableTribes.RandomItem());
		}

		if (list.Count > 1)
		{
			list.Shuffle();
		}

		List<Card> list2 = new List<Card>();
		for (int j = 0; j < options; j++)
		{
			list2.Add(CreateLeader(list[j]));
		}

		SetLeaderPositions();
		foreach (Card item in list2)
		{
			yield return item.UpdateData();
		}

		if (availableTribes.Count > 1)
		{
			foreach (Character c in characters)
			{
				Vector3 position = c.entity.transform.position + flagOffset;
				TribeFlagDisplay tribeFlagDisplay = Object.Instantiate(flagBase, position, Quaternion.identity, flagGroup);
				tribeFlagDisplay.SetFlagSprite(c.data.classData.flag);
				tribeFlagDisplay.AddPressAction(delegate
				{
					tribeDisplay.Run(c.data.classData.name);
				});
				tribeFlagDisplay.gameObject.SetActive(value: true);
				flags.Add(tribeFlagDisplay.gameObject);
			}
		}

		generating = false;
	}

	public Card CreateLeader(ClassData classData)
	{
		CardData data = leaderPools[classData].Pull().Clone();
		PlayerData data2 = new PlayerData(classData, classData.startingInventory.Clone());
		Card card = CardManager.Get(data, cardController, null, inPlay: false, isPlayerCard: true);
		leaderCardContainer.Add(card.entity);
		card.entity.flipper.FlipDownInstant();
		characters.Add(new Character(data2, card.entity));
		return card;
	}

	public void Clear()
	{
		if (characters == null)
		{
			characters = new List<Character>();
		}

		foreach (Character character in characters)
		{
			character.Clear();
		}

		current = null;
		characters.Clear();
		StopAllCoroutines();
		flags.DestroyAllAndClear();
	}

	public void SetLeaderPositions()
	{
		List<MonoBehaviour> list = new List<MonoBehaviour>();
		foreach (Character character in characters)
		{
			AngleWobbler[] componentsInChildren = character.entity.GetComponentsInChildren<AngleWobbler>();
			foreach (AngleWobbler angleWobbler in componentsInChildren)
			{
				if (angleWobbler.enabled)
				{
					angleWobbler.enabled = false;
					list.Add(angleWobbler);
				}
			}
		}

		leaderCardContainer.SetChildPositions();
		foreach (MonoBehaviour item in list)
		{
			item.enabled = true;
		}
	}

	public void FlipUpLeaders()
	{
		StartCoroutine(FlipUpRoutine());
	}

	public void FlipUpLeadersInstant()
	{
		foreach (Entity item in leaderCardContainer)
		{
			item.flipper.FlipUpInstant();
		}
	}

	public IEnumerator FlipUpRoutine()
	{
		foreach (Entity item in leaderCardContainer)
		{
			item.flipper.FlipUp(force: true);
			yield return Sequences.Wait(PettyRandom.Range(0f, 0.1f));
		}
	}

	public bool Reroll()
	{
		if (!generating)
		{
			InspectNewUnitSequence inspectNewUnitSequence = Object.FindObjectOfType<InspectNewUnitSequence>();
			if (!InspectSystem.IsActive() && (!inspectNewUnitSequence || !inspectNewUnitSequence.gameObject.activeSelf))
			{
				StartCoroutine(GenerateLeaders(useSeed: false));
				CardPopUp.Clear();
				return true;
			}
		}

		return false;
	}

	public void Select(Entity entity)
	{
		if (running)
		{
			Character character = characters.FirstOrDefault((Character a) => a.entity == entity);
			if (character != null)
			{
				Select(character);
				selectionSequence.SetUnit(entity);
				selectionSequence.Begin();
				cardController.enabled = false;
				cardController.UnHover();
			}
		}
	}

	public void Select(Character character)
	{
		current = character;
	}

	public void Cancel()
	{
		current = null;
		running = false;
		Clear();
	}
}
