#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class FrostoscopeSequence : MonoBehaviour
{
	[SerializeField]
	public EventReference enterSfxEvent;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer[] cardHolders;

	[SerializeField]
	public CardContainer leaderCardHolder;

	[SerializeField]
	public Transform scene;

	[SerializeField]
	public BattleGenerationScriptFinalBoss generationScript;

	[SerializeField]
	public Vector2 cameraLimits;

	[SerializeField]
	public GameObject nothingHere;

	[SerializeField]
	public GameObject group;

	[SerializeField]
	public bool enableGroupAfterGeneration;

	[SerializeField]
	[ShowIf("enableGroupAfterGeneration")]
	public float delayBeforeEnableGroup = 0.25f;

	[SerializeField]
	public bool unloadSceneOnEnd;

	public Entity hover;

	public bool hovering;

	public bool generated;

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
		cardController.Enable();
		StartCoroutine(Run());
		MusicSystem.FadePitchTo(0.25f, 0.2f);
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
		hover = null;
		hovering = false;
		cardController.Disable();
		StopAllCoroutines();
		generated = false;
		MusicSystem.FadePitchTo(1f, 0.2f);
	}

	public void EntityHover(Entity entity)
	{
		hover = entity;
		hovering = true;
	}

	public void EntityUnHover(Entity entity)
	{
		hover = null;
		hovering = false;
	}

	public void Update()
	{
		Vector3 localPosition = scene.localPosition;
		Vector3 target = -(hovering ? Vector3.Lerp(hover.transform.position.WithZ(0f), Cursor3d.Position, 0.5f) : Cursor3d.Position);
		target.x = Mathf.Clamp(target.x, 0f - cameraLimits.x, cameraLimits.x);
		target.y = Mathf.Clamp(target.y, 0f - cameraLimits.y, cameraLimits.y);
		scene.localPosition = Delta.Lerp(localPosition, target, 0.025f, Time.deltaTime);
	}

	public IEnumerator Run()
	{
		if (enableGroupAfterGeneration)
		{
			Routine.Clump clump = new Routine.Clump();
			clump.Add(CreateCards(startFlipped: false));
			clump.Add(Sequences.Wait(delayBeforeEnableGroup));
			yield return clump.WaitForEnd();
			group.SetActive(value: true);
		}

		SfxSystem.OneShot(enterSfxEvent);
		if (!generated)
		{
			yield return CreateCards(startFlipped: true);
		}
	}

	public IEnumerator CreateCards(bool startFlipped)
	{
		generated = true;
		var (gameMode, seed) = GetGameModeAndSeed();
		if (!TryGetPreGeneratedBattle(gameMode, out var cards))
		{
			CardData[] cardList = generationScript.LoadCards(gameMode);
			generationScript.GetBaseEnemies(cardList, seed, out cards, out var _, out var _, out var _, out var _, out var _);
		}

		if (cards == null || cards.Count <= 0)
		{
			nothingHere.SetActive(value: true);
			yield break;
		}

		nothingHere.SetActive(value: false);
		Routine.Clump clump = new Routine.Clump();
		List<CardContainer> list = cardHolders.ToList();
		foreach (CardData item in cards)
		{
			if (item.cardType.miniboss)
			{
				clump.Add(CreateCard(item, leaderCardHolder, startFlipped));
				continue;
			}

			if (list.Count <= 0)
			{
				break;
			}

			clump.Add(CreateCard(item, list[0], startFlipped));
			list.RemoveAt(0);
		}

		yield return clump.WaitForEnd();
	}

	public static bool TryGetPreGeneratedBattle(GameMode gameMode, out List<CardData> cards)
	{
		cards = null;
		if (SaveSystem.LoadCampaignData(gameMode, "result", Campaign.Result.None) != 0)
		{
			return false;
		}

		CampaignNodeSaveData campaignNodeSaveData = SaveSystem.LoadCampaignData<CampaignSaveData>(gameMode, "data")?.nodes?.FirstOrDefault((CampaignNodeSaveData a) => a.typeName == "CampaignNodeFinalBoss");
		if (campaignNodeSaveData != null && campaignNodeSaveData.data.TryGetValue("waves", out var value) && value is SaveCollection<BattleWaveManager.WaveData>)
		{
			BattleWaveManager.WaveData[] collection = ((SaveCollection<BattleWaveManager.WaveData>)value).collection;
			if (collection != null)
			{
				cards = new List<CardData>();
				if (!campaignNodeSaveData.cleared)
				{
					BattleWaveManager.WaveData[] array = collection;
					foreach (BattleWaveManager.WaveData waveData in array)
					{
						if (!(waveData is BattleWaveManager.WaveDataFull waveDataFull))
						{
							for (int j = 0; j < waveData.Count; j++)
							{
								cards.Add(waveData.GetCardData(j));
							}

							continue;
						}

						CardSaveData[] cardDatas = waveDataFull.cardDatas;
						foreach (CardSaveData cardSaveData in cardDatas)
						{
							cards.Add(cardSaveData.Load());
						}
					}
				}

				return true;
			}
		}

		return false;
	}

	public static (GameMode, int) GetGameModeAndSeed()
	{
		int num = -1;
		GameMode item;
		if ((bool)References.Campaign && Campaign.Data != null)
		{
			item = Campaign.Data.GameMode;
			num = Campaign.Data.Seed;
		}
		else
		{
			item = AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal");
			num = SaveSystem.LoadProgressData("nextSeed", -1);
			if (num == -1)
			{
				num = Dead.Random.Seed();
				SaveSystem.SaveProgressData("nextSeed", num);
			}
		}

		return (item, num);
	}

	public IEnumerator CreateCard(CardData cardData, CardContainer cardHolder, bool startFlipped)
	{
		Card card = CardManager.Get(cardData, cardController, null, inPlay: false, isPlayerCard: false);
		_ = card.transform;
		if (startFlipped)
		{
			card.entity.flipper.FlipDownInstant();
		}

		cardHolder.Add(card.entity);
		cardHolder.SetChildPositions();
		yield return card.UpdateData();
		if (startFlipped)
		{
			card.entity.flipper.FlipUp(force: true);
		}
	}

	public void End()
	{
		CardContainer[] array = cardHolders;
		foreach (CardContainer obj in array)
		{
			obj.DestroyAll();
			obj.Clear();
		}

		leaderCardHolder.DestroyAll();
		leaderCardHolder.Clear();
		base.gameObject.SetActive(value: false);
		if (unloadSceneOnEnd)
		{
			new Routine(SceneManager.Unload("NewFrostGuardian"));
		}
	}
}
