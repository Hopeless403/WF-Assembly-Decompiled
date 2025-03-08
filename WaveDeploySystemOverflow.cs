#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveDeploySystemOverflow : GameSystem, ISaveable<BattleWaveData>
{
	public bool visible = true;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public TMP_Text counterText;

	[SerializeField]
	public GameObject group;

	public UINavigationItem navigationItem;

	[SerializeField]
	public CanvasGroup canvasGroup;

	[SerializeField]
	public Button button;

	[Header("Glow")]
	[SerializeField]
	public Image glow;

	[SerializeField]
	public Color glowColor = Color.black;

	[SerializeField]
	public Color glowImminentColor = Color.red;

	[Header("Deploy Early")]
	[SerializeField]
	public int deployEarlyReward;

	[SerializeField]
	public int deployEarlyRewardPerTurn = 5;

	[SerializeField]
	public bool autoEarlyDeploy = true;

	[SerializeField]
	public bool canCallEarly;

	[Header("Popup")]
	[SerializeField]
	public UnityEngine.Localization.LocalizedString popupDesc;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString popupHitDesc;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString popupOverflowDesc;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString popupRewardDesc;

	[Header("Settings")]
	[SerializeField]
	public KeywordData popup;

	[SerializeField]
	public int counterStart = -1;

	[SerializeField]
	public float pauseAfterDeploy = 0.2f;

	public bool inBattle;

	public int currentWave;

	public int counter;

	public bool reset;

	public float fade = 1f;

	public float fadeTo;

	public float fadeAdd = 5f;

	public bool popped;

	public List<BattleWaveManager.Wave> waves;

	public List<Entity> deployedThisTurn;

	public List<ulong> deployed;

	public int overflowWaveIndex = -1;

	public void Awake()
	{
		group.SetActive(value: false);
		WaveDeploySystem.nav = navigationItem;
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
		StopAllCoroutines();
	}

	public void Update()
	{
		float num = fadeTo - fade;
		float num2 = fadeAdd * Time.deltaTime;
		if (Mathf.Abs(num) >= num2)
		{
			float num3 = Mathf.Clamp(num, 0f - num2, num2);
			fade += num3;
			if (Mathf.Abs(fadeTo - fade) <= num2)
			{
				fade = fadeTo;
			}

			canvasGroup.alpha = fade;
		}
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (inBattle)
		{
			if (phase != Battle.Phase.Play)
			{
				return;
			}

			if (References.Battle.turnCount == 0)
			{
				if (visible)
				{
					Show();
				}
			}
			else
			{
				ActionQueue.Add(new ActionSequence(CountDown())
				{
					parallel = true
				});
			}
		}
		else
		{
			switch (phase)
			{
				case Battle.Phase.Init:
					BattleStart();
					break;
				case Battle.Phase.Play:
					inBattle = true;
					break;
			}
		}
	}

	public void SceneChanged(Scene scene)
	{
		if (scene.name == "Battle")
		{
			Hide();
		}
		else if (inBattle)
		{
			Hide();
			BattleEnd();
		}
	}

	public void BattleStart()
	{
		inBattle = true;
		currentWave = 0;
		overflowWaveIndex = -1;
		BattleWaveManager component = References.Battle.enemy.GetComponent<BattleWaveManager>();
		waves = new List<BattleWaveManager.Wave>(component.list);
		deployedThisTurn = new List<Entity>();
		deployed = new List<ulong>();
		counter = counterStart;
		reset = false;
		ActionQueue.Add(new ActionSequence(CountDown())
		{
			parallel = true
		});
	}

	public void BattleEnd()
	{
		inBattle = false;
		currentWave = 0;
		overflowWaveIndex = -1;
		waves = null;
		deployedThisTurn = null;
		deployed = null;
		counter = 0;
		reset = false;
	}

	public void AssignCardController(CardController controller)
	{
		GetComponentInChildren<ToggleBasedOnCardController>(includeInactive: true)?.AssignCardController(controller);
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
		if (visible && currentWave < waves.Count)
		{
			group.SetActive(value: true);
			FadeIn();
			SfxSystem.OneShot("event:/sfx/inventory/wave_counter_showup");
			button.interactable = canCallEarly;
		}

		AssignCardController(References.Battle.playerCardController);
	}

	public void Hide()
	{
		group.SetActive(value: false);
		FadeOut();
	}

	public void FadeIn()
	{
		fade = 0f;
		fadeTo = 1f;
		canvasGroup.blocksRaycasts = true;
	}

	public void FadeOut()
	{
		fadeTo = 0f;
		canvasGroup.blocksRaycasts = false;
	}

	public IEnumerator CountDown()
	{
		if (currentWave >= waves.Count)
		{
			yield break;
		}

		Events.InvokeWaveDeployerPreCountDown(counter);
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
			if (!this)
			{
				yield break;
			}
		}

		if (earlyDeploy && (References.Battle.turnCount == 0 || autoEarlyDeploy))
		{
			counter = 1;
		}

		if (reset)
		{
			reset = false;
		}
		else
		{
			SetCounter(Mathf.Max(counter - 1, 0));
			if (counter <= 0)
			{
				yield return Activate();
				if (!this)
				{
					yield break;
				}
			}
		}

		if (visible && counter > 0)
		{
			Open();
			if (counter == waves[currentWave].counter)
			{
				SfxSystem.OneShot("event:/sfx/inventory/wave_counter_refresh");
			}
		}

		Events.InvokeWaveDeployerPostCountDown(counter);
	}

	public void SetCounter(int value)
	{
		counter = value;
		counterText.text = value.ToString();
		if (counter <= 1)
		{
			glow.color = glowImminentColor;
		}
		else
		{
			glow.color = glowColor;
		}
	}

	public static bool CheckEarlyDeploy()
	{
		return Battle.GetCardsOnBoard(References.Battle.enemy).Count <= 0;
	}

	public IEnumerator Activate()
	{
		int targetRow = Random.Range(0, Battle.instance.rowCount);
		FadeOut();
		yield return Sequences.Wait(0.2f);
		if (!this)
		{
			yield break;
		}

		yield return TryDeploy(targetRow);
		if (!this)
		{
			yield break;
		}

		if (deployedThisTurn.Count > 0)
		{
			yield return RevealBoard();
			if (!this)
			{
				yield break;
			}
		}

		if (++currentWave < waves.Count)
		{
			SetCounter(waves[currentWave].counter);
			FadeIn();
		}
		else
		{
			Hide();
		}
	}

	public IEnumerator TryDeploy(int rowIndex)
	{
		deployedThisTurn.Clear();
		List<Entity> failedToDeploy = new List<Entity>();
		BattleWaveManager.Wave wave = waves[currentWave];
		foreach (CardData cardData2 in wave.units.Where((CardData cardData) => (bool)cardData && !deployed.Contains(cardData.id)))
		{
			Entity entity = References.Battle.cards.FirstOrDefault((Entity e) => e.data.id == cardData2.id);
			if (!entity || !entity.containers.Contains(entity.owner.reserveContainer))
			{
				continue;
			}

			bool thisDeployed = false;
			for (int i = 0; i < References.Battle.rowCount; i++)
			{
				rowIndex = (rowIndex + 1) % References.Battle.rowCount;
				if (References.Battle.CanDeploy(entity, rowIndex, out var targetColumn))
				{
					thisDeployed = true;
					Deploy(entity, rowIndex, targetColumn);
					deployedThisTurn.Add(entity);
					deployed.Add(cardData2.id);
					yield return ActionQueue.Wait(includeParallel: false);
					if ((bool)this)
					{
						break;
					}

					yield break;
				}
			}

			if (!thisDeployed)
			{
				failedToDeploy.Add(entity);
			}
		}

		if (failedToDeploy.Count > 0)
		{
			Overflow(failedToDeploy);
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

	public void Overflow(IEnumerable<Entity> entities)
	{
		Debug.Log("WaveSpawner → Overflowing [" + string.Join(", ", entities) + "]");
		if (overflowWaveIndex <= currentWave)
		{
			CreateOverflowWave();
		}

		foreach (Entity entity in entities)
		{
			if (!TryToAddToOverflow(entity))
			{
				CreateOverflowWave();
				TryToAddToOverflow(entity);
			}
		}
	}

	public void CreateOverflowWave()
	{
		List<BattleWaveManager.Wave> list = waves;
		int index = list.Count - 1;
		BattleWaveManager.Wave wave = list[index];
		BattleWaveManager.Wave item = new BattleWaveManager.Wave
		{
			units = new List<CardData>(),
			counter = wave.counter
		};
		waves.Add(item);
		overflowWaveIndex = waves.Count - 1;
		Debug.Log($"WaveSpawner → New Overflow Wave Created [index: {overflowWaveIndex}]");
	}

	public bool TryToAddToOverflow(Entity entity)
	{
		BattleWaveManager.Wave wave = waves[overflowWaveIndex];
		if (wave.units.Count < 6)
		{
			wave.units.Add(entity.data);
			Debug.Log($"WaveSpawner → [{entity.name}] Added to Overflow Wave [index: {overflowWaveIndex}]");
			return true;
		}

		return false;
	}

	public void TryEarlyDeploy()
	{
		int num = CountEmptySpacesOnBoard();
		BattleWaveManager.Wave wave = waves[currentWave];
		if (!canCallEarly)
		{
			return;
		}

		if (wave.units.Count <= num)
		{
			ActionEarlyDeploy action = new ActionEarlyDeploy(base.transform, EarlyDeploy())
			{
				parallel = true
			};
			if (Events.CheckAction(action))
			{
				ActionQueue.Add(action);
			}

			return;
		}

		SfxSystem.OneShot("event:/sfx/inventory/wave_counter_deny");
		if (base.transform is RectTransform rectTransform)
		{
			rectTransform.anchoredPosition = new Vector2(-1f + 0.25f.WithRandomSign(), 2.5f);
			LeanTween.cancel(rectTransform);
			LeanTween.move(rectTransform, new Vector3(-1f, 2.5f, 0f), 1f).setEaseOutElastic();
		}
	}

	public IEnumerator EarlyDeploy()
	{
		InputSystem.Disable();
		Events.InvokeWaveDeployerEarlyDeploy();
		References.Battle.playerCardController.Disable();
		DropGold();
		counter = 1;
		yield return CountDown();
		References.Battle.playerCardController.Enable();
		InputSystem.Enable();
	}

	public void DropGold()
	{
		int num = deployEarlyReward + deployEarlyRewardPerTurn * counter;
		if (num > 0)
		{
			Events.InvokeDropGold(num, "Wave Bell", References.Player, base.transform.position);
		}
	}

	public static int CountEmptySpacesOnBoard()
	{
		return References.Battle.GetRows(References.Battle.enemy).Sum((CardContainer a) => a.canBePlacedOn ? (a.max - a.Count) : 0);
	}

	public void Pop()
	{
		if (popped || !popup || currentWave >= waves.Count)
		{
			return;
		}

		BattleWaveManager.Wave wave = waves[currentWave];
		int num = deployEarlyReward + deployEarlyRewardPerTurn * counter;
		string text = popupDesc.GetLocalizedString().Format(wave.units.Count, counter);
		int num2 = wave.units.Count - CountEmptySpacesOnBoard();
		if (num2 <= 0)
		{
			if (canCallEarly)
			{
				text = text + "\n\n" + popupHitDesc.GetLocalizedString();
				if (num > 0)
				{
					text = text + "\n\n" + popupRewardDesc.GetLocalizedString(num);
				}
			}
		}
		else
		{
			text = text + "\n\n" + popupOverflowDesc.GetLocalizedString(num2);
		}

		CardPopUp.AssignTo((RectTransform)base.transform, -1f, -0.25f);
		CardPopUp.AddPanel(popup, text);
		popped = true;
	}

	public void UnPop()
	{
		if (popped)
		{
			CardPopUp.RemovePanel(popup.name);
			popped = false;
		}
	}

	public BattleWaveData Save()
	{
		return new BattleWaveData
		{
			deployed = deployed,
			counter = counter,
			currentWave = currentWave,
			overflowWaveIndex = overflowWaveIndex,
			waves = waves.Select((BattleWaveManager.Wave a) => new BattleWaveData.Wave(a)).ToArray()
		};
	}

	public void Load(BattleWaveData data, IReadOnlyCollection<CardData> cards)
	{
		deployed = data.deployed;
		foreach (Entity item in References.Battle.enemy.reserveContainer)
		{
			int num = deployed.IndexOf(item.data.id);
			if (num >= 0)
			{
				deployed.RemoveAt(num);
			}
		}

		SetCounter(data.counter);
		currentWave = data.currentWave;
		overflowWaveIndex = data.overflowWaveIndex;
		waves = new List<BattleWaveManager.Wave>();
		BattleWaveData.Wave[] array = data.waves;
		foreach (BattleWaveData.Wave wave in array)
		{
			waves.Add(new BattleWaveManager.Wave
			{
				counter = wave.counter,
				isBossWave = wave.isBossWave,
				spawned = wave.spawned,
				units = wave.unitIds.Select((ulong a) => cards.FirstOrDefault((CardData b) => b.id == a)).ToList()
			});
		}

		if (currentWave < waves.Count)
		{
			Show();
		}

		deployedThisTurn = new List<Entity>();
	}
}
