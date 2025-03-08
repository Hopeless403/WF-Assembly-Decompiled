#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class DeadweightAfterBossModifierSystem : GameSystem
{
	[SerializeField]
	public string cardDataName = "Deadweight";

	[SerializeField]
	public ChooseNewCardSequence gainCardSequencePrefab;

	public void OnEnable()
	{
		Events.OnBattleWin += BattleWin;
	}

	public void OnDisable()
	{
		Events.OnBattleWin -= BattleWin;
	}

	public void BattleWin()
	{
		if (Campaign.FindCharacterNode(References.Player).type.isBoss)
		{
			CardData cardData = AddressableLoader.Get<CardData>("CardData", cardDataName);
			ActionQueue.Add(new ActionSequence(GainCardRoutine(cardData)));
		}
	}

	public IEnumerator GainCardRoutine(CardData cardData)
	{
		CardData data = cardData.Clone();
		ChooseNewCardSequence sequence = Object.Instantiate(gainCardSequencePrefab, References.Player.entity.display.transform);
		CardSelector componentInChildren = sequence.GetComponentInChildren<CardSelector>();
		if ((object)componentInChildren != null)
		{
			componentInChildren.character = References.Player;
			componentInChildren.selectEvent.AddListener(Events.InvokeEntityChosen);
		}

		sequence.owner = References.Player;
		sequence.cardController.owner = References.Player;
		Card card = CardManager.Get(data, sequence.cardController, References.Player, inPlay: false, isPlayerCard: true);
		card.entity.flipper.FlipDownInstant();
		sequence.cardContainer.Add(card.entity);
		yield return card.UpdateData();
		card.transform.localPosition = Vector3.down;
		card.entity.wobbler.WobbleRandom();
		sequence.cardContainer.TweenChildPositions();
		Events.InvokeEntityOffered(card.entity);
		yield return sequence.Run();
		sequence.gameObject.Destroy();
	}
}
