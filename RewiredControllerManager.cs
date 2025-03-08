#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

public class RewiredControllerManager : MonoBehaviour
{
	public class PlayerMap
	{
		public int rewiredPlayerId;

		public int gamePlayerId;

		public PlayerMap(int rewiredPlayerId, int gamePlayerId)
		{
			this.rewiredPlayerId = rewiredPlayerId;
			this.gamePlayerId = gamePlayerId;
		}
	}

	public static RewiredControllerManager instance;

	public int maxPlayers = 4;

	public static readonly List<PlayerMap> playerMaps = new List<PlayerMap>();

	public static int gamePlayerIdCounter = 0;

	public static Player leadPlayer;

	public const int maxJoysticksPerPlayer = 1;

	public void Awake()
	{
		if ((bool)instance && instance != this)
		{
			Object.Destroy(this);
			return;
		}

		instance = this;
		AssignNextPlayer(0);
	}

	public void OnEnable()
	{
		ReInput.ControllerConnectedEvent += ControllerConnected;
		ReInput.ControllerDisconnectedEvent += ControllerDisconnected;
	}

	public void OnDisable()
	{
		ReInput.ControllerConnectedEvent -= ControllerConnected;
		ReInput.ControllerDisconnectedEvent -= ControllerDisconnected;
	}

	public static void ControllerConnected(ControllerStatusChangedEventArgs args)
	{
		Player playerController = GetPlayerController(0);
		bool flag = false;
		for (int num = playerController.controllers.joystickCount + 1; num > 1; num--)
		{
			playerController.controllers.RemoveController(playerController.controllers.Joysticks[0]);
			flag = true;
		}

		if (!playerController.controllers.ContainsController(args.controller))
		{
			playerController.controllers.AddController(args.controller, removeFromOtherPlayers: true);
			flag = true;
		}

		if (flag)
		{
			Debug.LogWarning("Rewired: [" + playerController.name + "] controllers → " + string.Join(", ", playerController.controllers.Controllers.Select((Controller a) => a.name)));
			Events.InvokeControllerSwitched();
		}
	}

	public static void ControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		Player playerController = GetPlayerController(0);
		bool flag = false;
		if (playerController.controllers.ContainsController(args.controller))
		{
			playerController.controllers.RemoveController(args.controller);
			flag = true;
		}

		int num = playerController.controllers.joystickCount - 1;
		if (num < 1)
		{
			foreach (Joystick joystick in ReInput.controllers.Joysticks)
			{
				if (!playerController.controllers.ContainsController(joystick))
				{
					playerController.controllers.AddController(joystick, removeFromOtherPlayers: true);
					flag = true;
					if (++num >= 1)
					{
						break;
					}
				}
			}
		}

		if (flag)
		{
			Debug.LogWarning("Rewired: [" + playerController.name + "] controllers → " + string.Join(", ", playerController.controllers.Controllers.Select((Controller a) => a.name)));
			Events.InvokeControllerSwitched();
		}
	}

	public Player AssignNextPlayer(int rewiredPlayerId)
	{
		PlayerMap playerMap = playerMaps.FirstOrDefault((PlayerMap a) => a.rewiredPlayerId == rewiredPlayerId);
		if (playerMap != null)
		{
			return ReInput.players.GetPlayer(playerMap.rewiredPlayerId);
		}

		Debug.Log($"ControllerManager → Assigning Player {rewiredPlayerId}");
		if (playerMaps.Count >= maxPlayers)
		{
			Debug.LogError("Max player limit already reached!");
			return null;
		}

		int nextGamePlayerId = GetNextGamePlayerId();
		playerMaps.Add(new PlayerMap(rewiredPlayerId, nextGamePlayerId));
		Player player = ReInput.players.GetPlayer(rewiredPlayerId);
		Debug.Log($"Assigning Rewired Player id {rewiredPlayerId} to Game Player {nextGamePlayerId}");
		if (leadPlayer == null)
		{
			Debug.Log($"Player {rewiredPlayerId} set as lead player");
			leadPlayer = player;
		}

		if (player.controllers.joystickCount < 1)
		{
			foreach (Joystick joystick in ReInput.controllers.Joysticks)
			{
				if (!player.controllers.Joysticks.Contains(joystick))
				{
					player.controllers.AddController(joystick, removeFromOtherPlayers: true);
					if (player.controllers.joystickCount >= 1)
					{
						break;
					}
				}
			}
		}

		Debug.LogWarning("Rewired: [" + player.name + "] controllers → " + string.Join(", ", player.controllers.Controllers.Select((Controller a) => a.name)));
		return player;
	}

	public void AssignJoystickMap(Player rewiredPlayer, string newMap)
	{
		rewiredPlayer.controllers.maps.SetAllMapsEnabled(state: false);
		rewiredPlayer.controllers.maps.SetMapsEnabled(state: true, newMap);
	}

	public void AssignJoystickMap(int playerNumber, string newMap)
	{
		AssignJoystickMap(GetPlayerController(playerNumber), newMap);
	}

	public int GetNextGamePlayerId()
	{
		return gamePlayerIdCounter++;
	}

	public List<PlayerMap> GetActiveDevices()
	{
		return playerMaps;
	}

	public IList<Player> GetPlayers()
	{
		return ReInput.players.GetPlayers();
	}

	public static Player GetPlayerController(int gamePlayerId)
	{
		if (!ReInput.isReady)
		{
			return null;
		}

		if (!instance)
		{
			Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scene?");
			return null;
		}

		PlayerMap playerMap = playerMaps.FirstOrDefault((PlayerMap a) => a.gamePlayerId == gamePlayerId);
		if (playerMap == null)
		{
			return instance.AssignNextPlayer(gamePlayerId);
		}

		return ReInput.players.GetPlayer(playerMap.rewiredPlayerId);
	}

	public static int GetPlayerID(int rewiredControllerID)
	{
		if (!ReInput.isReady)
		{
			return -1;
		}

		if (instance == null)
		{
			Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
			return -1;
		}

		foreach (PlayerMap playerMap in playerMaps)
		{
			if (playerMap.rewiredPlayerId == rewiredControllerID)
			{
				return playerMap.gamePlayerId;
			}
		}

		return -1;
	}

	public void ClearAssignedControllers()
	{
		foreach (PlayerMap playerMap in playerMaps)
		{
			playerMap.gamePlayerId = -1;
		}
	}

	public bool IsPlayerControllerConnected(int playerIndex)
	{
		if (playerMaps.Count((PlayerMap x) => x.gamePlayerId == playerIndex) <= 0)
		{
			return false;
		}

		return true;
	}

	public bool IsControllerConnected(int controllerIndex)
	{
		if (ReInput.players.Players.Count((Player x) => x.id == controllerIndex) <= 0)
		{
			return false;
		}

		return true;
	}

	public void AssignControllerToPlayer(int i, int controllerIndex)
	{
		playerMaps.First((PlayerMap x) => x.rewiredPlayerId == controllerIndex).gamePlayerId = i;
	}

	public bool AnyControllerConnected()
	{
		if (playerMaps.Count <= 0)
		{
			return false;
		}

		return true;
	}

	public bool IsButtonPressed(string input = "", bool IsPositive = true)
	{
		if (!base.enabled)
		{
			return false;
		}

		int callingPlayer = -1;
		return IsButtonPressed(out callingPlayer, input, IsPositive);
	}

	public bool IsButtonPressed(out int callingPlayer, string input = "", bool IsPositive = true)
	{
		bool state = false;
		callingPlayer = -1;
		if (IsPositive)
		{
			foreach (Player player in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player.GetAnyButtonDown() : player.GetButtonDown(input))
				{
					instance.ReturnTrueWithPlayer(out callingPlayer, out state, player);
					break;
				}
			}
		}
		else
		{
			foreach (Player player2 in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player2.GetAnyNegativeButtonDown() : player2.GetNegativeButtonDown(input))
				{
					instance.ReturnTrueWithPlayer(out callingPlayer, out state, player2);
					break;
				}
			}
		}

		return state;
	}

	public bool IsButtonHeld(string input = "", bool IsPositive = true)
	{
		if (!base.enabled)
		{
			return false;
		}

		int callingPlayer = -1;
		return IsButtonHeld(out callingPlayer, input, IsPositive);
	}

	public bool IsButtonHeld(out int callingPlayer, string input = "", bool IsPositive = true)
	{
		bool state = false;
		callingPlayer = -1;
		if (IsPositive)
		{
			foreach (Player player in ReInput.players.GetPlayers())
			{
				bool num;
				if (!(input == ""))
				{
					num = player.GetButton(input);
				}
				else
				{
					if (player.GetAnyButton())
					{
						goto IL_004b;
					}

					num = player.GetAnyNegativeButton();
				}

				if (!num)
				{
					continue;
				}

				goto IL_004b;
				IL_004b:
				ReturnTrueWithPlayer(out callingPlayer, out state, player);
				break;
			}
		}
		else
		{
			foreach (Player player2 in ReInput.players.GetPlayers())
			{
				bool num2;
				if (!(input == ""))
				{
					num2 = player2.GetNegativeButton(input);
				}
				else
				{
					if (player2.GetAnyButton())
					{
						goto IL_00ae;
					}

					num2 = player2.GetAnyNegativeButton();
				}

				if (!num2)
				{
					continue;
				}

				goto IL_00ae;
				IL_00ae:
				ReturnTrueWithPlayer(out callingPlayer, out state, player2);
				break;
			}
		}

		return state;
	}

	public bool IsButtonLongHeld(string input = "", bool IsPositive = true)
	{
		if (!base.enabled)
		{
			return false;
		}

		int callingPlayer = -1;
		return IsButtonLongHeld(out callingPlayer, input, IsPositive);
	}

	public bool IsButtonLongHeld(out int callingPlayer, string input = "", bool IsPositive = true)
	{
		bool state = false;
		callingPlayer = -1;
		if (IsPositive)
		{
			foreach (Player player in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player.GetAnyButton() : player.GetButtonLongPress(input))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player);
					break;
				}
			}
		}
		else
		{
			foreach (Player player2 in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player2.GetAnyNegativeButton() : player2.GetNegativeButtonLongPress(input))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player2);
					break;
				}
			}
		}

		return state;
	}

	public bool IsButtonReleased(string input = "", bool IsPositive = true)
	{
		if (!base.enabled)
		{
			return false;
		}

		int callingPlayer = -1;
		return IsButtonReleased(out callingPlayer, input, IsPositive);
	}

	public bool IsButtonReleased(out int callingPlayer, string input = "", bool IsPositive = true)
	{
		bool state = false;
		callingPlayer = -1;
		if (IsPositive)
		{
			foreach (Player player in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player.GetAnyButtonUp() : player.GetButtonUp(input))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player);
					break;
				}
			}
		}
		else
		{
			foreach (Player player2 in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player2.GetAnyNegativeButtonUp() : player2.GetNegativeButtonUp(input))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player2);
					break;
				}
			}
		}

		return state;
	}

	public bool WasButtonReleased(string input = "", bool IsPositive = true)
	{
		int callingPlayer = -1;
		return WasButtonReleased(out callingPlayer, input, IsPositive);
	}

	public bool WasButtonReleased(out int callingPlayer, string input = "", bool IsPositive = true)
	{
		bool state = false;
		callingPlayer = -1;
		if (IsPositive)
		{
			foreach (Player player in ReInput.players.GetPlayers())
			{
				if ((input == "") ? (!player.GetAnyButtonPrev()) : (!player.GetButtonPrev(input)))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player);
					break;
				}
			}
		}
		else
		{
			foreach (Player player2 in ReInput.players.GetPlayers())
			{
				if ((input == "") ? (!player2.GetAnyNegativeButtonPrev()) : (!player2.GetNegativeButtonPrev(input)))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player2);
					break;
				}
			}
		}

		return state;
	}

	public bool WasButtonPressed(string input = "", bool IsPositive = true)
	{
		int callingPlayer = -1;
		return WasButtonPressed(out callingPlayer, input, IsPositive);
	}

	public bool WasButtonPressed(out int callingPlayer, string input = "", bool IsPositive = true)
	{
		bool state = false;
		callingPlayer = -1;
		if (IsPositive)
		{
			foreach (Player player in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player.GetAnyButtonPrev() : player.GetButtonPrev(input))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player);
					break;
				}
			}
		}
		else
		{
			foreach (Player player2 in ReInput.players.GetPlayers())
			{
				if ((input == "") ? player2.GetAnyNegativeButtonPrev() : player2.GetNegativeButtonPrev(input))
				{
					ReturnTrueWithPlayer(out callingPlayer, out state, player2);
					break;
				}
			}
		}

		return state;
	}

	public float GetAnalogPositive(string input = "")
	{
		if (!base.enabled)
		{
			return 0f;
		}

		int callingPlayer = -1;
		return GetAnalog(out callingPlayer, input, lookingForPositive: true);
	}

	public float GetAnalogNegative(string input = "")
	{
		if (!base.enabled)
		{
			return 0f;
		}

		int callingPlayer = -1;
		return GetAnalog(out callingPlayer, input, lookingForPositive: false);
	}

	public float GetAnalog(string input = "")
	{
		if (!base.enabled)
		{
			return 0f;
		}

		float num = 0f;
		foreach (Player player in ReInput.players.GetPlayers())
		{
			float axis = player.GetAxis(input);
			if (Mathf.Abs(axis) > Mathf.Abs(num))
			{
				num = axis;
			}
		}

		return num;
	}

	public float GetAnalog(out int callingPlayer, string input, bool lookingForPositive)
	{
		float num = 0f;
		callingPlayer = -1;
		foreach (Player x in ReInput.players.GetPlayers())
		{
			if (lookingForPositive)
			{
				if (!(x.GetAxis(input) > num))
				{
					continue;
				}

				if (playerMaps.Count((PlayerMap y) => y.rewiredPlayerId == x.id) > 0)
				{
					callingPlayer = playerMaps.First((PlayerMap y) => y.rewiredPlayerId == x.id).gamePlayerId;
				}
				else
				{
					callingPlayer = -1;
				}

				num = x.GetAxis(input);
			}
			else
			{
				if (!(x.GetAxis(input) < num))
				{
					continue;
				}

				if (playerMaps.Count((PlayerMap y) => y.rewiredPlayerId == x.id) > 0)
				{
					callingPlayer = playerMaps.First((PlayerMap y) => y.rewiredPlayerId == x.id).gamePlayerId;
				}
				else
				{
					callingPlayer = -1;
				}

				num = x.GetAxis(input);
			}
		}

		return num;
	}

	public void ReturnTrueWithPlayer(out int callingPlayer, out bool state, Player x)
	{
		if (playerMaps.Count((PlayerMap y) => y.rewiredPlayerId == x.id) > 0)
		{
			callingPlayer = playerMaps.First((PlayerMap y) => y.rewiredPlayerId == x.id).gamePlayerId;
		}
		else
		{
			callingPlayer = -1;
		}

		state = true;
	}

	public int GetDeviceCount()
	{
		return playerMaps.Count();
	}

	public void SetActiveControllersToMap(string newControllerMap)
	{
		foreach (PlayerMap activeDevice in instance.GetActiveDevices())
		{
			instance.AssignJoystickMap(activeDevice.gamePlayerId, newControllerMap);
		}
	}
}
