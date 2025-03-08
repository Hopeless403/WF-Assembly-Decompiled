#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BattleSaveSystem : GameSystem
{
	public static BattleSaveSystem instance;

	public bool loading;

	public bool justLoaded;

	public BattleSaveData state;

	public bool saveRequired;

	public bool campaignNodeIdSet;

	public int campaignNodeId;

	public void OnEnable()
	{
		instance = this;
		Events.OnBattlePreTurnStart += BattleTurnEnd;
		Events.OnCampaignFinal += CampaignFinal;
	}

	public void OnDisable()
	{
		Events.OnBattlePreTurnStart -= BattleTurnEnd;
		Events.OnCampaignFinal -= CampaignFinal;
	}

	public void OnApplicationQuit()
	{
		CheckSave();
	}

	public static BattleSaveData GetBattleState()
	{
		return instance.state;
	}

	public void BattleTurnEnd(int turnCount)
	{
		if (justLoaded)
		{
			justLoaded = false;
			return;
		}

		BuildBattleState();
		saveRequired = true;
	}

	public void CampaignFinal()
	{
		CheckSave();
	}

	public void CheckSave()
	{
		if (saveRequired && !loading)
		{
			if (Campaign.Data.GameMode.doSave)
			{
				Save();
			}

			Events.InvokeBattleSaved();
			Events.InvokeCampaignSaved();
			saveRequired = false;
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Save()
	{
		if (state == null)
		{
			Debug.LogWarning("Cannot save Battle State right now!");
			return;
		}

		Debug.Log("> Saving Battle State...");
		SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "battleState", state);
	}

	public void BuildBattleState()
	{
		Events.InvokeBattleStateBuild();
		StopWatch.Start();
		if (!campaignNodeIdSet)
		{
			campaignNodeId = Campaign.FindCharacterNode(References.Battle.player).id;
		}

		state = new BattleSaveData
		{
			gold = References.PlayerData.inventory.gold.Value + References.PlayerData.inventory.goldOwed,
			campaignNodeId = campaignNodeId,
			turnCount = References.Battle.turnCount + 1,
			statuses = (from e in StatusEffectSystem.activeEffects
				where (bool)e && e.count > e.temporary && (bool)e.target && e.target.alive
				select new BattleSaveData.Status(e)).ToArray()
		};
		foreach (StatusEffectData activeEffect in StatusEffectSystem.activeEffects)
		{
			object midBattleData = activeEffect.GetMidBattleData();
			if (midBattleData != null)
			{
				string key = $"{activeEffect.target.data.id}{activeEffect.name}";
				state.storeStatusData[key] = midBattleData;
			}
		}

		RedrawBellSystem redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>();
		if ((object)redrawBellSystem != null)
		{
			state.redrawBellCount = redrawBellSystem.counter.current;
		}

		WaveDeploySystemOverflow waveDeploySystemOverflow = Object.FindObjectOfType<WaveDeploySystemOverflow>();
		if ((object)waveDeploySystemOverflow != null)
		{
			state.enemyWaves = waveDeploySystemOverflow.Save();
		}

		BattleMusicSystem battleMusicSystem = Object.FindObjectOfType<BattleMusicSystem>();
		if ((object)battleMusicSystem != null)
		{
			state.battleMusicState = battleMusicSystem.Save();
		}

		state.playerRows = new BattleSaveData.ContainerGroup(References.Battle.GetRows(References.Battle.player));
		state.enemyRows = new BattleSaveData.ContainerGroup(References.Battle.GetRows(References.Battle.enemy));
		state.playerHand = new BattleSaveData.Container(References.Battle.player.handContainer);
		state.playerDraw = new BattleSaveData.Container(References.Battle.player.drawContainer);
		state.playerDiscard = new BattleSaveData.Container(References.Battle.player.discardContainer);
		state.playerReserve = new BattleSaveData.Container(References.Battle.player.reserveContainer);
		state.enemyReserve = new BattleSaveData.Container(References.Battle.enemy.reserveContainer);
		Debug.Log($"> Battle State Built ({StopWatch.Stop()}ms)");
		Events.InvokeBattleStateBuilt(state);
	}

	public bool TryLoadBattleState(CampaignNode campaignNode)
	{
		state = SaveSystem.LoadCampaignData<BattleSaveData>(Campaign.Data.GameMode, "battleState", null);
		if (state == null)
		{
			return false;
		}

		if (state.campaignNodeId != campaignNode.id)
		{
			state = null;
			return false;
		}

		return true;
	}

	public bool TryLoadBattleState(BattleSaveData state)
	{
		this.state = state;
		return state != null;
	}

	public IEnumerator LoadRoutine()
	{
		loading = true;
		References.Battle.loadMidBattle = true;
		References.Battle.turnCount = state.turnCount;
		References.PlayerData.inventory.gold.Value = state.gold;
		References.Player.entity.PromptUpdate();
		Object.FindObjectOfType<BattleMusicSystem>()?.Load(state.battleMusicState);
		Routine.Clump clump = new Routine.Clump();
		CardSlotLane[] rows = References.Battle.GetRows(References.Battle.player).Cast<CardSlotLane>().ToArray();
		clump.Add(CreateCardsInRows(rows, state.playerRows.containers));
		CardSlotLane[] rows2 = References.Battle.GetRows(References.Battle.enemy).Cast<CardSlotLane>().ToArray();
		clump.Add(CreateCardsInRows(rows2, state.enemyRows.containers));
		clump.Add(CreateCards(References.Battle.player.handContainer, state.playerHand.cards));
		clump.Add(CreateCards(References.Battle.player.drawContainer, state.playerDraw.cards));
		clump.Add(CreateCards(References.Battle.player.discardContainer, state.playerDiscard.cards));
		clump.Add(CreateCards(References.Battle.player.reserveContainer, state.playerReserve.cards));
		clump.Add(CreateCards(References.Battle.enemy.reserveContainer, state.enemyReserve.cards));
		yield return clump.WaitForEnd();
		BattleSaveData.Status[] statuses = state.statuses;
		foreach (BattleSaveData.Status status in statuses)
		{
			Entity entity = References.Battle.cards.FirstOrDefault((Entity a) => a.data.id == status.targetId);
			if (!entity)
			{
				Debug.LogError($"No Entity[{status.targetId}] found for Status ({status.name} {status.count})");
				continue;
			}

			entity.display.promptUpdateDescription = true;
			Entity applier = References.Battle.cards.FirstOrDefault((Entity a) => a.data.id == status.applierId);
			StatusEffectData statusEffectData = AddressableLoader.Get<StatusEffectData>("StatusEffectData", status.name);
			if ((bool)statusEffectData)
			{
				yield return StatusEffectSystem.Apply(entity, applier, statusEffectData, status.count, temporary: false, null, fireEvents: false);
			}
			else
			{
				Debug.LogError($"Effect [{status.name}] not found for target ({entity.data.name} {status.targetId})");
			}
		}

		foreach (Entity card in References.Battle.cards)
		{
			yield return UpdateCard(card, state.storeStatusData);
		}

		Object.FindObjectOfType<WaveDeploySystemOverflow>()?.Load(state.enemyWaves, (IReadOnlyCollection<CardData>)(object)References.Battle.cards.Select((Entity a) => a.data).ToArray());
		RedrawBellSystem redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>();
		if ((object)redrawBellSystem != null)
		{
			redrawBellSystem.BecomeInteractable();
			redrawBellSystem.SetCounter(state.redrawBellCount);
		}

		Events.InvokeBattleLoaded();
		justLoaded = true;
		loading = false;
	}

	public static IEnumerator CreateCardsInRows(IReadOnlyList<CardSlotLane> rows, IReadOnlyList<BattleSaveData.Container> rowSaveDatas)
	{
		Dictionary<ulong, Entity> entities = new Dictionary<ulong, Entity>();
		CardController cardController = References.Battle.playerCardController;
		Routine.Clump clump = new Routine.Clump();
		for (int rowI = 0; rowI < rows.Count; rowI++)
		{
			CardSlotLane row = rows[rowI];
			BattleEntityData[] cards = rowSaveDatas[rowI].cards;
			for (int i = 0; i < cards.Length; i++)
			{
				BattleEntityData d = cards[i];
				if (d == null)
				{
					continue;
				}

				if (!entities.ContainsKey(d.cardSaveData.id))
				{
					entities.Add(d.cardSaveData.id, null);
					int entityIndex = i;
					clump.Add(CreateCard(d, cardController, row.owner, delegate(Card a)
					{
						entities[d.cardSaveData.id] = a.entity;
						row.slots[entityIndex].Add(a.entity);
					}));
				}
				else
				{
					row.slots[i].Add(entities[d.cardSaveData.id]);
				}
			}

			yield return clump.WaitForEnd();
		}

		foreach (Entity value in entities.Values)
		{
			value.containers[0].TweenChildPosition(value);
		}
	}

	public static IEnumerator CreateCards(CardContainer container, IReadOnlyList<BattleEntityData> entitySaveData)
	{
		Character owner = container.owner;
		CardController playerCardController = References.Battle.playerCardController;
		Routine.Clump clump = new Routine.Clump();
		Entity[] entities = new Entity[entitySaveData.Count];
		for (int i = 0; i < entitySaveData.Count; i++)
		{
			BattleEntityData battleEntityData = entitySaveData[i];
			if (battleEntityData != null)
			{
				int arrayIndex = i;
				clump.Add(CreateCard(battleEntityData, playerCardController, owner, delegate(Card a)
				{
					entities[arrayIndex] = a.entity;
				}));
			}
		}

		yield return clump.WaitForEnd();
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			container.Add(entity);
		}

		container.SetChildPositions();
	}

	public static IEnumerator CreateCard(BattleEntityData entityData, CardController cardController, Character owner, UnityAction<Card> onComplete)
	{
		Card card = CardManager.Get(entityData.cardSaveData.Load(), cardController, owner, inPlay: true, owner.team == References.Player.team);
		Entity entity = card.entity;
		entity.startingEffectsApplied = true;
		entity.alive = false;
		if (entityData.flipped)
		{
			entity.flipper.FlipDownInstant();
		}

		entity.attackEffects = entityData.attackEffects.Select((StatusEffectSaveData e) => e.Load()).ToList();
		entity.traits.Clear();
		foreach (CardData.TraitStacks trait in entity.data.traits)
		{
			if ((bool)trait.data)
			{
				entity.traits.Add(new Entity.TraitStacks(trait.data, trait.count));
			}
		}

		onComplete?.Invoke(card);
		if (!entityData.flipped)
		{
			entity.enabled = true;
		}

		yield return card.UpdateData();
		entity.height = entityData.height;
		entity.damage.current = entityData.damage;
		entity.damage.max = entityData.damageMax;
		entity.hp.current = entityData.hp;
		entity.hp.max = entityData.hpMax;
		entity.counter.current = entityData.counter;
		entity.counter.max = entityData.counterMax;
		entity.uses.current = entityData.uses;
		entity.uses.max = entityData.usesMax;
	}

	public static IEnumerator UpdateCard(Entity card, IReadOnlyDictionary<string, object> customDatas)
	{
		card.alive = true;
		yield return card.UpdateTraits();
		if (customDatas != null)
		{
			foreach (StatusEffectData statusEffect in card.statusEffects)
			{
				if (customDatas.TryGetValue($"{card.data.id}{statusEffect.name}", out var value))
				{
					statusEffect.RestoreMidBattleData(value);
				}
			}
		}

		if (card.enabled)
		{
			Events.InvokeEntityEnabled(card);
			yield return StatusEffectSystem.EntityEnableEvent(card);
		}
		else
		{
			CoroutineManager.Start(card.display.UpdateDisplay());
		}

		card.PromptUpdate();
	}
}
