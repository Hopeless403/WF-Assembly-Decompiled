#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

public class SecretNakedGnomeSystem : GameSystem
{
	public const string nakedGnomeDataName = "NakedGnome";

	public const string nakedGnomeFriendlyDataName = "NakedGnomeFriendly";

	public static bool nakedGnomeSaved;

	[SerializeField]
	public InspectNewUnitSequence gainNakedGnomeSequencePrefab;

	public static Vector3 startPos = new Vector3(0f, 8f, 0f);

	public void OnEnable()
	{
		Events.OnBattleEnd += BattleEnd;
		Events.PostBattle += PostBattle;
	}

	public void OnDisable()
	{
		Events.OnBattleEnd -= BattleEnd;
		Events.PostBattle -= PostBattle;
	}

	public static void BattleEnd()
	{
		nakedGnomeSaved = Battle.GetCards(References.Battle.enemy).FirstOrDefault((Entity a) => a.data.name == "NakedGnome");
	}

	public void PostBattle(CampaignNode campaignNode)
	{
		if (nakedGnomeSaved)
		{
			ActionQueue.Add(new ActionSequence(Sequence()));
			nakedGnomeSaved = false;
		}
	}

	public IEnumerator Sequence()
	{
		InspectNewUnitSequence sequence = Object.Instantiate(gainNakedGnomeSequencePrefab, References.Player.entity.display.transform);
		sequence.cardSelector.character = References.Player;
		sequence.GetComponent<CardSelector>()?.selectEvent.AddListener(Events.InvokeEntityChosen);
		CardData data = AddressableLoader.Get<CardData>("CardData", "NakedGnomeFriendly").Clone();
		Card card = CardManager.Get(data, null, References.Player, inPlay: false, isPlayerCard: true);
		card.transform.SetParent(sequence.cardHolder);
		card.transform.localPosition = startPos;
		yield return card.UpdateData();
		sequence.SetUnit(card.entity, updateGreeting: false);
		Events.InvokeEntityOffered(card.entity);
		yield return sequence.Run();
	}
}
