#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialBattleSystem3 : TutorialBattleSystem
{
	public class PhaseWaitForLeader : Phase
	{
		public override void OnEnable()
		{
			Events.OnEntityMove += EntityMove;
		}

		public override void OnDisable()
		{
			Events.OnEntityMove -= EntityMove;
		}

		public void EntityMove(Entity entity)
		{
			if (entity.owner == References.Player && entity.data.cardType.miniboss && Battle.IsOnBoard(entity))
			{
				base.enabled = false;
			}
		}
	}

	public class PhasePlaceCompanion : Phase
	{
		public readonly Entity target;

		public PhasePlaceCompanion(Entity target)
		{
			this.target = target;
		}

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnBattleTurnStart += BattleTurnStart;
			new Routine(PromptAfter(0.5f));
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnBattleTurnStart -= BattleTurnStart;
			PromptSystem.Hide();
		}

		public IEnumerator PromptAfter(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (base.enabled)
			{
				PromptSystem.Create(Prompt.Anchor.Left, 0.1f, 2f, 5f);
				PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle3_1.GetLocalizedString());
			}
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (allow && !CorrectAction(action) && !Phase.FreeMoveAction(action) && !Phase.InspectAction(action))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public void BattleTurnStart(int turn)
		{
			base.enabled = false;
		}

		public bool CorrectAction(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.entity.data.name == target.data.name && actionMove.toContainers.Length == 1)
			{
				return Battle.IsOnBoard(actionMove.toContainers[0].Group);
			}

			return false;
		}
	}

	public Entity chosenUnit;

	public override void BattleStart()
	{
		Object.FindObjectOfType<BattleSaveSystem>()?.Disable();
		chosenUnit = FindChosenUnit();
		string[] nextCardNames = new string[6]
		{
			"SnowStick",
			"Sword",
			chosenUnit.data.name,
			"SnowStick",
			"PinkberryJuice",
			"Sword"
		};
		References.Player.OrderNextCards(nextCardNames);
		phases = new List<Phase>
		{
			new PhaseWaitForLeader(),
			new PhasePlaceCompanion(chosenUnit)
		};
	}

	public static Entity FindChosenUnit()
	{
		CardData chosenUnitData = References.PlayerData.inventory.deck.OrderByDescending((CardData a) => a.hp).FirstOrDefault((CardData a) => !a.cardType.miniboss);
		return Battle.GetCards(References.Player).LastOrDefault((Entity a) => a.data.id == chosenUnitData.id);
	}
}
