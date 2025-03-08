#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialBattleSystem1 : TutorialBattleSystem
{
	public class PhasePlaceLeader : Phase
	{
		public override void OnEnable()
		{
			Events.OnEntityMove += EntityMove;
			new Routine(PromptAfter(2f));
		}

		public override void OnDisable()
		{
			Events.OnEntityMove -= EntityMove;
			PromptSystem.Hide();
		}

		public void EntityMove(Entity entity)
		{
			if (entity.owner == References.Player && entity.data.cardType.miniboss && Battle.IsOnBoard(entity))
			{
				base.enabled = false;
			}
		}

		public IEnumerator PromptAfter(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (base.enabled)
			{
				PromptSystem.Create(Prompt.Anchor.Top, 0f, -2f, 8.7f);
				PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_1.GetLocalizedString());
			}
		}
	}

	public class PhasePlaceCompanion : Phase
	{
		public readonly CardData target;

		public override float delay => 1f;

		public PhasePlaceCompanion(CardData target)
		{
			this.target = target;
		}

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnBattleTurnStart += BattleTurnStart;
			PromptSystem.Create(Prompt.Anchor.TopLeft, 2f, -2f, 8f, Prompt.Emote.Type.Talk);
			PromptSystem.SetTextAction(() => string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_2.GetLocalizedString(), target.title));
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnBattleTurnStart -= BattleTurnStart;
			PromptSystem.Hide();
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
			if (action is ActionMove actionMove && actionMove.entity.data.name == target.name && actionMove.toContainers.Length == 1)
			{
				return Battle.IsOnBoard(actionMove.toContainers[0].Group);
			}

			return false;
		}
	}

	public class PhaseUseItem : Phase
	{
		public override float delay => 1f;

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnBattleTurnStart += BattleTurnStart;
			PromptSystem.Create(Prompt.Anchor.TopLeft, 1f, -2f, 6f, Prompt.Emote.Type.Point);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_3.GetLocalizedString());
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnBattleTurnStart -= BattleTurnStart;
			PromptSystem.Hide();
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
			if (action is ActionTriggerAgainst actionTriggerAgainst && actionTriggerAgainst.entity.data.name == "Sword")
			{
				return actionTriggerAgainst.target.data.name == "Pengoon";
			}

			return false;
		}
	}

	public class PhaseCounters : Phase
	{
		public override float delay => 1f;

		public override void OnEnable()
		{
			Events.OnBattleTurnStart += BattleTurnStart;
			PromptSystem.Create(Prompt.Anchor.Top, 0f, -1.5f, 9f, Prompt.Emote.Type.Explain);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_4.GetLocalizedString());
		}

		public override void OnDisable()
		{
			Events.OnBattleTurnStart -= BattleTurnStart;
			PromptSystem.Hide();
		}

		public void BattleTurnStart(int turn)
		{
			base.enabled = false;
		}
	}

	public class PhaseEnemiesAttackFirst : Phase
	{
		public override float delay => 0f;

		public override void OnEnable()
		{
			Events.OnBattlePhaseStart += BattlePhaseStart;
			Events.OnBattleTurnStart += BattleTurnStart;
		}

		public override void OnDisable()
		{
			Events.OnBattlePhaseStart -= BattlePhaseStart;
			Events.OnBattleTurnStart -= BattleTurnStart;
			PromptSystem.Hide();
		}

		public static void BattlePhaseStart(Battle.Phase phase)
		{
			if (phase == Battle.Phase.Play)
			{
				PromptSystem.Create(Prompt.Anchor.TopRight, -3f, -3f, 4f, Prompt.Emote.Type.Scared);
				PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_41.GetLocalizedString());
			}
		}

		public void BattleTurnStart(int turn)
		{
			base.enabled = false;
		}
	}

	public class PhaseWaitForNewEnemies : Phase
	{
		public override void OnEnable()
		{
			Events.OnActionPerform += ActionPerform;
		}

		public override void OnDisable()
		{
			Events.OnActionPerform -= ActionPerform;
		}

		public void ActionPerform(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.entity.owner == References.Battle.enemy)
			{
				base.enabled = false;
			}
		}
	}

	public class PhaseRedrawBell : Phase
	{
		public override float delay => 1f;

		public override void OnEnable()
		{
			Events.OnCheckAction += CheckAction;
			Events.OnActionPerform += ActionPerform;
			PromptSystem.Create(Prompt.Anchor.TopRight, -1f, -1.25f, 6f, Prompt.Emote.Type.Talk);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_5.GetLocalizedString());
			RedrawBellSystem redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>();
			if ((bool)redrawBellSystem)
			{
				redrawBellSystem.Enable();
				redrawBellSystem.BecomeInteractable();
			}
		}

		public override void OnDisable()
		{
			Events.OnCheckAction -= CheckAction;
			Events.OnActionPerform -= ActionPerform;
			PromptSystem.Hide();
		}

		public static void CheckAction(ref PlayAction action, ref bool allow)
		{
			if (allow && !CorrectAction(action) && !PlayCardAction(action) && !Phase.FreeMoveAction(action) && !Phase.InspectAction(action))
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

		public static bool CorrectAction(PlayAction action)
		{
			return action is ActionRedraw;
		}

		public static bool PlayCardAction(PlayAction action)
		{
			return action is ActionTrigger;
		}
	}

	public class PhaseRedrawBellPopUp : Phase
	{
		public TutorialSystem tutorialSystem;

		public override float delay => 1f;

		public override void OnEnable()
		{
			tutorialSystem = Object.FindObjectOfType<TutorialSystem>();
			Events.OnBattleTurnEnd += Show;
		}

		public override void OnDisable()
		{
			Events.OnBattleTurnEnd -= Show;
		}

		public void Show(int turn)
		{
			HelpPanelSystem.Show(tutorialSystem.redrawBellHelpKey);
			HelpPanelSystem.SetEmote(tutorialSystem.redrawBellHelpEmote);
			HelpPanelSystem.SetImage(1.5f, 1.5f, tutorialSystem.redrawBellHelpSprite);
			HelpPanelSystem.SetBackButtonActive(active: false);
			HelpPanelSystem.AddButton(HelpPanelSystem.ButtonType.Positive, tutorialSystem.redrawBellHelpButtonKey, "Select", End);
		}

		public void End()
		{
			base.enabled = false;
		}
	}

	public class PhaseProtectLeader : Phase
	{
		public override float delay => 1f;

		public override void OnEnable()
		{
			Show();
		}

		public override void OnDisable()
		{
			PromptSystem.Hide();
		}

		public void Show()
		{
			PromptSystem.Create(Prompt.Anchor.Left, 1f, 1f, 4f);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_6.GetLocalizedString());
			Events.OnBattleTurnStart += End;
		}

		public void End(int turn)
		{
			base.enabled = false;
			Events.OnBattleTurnStart -= End;
		}
	}

	public class PhaseWaveDeploy : Phase
	{
		public override float delay => 1f;

		public override void OnEnable()
		{
			Events.OnBattleTurnStart += BattleTurnStart;
			Object.FindObjectOfType<WaveDeploySystem>()?.Show();
			PromptSystem.Create(Prompt.Anchor.TopRight, -3f, -1.75f, 6f, Prompt.Emote.Type.Talk);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_7.GetLocalizedString());
			PromptSystem.Prompt.SetEmotePosition(Prompt.Emote.Position.Above, 2f, 0f, 1f);
		}

		public override void OnDisable()
		{
			Events.OnBattleTurnStart -= BattleTurnStart;
			PromptSystem.Hide();
		}

		public void BattleTurnStart(int turn)
		{
			base.enabled = false;
		}
	}

	public class PhaseWaitForMiniboss : Phase
	{
		public override void OnEnable()
		{
			Events.OnActionPerform += ActionPerform;
		}

		public override void OnDisable()
		{
			Events.OnActionPerform -= ActionPerform;
		}

		public void ActionPerform(PlayAction action)
		{
			if (action is ActionMove actionMove && actionMove.entity.owner == References.Battle.enemy && actionMove.entity.data.cardType.miniboss)
			{
				base.enabled = false;
			}
		}
	}

	public class PhaseMiniboss : Phase
	{
		public override float delay => 1f;

		public override void OnEnable()
		{
			Events.OnEntityKilled += EntityKilled;
			new Routine(PromptAfterDelay(2f));
		}

		public override void OnDisable()
		{
			Events.OnEntityKilled -= EntityKilled;
			PromptSystem.Hide();
		}

		public void EntityKilled(Entity entity, DeathType deathType)
		{
			if (entity.owner == References.Battle.enemy && entity.data.cardType.miniboss)
			{
				base.enabled = false;
			}
		}

		public IEnumerator PromptAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (base.enabled)
			{
				PromptSystem.Create(Prompt.Anchor.TopLeft, 2f, -1.5f, 5f, Prompt.Emote.Type.Talk);
				PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialBattle1_8.GetLocalizedString());
			}
		}
	}

	public static readonly string[] startingHand = new string[6] { "Sword", "SnowStick", "Sword", "Foxee", "Sword", "Sword" };

	public override void BattleStart()
	{
		Object.FindObjectOfType<BattleSaveSystem>()?.Disable();
		CardContainer cardContainer = References.Battle.rows[References.Battle.enemy][0];
		cardContainer.canBePlacedOn = false;
		cardContainer.transform.parent.gameObject.SetActive(value: false);
		Object.FindObjectOfType<RedrawBellSystem>()?.Disable();
		WaveDeploySystem waveDeploySystem = Object.FindObjectOfType<WaveDeploySystem>();
		if ((object)waveDeploySystem != null)
		{
			waveDeploySystem.Hide();
			waveDeploySystem.visible = false;
		}

		References.Player.OrderNextCards(startingHand);
		Entity entity = FindLastUnit();
		phases = new List<Phase>
		{
			new PhasePlaceLeader(),
			new PhasePlaceCompanion(entity.data),
			new PhaseUseItem(),
			new PhaseCounters(),
			new PhaseEnemiesAttackFirst(),
			new PhaseWaitForNewEnemies(),
			new PhaseRedrawBell(),
			new PhaseRedrawBellPopUp(),
			new PhaseProtectLeader(),
			new PhaseWaveDeploy(),
			new PhaseWaitForMiniboss(),
			new PhaseMiniboss()
		};
	}

	public static Entity FindLastUnit()
	{
		CardData chosenUnitData = References.PlayerData.inventory.deck.LastOrDefault((CardData a) => a.cardType.unit);
		return Battle.GetCards(References.Player).LastOrDefault((Entity a) => a.data.id == chosenUnitData.id);
	}

	public override void BattleEnd()
	{
		Object.FindObjectOfType<RedrawBellSystem>()?.Enable();
	}
}
