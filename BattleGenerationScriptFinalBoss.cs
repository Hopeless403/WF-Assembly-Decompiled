#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FinalBossBattleGenerator", menuName = "Battle Generation Scripts/Final Boss")]
public class BattleGenerationScriptFinalBoss : BattleGenerationScript
{
	[SerializeField]
	public FinalBossGenerationSettings settings;

	[SerializeField]
	public CardType leaderCardType;

	[SerializeField]
	public CardType enemyCardType;

	[Header("Waves")]
	[SerializeField]
	public int waveCount = 3;

	[SerializeField]
	public int leaderWave;

	[SerializeField]
	public int bossWave = 3;

	[SerializeField]
	public int[] waveMaxSize = new int[3] { 6, 6, 6 };

	[SerializeField]
	public bool insertBossAtFront;

	[SerializeField]
	public bool insertLeaderAtFront;

	[Header("Boss")]
	[SerializeField]
	public CardData[] bossOptions;

	[SerializeField]
	public StatusEffectData[] leaderNextPhase;

	[Header("Default Deck")]
	[SerializeField]
	public CardData[] defaultLeaderOptions;

	[SerializeField]
	public bool processDefaultLeader;

	[SerializeField]
	public CardData[] defaultDeck;

	public CardData[] LoadCards(GameMode gameMode)
	{
		if (!gameMode.mainGameMode)
		{
			return null;
		}

		return SaveSystem.LoadProgressData<CardSaveData[]>("finalBossDeck", null)?.LoadArray<CardData, CardSaveData>()?.Where((CardData a) => !MissingCardSystem.IsMissing(a)).ToArray();
	}

	public void GetBaseEnemies(CardData[] cardList, int seed, out List<CardData> enemiesCloned, out bool hasStoredCards, out bool hasLeader, out CardData leaderCloned, out bool hasBoss, out CardData bossCloned)
	{
		Random.State state = Random.state;
		Random.InitState(seed);
		int num = waveMaxSize.Sum();
		leaderCloned = null;
		hasLeader = false;
		hasStoredCards = cardList != null;
		if (!hasStoredCards)
		{
			enemiesCloned = new List<CardData>();
			foreach (CardData item in defaultDeck.InRandomOrder())
			{
				enemiesCloned.Add(item.Clone());
				if (enemiesCloned.Count >= num)
				{
					break;
				}
			}
		}
		else
		{
			List<CardData> list = cardList.Where(delegate(CardData a)
			{
				string text = a.cardType.name;
				return text == "Friendly" || text == "Enemy" || text == "Leader";
			}).ToList();
			settings.ReplaceCards(list);
			enemiesCloned = new List<CardData>(list.Select((CardData a) => a.Clone(a.random3, runCreateScripts: false)));
			enemiesCloned.Shuffle();
			leaderCloned = enemiesCloned.FirstOrDefault((CardData a) => a.cardType.name == "Leader");
			if ((bool)leaderCloned)
			{
				hasLeader = true;
				leaderCloned.cardType = leaderCardType;
			}

			settings.Process(leaderCloned, enemiesCloned);
			foreach (CardData item2 in enemiesCloned)
			{
				item2.SetCustomData("eyes", "frost");
			}
		}

		foreach (CardData item3 in enemiesCloned.Where((CardData a) => a.cardType.name == "Friendly"))
		{
			item3.cardType = enemyCardType;
		}

		if (!hasLeader && defaultLeaderOptions.Length != 0)
		{
			hasLeader = true;
			leaderCloned = defaultLeaderOptions.RandomItem().Clone();
			leaderCloned.cardType = leaderCardType;
			enemiesCloned.Insert(0, leaderCloned);
			if (processDefaultLeader)
			{
				settings.Process(leaderCloned, new List<CardData> { leaderCloned });
			}
		}

		hasBoss = bossOptions.Length != 0;
		bossCloned = null;
		if (hasBoss)
		{
			bossCloned = bossOptions.RandomItem().Clone();
			enemiesCloned.Insert(0, bossCloned);
		}

		int num2 = num + (hasLeader ? 1 : 0) - enemiesCloned.Count;
		if ((num2 > 0) & hasStoredCards)
		{
			enemiesCloned.AddRange(settings.GenerateBonusEnemies(num2, cardList, defaultDeck));
		}

		Random.state = state;
	}

	public override SaveCollection<BattleWaveManager.WaveData> Run(BattleData battleData, int points)
	{
		Debug.Log($"Creating FINAL BOSS WAVES for [{battleData}]");
		CardData[] cardList = LoadCards(Campaign.Data.GameMode);
		GetBaseEnemies(cardList, Campaign.Data.Seed, out var enemiesCloned, out var _, out var hasLeader, out var leaderCloned, out var hasBoss, out var bossCloned);
		if (hasLeader)
		{
			enemiesCloned.Remove(leaderCloned);
		}

		if (hasBoss)
		{
			enemiesCloned.Remove(bossCloned);
		}

		if (hasLeader && leaderNextPhase.Length != 0)
		{
			leaderCloned.startWithEffects = CardData.StatusEffectStacks.Stack(leaderCloned.startWithEffects, new CardData.StatusEffectStacks[1]
			{
				new CardData.StatusEffectStacks(leaderNextPhase.RandomItem(), 1)
			});
		}

		WaveList waveList = new WaveList(Mathf.RoundToInt((float)points * battleData.pointFactor));
		for (int i = 0; i < waveCount; i++)
		{
			waveList.Add(new BattleWavePoolData.Wave
			{
				units = new List<CardData>(),
				maxSize = waveMaxSize[i],
				positionPriority = i
			});
		}

		int num = 0;
		foreach (CardData item in enemiesCloned)
		{
			bool flag = false;
			for (int j = 0; j < waveCount; j++)
			{
				BattleWavePoolData.Wave wave = waveList.GetWave(j % waveCount);
				if (wave.maxSize > 0 && wave.CanAddTo())
				{
					flag = true;
					wave.units.Add(item);
					break;
				}

				num++;
			}

			num++;
			if (!flag)
			{
				break;
			}
		}

		if (hasLeader)
		{
			List<CardData> units = waveList.GetWave(leaderWave).units;
			if (insertLeaderAtFront)
			{
				units.Insert(0, leaderCloned);
			}
			else
			{
				units.Add(leaderCloned);
			}
		}

		if (hasBoss)
		{
			List<CardData> units2 = waveList.GetWave(bossWave).units;
			if (insertBossAtFront)
			{
				units2.Insert(0, bossCloned);
			}
			else
			{
				units2.Add(bossCloned);
			}
		}

		for (int num2 = waveCount - 1; num2 >= 0; num2--)
		{
			if (waveList.GetWave(num2).units.Count <= 0)
			{
				waveList.RemoveAt(num2);
			}
		}

		AddGoldGivers(waveList, battleData);
		AddBonusUnits(waveList, battleData);
		List<BattleWaveManager.WaveData> list = new List<BattleWaveManager.WaveData>();
		int count = waveList.Count;
		for (int k = 0; k < count; k++)
		{
			BattleWaveManager.WaveDataFull waveDataFull = new BattleWaveManager.WaveDataFull
			{
				counter = battleData.waveCounter
			};
			BattleWavePoolData.Wave wave2 = waveList.GetWave(k);
			List<CardSaveData> list2 = new List<CardSaveData>();
			foreach (CardData unit in wave2.units)
			{
				list2.Add(unit.Save());
				if (!waveDataFull.isBossWave && unit.cardType.miniboss)
				{
					waveDataFull.isBossWave = true;
				}
			}

			waveDataFull.cardDatas = list2.ToArray();
			list.Add(waveDataFull);
		}

		foreach (CardData item2 in enemiesCloned)
		{
			Object.Destroy(item2);
		}

		if ((bool)leaderCloned)
		{
			Object.Destroy(leaderCloned);
		}

		if ((bool)bossCloned)
		{
			Object.Destroy(bossCloned);
		}

		return new SaveCollection<BattleWaveManager.WaveData>(list);
	}
}
