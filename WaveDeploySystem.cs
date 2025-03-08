#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveDeploySystem : GameSystem
{
	public bool visible = true;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public WaveDeployerDots dotManager;

	[SerializeField]
	public TMP_Text counterText;

	[SerializeField]
	public GameObject group;

	[SerializeField]
	public UINavigationItem navigationItem;

	[Header("Settings")]
	[SerializeField]
	public int counterStart = -1;

	[SerializeField]
	public bool recallWhenUnsuccessful;

	[SerializeField]
	public int damageToOpponent;

	[SerializeField]
	public int damageIncreasePerTurn;

	[SerializeField]
	public float pauseAfterDeploy = 0.2f;

	public bool inBattle;

	public int damageToOpponentCurrent;

	public int currentWave;

	public BattleWaveManager waveManager;

	public int counter;

	public int counterMax;

	public bool reset;

	public bool deploySuccessful;

	public List<Entity> deployed;

	public static UINavigationItem nav;

	public void Awake()
	{
		group.SetActive(value: false);
		nav = navigationItem;
	}

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
			Hide();
			inBattle = false;
		}
	}

	public void BattleStart()
	{
		inBattle = true;
		damageToOpponentCurrent = damageToOpponent;
		currentWave = 0;
		waveManager = References.Battle.enemy.GetComponent<BattleWaveManager>();
		counter = counterStart;
		reset = false;
		if (visible)
		{
			Show();
		}

		dotManager.Init(waveManager, currentWave);
		ActionQueue.Add(new ActionSequence(CountDown())
		{
			parallel = true
		});
	}

	public void Close()
	{
		animator.SetBool("Close", value: true);
	}

	public void Open()
	{
		animator.SetBool("Close", value: false);
	}

	public void Show()
	{
		visible = true;
		group.SetActive(value: true);
		animator.SetTrigger("Reveal");
		SfxSystem.OneShot("event:/sfx/inventory/wave_counter_showup");
	}

	public void Hide()
	{
		group.SetActive(value: false);
	}

	public IEnumerator CountDown()
	{
		if (currentWave >= waveManager.list.Count)
		{
			yield break;
		}

		bool earlyDeploy = CheckEarlyDeploy();
		if (visible)
		{
			Close();
			if (counter <= 1 || earlyDeploy)
			{
				SfxSystem.OneShot("event:/sfx/inventory/wave_counter_decrease_last");
			}
			else
			{
				SfxSystem.OneShot("event:/sfx/inventory/wave_counter_decrease");
			}

			yield return Sequences.Wait(0.1f);
		}

		if (earlyDeploy)
		{
			counter = 1;
		}

		if (reset)
		{
			reset = false;
		}
		else
		{
			SetCounter(Mathf.Clamp(counter - 1, 0, counterMax));
			if (counter <= 0)
			{
				yield return Activate();
			}
		}

		if (visible && counter > 0)
		{
			Open();
			if (counter == counterMax)
			{
				SfxSystem.OneShot("event:/sfx/inventory/wave_counter_refresh");
			}
		}
	}

	public void SetCounter(int value)
	{
		counter = value;
		counterText.text = value.ToString();
	}

	public void SetCounterMax(int value)
	{
		counterMax = value;
		SetCounter(value);
	}

	public bool CheckEarlyDeploy()
	{
		return Battle.GetCardsOnBoard(References.Battle.enemy).Count <= 0;
	}

	public IEnumerator Activate()
	{
		int thisWaveIndex = currentWave;
		int targetRow = Random.Range(0, Battle.instance.rowCount);
		yield return Sequences.Wait(0.2f);
		yield return TryDeploy(targetRow);
		if (deploySuccessful)
		{
			yield return RevealBoard();
			if (++currentWave < waveManager.list.Count)
			{
				BattleWaveManager.Wave wave = waveManager.list[currentWave];
				SetCounterMax(wave.counter);
			}

			damageToOpponentCurrent = damageToOpponent;
		}
		else
		{
			Routine.Clump clump = new Routine.Clump();
			if (recallWhenUnsuccessful)
			{
				yield return Sequences.Wait(0.33f);
				foreach (Entity item in deployed)
				{
					clump.Add(Sequences.CardMove(item, new CardContainer[1] { References.Battle.enemy.reserveContainer }));
				}

				yield return clump.WaitForEnd();
			}
			else
			{
				yield return RevealBoard();
			}

			if (damageToOpponentCurrent > 0)
			{
				Entity entity = References.Player.entity;
				if (entity != null && entity.canBeHit)
				{
					Hit hit = new Hit(References.Battle.enemy.entity, entity, damageToOpponentCurrent);
					clump.Add(hit.Process());
					clump.Add(Sequences.Wait(0.2f));
				}
			}

			yield return clump.WaitForEnd();
			damageToOpponentCurrent += damageIncreasePerTurn;
			SetCounter(1);
		}

		waveManager.list[thisWaveIndex].spawned = true;
		if (dotManager != null)
		{
			dotManager.UpdateDots(waveManager, thisWaveIndex);
		}
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

	public IEnumerator RevealBoard()
	{
		ActionQueue.Add(new ActionRevealAll(References.Battle.GetRows(References.Battle.enemy).ToArray()));
		yield return ActionQueue.Wait(includeParallel: false);
	}
}
