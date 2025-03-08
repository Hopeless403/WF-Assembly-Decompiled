#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using NaughtyAttributes;
using UnityEngine;

public class LastStandSystem : GameSystem
{
	[SerializeField]
	public StatusEffectData effect;

	[SerializeField]
	public StatusEffectData killEffect;

	[SerializeField]
	public Transform entityGroup;

	[SerializeField]
	public GameObject background;

	[SerializeField]
	public GameObject button;

	[SerializeField]
	public Dice playerDicePrefab;

	[SerializeField]
	public Dice enemyDicePrefab;

	[SerializeField]
	public Dice bossDicePrefab;

	[SerializeField]
	public Transform playerDiceGroup;

	[SerializeField]
	public Transform enemyDiceGroup;

	[SerializeField]
	public CardType[] legalCardTypes;

	[SerializeField]
	[ReadOnly]
	public int lastStandCount;

	public static Battle.Phase previousPhase;

	public static StatusEffectLastStand target;

	public static Entity subject;

	public static Entity attacker;

	public List<Entity> entities;

	public readonly Dictionary<Entity, Transform> previousParents = new Dictionary<Entity, Transform>();

	public readonly List<Dice> dice = new List<Dice>();

	public int result = -1;

	public bool diceRolled;

	public bool _active;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
			background.SetActive(value);
			button.SetActive(value);
		}
	}

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
		Events.OnBattlePhaseStart += BattlePhaseStart;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
		Events.OnBattlePhaseStart -= BattlePhaseStart;
	}

	public void BattleStart()
	{
		Entity entity = References.Battle.cards.FirstOrDefault((Entity a) => a.data.cardType.miniboss && a.owner == References.Player);
		if (entity != null)
		{
			new Routine(StatusEffectSystem.Apply(entity, null, effect, 1));
		}
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (phase == Battle.Phase.LastStand)
		{
			ActionQueue.Stack(new ActionSequence(Process())
			{
				note = "Last Stand"
			}, fixedPosition: true);
		}
	}

	public IEnumerator Process()
	{
		BattleMusicSystem musicSystem = Object.FindObjectOfType<BattleMusicSystem>();
		if (musicSystem != null)
		{
			musicSystem.FadePitchTo(0.3333f);
		}

		entities = Battle.GetCardsOnBoard();
		foreach (Entity entity in entities)
		{
			previousParents[entity] = entity.transform.parent;
			entity.transform.parent = entityGroup;
			entity.silenceCount++;
		}

		active = true;
		yield return new WaitUntil(() => diceRolled);
		yield return RollSequence();
		Clear();
		foreach (KeyValuePair<Entity, Transform> item in previousParents.Where((KeyValuePair<Entity, Transform> pair) => pair.Key != null && pair.Value != null))
		{
			item.Key.transform.parent = item.Value;
		}

		previousParents.Clear();
		active = false;
		if (musicSystem != null)
		{
			musicSystem.FadePitchTo(1f);
		}

		target.preventDeath = false;
		switch (result)
		{
			case 0:
				Debug.Log("Player Wins!");
			if (Battle.IsOnBoard(attacker) && attacker.owner.team != subject.owner.team)
			{
				yield return AttackAndKill(References.Battle.player, attacker);
				}
	
				break;
			case 1:
				Debug.Log("Enemy Wins!");
				yield return AttackAndKill(References.Battle.enemy, subject);
				break;
		}

		result = -1;
		References.Battle.CancelTurn();
		foreach (Entity item2 in entities.Where((Entity e) => e != null))
		{
			item2.silenceCount--;
		}

		entities = null;
		if (target != null)
		{
			target.ReEnable();
		}

		if (!References.Battle.CheckEnd())
		{
			References.Battle.phase = previousPhase;
		}
	}

	public void Roll()
	{
		button.SetActive(value: false);
		diceRolled = true;
	}

	public IEnumerator RollSequence()
	{
		Debug.Log("Last Stand: Rolling Dice...");
		float delayBetween = 1f;
		Entity[] legalEntities = entities.Where((Entity a) => legalCardTypes.Contains(a.data.cardType)).ToArray();
		int count = legalEntities.Count((Entity a) => a.owner == References.Battle.player);
		CreateDice(count, playerDicePrefab, playerDiceGroup);
		int playerTotal = ThrowDice(playerDiceGroup);
		yield return new WaitForSeconds(delayBetween);
		int num = legalEntities.Count((Entity a) => a.owner == References.Battle.enemy && !a.data.cardType.miniboss);
		num += lastStandCount;
		CreateDice(num, enemyDicePrefab, enemyDiceGroup);
		int count2 = legalEntities.Count((Entity a) => a.owner == References.Battle.enemy && a.data.cardType.miniboss);
		CreateDice(count2, bossDicePrefab, enemyDiceGroup);
		int enemyTotal = ThrowDice(enemyDiceGroup);
		yield return new WaitForSeconds(delayBetween);
		do
		{
			result = ((playerTotal <= enemyTotal) ? ((enemyTotal > playerTotal) ? 1 : (-1)) : 0);
			if (result == -1)
			{
				Debug.Log("It's a draw!");
				playerTotal = ThrowDice(playerDiceGroup);
				yield return new WaitForSeconds(delayBetween);
				enemyTotal = ThrowDice(enemyDiceGroup);
				yield return new WaitForSeconds(delayBetween);
			}
		}
		while (result == -1);
		lastStandCount++;
	}

	public void Clear()
	{
		foreach (Dice die in dice)
		{
			die.gameObject.Destroy();
		}

		dice.Clear();
		diceRolled = false;
	}

	public void CreateDice(int count, Dice prefab, Transform group)
	{
		for (int i = 0; i < count; i++)
		{
			Dice dice = Object.Instantiate(prefab, group);
			dice.gameObject.SetActive(value: true);
			this.dice.Add(dice);
		}
	}

	public static int ThrowDice(Transform group)
	{
		int num = 0;
		float num2 = 0.5f;
		Dice[] componentsInChildren = group.GetComponentsInChildren<Dice>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Dice dice = componentsInChildren[i];
			dice.transform.localPosition = new Vector3(num2 * (float)i, 0f);
			dice.Throw(new Vector2(Dead.Random.Range(-1f, 1f), 1f).normalized);
			num += dice.value;
		}

		return num;
	}

	public IEnumerator AttackAndKillAll(Character attackingTeam, Character defendingTeam)
	{
		Entity[] source = entities.Where((Entity a) => legalCardTypes.Contains(a.data.cardType)).ToArray();
		List<Entity> attackers = source.Where((Entity a) => a.owner == attackingTeam).ToList();
		List<Entity> defenders = entities.Where((Entity a) => a.owner == defendingTeam).ToList();
		while (defenders.Count > 0)
		{
			Entity entity = attackers.RandomItem();
			Entity entity2 = defenders.RandomItem();
			defenders.Remove(entity2);
			Hit hit = new Hit(entity, entity2, 99)
			{
				canRetaliate = false,
				canBeNullified = false
			};
			hit.AddStatusEffect(killEffect, 1);
			Trigger trigger = new Trigger(entity, entity, "laststand", new Entity[1] { entity2 });
			trigger.countsAsTrigger = false;
			trigger.hits = new Hit[1] { hit };
			Trigger trigger2 = trigger;
			yield return trigger2.Process();
			yield return new WaitForSeconds(Dead.Random.Range(0f, 0.1f));
		}

		yield return new WaitForSeconds(1f);
	}

	public IEnumerator AttackOnce(Character attackingTeam)
	{
		List<Entity> list = (from a in entities.Where((Entity a) => legalCardTypes.Contains(a.data.cardType)).ToArray()
			where a.owner == attackingTeam
			select a).ToList();
		list.Shuffle();
		foreach (Entity attacker in list)
		{
			if (!attacker.data.hasAttack)
			{
				continue;
			}

			Entity[] array = ((attacker.targetMode != null) ? attacker.targetMode.GetTargets(attacker, null, null) : null);
			if (array != null && array.Length != 0)
			{
				Hit[] hits = array.Select((Entity a) => new Hit(attacker, a)
				{
					canRetaliate = false,
					canBeNullified = false
				}).ToArray();
				Trigger trigger = new Trigger(attacker, attacker, "laststand", array)
				{
					countsAsTrigger = false,
					hits = hits
				};
				yield return trigger.Process();
				yield return new WaitForSeconds(Dead.Random.Range(0.1f, 0.1f));
			}
		}

		yield return new WaitForSeconds(1f);
	}

	public IEnumerator AttackAndKill(Character attackingTeam, Entity target)
	{
		target.alive = false;
		Entity[] source = entities.Where((Entity a) => legalCardTypes.Contains(a.data.cardType)).ToArray();
		List<Entity> attackers = source.Where((Entity a) => a.owner == attackingTeam).ToList();
		attackers.Shuffle();
		int attackersCount = attackers.Count;
		for (int i = 0; i < attackersCount; i++)
		{
			bool num = i == attackersCount - 1;
			Entity attacker = attackers[i];
			Hit hit = new Hit(attacker, target, 0)
			{
				canRetaliate = false,
				canBeNullified = false,
				extraOffensiveness = 5
			};
			hit.FlagAsOffensive();
			if (num)
			{
				target.alive = true;
				hit.AddStatusEffect(killEffect, 1);
				hit.extraOffensiveness = 10;
				yield return new WaitForSeconds(0.25f);
			}

			Trigger trigger = new Trigger(attacker, attacker, "laststand", new Entity[1] { target });
			trigger.countsAsTrigger = false;
			trigger.hits = new Hit[1] { hit };
			Trigger trigger2 = trigger;
			yield return trigger2.Process();
			yield return new WaitForSeconds(Dead.Random.Range(0f, 0.01f));
		}

		yield return new WaitForSeconds(1f);
	}
}
