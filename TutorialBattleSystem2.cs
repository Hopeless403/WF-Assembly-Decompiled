#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialBattleSystem2 : TutorialBattleSystem
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

	public class PhaseInspectEnemy : Phase
	{
		public Entity target;

		public override float delay => 0.2f;

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnInspect += Inspect;
			Events.OnInspectEnd += InspectEnd;
			target = References.Battle.GetRow(References.Battle.enemy, 0).GetTop();
			PromptSystem.Create(Prompt.Anchor.TopLeft, 1.5f, -2f, 5f, Prompt.Emote.Type.Point);
			PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<Cursor3d>.instance.usingMouse ? ControllerButtonSystem.ProcessActionTags(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_1) : ControllerButtonSystem.ProcessActionTags(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_1Gamepad), target.data.title));
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnInspect -= Inspect;
			Events.OnInspectEnd -= InspectEnd;
			PromptSystem.Hide();
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (allow && !CorrectAction(action) && !Phase.FreeMoveAction(action))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public void Inspect(Entity entity)
		{
			PromptSystem.Hide();
		}

		public void InspectEnd(Entity entity)
		{
			base.enabled = false;
		}

		public bool CorrectAction(PlayAction action)
		{
			if (action is ActionInspect actionInspect)
			{
				return actionInspect.entity == target;
			}

			return false;
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
			new Routine(PromptAfter(1f));
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
				PromptSystem.Create(Prompt.Anchor.TopLeft, 1.5f, -2f, 5f);
				PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_2.GetLocalizedString(), target.data.title));
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
			if (action is ActionMove actionMove && actionMove.entity.data.name == target.data.name && actionMove.toContainers.Length == 1 && Battle.IsOnBoard(actionMove.toContainers[0].Group))
			{
				return actionMove.toContainers[0].Group.Count == 0;
			}

			return false;
		}
	}

	public class PhaseWait : Phase
	{
		public int turns;

		public PhaseWait(int turns)
		{
			this.turns = turns;
		}

		public override void OnEnable()
		{
			Events.OnBattleTurnEnd += End;
		}

		public override void OnDisable()
		{
			Events.OnBattleTurnEnd -= End;
		}

		public void End(int turn)
		{
			if (--turns <= 0)
			{
				base.enabled = false;
			}
		}
	}

	public class PhaseWaitDisallowRecall : Phase
	{
		public int turns;

		public PhaseWaitDisallowRecall(int turns)
		{
			this.turns = turns;
		}

		public override void OnEnable()
		{
			Events.OnBattleTurnEnd += End;
			Events.OnCheckAction += CheckAction;
		}

		public override void OnDisable()
		{
			Events.OnBattleTurnEnd -= End;
			Events.OnCheckAction -= CheckAction;
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (IsRecallAction(action) || IsEarlyDeployAction(action))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public bool IsRecallAction(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.toContainers.Length == 1)
			{
				return actionMove.toContainers[0] == References.Player.discardContainer;
			}

			return false;
		}

		public bool IsEarlyDeployAction(PlayAction action)
		{
			return action is ActionEarlyDeploy;
		}

		public void End(int turn)
		{
			if (--turns <= 0)
			{
				base.enabled = false;
			}
		}
	}

	public class PhaseMoveCompanionInFrontOfLeader : Phase
	{
		public Entity leader;

		public Entity target;

		public CardSlot leaderSlot;

		public CardSlot targetSlot;

		public PhaseMoveCompanionInFrontOfLeader(Entity target)
		{
			this.target = target;
		}

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnActionPerform += ActionPerform;
			foreach (Entity item in Battle.GetCardsOnBoard(References.Player))
			{
				if (item.data.cardType.miniboss)
				{
					leader = item;
					break;
				}
			}

			CardSlotLane cardSlotLane = leader.containers[0] as CardSlotLane;
			int num = cardSlotLane.IndexOf(leader);
			leaderSlot = cardSlotLane.slots[num];
			int index = Mathf.Max(0, num - 1);
			targetSlot = cardSlotLane.slots[index];
			Debug.Log($"Leader is in [{leaderSlot}]");
			Debug.Log($"[{target.data.title}] SHOULD move to [{targetSlot}]");
			if (targetSlot.GetTop() == target)
			{
				base.enabled = false;
				Debug.Log($"[{target.data.title}] is already in [{targetSlot}]");
				Object.FindObjectOfType<TutorialBattleSystem2>()?.InsertPhase(new PhaseWait(1));
				return;
			}

			PromptSystem.Create(Prompt.Anchor.Right, -2f, 1f, 5f, Prompt.Emote.Type.Talk);
			PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_3.GetLocalizedString(), target.data.title));
			Object.FindObjectOfType<TutorialBattleSystem2>()?.InsertPhase(new PhaseFreeMove());
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnActionPerform -= ActionPerform;
			PromptSystem.Hide();
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (allow && !CorrectAction(action) && !Phase.InspectAction(action))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public void ActionPerform(PlayAction action)
		{
			if (CorrectAction(action))
			{
				base.enabled = false;
			}
		}

		public bool CorrectAction(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.entity.data.name == target.data.name && actionMove.toContainers.Length == 1 && actionMove.toContainers[0] is CardSlot cardSlot)
			{
				return cardSlot == targetSlot;
			}

			return false;
		}
	}

	public class PhaseBarrage : Phase
	{
		public Entity unitToMove;

		public readonly Entity chosenUnit;

		public Entity barrageEnemy;

		public PlayAction correctAction;

		public PhaseBarrage(Entity chosenUnit)
		{
			this.chosenUnit = chosenUnit;
		}

		public override void OnEnable()
		{
			barrageEnemy = Battle.GetCardsOnBoard(References.Battle.enemy).FirstOrDefault((Entity a) => a.data.traits.Any((CardData.TraitStacks t) => t.data.name == "Barrage"));
			if (!barrageEnemy)
			{
				base.enabled = false;
			}
			else
			{
				int rowIndex = References.Battle.GetRowIndex(barrageEnemy.containers[0]);
				int rowIndex2 = References.Battle.GetRowIndex(chosenUnit.containers[0]);
				if (rowIndex != rowIndex2)
				{
					PromptSystem.Create(Prompt.Anchor.Left, 0.1f, 1f, 5f, Prompt.Emote.Type.Point);
					PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_41a.GetLocalizedString(), barrageEnemy.data.title, chosenUnit.data.title));
					unitToMove = chosenUnit;
				}
				else
				{
					PromptSystem.Create(Prompt.Anchor.Left, 0.1f, 1f, 5f, Prompt.Emote.Type.Point);
					PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_41b.GetLocalizedString(), barrageEnemy.data.title, chosenUnit.data.title));
					foreach (Entity item in Battle.GetCardsOnBoard(References.Player))
					{
						if (item.data.cardType.miniboss)
						{
							unitToMove = item;
							break;
						}
					}
				}
			}

			Events.OnCheckAction += CheckAction;
			Events.OnActionPerform += ActionPerform;
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnActionPerform -= ActionPerform;
			PromptSystem.Hide();
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (CorrectAction(action))
			{
				correctAction = action;
				return;
			}

			allow = false;
			PromptSystem.Shake();
		}

		public void ActionPerform(PlayAction action)
		{
			if (correctAction != null && action == correctAction)
			{
				base.enabled = false;
			}
		}

		public bool CorrectAction(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.entity == unitToMove && actionMove.toContainers.Length == 1)
			{
				CardContainer cardContainer = actionMove.toContainers[0];
				if ((object)cardContainer != null && Battle.IsOnBoard(cardContainer.Group))
				{
					return unitToMove.containers[0] != cardContainer.Group;
				}
			}

			return false;
		}
	}

	public class PhaseFreeMove : Phase
	{
		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnBattleTurnStart += HidePrompt;
			Events.OnBattleTurnEnd += End;
			PromptSystem.Create(Prompt.Anchor.Right, -2f, -1f, 5f, Prompt.Emote.Type.Point);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_4.GetLocalizedString());
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnBattleTurnStart -= HidePrompt;
			Events.OnBattleTurnEnd -= End;
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (allow && IllegalAction(action))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public void HidePrompt(int value)
		{
			PromptSystem.Hide();
		}

		public void End(int value)
		{
			base.enabled = false;
		}

		public bool IllegalAction(PlayAction action)
		{
			if (Phase.FreeMoveAction(action))
			{
				return true;
			}

			if (action is ActionEarlyDeploy)
			{
				return true;
			}

			if (action is ActionMove actionMove && actionMove.toContainers.Length == 1 && actionMove.toContainers[0] == actionMove.entity.owner.discardContainer)
			{
				return true;
			}

			return false;
		}
	}

	public class PhaseRecallToHeal : Phase
	{
		public Entity target;

		public WaveDeploySystemOverflow disabledWaveDeploySystem;

		public PhaseRecallToHeal(Entity target)
		{
			this.target = target;
		}

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnActionPerform += ActionPerform;
			PromptSystem.Create(Prompt.Anchor.Right, 0f, 1f, 5f);
			PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_5.GetLocalizedString(), target.data.title));
			disabledWaveDeploySystem = Object.FindObjectOfType<WaveDeploySystemOverflow>();
			if ((bool)disabledWaveDeploySystem)
			{
				disabledWaveDeploySystem.navigationItem.enabled = false;
			}
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnActionPerform -= ActionPerform;
			PromptSystem.Hide();
			if ((bool)disabledWaveDeploySystem)
			{
				disabledWaveDeploySystem.navigationItem.enabled = true;
			}
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (allow && !CorrectAction(action) && !Phase.FreeMoveAction(action) && !Phase.InspectAction(action) && !DiscardHealAction(action))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public void ActionPerform(PlayAction action)
		{
			if (CorrectAction(action))
			{
				base.enabled = false;
			}
		}

		public bool CorrectAction(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.entity.data.name == target.data.name && actionMove.toContainers.Length == 1)
			{
				return actionMove.toContainers[0] == References.Player.discardContainer;
			}

			return false;
		}

		public static bool DiscardHealAction(PlayAction action)
		{
			return action is ActionDiscardEffect;
		}
	}

	public class PhaseRecallFree : Phase
	{
		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnBattleTurnStart += End;
			Events.OnBattleTurnEnd += End;
			MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_6.GetLocalizedString();
			PromptSystem.Create(Prompt.Anchor.Right, -2f, -1f, 5f, Prompt.Emote.Type.Happy);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle2_6.GetLocalizedString());
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnBattleTurnStart -= End;
			Events.OnBattleTurnEnd -= End;
			PromptSystem.Hide();
		}

		public void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (action is ActionEarlyDeploy)
			{
				allow = false;
				PromptSystem.Shake();
			}
		}

		public void End(int value)
		{
			base.enabled = false;
		}
	}

	public Entity chosenUnit;

	public override void BattleStart()
	{
		Object.FindObjectOfType<BattleSaveSystem>()?.Disable();
		chosenUnit = FindChosenUnit();
		string[] nextCardNames = new string[6]
		{
			"Sword",
			"PinkberryJuice",
			chosenUnit.data.name,
			"Sword",
			"Sword",
			"Sword"
		};
		References.Player.OrderNextCards(nextCardNames);
		phases = new List<Phase>
		{
			new PhaseWaitForLeader(),
			new PhaseInspectEnemy(),
			new PhasePlaceCompanion(chosenUnit),
			new PhaseWaitDisallowRecall(2),
			new PhaseMoveCompanionInFrontOfLeader(chosenUnit),
			new PhaseBarrage(chosenUnit),
			new PhaseWaitDisallowRecall(1),
			new PhaseRecallToHeal(chosenUnit),
			new PhaseRecallFree()
		};
	}

	public void InsertPhase(Phase phase)
	{
		phases.Insert(0, phase);
	}

	public static Entity FindChosenUnit()
	{
		CardData chosenUnitData = References.PlayerData.inventory.deck.LastOrDefault((CardData a) => a.cardType.unit);
		return Battle.GetCards(References.Player).LastOrDefault((Entity a) => a.data.id == chosenUnitData.id);
	}
}
