#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dead;
using Deadpan.Enums.Engine.Components.Modding;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Console : MonoBehaviourSingleton<Console>
{
	public abstract class Command
	{
		public bool success;

		public string[] predictedArgs;

		public virtual string id => "";

		public virtual string desc => "";

		public virtual string format => id;

		public virtual bool hidden => false;

		public virtual bool logOnSuccess => true;

		public string failMessage { get; set; }

		public virtual bool IsRoutine => false;

		public virtual void Run(string args)
		{
		}

		public virtual IEnumerator Routine(string args)
		{
			return null;
		}

		public virtual IEnumerator GetArgOptions(string currentArgs)
		{
			return null;
		}

		public void Fail(string message)
		{
			success = false;
			failMessage = failMessage;
			LogError(message);
		}

		public void FailCannotUse()
		{
			Fail("Cannot use [" + id + "] command here");
		}

		public bool TryGetPlayer(out Character player, bool doFail = true)
		{
			player = null;
			if (!Campaign.instance)
			{
				if (doFail)
				{
					FailCannotUse();
				}

				return false;
			}

			player = References.Player;
			if (!player)
			{
				if (doFail)
				{
					FailCannotUse();
				}

				return false;
			}

			return true;
		}

		public static string[] Split(string text)
		{
			if (text.Length <= 0)
			{
				return new string[1] { "" };
			}

			return text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

		public Command()
		{
		}
	}

	public class CommandHelp : Command
	{
		public override string id => "help";

		public override bool hidden => true;

		public override bool logOnSuccess => false;

		public override void Run(string args)
		{
			ToggleHelp();
		}
	}

	public class CommandRepeat : Command
	{
		public override string id => "repeat";

		public override string format => "repeat <times>";

		public override bool logOnSuccess => false;

		public override void Run(string args)
		{
			if (previous.Count > 0)
			{
				int result = 1;
				if (args.Length > 0)
				{
					int.TryParse(args, out result);
				}

				new Routine(Repeat(previous[0], result));
			}
		}

		public static IEnumerator Repeat(string command, int repeats)
		{
			while (repeats > 0)
			{
				repeats--;
				yield return HandleCommand(command);
			}
		}
	}

	public class CommandGainCard : Command
	{
		public override string id => "gain card";

		public override string format => "gain card <name>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			if (args.Length > 0)
			{
				if (!TryGetPlayer(out var player))
				{
					yield break;
				}

				yield return AddressableLoader.LoadGroup("CardData");
				IEnumerable<CardData> source = from a in AddressableLoader.GetGroup<CardData>("CardData")
					where string.Equals(a.name, args, StringComparison.CurrentCultureIgnoreCase)
					select a;
				if (source.Any())
				{
					CardData cardData = source.First();
					if ((object)cardData != null)
					{
						CardData cardData2 = cardData.Clone();
						player.data.inventory.deck.Add(cardData2);
						if ((bool)Battle.instance && (bool)player.handContainer)
						{
							Card card = CardManager.Get(cardData2, Battle.instance.playerCardController, player, inPlay: true, isPlayerCard: true);
							card.entity.flipper.FlipDownInstant();
							card.transform.localPosition = new Vector3(-100f, 0f, 0f);
							yield return card.UpdateData();
							player.handContainer.Add(card.entity);
							player.handContainer.TweenChildPositions();
							ActionQueue.Add(new ActionReveal(card.entity));
							ActionQueue.Add(new ActionRunEnableEvent(card.entity));
							yield return ActionQueue.Wait();
						}

						yield break;
					}
				}

				Fail("Card [" + args + "] does not exist!");
			}
			else
			{
				Fail("You must provide a card name");
			}
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return AddressableLoader.LoadGroup("CardData");
			IEnumerable<CardData> source = from a in AddressableLoader.GetGroup<CardData>("CardData")
				where a.name.ToLower().Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((CardData cardData) => cardData.name).ToArray();
		}
	}

	public class CommandGainUpgrade : Command
	{
		public override string id => "gain upgrade";

		public override string format => "gain upgrade <name>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			if (args.Length > 0)
			{
				if (TryGetPlayer(out var player, doFail: false))
				{
					yield return AddressableLoader.LoadGroup("CardUpgradeData");
					try
					{
						CardUpgradeData cardUpgradeData = AddressableLoader.GetGroup<CardUpgradeData>("CardUpgradeData").First((CardUpgradeData a) => string.Equals(a.name, args, StringComparison.CurrentCultureIgnoreCase));
						player.data.inventory.upgrades.Add(cardUpgradeData.Clone());
					}
					catch
					{
						Fail("Upgrade [" + args + "] does not exist!");
					}
					yield break;
				}

				GameObject gameObject = GameObject.FindWithTag("CharmHolder");
				if ((object)gameObject == null)
				{
					yield break;
				}

				CardCharmHolder cardCharmHolder = gameObject.GetComponent<CardCharmHolder>();
				if ((object)cardCharmHolder == null)
				{
					yield break;
				}

				yield return AddressableLoader.LoadGroup("CardUpgradeData");
				try
				{
					CardUpgradeData cardUpgradeData2 = AddressableLoader.GetGroup<CardUpgradeData>("CardUpgradeData").First((CardUpgradeData a) => string.Equals(a.name, args, StringComparison.CurrentCultureIgnoreCase));
					cardCharmHolder.Create(cardUpgradeData2.Clone());
					cardCharmHolder.SetPositions();
				}
				catch
				{
					Fail("Upgrade [" + args + "] does not exist!");
				}
			}
			else
			{
				Fail("You must provide an upgrade name");
			}
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return AddressableLoader.LoadGroup("CardUpgradeData");
			IEnumerable<CardUpgradeData> enumerable = from a in AddressableLoader.GetGroup<CardUpgradeData>("CardUpgradeData")
				where a.name.ToLower().Contains(currentArgs.ToLower())
				select a;
			List<string> list = new List<string>();
			foreach (CardUpgradeData item in enumerable)
			{
				list.Add(item.name);
			}

			predictedArgs = list.ToArray();
		}
	}

	public class CommandGainGold : Command
	{
		public override string id => "gain blings";

		public override string format => "gain blings <amount>";

		public override void Run(string args)
		{
			int result = 10;
			Character player;
			if (args.Length > 0 && !int.TryParse(args, out result))
			{
				Fail("Invalid amount! (" + args + ")");
			}
			else if (TryGetPlayer(out player))
			{
				player.GainGold(result);
			}
		}
	}

	public class CommandSpawn : Command
	{
		public override string id => "spawn";

		public override string format => "spawn <unit>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			if (!References.Battle)
			{
				Fail("Must be in battle to use this command");
				yield break;
			}

			if (args.Length <= 0)
			{
				Fail("You must provide a card name");
				yield break;
			}

			if (!slotHover)
			{
				Fail("You must hover over a slot to use this command");
				yield break;
			}

			if (!slotHover.Empty)
			{
				Fail("That slot is not empty!");
				yield break;
			}

			yield return AddressableLoader.LoadGroup("CardData");
			IEnumerable<CardData> source = from a in AddressableLoader.GetGroup<CardData>("CardData")
				where a.cardType.unit && string.Equals(a.name, args, StringComparison.CurrentCultureIgnoreCase)
				select a;
			if (source.Any())
			{
				CardData cardData = source.First();
				if ((object)cardData != null)
				{
					CardData data = cardData.Clone();
					Card card = CardManager.Get(data, References.Battle.playerCardController, slotHover.owner, inPlay: true, slotHover.owner.team == References.Player.team);
					card.entity.flipper.FlipDownInstant();
					card.transform.localPosition = new Vector3(-100f, 0f, 0f);
					yield return card.UpdateData();
					slotHover.Add(card.entity);
					slotHover.TweenChildPositions();
					ActionQueue.Add(new ActionReveal(card.entity));
					ActionQueue.Add(new ActionRunEnableEvent(card.entity));
					yield return ActionQueue.Wait();
					yield break;
				}
			}

			Fail("Card [" + args + "] does not exist!");
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return AddressableLoader.LoadGroup("CardData");
			IEnumerable<CardData> source = from a in AddressableLoader.GetGroup<CardData>("CardData")
				where a.cardType.unit && a.name.ToLower().Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((CardData cardData) => cardData.name).ToArray();
		}
	}

	public class CommandBattleWin : Command
	{
		public override string id => "battle win";

		public override void Run(string args)
		{
			if (Battle.instance == null)
			{
				FailCannotUse();
			}
			else if (Battle.instance.phase == Battle.Phase.End)
			{
				Fail("The battle is already over!");
			}
			else
			{
				Battle.instance.PlayerWin();
			}
		}
	}

	public class CommandBattleLose : Command
	{
		public override string id => "battle lose";

		public override void Run(string args)
		{
			if (Battle.instance == null)
			{
				FailCannotUse();
			}
			else if (Battle.instance.phase == Battle.Phase.End)
			{
				Fail("The battle is already over!");
			}
			else
			{
				Battle.instance.EnemyWin();
			}
		}
	}

	public class CommandBattleSkip : Command
	{
		public override string id => "battle skip";

		public override string desc => "to next wave";

		public override void Run(string args)
		{
			if (Battle.instance == null)
			{
				FailCannotUse();
				return;
			}

			if (Battle.instance.phase == Battle.Phase.End)
			{
				Fail("The battle is already over!");
				return;
			}

			foreach (Entity item in Battle.GetCardsOnBoard(Battle.instance.enemy))
			{
				item.RemoveFromContainers();
				CardManager.ReturnToPool(item);
			}

			ActionQueue.Add(new ActionEndTurn(Battle.instance.player));
			Battle.instance.playerCardController.Disable();
			CardPopUp.Clear();
		}
	}

	public class CommandBattleAuto : Command
	{
		public override string id => "battle auto";

		public override string desc => "play out the rest of the battle automatically";

		public override void Run(string args)
		{
			if (!References.Battle)
			{
				FailCannotUse();
			}
			else if (References.Battle.phase == Battle.Phase.End)
			{
				Fail("The battle is already over!");
			}
			else
			{
				References.Battle.auto = !References.Battle.auto;
			}
		}
	}

	public class CommandSkipTurn : Command
	{
		public override string id => "skip turn";

		public override void Run(string args)
		{
			if (Battle.instance == null)
			{
				FailCannotUse();
				return;
			}

			if (Battle.instance.phase == Battle.Phase.End)
			{
				Fail("The battle is already over!");
				return;
			}

			ActionQueue.Add(new ActionEndTurn(Battle.instance.player));
			Battle.instance.playerCardController.Disable();
			CardPopUp.Clear();
		}
	}

	public class CommandSetHealth : Command
	{
		public override string id => "set health";

		public override string format => "set health <value>";

		public override void Run(string args)
		{
			if (args.Length < 1)
			{
				Fail("You must provide a value");
				return;
			}

			if (hover == null)
			{
				Fail("Please hover over a card to use this command");
				return;
			}

			if (!int.TryParse(args, out var result) || result <= 0)
			{
				Fail("Invalid value! (" + args + ")");
				return;
			}

			if (!hover.enabled || !hover.data.hasHealth)
			{
				Fail("Cannot use on this card");
				return;
			}

			hover.hp.current = result;
			hover.hp.max = Mathf.Max(hover.hp.max, hover.hp.current);
			hover.PromptUpdate();
		}
	}

	public class CommandSetAttack : Command
	{
		public override string id => "set attack";

		public override string format => "set attack <value>";

		public override void Run(string args)
		{
			if (args.Length < 1)
			{
				Fail("You must provide a value");
				return;
			}

			if (hover == null)
			{
				Fail("Please hover over a card to use this command");
				return;
			}

			if (!int.TryParse(args, out var result) || result <= 0)
			{
				Fail("Invalid value! (" + args + ")");
				return;
			}

			if (!hover.enabled || !hover.data.hasAttack)
			{
				Fail("Cannot use on this card");
				return;
			}

			hover.damage.current = result;
			hover.damage.max = result;
			hover.PromptUpdate();
		}
	}

	public class CommandSetCounter : Command
	{
		public override string id => "set counter";

		public override string format => "set counter <value>";

		public override void Run(string args)
		{
			if (args.Length < 1)
			{
				Fail("You must provide a value");
				return;
			}

			if (hover == null)
			{
				Fail("Please hover over a card to use this command");
				return;
			}

			if (!int.TryParse(args, out var result) || result <= 0)
			{
				Fail("Invalid value! (" + args + ")");
				return;
			}

			if (!hover.enabled || !hover.data.hasAttack)
			{
				Fail("Cannot use on this card");
				return;
			}

			hover.counter.current = result;
			hover.counter.max = Mathf.Max(hover.counter.max, result);
			hover.PromptUpdate();
		}
	}

	public class CommandAddStatus : Command
	{
		public override string id => "add status";

		public override string format => "add status <name>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			if (args.Length > 0)
			{
				if (hover != null)
				{
					if (hover.enabled)
					{
						string[] array = Command.Split(args);
						string statusName = array[0];
						int count = 1;
						if (array.Length > 1)
						{
							int.TryParse(array[1], out count);
						}

						yield return AddressableLoader.LoadGroup("StatusEffectData");
						IEnumerable<StatusEffectData> source = from a in AddressableLoader.GetGroup<StatusEffectData>("StatusEffectData")
							where a.visible && !a.name.Contains(' ') && string.Equals(a.name, statusName, StringComparison.CurrentCultureIgnoreCase)
							select a;
						if (source.Any())
						{
							StatusEffectData statusEffectData = source.First();
							if ((object)statusEffectData != null)
							{
								yield return StatusEffectSystem.Apply(hover, null, statusEffectData, count);
								yield break;
							}
						}

						Fail("StatusEffect [" + statusName + "] does not exist!");
					}
					else
					{
						Fail("Cannot use on that card");
					}
				}
				else
				{
					Fail("Please hover over a card to use this command");
				}
			}
			else
			{
				Fail("You must provide a StatusEffect name");
			}
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return AddressableLoader.LoadGroup("StatusEffectData");
			IEnumerable<StatusEffectData> source = from a in AddressableLoader.GetGroup<StatusEffectData>("StatusEffectData")
				where a.visible && !a.name.Contains(' ') && a.name.ToLower().Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((StatusEffectData effectData) => effectData.name).ToArray();
		}
	}

	public class CommandAddUpgrade : Command
	{
		public override string id => "add upgrade";

		public override string format => "add upgrade <name>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			if (args.Length > 0)
			{
				if (hover != null)
				{
					yield return AddressableLoader.LoadGroup("CardUpgradeData");
					IEnumerable<CardUpgradeData> source = from a in AddressableLoader.GetGroup<CardUpgradeData>("CardUpgradeData")
						where a.name.ToLower() == args.ToLower()
						select a;
					if (source.Any())
					{
						CardUpgradeData cardUpgradeData = source.First();
						if ((object)cardUpgradeData != null)
						{
							if (cardUpgradeData.CanAssign(hover))
							{
								yield return cardUpgradeData.Clone().Assign(hover);
								yield break;
							}

							Fail("Upgrade [" + cardUpgradeData.name + "] cannot be assigned to [" + hover.data.title + "]");
							yield break;
						}
					}

					Fail("Upgrade [" + args + "] does not exist!");
				}
				else
				{
					Fail("Please hover over a card to use this command");
				}
			}
			else
			{
				Fail("You must provide an upgrade name");
			}
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return AddressableLoader.LoadGroup("CardUpgradeData");
			IEnumerable<CardUpgradeData> source = from a in AddressableLoader.GetGroup<CardUpgradeData>("CardUpgradeData")
				where a.name.ToLower().Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((CardUpgradeData upgradeData) => upgradeData.name).ToArray();
		}
	}

	public class CommandDestroy : Command
	{
		public override string id => "destroy";

		public override void Run(string args)
		{
			if (hover == null)
			{
				Fail("Please hover over a card to use this command");
				return;
			}

			if (!hover.enabled)
			{
				Fail("Cannot destroy this card");
				return;
			}

			hover.RemoveFromContainers();
			CardManager.ReturnToPool(hover);
			CardPopUp.Clear();
		}
	}

	public class CommandDestroyAll : Command
	{
		public override string id => "destroy all";

		public override void Run(string args)
		{
			if (Battle.instance == null)
			{
				Fail("Must be in battle to use this command");
				return;
			}

			foreach (Entity item in Battle.GetCardsOnBoard(Battle.instance.enemy))
			{
				item.RemoveFromContainers();
				CardManager.ReturnToPool(item);
			}

			ActionQueue.Add(new ActionEndTurn(Battle.instance.player));
			Battle.instance.playerCardController.Disable();
			CardPopUp.Clear();
		}
	}

	public class CommandHit : Command
	{
		public override string id => "hit";

		public override string format => "hit <damage>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			if (!hover)
			{
				Fail("Please hover over a card to use this command");
				yield break;
			}

			int result = 1;
			if (args.Length > 0)
			{
				int.TryParse(args, out result);
			}

			Character player = References.Player;
			if ((object)player != null && (bool)player.entity)
			{
				Hit hit = new Hit(player.entity, hover, result)
				{
					canRetaliate = false
				};
				yield return hit.Process();
			}
			else
			{
				FailCannotUse();
			}
		}
	}

	public class CommandKill : Command
	{
		public override string id => "kill";

		public override void Run(string args)
		{
			if (hover == null)
			{
				Fail("Please hover over a card to use this command");
				return;
			}

			if (!hover.enabled)
			{
				Fail("Cannot kill this card");
				return;
			}

			hover.forceKill = DeathType.Normal;
			hover.PromptUpdate();
		}
	}

	public class CommandKillAll : Command
	{
		public override string id => "kill all";

		public override void Run(string args)
		{
			if (Battle.instance == null)
			{
				Fail("Must be in battle to use this command");
				return;
			}

			foreach (Entity item in Battle.GetCardsOnBoard(Battle.instance.enemy))
			{
				item.forceKill = DeathType.Normal;
				item.PromptUpdate();
			}

			ActionQueue.Add(new ActionEndTurn(Battle.instance.player));
			Battle.instance.playerCardController.Disable();
		}
	}

	public class CommandReroll : Command
	{
		public override string id => "reroll";

		public override string desc => "new leaders, or card rewards";

		public override void Run(string args)
		{
			foreach (IRerollable item in UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<IRerollable>())
			{
				Debug.Log($"Rerolling [{item}]");
				if (item.Reroll())
				{
					return;
				}
			}

			Fail("Nothing to reroll");
		}
	}

	public class CommandSystemDisable : Command
	{
		public override string id => "system disable";

		public override string format => "system disable <name>";

		public override void Run(string args)
		{
			if (args.Length <= 0)
			{
				Fail("You must provide a system name");
				return;
			}

			Type type = Type.GetType(args + ",Assembly-CSharp");
			if (type == null)
			{
				Fail("System '" + args + "' not found! (it's case sensitive)");
				return;
			}

			MonoBehaviour monoBehaviour = UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour;
			if (monoBehaviour == null)
			{
				Fail("System '" + args + "' does not exist!");
				return;
			}

			if (!monoBehaviour.enabled)
			{
				Fail("'" + args + "' is already disabled");
			}

			monoBehaviour.enabled = false;
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return null;
			IEnumerable<GameSystem> source = from a in UnityEngine.Object.FindObjectsOfType<GameSystem>()
				where a.enabled && a.GetType().ToString().ToLower()
					.Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((GameSystem s) => s.GetType().ToString()).ToArray();
		}
	}

	public class CommandSystemEnable : Command
	{
		public override string id => "system enable";

		public override string format => "system enable <name>";

		public override void Run(string args)
		{
			if (args.Length <= 0)
			{
				Fail("You must provide a system name");
				return;
			}

			Type type = Type.GetType(args + ",Assembly-CSharp");
			if (type == null)
			{
				Fail("System '" + args + "' not found! (it's case sensitive)");
				return;
			}

			MonoBehaviour monoBehaviour = UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour;
			if (monoBehaviour == null)
			{
				Fail("System '" + args + "' does not exist!");
				return;
			}

			if (monoBehaviour.enabled)
			{
				Fail("'" + args + "' is already enabled");
			}

			monoBehaviour.enabled = true;
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return null;
			IEnumerable<GameSystem> source = from a in UnityEngine.Object.FindObjectsOfType<GameSystem>()
				where !a.enabled && a.GetType().ToString().ToLower()
					.Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((GameSystem s) => s.GetType().ToString()).ToArray();
		}
	}

	public class CommandGameSpeed : Command
	{
		public override string id => "gamespeed";

		public override string format => "gamespeed <value>";

		public override void Run(string args)
		{
			float result;
			if (args.Length <= 0)
			{
				Fail("You must provide a value (1 = normal speed, 2 = double speed, 0.5 = half speed)");
			}
			else if (float.TryParse(args, out result) && result >= 0f)
			{
				Events.InvokeTimeScaleChange(result);
			}
			else
			{
				Fail("Invalid value! (" + args + ")");
			}
		}
	}

	public class CommandMapJump : Command
	{
		public override string id => "map jump";

		public override string desc => "to the selected map node";

		public override void Run(string args)
		{
			MapNode[] array = UnityEngine.Object.FindObjectsOfType<MapNode>();
			if (array.Length == 0)
			{
				FailCannotUse();
				return;
			}

			MapNode mapNode = array.FirstOrDefault((MapNode n) => n.IsHovered);
			if (mapNode == null)
			{
				Fail("You must be hovering over a map node");
				return;
			}

			Character player = References.Player;
			if ((object)player != null)
			{
				MapNew.MoveTo(player, mapNode);
				if (!mapNode.campaignNode.type.canSkip)
				{
					mapNode.campaignNode.SetCleared();
				}

				mapNode.map.Continue(forceCanSkip: true);
			}
			else
			{
				Fail("Player does not exist!");
			}
		}
	}

	public class CommandMapInfo : Command
	{
		public override string id => "map info";

		public override string format => "map info";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			MapNode[] array = UnityEngine.Object.FindObjectsOfType<MapNode>();
			if (array.Length == 0)
			{
				FailCannotUse();
				return;
			}

			MapNode mapNode = array.FirstOrDefault((MapNode n) => n.IsHovered);
			if (!mapNode)
			{
				Fail("You must be hovering over a map node");
				return;
			}

			Debug.Log($"[{mapNode.campaignNode.name} {mapNode.campaignNode.id}] info:");
			foreach (KeyValuePair<string, object> datum in mapNode.campaignNode.data)
			{
				object value = datum.Value;
				if (!(value is ICollection<string> values))
				{
					if (value is SaveCollection<string> saveCollection)
					{
						Debug.Log(datum.Key + ": " + string.Join(", ", saveCollection.collection));
					}
					else
					{
						Debug.Log($"{datum.Key}: {datum.Value}");
					}
				}
				else
				{
					Debug.Log(datum.Key + ": " + string.Join(", ", values));
				}
			}
		}
	}

	public class CommandSetSaveProfile : Command
	{
		public override string id => "save profile";

		public override string format => "save profile <name>";

		public override string desc => "switch save profile";

		public override void Run(string args)
		{
			if (Campaign.instance == null)
			{
				SaveSystem.SetProfile(args);
			}
			else
			{
				Fail("Cannot switch save profile here!");
			}
		}
	}

	public class CommandVolume : Command
	{
		public readonly string busName;

		public readonly string internalId = "volume";

		public readonly string internalFormat = "volume <0-1>";

		public override string id => internalId;

		public override string format => internalFormat;

		public CommandVolume(string busName = "Master")
		{
			string text = busName.ToLower();
			this.busName = ((text == "sfx") ? "SFX" : text.ToUpperFirstLetter());
			if (text != "master")
			{
				internalId = "volume " + text;
				internalFormat = internalId + " <0-1>";
			}
		}

		public override void Run(string args)
		{
			float result;
			if (args.Length <= 0)
			{
				Fail("You must provide a value between 0 and 1");
			}
			else if (float.TryParse(args, out result))
			{
				AudioSettingsSystem.Volume(busName, result);
			}
			else
			{
				Fail("Invalid value! (" + args + ")");
			}
		}
	}

	public abstract class CommandOptions : Command
	{
		public override string format => id + " <" + string.Join("/", options) + ">";

		public virtual string[] options => new string[2] { "on", "off" };

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			predictedArgs = options.Where((string a) => a.Contains(currentArgs.ToLower())).ToArray();
			yield return null;
		}

		public CommandOptions()
		{
		}
	}

	public abstract class CommandToggle : CommandOptions
	{
		public override void Run(string args)
		{
			string text = args.ToLower();
			if (!(text == "on"))
			{
				if (text == "off")
				{
					TurnOff();
				}
				else
				{
					Fail("You must enter either 'on' or 'off'");
				}
			}
			else
			{
				TurnOn();
			}
		}

		public virtual void TurnOn()
		{
		}

		public virtual void TurnOff()
		{
		}

		public CommandToggle()
		{
		}
	}

	public class CommandToggleHUD : CommandToggle
	{
		public override string id => "hud";

		public override void TurnOn()
		{
			Settings.Save("HudAlpha", 1f);
		}

		public override void TurnOff()
		{
			Settings.Save("HudAlpha", 0f);
		}
	}

	public class CommandToggleFps : CommandToggle
	{
		public override string id => "fps";

		public override void TurnOn()
		{
			Settings.Save("ShowFps", value: true);
		}

		public override void TurnOff()
		{
			Settings.Save("ShowFps", value: false);
		}
	}

	public class CommandCursor : CommandOptions
	{
		public override string id => "cursor";

		public override string[] options => new string[3] { "off", "game", "system" };

		public override void Run(string args)
		{
			switch (args)
			{
				case "off":
					CustomCursor.visible = false;
					CustomCursor.UpdateState();
					break;
				case "game":
					CustomCursor.visible = true;
					CustomCursor.SetStyle("default");
					CustomCursor.UpdateState();
					break;
				case "system":
					CustomCursor.visible = true;
					CustomCursor.SetStyle("system");
					CustomCursor.UpdateState();
					break;
				default:
					Fail("Invalid cursor option");
					break;
			}
		}
	}

	public class CommandToggleHandOverlay : CommandToggle
	{
		public override string id => "handoverlay";

		public override void TurnOn()
		{
			Settings.Save("HideHandOverlay", value: false);
		}

		public override void TurnOff()
		{
			Settings.Save("HideHandOverlay", value: true);
		}
	}

	public class CommandPrompt : Command
	{
		public override string id => "prompt";

		public override string format => "prompt <anchor> <x> <y> <maxWidth> <text>";

		public override void Run(string args)
		{
			List<string> list = args.Split(' ').ToList();
			if (list.Count > 4)
			{
				if (Enum.TryParse<Prompt.Anchor>(list[0].ToUpperFirstLetter(), out var result) && float.TryParse(list[1], out var result2) && float.TryParse(list[2], out var result3) && float.TryParse(list[3], out var result4))
				{
					list.RemoveRange(0, 4);
					string.Join(" ", list);
					PromptSystem.Create(result, result2, result3, result4);
				}
				else
				{
					Fail("Invalid arguments");
				}
			}
		}
	}

	public class CommandPromptHide : Command
	{
		public override string id => "prompthide";

		public override void Run(string args)
		{
			PromptSystem.Hide();
		}
	}

	public class CommandErrorTest : Command
	{
		public override string id => "errortest";

		public override string format => "errortest <message>";

		public override void Run(string args)
		{
			MonoBehaviourSingleton<Console>.instance.Toggle();
			throw new Exception(args);
		}
	}

	public class CommandPanSpeed : Command
	{
		public override string id => "pan speed";

		public override string format => "pan speed <value (default 5)>";

		public override void Run(string args)
		{
			if ((object)UnityEngine.Object.FindObjectOfType<Scroller>() != null)
			{
				List<string> list = args.Split(' ').ToList();
				if (list.Count <= 0 || !float.TryParse(list[0], out var _))
				{
					Fail("Please enter a value");
				}
			}
			else
			{
				Fail("You must be in the town scene to use this command");
			}
		}
	}

	public class CommandNextBattle : Command
	{
		public override string id => "next battle";

		public override string format => "next battle <battle>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			Campaign campaign = References.Campaign;
			if ((object)campaign != null)
			{
				if (args.Length > 0)
				{
					yield return AddressableLoader.LoadGroup("BattleData");
					BattleData battleData = AddressableLoader.GetGroup<BattleData>("BattleData").FirstOrDefault((BattleData a) => string.Equals(a.name, args.Trim(), StringComparison.CurrentCultureIgnoreCase));
					if ((bool)battleData)
					{
						CampaignNode item = Campaign.FindCharacterNode(References.Player);
						int num = campaign.nodes.IndexOf(item);
						CampaignNode campaignNode = null;
						for (int i = num; i < campaign.nodes.Count; i++)
						{
							CampaignNode campaignNode2 = campaign.nodes[i];
							if (!campaignNode2.cleared && campaignNode2.type.isBattle)
							{
								campaignNode = campaignNode2;
								break;
							}
						}

						if (campaignNode != null)
						{
							campaignNode.data = new Dictionary<string, object>
							{
								["battle"] = battleData.name,
								["waves"] = battleData.generationScript.Run(battleData, 1000)
							};
						}
						else
						{
							Fail("There are no more battles!");
						}
					}
					else
					{
						Fail("Battle [" + args + "] does not exist!");
					}
				}
				else
				{
					Fail("You must provide a battle name");
				}
			}
			else
			{
				Fail("You must be mid-run to use this command");
			}
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			yield return AddressableLoader.LoadGroup("BattleData");
			IEnumerable<BattleData> source = from a in AddressableLoader.GetGroup<BattleData>("BattleData")
				where a.name.ToLower().Contains(currentArgs.ToLower())
				select a;
			predictedArgs = source.Select((BattleData upgradeData) => upgradeData.name).ToArray();
		}
	}

	public class CommandProgressGain : Command
	{
		public override string id => "progress gain";

		public override string format => "progress gain <amount>";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			MetaprogressSequence metaprogressSequence = UnityEngine.Object.FindObjectOfType<MetaprogressSequence>();
			float result;
			if (metaprogressSequence == null)
			{
				Fail("You must be on the end screen to use this command");
			}
			else if (metaprogressSequence.running)
			{
				Fail("Wait for current progress sequence to end please");
			}

			else if (float.TryParse(args.Trim(), out result))
			{
				metaprogressSequence.StartCoroutine(metaprogressSequence.Sequence(result));
			}
			else
			{
				Fail("Invalid progress amount");
			}
		}
	}

	public class CommandProgressReset : Command
	{
		public override string id => "progress reset";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			SaveSystem.DeleteProgress();
		}
	}

	public class LoadModCommand : Command
	{
		public override string format => "loadmod modguid";

		public override string id => "loadmod";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			string text = args.Split(' ')[0];
			foreach (WildfrostMod mod in Bootstrap.Mods)
			{
				if (mod.GUID == text)
				{
					mod.ModLoad();
					break;
				}
			}
		}
	}

	public class PublishMod : Command
	{
		public override string format => "publish modguid";

		public override string id => "publish";

		public override bool IsRoutine => false;

		public override async void Run(string args)
		{
			string text = args.Split(' ')[0];
			foreach (WildfrostMod mod in Bootstrap.Mods)
			{
				if (mod.GUID == text)
				{
					mod.UpdateOrPublishWorkshop();
					break;
				}
			}
		}
	}

	public class UnLoadModCommand : Command
	{
		public override string format => "unloadmod modguid";

		public override string id => "unloadmod";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			string text = args.Split(' ')[0];
			foreach (WildfrostMod mod in Bootstrap.Mods)
			{
				if (mod.GUID == text)
				{
					mod.ModUnload();
					break;
				}
			}
		}
	}

	public class CommandScreenshot : Command
	{
		public override string id => "screenshot";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			string text = Application.persistentDataPath + "/Screenshots";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}

			string[] files = Directory.GetFiles(text, "screen*.png");
			int num = 0;
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				if (int.TryParse(array[i].Split(new string[1] { "screen" }, StringSplitOptions.None)[1].Replace(".png", ""), out var result))
				{
					num = Mathf.Max(num, result);
				}
			}

			ScreenCapture.CaptureScreenshot($"{text}/screen{num + 1}.png");
		}
	}

	public class CommandBlood : Command
	{
		public readonly Dictionary<string, string> colours = new Dictionary<string, string>
		{
			{ "red", "#E04141" },
			{ "berry", "#FD557E" },
			{ "black", "#222929" },
			{ "blue", "#639FF1" },
			{ "green", "#B8CC4B" },
			{ "purple", "#392463" },
			{ "pink", "#FE69FF" },
			{ "snow", "#A9D5E9" }
		};

		public override string id => "blood";

		public override string format => "blood <color> <amount>";

		public override bool IsRoutine => true;

		public override IEnumerator Routine(string args)
		{
			string[] array = args.Split(' ');
			string htmlString = ((array.Length == 0) ? "" : (colours.ContainsKey(array[0]) ? colours[array[0]] : array[0]));
			int result = 1;
			if (array.Length > 1)
			{
				int.TryParse(array[1].Trim(), out result);
			}

			if (result < 1)
			{
				result = 1;
			}

			float num = Mathf.Min((float)(result - 1) * 0.05f, 1f);
			Color color;
			bool setColor = ColorUtility.TryParseHtmlString(htmlString, out color);
			Routine.Clump clump = new Routine.Clump();
			for (int i = 0; i < result; i++)
			{
				Vector3 pos = Cursor3d.Position + PettyRandom.Vector3() * num;
				clump.Add(Create(pos, setColor, color));
			}

			yield return clump.WaitForEnd();
		}

		public static IEnumerator Create(Vector3 pos, bool setColor, Color color)
		{
			AsyncOperationHandle<GameObject> handle = AddressableLoader.InstantiateAsync("SplatterParticle", pos, Quaternion.identity);
			yield return handle;
			if (setColor && (bool)handle.Result)
			{
				SplatterParticle component = handle.Result.GetComponent<SplatterParticle>();
				if ((object)component != null)
				{
					component.color = color;
				}
			}
		}

		public override IEnumerator GetArgOptions(string currentArgs)
		{
			predictedArgs = colours.Keys.Where((string a) => a.Contains(currentArgs.ToLower())).ToArray();
			yield break;
		}
	}

	public class CommandRunFinalBossScript : Command
	{
		public override string id => "finalbosstest";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			PlayerData playerData = References.PlayerData;
			if (playerData != null)
			{
				FinalBossDeckGenerationSystem.SetNewBoss(playerData);
			}
			else
			{
				Fail("Cannot use this command here");
			}
		}
	}

	public class CommandDailyReset : Command
	{
		public override string id => "daily reset";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			SaveSystem.DeleteProgressData("dailyPlayed");
		}
	}

	public class CommandDailyOffset : Command
	{
		public override string id => "daily offset";

		public override string format => "daily offset <days>";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			if (int.TryParse(args, out var result))
			{
				DailyFetcher.DayOffset = result;
				UnityEngine.Object.FindObjectOfType<BalloonSequence>()?.Close();
			}
			else
			{
				Fail("Pls provide number of offset days");
			}
		}
	}

	public class CommandEncrypt : Command
	{
		public override string id => "encrypt";

		public override string format => "encrypt";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			Encrypt("Save.sav");
			Encrypt("Campaign.sav");
			Encrypt("CampaignDemo.sav");
			Encrypt("CampaignDaily.sav");
			Encrypt("Stats.sav");
			Encrypt("History.sav");
		}

		public static void Encrypt(string fileName)
		{
			string text = SaveSystem.folderName + "/Decrypt/" + fileName;
			if (ES3.FileExists(text, ES3Settings.defaultSettings))
			{
				byte[] bytes = ES3.LoadRawBytes(text, ES3Settings.defaultSettings);
				string filePath = SaveSystem.folderName + "/" + fileName;
				ES3.SaveRaw(bytes, filePath, SaveSystem.settings);
				Debug.Log("Re-encrypted [" + fileName + "]");
			}
			else
			{
				Debug.Log("[" + text + "] does not exist!");
			}
		}
	}

	public class CommandDecrypt : Command
	{
		public override string id => "decrypt";

		public override string format => "decrypt";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			Decrypt("Save.sav");
			Decrypt("Campaign.sav");
			Decrypt("CampaignDemo.sav");
			Decrypt("CampaignDaily.sav");
			Decrypt("CampaignTutorial.sav");
			Decrypt("Stats.sav");
			Decrypt("History.sav");
		}

		public static void Decrypt(string fileName)
		{
			string text = SaveSystem.folderName + "/" + fileName;
			if (ES3.FileExists(text, SaveSystem.settings))
			{
				byte[] bytes = ES3.LoadRawBytes(text, SaveSystem.settings);
				string text2 = SaveSystem.folderName + "/Decrypt/" + fileName;
				ES3.SaveRaw(bytes, text2, ES3Settings.defaultSettings);
				Debug.Log("Decrypted [" + fileName + "] to [" + text2 + "]");
			}
			else
			{
				Debug.Log("[" + text + "] does not exist!");
			}
		}
	}

	public class CommandDisplay : Command
	{
		public override string id => "resolution";

		public override string format => "resolution <width> <height>";

		public override bool IsRoutine => false;

		public override void Run(string args)
		{
			string[] array = args.Split(' ');
			if (array.Length > 1 && int.TryParse(array[0].Trim(), out var result) && int.TryParse(array[1].Trim(), out var result2))
			{
				ScreenSystem.SetResolutionWindowed(result, result2);
			}
			else
			{
				Fail("Incorrect format. Should be written as \"resolution <width> <height>\"");
			}
		}
	}

	[SerializeField]
	public KeyCode[] toggle = new KeyCode[2]
	{
		KeyCode.BackQuote,
		KeyCode.F12
	};

	[SerializeField]
	public KeyCode takePredict = KeyCode.Tab;

	[SerializeField]
	public Color logColor;

	[SerializeField]
	public Color logErrorColor;

	[SerializeField]
	public string unknownCommandFormat = "Unknown Command: {0}";

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public TMP_InputField input;

	[SerializeField]
	public TMP_Text textPrefab;

	[SerializeField]
	public Transform log;

	[SerializeField]
	public GameObject helpWindow;

	[SerializeField]
	public TMP_Text helpText;

	[SerializeField]
	public ConsoleArgsDisplay argsDisplay;

	[Header("Saving Commands")]
	[SerializeField]
	public KeyCode[] saveKeys = new KeyCode[8]
	{
		KeyCode.F1,
		KeyCode.F2,
		KeyCode.F3,
		KeyCode.F4,
		KeyCode.F5,
		KeyCode.F6,
		KeyCode.F7,
		KeyCode.F8
	};

	[SerializeField]
	public string saveFileName = "commands.sav";

	[SerializeField]
	public string[] savedCommands;

	public static readonly List<string> previous = new List<string>();

	public int preIndex;

	public static List<Command> commands;

	public static bool active;

	public static Entity hover;

	public static CardSlot slotHover;

	public bool promptUpdatePredict;

	public void Start()
	{
		LoadCommands();
		canvas.gameObject.SetActive(value: false);
		Commands();
		PopulateHelp();
		helpWindow.SetActive(value: false);
		StartCoroutine(UpdatePredictRoutine());
	}

	public IEnumerator UpdatePredictRoutine()
	{
		while (true)
		{
			if (promptUpdatePredict)
			{
				promptUpdatePredict = false;
				yield return PredictArgsRoutine(input.text);
			}

			yield return null;
		}
	}

	public void OnEnable()
	{
		Events.OnEntityHover += Hover;
		Events.OnSlotHover += SlotHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= Hover;
		Events.OnSlotHover -= SlotHover;
	}

	public static void Hover(Entity entity)
	{
		hover = entity;
	}

	public static void SlotHover(CardSlot slot)
	{
		slotHover = slot;
	}

	public void Update()
	{
		if (CheckToggle())
		{
			return;
		}

		if (active)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Toggle();
			}
			else if (!CheckTakePredict())
			{
				CheckScrollPrevious();
				CheckRunCommand();
				CheckSaveCommand();
				KeepFocus();
			}
		}
		else
		{
			CheckRunSavedCommands();
		}
	}

	public bool CheckToggle()
	{
		if (toggle.Any(Input.GetKeyDown))
		{
			Toggle();
			return true;
		}

		return false;
	}

	public bool CheckTakePredict()
	{
		if (Input.GetKeyDown(takePredict) && argsDisplay.Count > 0)
		{
			Command exactCommand = GetExactCommand(input.text.TrimStart());
			if (exactCommand != null)
			{
				input.text = exactCommand.id + " " + argsDisplay.TopArgument;
			}
			else
			{
				input.text = argsDisplay.TopCommand;
			}

			input.MoveToEndOfLine(shift: false, ctrl: false);
			return true;
		}

		return false;
	}

	public void CheckScrollPrevious()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (previous.Count > 0)
			{
				preIndex = Mathf.Min(preIndex + 1, previous.Count - 1);
				input.text = previous[preIndex];
			}

			input.MoveToEndOfLine(shift: false, ctrl: false);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (previous.Count > 0)
			{
				preIndex = Mathf.Max(preIndex - 1, 0);
				input.text = previous[preIndex];
			}

			input.MoveToEndOfLine(shift: false, ctrl: false);
		}
	}

	public void CheckRunCommand()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string text = input.text.Trim();
			if (text.Length > 0)
			{
				new Routine(HandleCommand(text));
				input.text = "";
				preIndex = -1;
			}
		}
	}

	public void CheckSaveCommand()
	{
		for (int i = 0; i < saveKeys.Length; i++)
		{
			KeyCode keyCode = saveKeys[i];
			if (!Input.GetKeyDown(keyCode))
			{
				continue;
			}

			string text = input.text.Trim();
			if (text.Length > 0)
			{
				savedCommands[i] = text;
				Log($"Command '{text}' Saved to {keyCode}");
				SaveCommands();
				input.text = "";
				preIndex = -1;
			}
			else if (savedCommands.Length > i)
			{
				string text2 = savedCommands[i];
				if (text2.Length > 0)
				{
					new Routine(HandleCommand(text2));
					input.text = "";
					preIndex = -1;
				}
			}
		}
	}

	public void KeepFocus()
	{
		if (!input.isFocused)
		{
			EventSystem current = EventSystem.current;
			if ((object)current != null)
			{
				current.SetSelectedGameObject(input.gameObject, null);
				input.OnPointerClick(new PointerEventData(current));
			}
		}
	}

	public void CheckRunSavedCommands()
	{
		int num = Mathf.Min(saveKeys.Length, savedCommands.Length);
		for (int i = 0; i < num; i++)
		{
			if (Input.GetKeyDown(saveKeys[i]))
			{
				string text = savedCommands[i];
				if (text.Length > 0)
				{
					new Routine(HandleCommand(text));
				}
			}
		}
	}

	public static void Commands()
	{
		commands = new List<Command>
		{
			new CommandHelp(),
			new CommandGainCard(),
			new CommandGainUpgrade(),
			new CommandGainGold(),
			new CommandSetHealth(),
			new CommandSetAttack(),
			new CommandSetCounter(),
			new CommandAddStatus(),
			new CommandAddUpgrade(),
			new CommandHit(),
			new CommandKill(),
			new CommandKillAll(),
			new CommandDestroy(),
			new CommandDestroyAll(),
			new CommandSpawn(),
			new CommandBattleWin(),
			new CommandBattleLose(),
			new CommandBattleSkip(),
			new CommandBattleAuto(),
			new CommandSkipTurn(),
			new CommandMapJump(),
			new CommandMapInfo(),
			new CommandSystemDisable(),
			new CommandSystemEnable(),
			new CommandGameSpeed(),
			new CommandSetSaveProfile(),
			new CommandVolume(),
			new CommandVolume("Music"),
			new CommandVolume("Sfx"),
			new CommandVolume("Ambience"),
			new CommandToggleHUD(),
			new CommandToggleFps(),
			new CommandToggleHandOverlay(),
			new CommandCursor(),
			new CommandReroll(),
			new CommandRepeat(),
			new CommandPrompt(),
			new CommandPromptHide(),
			new CommandErrorTest(),
			new CommandPanSpeed(),
			new CommandNextBattle(),
			new CommandProgressGain(),
			new CommandProgressReset(),
			new CommandScreenshot(),
			new CommandBlood(),
			new CommandRunFinalBossScript(),
			new CommandDailyReset(),
			new CommandDailyOffset(),
			new CommandEncrypt(),
			new CommandDecrypt(),
			new CommandDisplay(),
			new LoadModCommand(),
			new UnLoadModCommand(),
			new PublishMod()
		};
	}

	public void SaveCommands()
	{
		File.WriteAllLines(Application.persistentDataPath + "\\" + saveFileName, savedCommands);
	}

	public void LoadCommands()
	{
		string path = Application.persistentDataPath + "\\" + saveFileName;
		if (File.Exists(path))
		{
			savedCommands = File.ReadAllLines(path);
			return;
		}

		savedCommands = new string[saveKeys.Length];
		savedCommands[0] = "repeat";
	}

	public static IEnumerator HandleCommand(string text)
	{
		Debug.Log(text);
		bool commandFound = false;
		if (commands == null)
		{
			Commands();
		}

		for (int num = commands.Count - 1; num >= 0; num--)
		{
			Command command = commands[num];
			if (text.StartsWith(command.id))
			{
				commandFound = true;
				string args = text.Replace(command.id, "").Trim();
				command.success = true;
				if (command.IsRoutine)
				{
					yield return command.Routine(args);
				}
				else
				{
					command.Run(args);
				}

				if (command.success && command.logOnSuccess)
				{
					Log(text);
				}

				break;
			}
		}

		if (!commandFound)
		{
			LogError(string.Format(MonoBehaviourSingleton<Console>.instance.unknownCommandFormat, text));
		}

		previous.Insert(0, text);
	}

	public static void Log(string text)
	{
		TMP_Text tMP_Text = UnityEngine.Object.Instantiate(MonoBehaviourSingleton<Console>.instance.textPrefab, MonoBehaviourSingleton<Console>.instance.log);
		tMP_Text.text = text;
		tMP_Text.color = MonoBehaviourSingleton<Console>.instance.logColor;
		tMP_Text.gameObject.SetActive(value: true);
	}

	public static void LogError(string text)
	{
		TMP_Text tMP_Text = UnityEngine.Object.Instantiate(MonoBehaviourSingleton<Console>.instance.textPrefab, MonoBehaviourSingleton<Console>.instance.log);
		tMP_Text.text = text;
		tMP_Text.color = MonoBehaviourSingleton<Console>.instance.logErrorColor;
		tMP_Text.gameObject.SetActive(value: true);
	}

	public void PredictArgs()
	{
		promptUpdatePredict = true;
	}

	public IEnumerator PredictArgsRoutine(string text)
	{
		text = text.TrimStart();
		if (text.Length > 0)
		{
			yield return new WaitForEndOfFrame();
			yield return null;
			Command[] array = commands.Where((Command a) => a.id.StartsWith(text)).ToArray();
			int num = array.Length;
			Bounds bounds = input.textComponent.textBounds;
			float x = 10f;
			if (num > 0)
			{
				argsDisplay.Show();
				argsDisplay.DisplayCommands(array.OrderByDescending((Command a) => a.id.Length).ToArray());
				argsDisplay.MoveTo(x);
			}
			else
			{
				Command exactCommand = GetExactCommand(text);
				if (exactCommand != null)
				{
					string args = text.Replace(exactCommand.id + " ", "");
					yield return exactCommand.GetArgOptions(args);
					if (exactCommand.predictedArgs != null && exactCommand.predictedArgs.Length != 0)
					{
						argsDisplay.Show();
						string[] array2 = exactCommand.predictedArgs.Where((string a) => string.Equals(a, args, StringComparison.CurrentCultureIgnoreCase)).ToArray();
						if (array2.Length != 0)
						{
							argsDisplay.DisplayArgs(array2);
						}
						else
						{
							argsDisplay.DisplayArgs(exactCommand.predictedArgs.OrderByDescending((string a) => a.Length).ToArray());
						}

						argsDisplay.MoveTo(x + 10f + bounds.size.x);
					}
					else
					{
						argsDisplay.Hide();
					}
				}
				else
				{
					argsDisplay.Hide();
				}
			}
		}
		else
		{
			argsDisplay.Hide();
		}

		yield return null;
	}

	public static Command GetExactCommand(string text)
	{
		return commands.FirstOrDefault((Command a) => text.StartsWith(a.id + " "));
	}

	public void Toggle()
	{
		active = !active;
		canvas.gameObject.SetActive(active);
		if (active)
		{
			preIndex = -1;
			InputSystem.Disable();
		}
		else
		{
			input.text = "";
			InputSystem.Enable();
		}
	}

	public static void ToggleHelp()
	{
		MonoBehaviourSingleton<Console>.instance.helpWindow.SetActive(!MonoBehaviourSingleton<Console>.instance.helpWindow.activeSelf);
	}

	public void PopulateHelp()
	{
		string text = "";
		foreach (Command command in commands)
		{
			if (!command.hidden)
			{
				string text2 = command.format;
				if (command.desc.Length > 0)
				{
					text2 = text2 + " <i><#fffd>" + command.desc + "</color></i>";
				}

				text = text + text2 + "\n";
			}
		}

		helpText.text = text.TrimEnd();
	}
}
