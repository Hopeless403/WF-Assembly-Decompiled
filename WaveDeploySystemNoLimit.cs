#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveDeploySystemNoLimit : GameSystem
{
	[SerializeField]
	public GameObject blocker;

	[SerializeField]
	public GameObject icon;

	[SerializeField]
	public CanvasGroup group;

	[SerializeField]
	public TMP_Text numberElement;

	[Header("Settings")]
	[SerializeField]
	public int counterStart = -1;

	[SerializeField]
	public int counterMax = 3;

	[SerializeField]
	public int popUpCounterAt = 5;

	[SerializeField]
	public int bigIconAt = 3;

	[SerializeField]
	public Vector3 smallIconPos = new Vector3(0f, 3.4f, 0f);

	[SerializeField]
	public Vector3 smallIconScale = new Vector3(0.5f, 0.5f, 1f);

	[SerializeField]
	public float pauseAfterDeploy = 0.2f;

	[SerializeField]
	public bool countWhenXEnemies;

	[SerializeField]
	[ShowIf("countWhenXEnemies")]
	public int countWhenEnemiesRemaining = 1;

	[SerializeField]
	public bool needSpace = true;

	public bool inBattle;

	public int currentWave;

	public BattleWaveManager waveManager;

	public int counter;

	public bool reset;

	public bool deploySuccessful;

	public List<Entity> deployed;

	public void OnEnable()
	{
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnSceneChanged -= SceneChanged;
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (inBattle)
		{
			if (phase == Battle.Phase.Play && References.Battle.turnCount > 0)
			{
				ActionQueue.Add(new ActionSequence(CountDown())
				{
					parallel = true
				});
			}
		}
		else if (phase == Battle.Phase.Init)
		{
			BattleStart();
		}
	}

	public void SceneChanged(Scene scene)
	{
		if (inBattle)
		{
			inBattle = false;
		}
	}

	public void BattleStart()
	{
		inBattle = true;
		currentWave = 0;
		waveManager = References.Battle.enemy.GetComponent<BattleWaveManager>();
		counter = counterStart;
		reset = false;
		ActionQueue.Add(new ActionSequence(CountDown())
		{
			parallel = true
		});
	}

	public IEnumerator CountDown()
	{
		int enemyCount = GetEnemyCount();
		if ((countWhenXEnemies && enemyCount > countWhenEnemiesRemaining) || currentWave >= waveManager.list.Count)
		{
			yield break;
		}

		int num = References.Battle.GetRows(References.Battle.enemy).Cast<CardSlotLane>().SelectMany((CardSlotLane a) => a.slots)
			.Count((CardSlot a) => a.Empty);
		int count = waveManager.list[currentWave].units.Count;
		if (num >= count)
		{
			if (enemyCount <= 0)
			{
				counter = 0;
			}

			counter--;
			if (counter <= 0)
			{
				yield return Activate();
			}
			else if (counter <= popUpCounterAt)
			{
				new Routine(Pop());
			}
		}
	}

	public IEnumerator Pop()
	{
		numberElement.text = counter.ToString();
		group.gameObject.SetActive(value: true);
		blocker.SetActive(value: true);
		group.alpha = 0f;
		icon.transform.localPosition = ((counter <= bigIconAt) ? Vector3.zero : smallIconPos);
		Vector3 vector = ((counter <= bigIconAt) ? Vector3.one : smallIconScale);
		icon.transform.localScale = vector * 1.5f;
		LeanTween.scale(icon, vector, 1.5f).setEaseOutElastic();
		LeanTween.value(base.gameObject, 0f, 1f, 0.25f).setEaseLinear().setOnUpdate(delegate(float a)
		{
			group.alpha = a;
		});
		blocker.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		LeanTween.value(base.gameObject, 1f, 0f, 0.15f).setEaseLinear().setOnUpdate(delegate(float a)
		{
			group.alpha = a;
		})
			.setOnComplete((Action)delegate
			{
				group.gameObject.SetActive(value: false);
			});
	}

	public IEnumerator Activate()
	{
		int thisWaveIndex = currentWave;
		int targetRow = UnityEngine.Random.Range(0, Battle.instance.rowCount);
		yield return Sequences.Wait(0.2f);
		yield return TryDeploy(targetRow);
		if (deploySuccessful)
		{
			yield return RevealBoard();
			if (++currentWave < waveManager.list.Count)
			{
				counter = counterMax + 1;
			}
		}
		else
		{
			yield return RevealBoard();
			counter = 1;
		}

		waveManager.list[thisWaveIndex].spawned = true;
	}

	public IEnumerator TryDeploy(int rowIndex)
	{
		if (deployed == null)
		{
			deployed = new List<Entity>();
		}

		deployed.Clear();
		deploySuccessful = true;
		Entity[] array = waveManager.Peek();
		Entity[] array2 = array;
		foreach (Entity e in array2)
		{
			if (!(e != null) || !e.containers.Contains(e.owner.reserveContainer))
			{
				continue;
			}

			for (int j = 0; j < References.Battle.rowCount; j++)
			{
				rowIndex = (rowIndex + 1) % References.Battle.rowCount;
				if (References.Battle.CanDeploy(e, rowIndex, out var targetColumn))
				{
					Deploy(e, rowIndex, targetColumn);
					deployed.Add(e);
					yield return ActionQueue.Wait(includeParallel: false);
					break;
				}
			}

			if (!deployed.Contains(e))
			{
				deploySuccessful = false;
			}
		}

		if (deploySuccessful)
		{
			waveManager.Pull();
		}
	}

	public void Deploy(Entity entity, int targetRow, int targetColumn)
	{
		List<CardContainer> rows = Battle.instance.GetRows(entity.owner);
		List<CardContainer> list = new List<CardContainer>();
		for (int i = 0; i < entity.height; i++)
		{
			CardContainer item = rows[(targetRow + i) % rows.Count];
			list.Add(item);
		}

		entity.flipper.FlipUpInstant();
		ActionQueue.Add(new ActionMove(entity, list.ToArray())
		{
			insertPos = targetColumn,
			pauseAfter = pauseAfterDeploy
		});
	}

	public static IEnumerator RevealBoard()
	{
		ActionQueue.Add(new ActionRevealAll(References.Battle.GetRows(References.Battle.enemy).ToArray()));
		yield return ActionQueue.Wait(includeParallel: false);
	}

	public static int GetEnemyCount()
	{
		return Battle.GetCardsOnBoard(References.Battle.enemy).Count((Entity a) => !a.data.cardType.miniboss);
	}
}
