#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Dead;
using NaughtyAttributes;
using UnityEngine;

public class Campaign : MonoBehaviour, ISaveable<CampaignSaveData>
{
	public enum Result
	{
		None,
		Win,
		Lose,
		Restart
	}

	public static Campaign instance;

	public Transform characterContainer;

	public List<Character> characters = new List<Character>();

	public List<CampaignNode> nodes = new List<CampaignNode>();

	public GameObject systems;

	[ReadOnly]
	public TextAsset preset;

	[ReadOnly]
	public string battleTiers;

	public static CampaignData Data;

	public Result result { get; set; }

	public void Awake()
	{
		instance = this;
		References.Campaign = this;
	}

	public void OnEnable()
	{
		Events.OnBattleEnd += BattleEnd;
	}

	public void OnDisable()
	{
		Events.OnBattleEnd -= BattleEnd;
	}

	public void BattleEnd()
	{
		if (References.Battle.winner == References.Battle.player)
		{
			if (CheckVictory())
			{
				End(Result.Win);
				JournalNameHistory.MostRecentNameMissing();
			}
		}
		else
		{
			End(Result.Lose);
		}
	}

	public static void Begin()
	{
		if (Data.GameMode.doSave)
		{
			SaveSystem.DeleteCampaign(Data.GameMode);
			SaveSystem.SaveCampaignData(Data.GameMode, "seed", Data.Seed);
		}

		if (Data.GameMode.mainGameMode)
		{
			SaveSystem.SaveProgressData("nextSeed", Dead.Random.Seed());
		}
	}

	public void End(Result result)
	{
		this.result = result;
		Debug.Log(">>>> CAMPAIGN END <<<<");
		Events.InvokeCampaignEnd(result, StatsSystem.Get(), References.PlayerData);
		PromptSave();
	}

	public void Final()
	{
		StopAllCoroutines();
		ActionQueue.Restart();
		References.Campaign = null;
		Events.InvokeCampaignFinal();
	}

	public IEnumerator Start()
	{
		yield return new WaitUntil(() => !GameManager.Busy);
		yield return SceneManager.Load("UI", SceneType.Persistent);
		Events.InvokeCampaignStart();
		UnityEngine.Random.InitState(Data.Seed);
		CampaignNode continueBattleNode = null;
		if (CheckContinue(Data.GameMode))
		{
			CampaignSaveData campaignSaveData = SaveSystem.LoadCampaignData<CampaignSaveData>(Data.GameMode, "data");
			StatsSystem.Set(SaveSystem.LoadCampaignData<CampaignStats>(Data.GameMode, "stats"));
			nodes = campaignSaveData.LoadNodes();
			Data.GameMode.populator.LoadCharacters(this, campaignSaveData.characters);
			References.Player = characters[campaignSaveData.playerId];
			References.PlayerData = References.Player.data;
			CharacterDisplay.FindAndAssign(References.Player);
			BattleSaveData battleSaveData = SaveSystem.LoadCampaignData<BattleSaveData>(Data.GameMode, "battleState", null);
			if (battleSaveData != null)
			{
				CampaignNode campaignNode = FindCharacterNode(References.Player);
				if (campaignNode != null && campaignNode.id == battleSaveData.campaignNodeId)
				{
					if (battleSaveData.HasMissingCardData())
					{
						SaveSystem.SaveCampaignData<BattleSaveData>(Data.GameMode, "battleState", null);
					}
					else
					{
						continueBattleNode = campaignNode;
					}
				}
			}

			if (campaignSaveData.modifiers != null)
			{
				string[] modifiers = campaignSaveData.modifiers;
				foreach (string assetName in modifiers)
				{
					GameModifierData gameModifierData = AddressableLoader.Get<GameModifierData>("GameModifierData", assetName);
					if (gameModifierData != null)
					{
						ModifierSystem.AddModifier(Data, gameModifierData);
					}
				}
			}

			Events.InvokeCampaignLoaded();
		}
		else
		{
			yield return Data.GameMode.generator.Generate(this);
			Events.InvokePreCampaignPopulate();
			yield return Data.GameMode.populator.Populate(this);
			StatsSystem.Set(new CampaignStats());
			yield return Events.InvokeCampaignGenerated();
			FirstSave();
		}

		if (Data.GameMode.campaignSystemNames != null)
		{
			string[] modifiers = Data.GameMode.campaignSystemNames;
			foreach (string componentName in modifiers)
			{
				base.gameObject.AddComponentByName(componentName);
			}
		}

		if (Data.GameMode.systemsToDisable != null)
		{
			string[] modifiers = Data.GameMode.systemsToDisable;
			foreach (string text in modifiers)
			{
				Type type = Type.GetType(text);
				if ((object)type != null && UnityEngine.Object.FindObjectOfType(type, includeInactive: true) is GameSystem gameSystem)
				{
					Debug.Log("Disabling [" + text + "]");
					gameSystem.Disable();
				}
			}
		}

		yield return SceneManager.Load("MapNew", SceneType.Active);
		if (continueBattleNode == null || !TryEnterNode(continueBattleNode, delay: false))
		{
			Transition.End();
		}
	}

	public static CampaignNode GetNode(int id)
	{
		return instance.nodes[id];
	}

	public static Character GetCharacter(int id)
	{
		return instance.characters[id];
	}

	public static int GetCharacterId(Character character)
	{
		return instance.characters.IndexOf(character);
	}

	public static void MoveCharacter(Character character, CampaignNode toNode)
	{
		FindCharacterNode(character)?.characters?.Remove(GetCharacterId(character));
		toNode.characters.Add(GetCharacterId(character));
		PromptSave();
	}

	public static CampaignNode FindCharacterNode(Character character)
	{
		foreach (CampaignNode node in instance.nodes)
		{
			if (node.characters.Contains(GetCharacterId(character)))
			{
				return node;
			}
		}

		return null;
	}

	public static bool TryEnterNode(CampaignNode node, bool delay = true)
	{
		if (!node.cleared && node.characters.Contains(GetCharacterId(References.Player)))
		{
			instance.StartCoroutine(EnterNode(node, delay));
			return true;
		}

		return false;
	}

	public static IEnumerator EnterNode(CampaignNode node, bool delay = true)
	{
		if ((bool)node?.type)
		{
			InputSystem.Disable();
			if (delay)
			{
				yield return Sequences.Wait(0.5f);
			}

			InputSystem.Enable();
			UnityEngine.Random.InitState(node.seed);
			yield return node.type.Run(node);
		}
	}

	public static void FirstSave()
	{
		if (Data.GameMode.doSave)
		{
			SaveSystem.SaveCampaignData(Data.GameMode, "gameVersion", Data.GameVersion);
			SaveSystem.SaveCampaignData(Data.GameMode, "gameMode", Data.GameMode.name);
			string value = (Data.GameMode.dailyRun ? DailyFetcher.GetDateTime().ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"));
			SaveSystem.SaveCampaignData(Data.GameMode, "startDate", value);
			PromptSave();
		}
	}

	public static void PromptSave()
	{
		if (Data.GameMode.doSave)
		{
			SaveSystem.SaveCampaignData(Data.GameMode, "data", instance.Save());
			if (instance.result != 0)
			{
				SaveSystem.SaveCampaignData(Data.GameMode, "result", instance.result);
			}
		}

		Events.InvokeCampaignSaved();
	}

	public static bool CheckVictory()
	{
		CampaignNode campaignNode = FindCharacterNode(References.Player);
		if (!campaignNode.finalNode)
		{
			return campaignNode.connections.Count <= 0;
		}

		return true;
	}

	public static bool CheckContinue(GameMode gameMode)
	{
		if (!gameMode.doSave || !SaveSystem.CampaignExists(gameMode))
		{
			return false;
		}

		if (SaveSystem.LoadCampaignData(gameMode, "result", Result.None) != 0)
		{
			return false;
		}

		return SaveSystem.CampaignDataExists(gameMode, "data");
	}

	public CampaignSaveData Save()
	{
		return new CampaignSaveData(this);
	}
}
