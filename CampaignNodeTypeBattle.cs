#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "CampaignNodeTypeBattle", menuName = "Campaign/Node Type/Battle")]
public class CampaignNodeTypeBattle : CampaignNodeType
{
	public EventReference overrideMusic;

	public bool overrideBackground;

	[ShowIf("overrideBackground")]
	public AssetReferenceGameObject background;

	[SerializeField]
	public bool hasRewards;

	public override IEnumerator SetUp(CampaignNode node)
	{
		if (hasRewards)
		{
			CampaignNodeTypeBoss.GetRewards(node);
		}

		yield break;
	}

	public override IEnumerator Run(CampaignNode node)
	{
		yield return Transition.To("Battle");
		BattleSetUp battleSetUp = Object.FindObjectOfType<BattleSetUp>();
		if ((bool)battleSetUp)
		{
			yield return battleSetUp.Run();
		}

		BattleSaveSystem battleSaveSystem = Object.FindObjectOfType<BattleSaveSystem>();
		object value;
		if ((object)battleSaveSystem != null && battleSaveSystem.TryLoadBattleState(node))
		{
			yield return battleSaveSystem.LoadRoutine();
		}
		else if (node.data.TryGetValue("battle", out value) && value is string assetName)
		{
			BattleData battleData = AddressableLoader.Get<BattleData>("BattleData", assetName);
			if ((object)battleData != null && (bool)battleData.setUpScript)
			{
				yield return battleData.setUpScript.Run();
			}
		}

		Transition.End();
		if ((bool)battleSetUp)
		{
			yield return battleSetUp.StartAnimation();
		}

		yield return References.Battle.Run();
		if (Campaign.CheckVictory())
		{
			yield return Transition.WaitUntilDone(Transition.Begin());
			yield return BattleEnd(node);
			new Routine(Sequences.EndCampaign("MainMenu"));
			yield break;
		}

		yield return Transition.WaitUntilDone(Transition.Begin());
		yield return BattleEnd(node);
		yield return Sequences.SceneChange("MapNew");
		Transition.End();
		Events.InvokePostBattle(node);
		yield return ActionQueue.Wait();
		yield return CheckRecovery();
		yield return MapNew.CheckCompanionLimit();
	}

	public static IEnumerator BattleEnd(CampaignNode node)
	{
		Character player = Battle.instance.player;
		if ((bool)player.handContainer)
		{
			player.handContainer.Clear();
		}

		if ((bool)player.reserveContainer)
		{
			player.reserveContainer.Clear();
		}

		if ((bool)player.drawContainer)
		{
			player.drawContainer.Clear();
		}

		if ((bool)player.discardContainer)
		{
			player.discardContainer.Clear();
		}

		List<Entity> cards = References.Battle.cards;
		Debug.Log($"BattleEnd → Destroying [{cards.Count}] Cards!");
		for (int num = cards.Count - 1; num >= 0; num--)
		{
			CardManager.ReturnToPool(cards[num]);
		}

		yield return StatusEffectSystem.Clear();
		if (player.entity.alive && player.data.inventory.reserve != null)
		{
			node.SetCleared();
			if (player.entity.display is CharacterDisplay characterDisplay)
			{
				yield return characterDisplay.handOverlay.Hide();
			}
		}
	}

	public static IEnumerator CheckRecovery()
	{
		if (References.Player.entity.display is CharacterDisplay characterDisplay)
		{
			yield return characterDisplay.deckDisplay.companionRecoverSequence.Run();
		}
	}

	public override bool HasMissingData(CampaignNode node)
	{
		if (node.data.TryGetValue("battle", out var value) && value is string assetName)
		{
			BattleData battleData = AddressableLoader.Get<BattleData>("BattleData", assetName);
			if ((object)battleData != null && (bool)battleData.setUpScript)
			{
				BattleWaveManager.WaveData[] saveCollection = node.data.GetSaveCollection<BattleWaveManager.WaveData>("waves");
				if (saveCollection == null)
				{
					return true;
				}

				BattleWaveManager.WaveData[] array = saveCollection;
				foreach (BattleWaveManager.WaveData waveData in array)
				{
					for (int j = 0; j < waveData.Count; j++)
					{
						if (waveData.PeekCardData(j) == null)
						{
							return true;
						}
					}
				}

				return false;
			}
		}

		return true;
	}
}
