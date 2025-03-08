#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BattleSetUp : SceneRoutine
{
	[Serializable]
	public struct Background
	{
		public string key;

		public GameObject prefab;
	}

	[SerializeField]
	[Required(null)]
	public Battle battle;

	[SerializeField]
	public Character enemy;

	[SerializeField]
	public CharacterDisplay enemyCharacterDisplay;

	[SerializeField]
	[Required(null)]
	public BoardDisplay board;

	[SerializeField]
	[Required(null)]
	public CardController cardController;

	[SerializeField]
	public TweenUI startTween;

	[Header("Background")]
	[SerializeField]
	public PrefabLoaderAsync backgroundLoader;

	public CharacterDisplay playerDisplay;

	public override IEnumerator Run()
	{
		Routine updateBackgroundRoutine = new Routine(UpdateBackground());
		Character player = References.Player;
		CampaignNode node = Campaign.FindCharacterNode(player);
		playerDisplay = (CharacterDisplay)player.entity.display;
		if ((bool)playerDisplay)
		{
			Debug.Log("BATTLE SET UP");
			battle.player = player;
			battle.enemy = enemy;
			board.player = player;
			board.enemy = enemy;
			cardController.owner = player;
			battle.rows[player] = new List<CardContainer>();
			battle.rows[enemy] = new List<CardContainer>();
			yield return null;
			yield return board.SetUp(node, cardController);
			player.handContainer.AssignController(cardController);
			player.handContainer.SetSize(player.data.handSize, player.handContainer.CardScale);
			player.discardContainer.AssignController(cardController);
			player.drawContainer.AssignController(cardController);
			enemyCharacterDisplay.Assign(enemy);
		}
		else
		{
			Debug.LogError($"PLAYER [{player}] IS NOT ASSIGNED TO → CharacterDisplay");
		}

		yield return new WaitUntil(() => !updateBackgroundRoutine.IsRunning);
	}

	public IEnumerator StartAnimation()
	{
		if ((bool)startTween)
		{
			startTween.Fire();
		}

		new Routine.Clump().Add(playerDisplay.handOverlay.Show());
		yield return Sequences.Wait(0.1f);
	}

	public IEnumerator UpdateBackground()
	{
		if (Campaign.FindCharacterNode(References.Player).type is CampaignNodeTypeBattle campaignNodeTypeBattle && campaignNodeTypeBattle.overrideBackground)
		{
			yield return backgroundLoader.Load(campaignNodeTypeBattle.background);
			yield break;
		}

		AreaData currentArea = References.GetCurrentArea();
		if ((bool)currentArea)
		{
			yield return backgroundLoader.Load(currentArea.battleBackgroundPrefabRef);
		}
	}
}
