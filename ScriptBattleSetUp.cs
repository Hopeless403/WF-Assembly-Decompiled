#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleSetUp", menuName = "Scripts/Battle Set Up")]
public class ScriptBattleSetUp : Script
{
	public override IEnumerator Run()
	{
		Character player = References.Player;
		Character enemy = References.Battle.enemy;
		CampaignNode node = Campaign.FindCharacterNode(player);
		Events.InvokePreBattleSetUp(node);
		SetUpEnemyWaves(enemy, node);
		Random.InitState(node.seed);
		yield return CreateCards(player, enemy);
		Events.InvokePostBattleSetUp(node);
	}

	public static void SetUpEnemyWaves(Character enemy, CampaignNode node)
	{
		BattleWaveManager component = enemy.GetComponent<BattleWaveManager>();
		component.list = new List<BattleWaveManager.Wave>();
		BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
		for (int i = 0; i < saveCollection.Length; i++)
		{
			BattleWaveManager.Wave item = new BattleWaveManager.Wave(saveCollection[i]);
			component.list.Add(item);
		}
	}

	public static IEnumerator CreateCards(Character player, Character enemy)
	{
		CardController cardController = Object.FindObjectOfType<CardController>();
		Routine.Clump clump = new Routine.Clump();
		List<Entity> enemyCards = new List<Entity>();
		List<Entity> playerCards = new List<Entity>();
		clump.Add(CreateEnemyCards(enemy, cardController, enemyCards));
		clump.Add(CreatePlayerCards(player, cardController, playerCards));
		yield return clump.WaitForEnd();
		Vector3 localScale = Vector3.one * enemy.reserveContainer.CardScale;
		foreach (Entity item in enemyCards)
		{
			enemy.reserveContainer.Add(item);
			item.transform.localScale = localScale;
		}

		enemy.reserveContainer.SetChildPositions();
		Vector3 localScale2 = Vector3.one * player.drawContainer.CardScale;
		foreach (Entity item2 in playerCards.InRandomOrder())
		{
			player.drawContainer.Add(item2);
			item2.transform.localScale = localScale2;
		}

		player.drawContainer.SetChildPositions();
	}

	public static IEnumerator CreateEnemyCards(Character enemy, CardController cardController, IList<Entity> entities)
	{
		Routine.Clump clump = new Routine.Clump();
		BattleWaveManager waveManager = enemy.GetComponent<BattleWaveManager>();
		int num = waveManager.list.Sum((BattleWaveManager.Wave a) => a.units.Count);
		while (entities.Count < num)
		{
			entities.Add(null);
		}

		int entityIndex = 0;
		for (int waveIndex = 0; waveIndex < waveManager.list.Count; waveIndex++)
		{
			BattleWaveManager.Wave wave = waveManager.list[waveIndex];
			List<Entity> entitiesThisWave = new List<Entity>();
			while (entitiesThisWave.Count < wave.units.Count)
			{
				entitiesThisWave.Add(null);
			}

			for (int i = 0; i < wave.units.Count; i++)
			{
				Card card = CardManager.Get(wave.units[i], cardController, enemy, inPlay: true, isPlayerCard: false);
				entitiesThisWave[i] = card.entity;
				card.entity.flipper.FlipDownInstant();
				entities[entityIndex++] = card.entity;
				clump.Add(card.UpdateData());
			}

			yield return clump.WaitForEnd();
			waveManager.AddEntities(entitiesThisWave.ToArray());
			Debug.Log($"{wave} Created");
		}
	}

	public static IEnumerator CreatePlayerCards(Character player, CardController cardController, IList<Entity> entities)
	{
		Routine.Clump clump = new Routine.Clump();
		int count = player.data.inventory.deck.Count;
		while (entities.Count < count)
		{
			entities.Add(null);
		}

		for (int i = 0; i < count; i++)
		{
			Card card = CardManager.Get(player.data.inventory.deck[i], cardController, player, inPlay: true, isPlayerCard: true);
			entities[i] = card.entity;
			card.entity.flipper.FlipDownInstant();
			clump.Add(card.UpdateData());
		}

		yield return clump.WaitForEnd();
	}
}
