#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMusicSystem : GameSystem, ISaveable<BattleMusicSaveData>
{
	[Serializable]
	public struct MinibossIntroRef
	{
		[SerializeField]
		public CardData cardData;

		[SerializeField]
		public EventReference introEvent;
	}

	public int startingIntensity;

	public int normalIntensity = 1;

	[Header("Win/Lose Jingles")]
	[SerializeField]
	public EventReference winJingle;

	[SerializeField]
	public EventReference loseJingle;

	[Header("Miniboss Intros")]
	[SerializeField]
	public EventReference minibossIntroDefault;

	[SerializeField]
	public MinibossIntroRef[] minibossIntros;

	[SerializeField]
	public float minibossIntroDuration = 2f;

	public readonly Dictionary<string, EventReference> minibossIntroLookup = new Dictionary<string, EventReference>();

	public Scene currentScene;

	public EventInstance current;

	public EventInstance minibossIntroInstance;

	public int intensity;

	public PARAMETER_ID intensityParameterId;

	public bool bossEntered;

	public float promptStartMiniboss;

	public int bossPhase = 1;

	public float volume = 1f;

	public float pitch = 1f;

	public float targetVolume = 1f;

	public float targetPitch = 1f;

	public const float fadeAmount = 1f;

	public void Awake()
	{
		MinibossIntroRef[] array = minibossIntros;
		for (int i = 0; i < array.Length; i++)
		{
			MinibossIntroRef minibossIntroRef = array[i];
			minibossIntroLookup[minibossIntroRef.cardData.name] = minibossIntroRef.introEvent;
		}
	}

	public void OnEnable()
	{
		Events.OnSceneChanged += SceneChange;
		Events.OnBattlePhaseStart += BattlePhaseChange;
		Events.OnBattleEnd += BattleEnd;
		Events.OnEntityHit += EntityHit;
		Events.OnEntityMove += EntityMove;
		Events.OnMinibossIntro += MinibossIntro;
		Events.OnEntityChangePhase += EntityChangePhase;
		Check();
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChange;
		Events.OnBattlePhaseStart -= BattlePhaseChange;
		Events.OnBattleEnd -= BattleEnd;
		Events.OnEntityHit -= EntityHit;
		Events.OnEntityMove -= EntityMove;
		Events.OnMinibossIntro -= MinibossIntro;
		Events.OnEntityChangePhase -= EntityChangePhase;
		StopMusic();
	}

	public void OnDestroy()
	{
		StopMusic();
	}

	public void Update()
	{
		if (promptStartMiniboss > 0f)
		{
			promptStartMiniboss -= Time.deltaTime;
			if (promptStartMiniboss <= 0f)
			{
				StartMusic(References.GetCurrentArea().minibossMusicEvent);
				SetParam("bossHealth", 1f);
			}
		}

		float num = 1f * Time.deltaTime;
		if (Mathf.Abs(pitch - targetPitch) > num)
		{
			float num2 = Mathf.Clamp(targetPitch - pitch, 0f - num, num);
			pitch += num2;
			if (IsRunning(current))
			{
				current.setPitch(pitch);
			}
		}
	}

	public void FadePitchTo(float value)
	{
		targetPitch = value;
	}

	public void SceneChange(Scene scene)
	{
		currentScene = scene;
		Check();
	}

	public void BattlePhaseChange(Battle.Phase phase)
	{
		if (phase == Battle.Phase.End)
		{
			StopMusic(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	public void BattleEnd()
	{
		if (Battle.instance.winner == Battle.instance.player)
		{
			SfxSystem.OneShot(winJingle);
		}
		else
		{
			SfxSystem.OneShot(loseJingle);
		}
	}

	public void EntityHit(Hit hit)
	{
		if (!bossEntered && intensity == startingIntensity && hit.Offensive)
		{
			SetIntensity(normalIntensity);
		}
	}

	public void EntityMove(Entity entity)
	{
		if (!bossEntered && currentScene.IsValid() && (bool)References.Battle && entity.owner == References.Battle.enemy)
		{
			CardType cardType = entity.data?.cardType;
			if ((object)cardType != null && cardType.miniboss && Battle.IsOnBoard(entity) && !Battle.IsOnBoard(entity.preContainers))
			{
				bossEntered = true;
				StopMusic(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}
	}

	public void MinibossIntro(Entity entity)
	{
		if (!IsBossBattle())
		{
			EventReference eventRef = (minibossIntroLookup.ContainsKey(entity.data.name) ? minibossIntroLookup[entity.data.name] : minibossIntroDefault);
			minibossIntroInstance = SfxSystem.OneShot(eventRef);
			promptStartMiniboss = minibossIntroDuration;
		}
	}

	public void EntityChangePhase(Entity entity)
	{
		if (entity.data.cardType.miniboss && entity.owner.team == References.Battle.enemy.team && bossPhase < 2)
		{
			bossPhase++;
			SetParam("finalboss", bossPhase);
		}
	}

	public void Check()
	{
		Scene scene = currentScene;
		CampaignNode campaignNode;
		if (scene.name == "Battle")
		{
			campaignNode = Campaign.FindCharacterNode(References.Player);
			AreaData areaData = References.Areas[campaignNode.areaIndex];
			if (campaignNode.type is CampaignNodeTypeBattle campaignNodeTypeBattle)
			{
				EventReference overrideMusic = campaignNodeTypeBattle.overrideMusic;
				if (!overrideMusic.IsNull)
				{
					StartMusic(campaignNodeTypeBattle.overrideMusic);
					goto IL_0090;
				}
			}

			if (campaignNode.type.isBoss)
			{
				StartMusic(areaData.bossMusicEvent);
			}
			else
			{
				StartMusic(areaData.battleMusicEvent);
				InitBattleMusic();
			}

			goto IL_0090;
		}

		StopMusic(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		return;
		IL_0090:
		if (campaignNode.type.isBoss)
		{
			bossEntered = true;
			bossPhase = 1;
			SetParam("finalboss", bossPhase);
		}
	}

	public void InitBattleMusic()
	{
		current.getDescription(out var description);
		description.getParameterDescriptionByName("Phase", out var parameter);
		intensityParameterId = parameter.id;
		SetIntensity(startingIntensity);
		bossEntered = false;
	}

	public void StartMusic(EventReference eventReference)
	{
		StartMusic(eventReference.Guid);
	}

	public void StartMusic(GUID eventGUID)
	{
		try
		{
			current = RuntimeManager.CreateInstance(eventGUID);
			current.start();
			current.setPitch(pitch);
		}
		catch (EventNotFoundException message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	public void StopMusic(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		current.stop(stopMode);
		current.release();
		targetPitch = 1f;
		targetVolume = 1f;
	}

	public void SetIntensity(int amount)
	{
		intensity = amount;
		current.setParameterByID(intensityParameterId, intensity);
		UnityEngine.Debug.Log($"Battle Music System → Intensity Set to {intensity}");
	}

	public void SetParam(string name, float value)
	{
		if (IsRunning(current))
		{
			current.setParameterByName(name, value);
			UnityEngine.Debug.Log($"Param Set: {name}: {value}");
		}
	}

	public static bool IsRunning(EventInstance instance)
	{
		if (instance.isValid())
		{
			instance.getPlaybackState(out var state);
			if (state != PLAYBACK_STATE.STOPPED)
			{
				return true;
			}
		}

		return false;
	}

	public static bool IsBossBattle()
	{
		return Campaign.FindCharacterNode(References.Player).type.isBoss;
	}

	public BattleMusicSaveData Save()
	{
		return new BattleMusicSaveData
		{
			intensity = intensity,
			bossEntered = bossEntered,
			bossPhase = bossPhase
		};
	}

	public void Load(BattleMusicSaveData state)
	{
		if (!IsRunning(current) || bossEntered != state.bossEntered || bossPhase != state.bossPhase)
		{
			StopMusic();
			bossEntered = state.bossEntered;
			bossPhase = state.bossPhase;
			if (IsBossBattle())
			{
				StartMusic(References.GetCurrentArea().bossMusicEvent);
				SetParam("finalboss", bossPhase);
			}
			else if (bossEntered)
			{
				StartMusic(References.GetCurrentArea().minibossMusicEvent);
			}
			else
			{
				StartMusic(References.GetCurrentArea().battleMusicEvent);
				SetIntensity(state.intensity);
			}
		}
	}
}
