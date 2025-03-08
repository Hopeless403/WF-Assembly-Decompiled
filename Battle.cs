#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

public class Battle : SceneRoutine
{
	public enum Phase
	{
		None,
		Init,
		Play,
		Battle,
		End,
		LastStand
	}

	public static Battle instance;

	public Character player;

	public Character enemy;

	public Character winner;

	public readonly List<Entity> minibosses = new List<Entity>();

	public Phase _phase;

	public Transform outOfUseCardsGroup;

	public CardController playerCardController;

	public readonly Dictionary<Character, List<CardContainer>> rows = new Dictionary<Character, List<CardContainer>>();

	public readonly Dictionary<CardContainer, int> rowIndices = new Dictionary<CardContainer, int>();

	public List<Entity> cards = new List<Entity>();

	public float startDelay = 1f;

	public bool canEnd = true;

	public int rowCount;

	public int turnCount;

	public bool cancelTurn;

	public bool auto;

	public bool loadMidBattle;

	public int playerMinibossCount => minibosses.Count((Entity a) => (bool)a && a.owner == player);

	public int enemyMinibossCount => minibosses.Count((Entity a) => (bool)a && a.owner == enemy);

	public Phase phase
	{
		get
		{
			return _phase;
		}
		set
		{
			if (_phase != value && !ended)
			{
				Debug.Log($"Battle Phase: {value}");
				Events.InvokeBattlePhaseStart(value);
				_phase = value;
			}
		}
	}

	public IEnumerable<CardSlotLane> allRows => rows.Values.SelectMany((List<CardContainer> a) => a).Cast<CardSlotLane>();

	public IEnumerable<CardSlot> allSlots => allRows.SelectMany((CardSlotLane a) => a.slots);

	public bool ended => phase == Phase.End;

	public void Awake()
	{
		instance = this;
		References.Battle = this;
	}

	[Button("Win", EButtonEnableMode.Always)]
	public void PlayerWin()
	{
		winner = player;
		phase = Phase.End;
	}

	[Button("Lose", EButtonEnableMode.Always)]
	public void EnemyWin()
	{
		winner = enemy;
		phase = Phase.End;
	}

	public void Start()
	{
		Events.OnEntityCreated += EntityCreated;
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntityDestroyed += EntityDestroyed;
	}

	public void OnDestroy()
	{
		Events.OnEntityCreated -= EntityCreated;
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntityDestroyed -= EntityDestroyed;
	}

	public void EntityCreated(Entity entity)
	{
		if (entity.data.cardType.miniboss)
		{
			minibosses.Add(entity);
		}

		cards.Add(entity);
	}

	public void CancelTurn()
	{
		cancelTurn = true;
	}

	public void EntityDestroyed(Entity entity)
	{
		EntityKilled(entity, DeathType.Normal);
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if (minibosses.Remove(entity) && phase != Phase.LastStand)
		{
			CheckEnd();
		}

		cards.Remove(entity);
	}

	public bool CheckEnd()
	{
		if (!canEnd)
		{
			return false;
		}

		if (playerMinibossCount <= 0)
		{
			winner = enemy;
			phase = Phase.End;
			return true;
		}

		if (enemyMinibossCount <= 0)
		{
			winner = player;
			phase = Phase.End;
			return true;
		}

		return false;
	}

	public static Character GetOpponent(Character character)
	{
		if (!(character == instance.enemy))
		{
			if (!(character == instance.player))
			{
				return null;
			}

			return instance.enemy;
		}

		return instance.player;
	}

	public int GetRowIndex(CardContainer rowContainer)
	{
		if (!rowContainer || !rowContainer.owner)
		{
			return -1;
		}

		return rows[rowContainer.owner].IndexOf(rowContainer);
	}

	public int[] GetRowIndices(Entity entity)
	{
		List<int> list = GenericPool<List<int>>.Get();
		list.Clear();
		if ((bool)entity.owner)
		{
			List<CardContainer> list2 = rows[entity.owner];
			for (int i = 0; i < rowCount; i++)
			{
				if (list2[i].Contains(entity))
				{
					list.Add(i);
				}
			}
		}

		int[] result = list.ToArray();
		GenericPool<List<int>>.Release(list);
		return result;
	}

	public int[] GetRowIndices(IEnumerable<CardContainer> containers)
	{
		HashSet<int> hashSet = GenericPool<HashSet<int>>.Get();
		hashSet.Clear();
		foreach (CardContainer container in containers)
		{
			if (rowIndices.TryGetValue(container, out var value))
			{
				hashSet.Add(value);
			}
		}

		int[] result = hashSet.ToArray();
		GenericPool<HashSet<int>>.Release(hashSet);
		return result;
	}

	public CardContainer GetRow(Character owner, int rowIndex)
	{
		return rows[owner][rowIndex];
	}

	public List<CardContainer> GetRows(Character owner)
	{
		List<CardContainer> list = rows[owner];
		List<CardContainer> list2 = new List<CardContainer>();
		foreach (CardContainer item in list)
		{
			list2.Add(item);
		}

		return list2;
	}

	public CardSlotLane GetOppositeRow(CardSlotLane row)
	{
		int rowIndex = GetRowIndex(row);
		if (rowIndex >= 0)
		{
			Character opponent = GetOpponent(row.owner);
			if ((bool)opponent && GetRow(opponent, rowIndex) is CardSlotLane result)
			{
				return result;
			}
		}

		return null;
	}

	public CardContainer[] GetOppositeRows(CardContainer[] rows)
	{
		CardContainer[] array = new CardContainer[rows.Length];
		int num = 0;
		Character character = null;
		foreach (CardContainer cardContainer in rows)
		{
			int rowIndex = GetRowIndex(cardContainer);
			if (rowIndex < 0)
			{
				continue;
			}

			if ((object)character == null)
			{
				character = GetOpponent(cardContainer.owner);
			}

			if ((bool)character)
			{
				CardContainer row = GetRow(character, rowIndex);
				if ((object)row != null)
				{
					array[num++] = row;
				}
			}
		}

		return array;
	}

	public List<CardSlot> GetSlots()
	{
		List<CardSlot> list = new List<CardSlot>();
		foreach (CardContainer item in rows.Values.SelectMany((List<CardContainer> a) => a))
		{
			if (item is CardSlotLane cardSlotLane)
			{
				list.AddRange(cardSlotLane.slots);
			}
		}

		return list;
	}

	public List<CardSlot> GetSlots(Character owner)
	{
		List<CardContainer> list = rows[owner];
		List<CardSlot> list2 = new List<CardSlot>();
		foreach (CardContainer item in list)
		{
			if (item is CardSlotLane cardSlotLane)
			{
				list2.AddRange(cardSlotLane.slots);
			}
		}

		return list2;
	}

	public static List<Entity> GetCards(Character character)
	{
		List<Entity> cardsOnBoard = GetCardsOnBoard(character);
		List<CardContainer> list = new List<CardContainer>();
		list.AddIfNotNull(character.reserveContainer);
		list.AddIfNotNull(character.handContainer);
		list.AddIfNotNull(character.drawContainer);
		list.AddIfNotNull(character.discardContainer);
		foreach (CardContainer item in list)
		{
			foreach (Entity item2 in item)
			{
				cardsOnBoard.Add(item2);
			}
		}

		return cardsOnBoard;
	}

	public static List<Entity> GetAllCards()
	{
		List<Entity> list = new List<Entity>();
		list.AddRange(GetCards(instance.player));
		list.AddRange(GetCards(instance.enemy));
		return list;
	}

	public static List<Entity> GetCardsOnBoard()
	{
		List<Entity> list = new List<Entity>();
		list.AddRange(GetCardsOnBoard(instance.player));
		list.AddRange(GetCardsOnBoard(instance.enemy));
		return list;
	}

	public static List<Entity> GetCardsOnBoard(Character character)
	{
		List<Entity> list = new List<Entity>();
		int num = 2;
		int num2 = 3;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (instance.GetRow(character, j) is CardSlotLane cardSlotLane)
				{
					Entity top = cardSlotLane.slots[i].GetTop();
					if ((object)top != null && !list.Contains(top))
					{
						list.Add(top);
					}
				}
			}
		}

		return list;
	}

	public static bool IsOnBoard(Entity entity)
	{
		if (entity.containers.Length != 0)
		{
			return IsOnBoard(entity.containers);
		}

		if (!entity.alive && entity.preContainers != null)
		{
			return IsOnBoard(entity.preContainers);
		}

		return false;
	}

	public static bool IsOnBoard(CardContainer[] containers)
	{
		foreach (CardContainer cardContainer in containers)
		{
			if (instance.GetRowIndex(cardContainer.Group) >= 0)
			{
				return true;
			}
		}

		return false;
	}

	public static bool IsOnBoard(CardContainer container)
	{
		return instance.GetRowIndex(container) >= 0;
	}

	public override IEnumerator Run()
	{
		CampaignNode node = Campaign.FindCharacterNode(player);
		NavigationState.Start(new NavigationStateBattle());
		NavigationState.Start(new NavigationStateWait());
		if (!loadMidBattle)
		{
			phase = Phase.Init;
			yield return Sequences.Wait(startDelay);
			yield return ActionQueue.Wait();
			Debug.Log("BATTLE START!");
			Events.InvokeBattleStart();
			yield return DrawChampions(player, player.drawContainer, player.handContainer);
			SetSeed(node.seed - 9999, 0);
			NavigationState.BackToPreviousState();
			yield return WaitForChampionsToDeploy(player, playerCardController, player.handContainer);
			NavigationState.Start(new NavigationStateWait());
			Events.InvokeBattleTurnEnd(turnCount);
			ActionQueue.Add(new ActionDraw(player, player.handContainer.max));
			yield return ActionQueue.Wait();
		}

		yield return BattleLoop(node);
		Debug.Log("BATTLE END!");
		Events.InvokeBattleEnd();
		NavigationState.Reset();
		if ((bool)playerCardController)
		{
			playerCardController.Disable();
		}

		List<Entity> cardsOnBoard = GetCardsOnBoard(GetOpponent(winner));
		if (cardsOnBoard.Count > 0)
		{
			Debug.Log($"[{cardsOnBoard.Count}] Cards To Flee!");
			foreach (Entity item in cardsOnBoard)
			{
				ActionQueue.Stack(new ActionFlee(item));
			}

			yield return ActionQueue.Wait();
		}

		yield return Events.InvokePreBattleEnd();
		if (Campaign.CheckVictory() || winner != player)
		{
			if (winner != player)
			{
				if (Settings.Load("showJournalNameOnEnd", defaultValue: false))
				{
					yield return JournalVoidNameSequence.LoadAndRun(unloadAfter: true);
				}
				else
				{
					JournalNameHistory.MostRecentNameKilled();
				}
			}

			yield return SceneManager.Load("CampaignEnd", SceneType.Temporary);
			yield return SceneManager.WaitUntilUnloaded("CampaignEnd");
		}
		else if (winner == player)
		{
			Events.InvokeBattleWinPreRewards();
			yield return ActionQueue.Wait();
			if (node.data.ContainsKey("rewards"))
			{
				yield return SceneManager.Load("BossReward", SceneType.Temporary);
				yield return SceneManager.WaitUntilUnloaded("BossReward");
			}

			Events.InvokeBattleWin();
			yield return ActionQueue.Wait();
			yield return SceneManager.Load("BattleWin", SceneType.Temporary);
			BattleVictorySequence battleVictorySequence = Object.FindObjectOfType<BattleVictorySequence>();
			yield return battleVictorySequence.Run();
		}
	}

	public IEnumerator BattleLoop(CampaignNode node)
	{
		while (!ended)
		{
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			yield return UpdateBoard(enemy);
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			yield return UpdateBoard(player);
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			yield return UpdateContainer(player.handContainer);
			if (!ended)
			{
				SetSeed(node.seed, turnCount);
				if (!auto)
				{
					cancelTurn = false;
					phase = Phase.Play;
				}

				yield return ActionQueue.Wait();
				Events.InvokeBattlePreTurnStart(turnCount);
				turnCount++;
				NavigationState.BackToPreviousState();
				if (!auto)
				{
					playerCardController.Enable();
					yield return WaitForTurnEnd(player, playerCardController);
				}

				NavigationState.Start(new NavigationStateWait());
				Events.InvokeBattleTurnStart(turnCount);
				if (Deckpack.IsOpen)
				{
					yield return WaitForDeckpack();
				}

				phase = Phase.Battle;
				if (!auto)
				{
					yield return Sequences.Wait(0.5f);
				}

				yield return CheckUnitsTakeTurns();
				yield return ActionQueue.Wait();
				yield return ProcessHandStart(player);
				if (!ended)
				{
					yield return CheckUnitsTakeTurns();
					yield return ProcessUnits(enemy);
					yield return ProcessUnits(player);
					yield return CheckUnitsTakeTurns();
					yield return ProcessUnitTurnEnd();
					yield return CheckUnitsTakeTurns();
					yield return ProcessHandEnd(player);
					yield return CheckUnitsTakeTurns();
				}

				yield return CheckUnitsTakeTurns();
				Events.InvokeBattleTurnEnd(turnCount);
			}
		}
	}

	public static void SetSeed(int baseSeed, int offset)
	{
		Debug.Log($"Battle Setting Seed: {baseSeed} Offset: {offset}");
		Random.InitState(baseSeed);
		for (int i = 0; i < offset; i++)
		{
			Random.Range(0f, 1f);
		}
	}

	public static IEnumerator WaitForDeckpack()
	{
		yield return new WaitUntil(() => !Deckpack.IsOpen);
		yield return new WaitForSeconds(0.5f);
	}

	public IEnumerator WaitForTurnEnd(Character character, CardController cardController)
	{
		while (!ended && !auto)
		{
			yield return ActionQueue.Wait();
			if (character.endTurn)
			{
				if (!character.freeAction)
				{
					phase = Phase.Battle;
				}

				yield return CheckUnitsTakeTurns(enemy);
				yield return CheckUnitsTakeTurns(player);
				yield return UpdateBoard(enemy);
				yield return UpdateBoard(player);
				Events.InvokeCharacterActionPerformed(character);
				if (!character.freeAction)
				{
					character.endTurn = false;
					break;
				}

				character.freeAction = false;
				character.endTurn = false;
				cardController.Enable();
				character.handContainer.TweenChildPositions();
				phase = Phase.Play;
			}

			yield return ActionQueue.Wait();
		}

		cardController.Disable();
	}

	public IEnumerator DrawChampions(Character character, CardContainer fromContainer, CardContainer toContainer)
	{
		float pauseBetween = 0.1f;
		int count = fromContainer.Count;
		for (int i = count - 1; i >= 0; i--)
		{
			Entity entity = fromContainer[i];
			if (entity.data.HasCrown)
			{
				if ((bool)entity)
				{
					StartCoroutine(Sequences.CardMove(entity, new CardContainer[1] { toContainer }));
					toContainer.TweenChildPositions();
				}

				yield return Sequences.Wait(pauseBetween);
			}
		}

		ActionQueue.Stack(new ActionRevealAll(toContainer));
		yield return ActionQueue.Wait();
	}

	public IEnumerator WaitForChampionsToDeploy(Character character, CardController cardController, CardContainer handContainer)
	{
		if (CrownCardsInContainer(handContainer) <= 0)
		{
			yield break;
		}

		cardController.Enable();
		while (!ended)
		{
			if (character.endTurn)
			{
				yield return null;
				character.endTurn = false;
				Routine.Clump clump = new Routine.Clump();
				clump.Add(UpdateBoard(enemy));
				clump.Add(UpdateBoard(player));
				clump.Add(UpdateContainer(player.handContainer));
				yield return clump.WaitForEnd();
				if (CrownCardsInContainer(handContainer) <= 0)
				{
					break;
				}

				cardController.Enable();
			}

			yield return null;
		}

		character.freeAction = false;
	}

	public static int CrownCardsInContainer(CardContainer container)
	{
		return container.Count((Entity a) => a.data.HasCrown);
	}

	public IEnumerator UpdateBoard(Character character)
	{
		float seconds = 0f;
		foreach (CardContainer row in GetRows(character))
		{
			row.MoveChildrenForward();
			row.TweenChildPositions();
		}

		yield return Sequences.Wait(seconds);
	}

	public bool CanDeploy(Entity entity, int targetRow, out int targetColumn)
	{
		targetColumn = 0;
		bool result = false;
		List<CardContainer> list = GetRows(entity.owner);
		int num = int.MaxValue;
		foreach (CardContainer item in list)
		{
			num = Mathf.Min(num, item.max);
		}

		if (entity.positionPriority >= 0)
		{
			for (int i = 0; i < num; i++)
			{
				bool flag = false;
				for (int j = 0; j < entity.height; j++)
				{
					CardContainer cardContainer = list[(targetRow + j) % list.Count];
					if (!(cardContainer is CardSlotLane cardSlotLane) || !cardContainer.canBePlacedOn)
					{
						continue;
					}

					CardSlot cardSlot = cardSlotLane.slots[i];
					if (!cardSlot.canBePlacedOn)
					{
						continue;
					}

					Entity top = cardSlot.GetTop();
					if (top == null)
					{
						flag = true;
					}
					else
					{
						if ((top.positionPriority >= entity.positionPriority && (entity.positionPriority <= 1 || top.positionPriority > entity.positionPriority)) || !CanPushBack(top))
						{
							continue;
						}

						bool flag2 = true;
						for (int k = i + 1; k < cardSlotLane.max; k++)
						{
							Entity top2 = cardSlotLane.slots[k].GetTop();
							if ((bool)top2 && top2.positionPriority >= entity.positionPriority)
							{
								flag2 = false;
								break;
							}
						}

						if (flag2)
						{
							flag = true;
						}
					}
				}

				if (flag)
				{
					result = true;
					targetColumn = i;
					break;
				}
			}
		}
		else
		{
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				bool flag3 = true;
				for (int l = 0; l < entity.height; l++)
				{
					if (list[(targetRow + l) % list.Count] is CardSlotLane cardSlotLane2)
					{
						Entity top3 = cardSlotLane2.slots[num2].GetTop();
						if ((bool)top3 && (top3.positionPriority > entity.positionPriority || !CanPushForwards(top3)))
						{
							flag3 = false;
						}
					}
				}

				if (flag3)
				{
					result = true;
					targetColumn = num2;
					break;
				}
			}
		}

		return result;
	}

	public static bool CanPushBack(Entity entity)
	{
		bool result = true;
		CardContainer[] containers = entity.containers;
		for (int i = 0; i < containers.Length; i++)
		{
			if (containers[i] is CardSlotLane cardSlotLane)
			{
				int num = cardSlotLane.IndexOf(entity) + 1;
				CardSlot cardSlot = ((num < cardSlotLane.max) ? cardSlotLane.slots[num] : null);
				if (cardSlot == null)
				{
					result = false;
					break;
				}

				Entity top = cardSlot.GetTop();
				if ((bool)top && !CanPushBack(top))
				{
					result = false;
					break;
				}
			}
		}

		return result;
	}

	public static bool CanPushForwards(Entity entity)
	{
		bool result = true;
		CardContainer[] containers = entity.containers;
		for (int i = 0; i < containers.Length; i++)
		{
			if (containers[i] is CardSlotLane cardSlotLane)
			{
				int num = cardSlotLane.IndexOf(entity) - 1;
				CardSlot cardSlot = ((num >= 0) ? cardSlotLane.slots[num] : null);
				if (cardSlot == null || !cardSlot.Empty)
				{
					result = false;
					break;
				}
			}
		}

		return result;
	}

	public IEnumerator UpdateContainer(CardContainer container)
	{
		float seconds = 0f;
		container.TweenChildPositions();
		yield return Sequences.Wait(seconds);
	}

	public IEnumerator CheckUnitsTakeTurns()
	{
		if (!cancelTurn)
		{
			yield return CheckUnitsTakeTurns(enemy);
			if (!cancelTurn)
			{
				yield return CheckUnitsTakeTurns(player);
			}
		}
	}

	public IEnumerator CheckUnitsTakeTurns(Character character)
	{
		float pauseAfter = 0.167f;
		List<CardContainer> list = GetRows(character);
		List<Entity> list2 = new List<Entity>();
		foreach (CardContainer item in list)
		{
			for (int i = 0; i < item.Count; i++)
			{
				list2.Add(item[i]);
			}
		}

		foreach (Entity unit in list2)
		{
			if (cancelTurn)
			{
				break;
			}

			if (unit.counter.current <= 0 && unit.counter.max > 0 && !unit.IsSnowed && unit.alive && unit.owner.autoTriggerUnits)
			{
				ActionTriggerByCounter triggerAction = new ActionTriggerByCounter(unit, unit);
				if (Deckpack.IsOpen)
				{
					yield return WaitForDeckpack();
				}

				if (Events.CheckAction(triggerAction))
				{
					ActionQueue.Add(triggerAction);
					yield return ActionQueue.Wait();
					yield return Sequences.Wait(pauseAfter);
				}

				unit.counter.current = unit.counter.max;
				unit.PromptUpdate();
			}
		}
	}

	public IEnumerator ProcessUnits(Character character)
	{
		List<Entity> processed = new List<Entity>();
		Events.InvokePreProcessUnits(character);
		bool dirty;
		do
		{
			dirty = false;
			List<Entity> list = GetAllUnits(character).ToList();
			list.RemoveMany(processed);
			Dictionary<Entity, CardContainer[]> positions = list.ToDictionary((Entity e) => e, (Entity e) => e.actualContainers.ToArray());
			foreach (Entity entity in list)
			{
				if (!entity.IsAliveAndExists())
				{
					dirty = true;
					Debug.Log("BATTLE PROCESS LIST DIRTIED! An entity in the list no longer exists");
					break;
				}

				CardContainer[] array = entity.actualContainers.ToArray();
				if (!positions.ContainsKey(entity) || !positions[entity].ContainsAll(array))
				{
					dirty = true;
					Debug.Log($"BATTLE PROCESS LIST DIRTIED! [{entity.name}] was expected at [{positions[entity]}], but was actually at [{array}]");
					break;
				}

				if (minibosses.Count((Entity a) => a.owner.team == entity.owner.team) <= 0)
				{
					Debug.Log(entity.name + "'s Leader No Longer Exists! Skipping Processing...");
					continue;
				}

				yield return ProcessUnit(entity);
				processed.Add(entity);
				if (cancelTurn)
				{
					break;
				}
			}
		}
		while (dirty && !cancelTurn);
		Events.InvokePostProcessUnits(character);
	}

	public static HashSet<Entity> GetAllUnits(Character character)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		hashSet.AddRange(GetCardsOnBoard(character));
		if ((bool)character.entity && character.entity.alive && character.entity.canBeHit)
		{
			hashSet.Add(character.entity);
		}

		return hashSet;
	}

	public static HashSet<Entity> GetAllUnits()
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		Character[] array = new Character[2] { instance.enemy, instance.player };
		foreach (Character character in array)
		{
			hashSet.AddRange(GetAllUnits(character));
		}

		return hashSet;
	}

	public IEnumerator ProcessUnit(Entity unit)
	{
		float pauseAfter = 0.133f;
		if (Deckpack.IsOpen)
		{
			yield return WaitForDeckpack();
		}

		bool snowed = unit.IsSnowed;
		yield return StatusEffectSystem.TurnStartEvent(unit);
		if ((unit.counter.max > 0 || snowed) && unit.alive)
		{
			yield return CardCountDown(unit);
			if (unit.counter.current <= 0 && unit.counter.max > 0 && !snowed && unit.owner.autoTriggerUnits)
			{
				ActionTriggerByCounter action = new ActionTriggerByCounter(unit, unit);
				if (Events.CheckAction(action))
				{
					ActionQueue.Add(action);
					yield return ActionQueue.Wait();
					if (cancelTurn)
					{
						yield break;
					}
				}

				unit.counter.current = unit.counter.max;
				unit.PromptUpdate();
			}

			Routine.Clump clump = new Routine.Clump();
			clump.Add(StatusEffectSystem.TurnEvent(unit));
			clump.Add(Sequences.Wait(pauseAfter));
			yield return clump.WaitForEnd();
		}
		else
		{
			yield return StatusEffectSystem.TurnEvent(unit);
		}

		if (StatusEffectSystem.EventsRunning)
		{
			yield return Sequences.WaitForStatusEffectEvents();
		}
	}

	public IEnumerator ProcessHandStart(Character character)
	{
		if (cancelTurn)
		{
			yield break;
		}

		foreach (Entity entity2 in character.handContainer)
		{
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			yield return StatusEffectSystem.TurnStartEvent(entity2);
			if (cancelTurn)
			{
				break;
			}
		}

		foreach (Entity entity2 in character.handContainer)
		{
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			yield return StatusEffectSystem.TurnEvent(entity2);
			if (cancelTurn)
			{
				break;
			}
		}
	}

	public IEnumerator ProcessHandEnd(Character character)
	{
		if (cancelTurn)
		{
			yield break;
		}

		foreach (Entity entity in character.handContainer)
		{
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			yield return StatusEffectSystem.TurnEndEvent(entity);
			if (cancelTurn)
			{
				break;
			}
		}
	}

	public IEnumerator ProcessUnitTurnEnd()
	{
		if (cancelTurn)
		{
			yield break;
		}

		HashSet<Entity> hashSet = new HashSet<Entity>();
		Character[] array = new Character[2] { enemy, player };
		foreach (Character character in array)
		{
			hashSet.AddRange(GetCardsOnBoard(character));
			if ((bool)character.entity && character.entity.alive)
			{
				hashSet.Add(character.entity);
			}
		}

		foreach (Entity unit in hashSet)
		{
			if (Deckpack.IsOpen)
			{
				yield return WaitForDeckpack();
			}

			ActionQueue.Stack(new ActionSequence(StatusEffectSystem.TurnEndEvent(unit)), fixedPosition: true);
			yield return ActionQueue.Wait();
			if (cancelTurn)
			{
				break;
			}
		}
	}

	public static IEnumerator CardCountDown(Entity entity)
	{
		if (Deckpack.IsOpen)
		{
			yield return WaitForDeckpack();
		}

		int amount = 1;
		Events.InvokeEntityCountDown(entity, ref amount);
		Hit hit = new Hit(null, entity)
		{
			counterReduction = amount,
			screenShake = 0f
		};
		yield return hit.Process();
	}
}
