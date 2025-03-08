#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class DynamicTutorialSystem : GameSystem
{
	[Serializable]
	public class Tutorial
	{
		public bool onlyShowOnce;

		public int turnsRequired;

		public int resetOffset = -50;

		public string saveString;

		public UnityEngine.Localization.LocalizedString stringRef;

		public Prompt.Anchor promptAnchor;

		public Vector2 promptPosition;

		public float promptWidth;

		public Prompt.Emote.Type promptEmote;

		public int flipEmote = 1;

		public int current { get; set; }

		public bool currentBool { get; set; }

		public bool shown { get; set; }

		public bool actionDoneThisTurn { get; set; }

		public void ResetCount()
		{
			current = resetOffset;
			if (shown)
			{
				Hide();
			}
		}

		public void Load()
		{
			if (!onlyShowOnce)
			{
				current = SaveSystem.LoadProgressData(saveString, 0);
			}
		}

		public void Save()
		{
			if (!onlyShowOnce)
			{
				SaveSystem.SaveProgressData(saveString, current);
			}
		}

		public void CheckIncreaseCount()
		{
			if (!onlyShowOnce)
			{
				if (!actionDoneThisTurn)
				{
					current++;
				}

				actionDoneThisTurn = false;
			}
		}

		public bool Check()
		{
			return current >= turnsRequired;
		}

		public void Show(params object[] args)
		{
			shown = true;
			PromptSystem.Create(promptAnchor, promptPosition, promptWidth, promptEmote);
			PromptSystem.Prompt.SetEmotePosition(Prompt.Emote.Position.Above, 0f, 0f, flipEmote);
			if (args.Length != 0)
			{
				PromptSystem.SetTextAction(() => string.Format(stringRef.GetLocalizedString(), args));
			}
			else
			{
				PromptSystem.SetTextAction(() => stringRef.GetLocalizedString());
			}

			current = 0;
		}

		public void Hide()
		{
			shown = false;
			PromptSystem.Hide();
		}
	}

	[SerializeField]
	public Tutorial redrawTutorial;

	[SerializeField]
	public Tutorial moveTutorial;

	[SerializeField]
	public Tutorial recallTutorial;

	[SerializeField]
	public Tutorial aimlessTutorial;

	[SerializeField]
	public Tutorial reactionTutorial;

	public Tutorial[] tutorials;

	public bool aimlessTutorialDone;

	public bool reactionTutorialDone;

	public Entity aimlessEnemy;

	public Entity reactionEnemy;

	public void OnEnable()
	{
		tutorials = new Tutorial[5] { redrawTutorial, moveTutorial, recallTutorial, aimlessTutorial, reactionTutorial };
		Tutorial[] array = tutorials;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Load();
		}

		aimlessTutorialDone = SaveSystem.LoadProgressData("dynamicAimlessTutorial", defaultValue: false);
		reactionTutorialDone = SaveSystem.LoadProgressData("dynamicReactionTutorial", defaultValue: false);
		Events.OnEntityPlace += EntityPlace;
		Events.OnDiscard += Discard;
		Events.OnRedrawBellHit += RedrawBellHit;
		Events.OnBattleTurnStart += TurnStart;
		Events.OnBattleTurnEnd += TurnEnd;
		Events.OnCampaignSaved += Save;
	}

	public void OnDisable()
	{
		Events.OnEntityPlace -= EntityPlace;
		Events.OnDiscard -= Discard;
		Events.OnRedrawBellHit -= RedrawBellHit;
		Events.OnBattleTurnStart -= TurnStart;
		Events.OnBattleTurnEnd -= TurnEnd;
		Events.OnCampaignSaved -= Save;
	}

	public void EntityPlace(Entity entity, CardContainer[] slots, bool freeMove)
	{
		if (freeMove && entity.owner.team == References.Player.team)
		{
			moveTutorial.actionDoneThisTurn = true;
			moveTutorial.ResetCount();
		}
	}

	public void Discard(Entity entity)
	{
		if (entity.data.hasHealth)
		{
			recallTutorial.actionDoneThisTurn = true;
			recallTutorial.ResetCount();
		}
	}

	public void RedrawBellHit(RedrawBellSystem redrawBellSystem)
	{
		if (!redrawBellSystem.IsCharged)
		{
			redrawTutorial.actionDoneThisTurn = true;
			redrawTutorial.ResetCount();
		}
	}

	public void TurnStart(int turnCount)
	{
		Tutorial[] array = tutorials;
		foreach (Tutorial tutorial in array)
		{
			if (tutorial.shown)
			{
				tutorial.Hide();
				break;
			}
		}
	}

	public void TurnEnd(int turnCount)
	{
		Tutorial[] array = tutorials;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CheckIncreaseCount();
		}

		if (!References.Battle.ended && !CheckAimlessTutorial(turnCount) && !CheckReactionTutorial(turnCount) && !CheckMoveTutorial() && !CheckRedrawTutorial())
		{
			CheckRecallTutorial();
		}
	}

	public bool CheckAimlessTutorial(int turnCount)
	{
		if (aimlessTutorialDone)
		{
			return false;
		}

		if (turnCount == 0)
		{
			List<Entity> cardsOnBoard = Battle.GetCardsOnBoard(References.Battle.enemy);
			aimlessEnemy = cardsOnBoard.FirstOrDefault((Entity a) => a.data.traits.Any((CardData.TraitStacks t) => t.data.name == "Aimless"));
			return false;
		}

		if ((bool)aimlessEnemy && aimlessEnemy.IsAliveAndExists() && aimlessEnemy.counter.current == 1)
		{
			aimlessTutorial.Show(aimlessEnemy.data.title);
			SaveSystem.SaveProgressData("dynamicAimlessTutorial", value: true);
			aimlessTutorialDone = true;
			return true;
		}

		return false;
	}

	public bool CheckReactionTutorial(int turnCount)
	{
		if (reactionTutorialDone)
		{
			return false;
		}

		if (turnCount == 0)
		{
			List<Entity> cardsOnBoard = Battle.GetCardsOnBoard(References.Battle.enemy);
			reactionEnemy = cardsOnBoard.FirstOrDefault((Entity a) => a.statusEffects.Any((StatusEffectData s) => s.isReaction));
			if ((bool)reactionEnemy)
			{
				reactionTutorial.Show(reactionEnemy.data.title);
				SaveSystem.SaveProgressData("dynamicReactionTutorial", value: true);
				reactionTutorialDone = true;
				return true;
			}
		}

		return false;
	}

	public bool CheckRecallTutorial()
	{
		if (recallTutorial.Check())
		{
			foreach (Entity item in Battle.GetCardsOnBoard(References.Battle.player))
			{
				if (item.data.hasHealth && (float)item.hp.current <= (float)item.hp.max * 0.5f && item.CanRecall())
				{
					recallTutorial.Show();
					PromptSystem.Prompt.SetEmotePosition(Prompt.Emote.Position.Above, 2f);
					return true;
				}
			}
		}

		return false;
	}

	public bool CheckRedrawTutorial()
	{
		if (redrawTutorial.Check() && References.Battle.turnCount == 0 && Battle.GetCardsOnBoard(References.Battle.player).Count == 1)
		{
			foreach (Entity item in References.Battle.player.handContainer)
			{
				if (item.data.counter > 0)
				{
					return false;
				}
			}

			int num = References.Battle.player.handContainer.max - References.Battle.player.handContainer.Count;
			int count = References.Battle.player.drawContainer.Count;
			for (int i = 0; i < num; i++)
			{
				int num2 = count - 1 - i;
				if (num2 >= 0 && References.Battle.player.drawContainer[num2].data.counter > 0)
				{
					return false;
				}
			}

			redrawTutorial.Show();
			PromptSystem.Prompt.SetEmotePosition(Prompt.Emote.Position.Above, 1.5f, 0f, 1f);
			return true;
		}

		return false;
	}

	public bool CheckMoveTutorial()
	{
		if (moveTutorial.Check())
		{
			bool flag = false;
			foreach (Entity item in Battle.GetCardsOnBoard(References.Battle.player))
			{
				if (item.statusEffects.All((StatusEffectData a) => a.name != "Unmovable"))
				{
					flag = true;
					break;
				}
			}

			if (flag)
			{
				moveTutorial.Show();
				return true;
			}
		}

		return false;
	}

	public void Save()
	{
		Tutorial[] array = tutorials;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Save();
		}
	}
}
