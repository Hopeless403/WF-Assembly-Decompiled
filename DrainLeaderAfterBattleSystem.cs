#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class DrainLeaderAfterBattleSystem : GameSystem
{
	[SerializeField]
	public int[] modifyAfterBattleTier = new int[1] { 5 };

	[SerializeField]
	public CardScript modifyScript;

	[SerializeField]
	public bool modifyLeader;

	[SerializeField]
	[HideIf("modifyLeader")]
	public int modifyCardIndex;

	[SerializeField]
	public ModifyCardSequence sequencePrefab;

	public void OnEnable()
	{
		Events.OnBattleWinPreRewards += BattleWinPreRewards;
	}

	public void OnDisable()
	{
		Events.OnBattleWinPreRewards -= BattleWinPreRewards;
	}

	public void BattleWinPreRewards()
	{
		CampaignNodeType type = Campaign.FindCharacterNode(References.Player).type;
		if (!(type is CampaignNodeTypeBattle) || !type.isBoss)
		{
			return;
		}

		CardData highestHealthCard = References.PlayerData.inventory.deck.OrderByDescending((CardData a) => a.hp).FirstOrDefault();
		if ((bool)highestHealthCard)
		{
			CardData cardData = References.PlayerData.inventory.deck.Where((CardData a) => a.hp == highestHealthCard.hp).InRandomOrder().FirstOrDefault();
			if ((bool)cardData)
			{
				ActionQueue.Add(new ActionSequence(ModifyCardRoutine(cardData)));
			}
		}
	}

	public IEnumerator ModifyCardRoutine(CardData cardToModify)
	{
		ModifyCardSequence sequence = Object.Instantiate(sequencePrefab, References.Player.entity.display.transform);
		yield return sequence.Run(cardToModify, modifyScript);
		sequence.gameObject.Destroy();
	}
}
