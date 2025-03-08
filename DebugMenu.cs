#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
	[SerializeField]
	public GameObject menu;

	[SerializeField]
	public float holdTime = 2f;

	[SerializeField]
	public TMP_Text resultText;

	public float currentHoldTime;

	public bool active;

	public void Update()
	{
		if (active)
		{
			return;
		}

		if (CheckMouse() || CheckController())
		{
			currentHoldTime += Time.unscaledDeltaTime;
			if (currentHoldTime > holdTime)
			{
				Activate();
			}
		}
		else
		{
			currentHoldTime = 0f;
		}
	}

	public static bool CheckMouse()
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			if (!Input.GetKey(KeyCode.Space))
			{
				return Input.GetKey(KeyCode.Mouse2);
			}

			return true;
		}

		return false;
	}

	public static bool CheckController()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			if (!InputSystem.IsButtonHeld("Backpack") || !InputSystem.IsButtonHeld("Redraw Bell"))
			{
				if (InputSystem.IsButtonHeld("Up"))
				{
					return InputSystem.IsButtonHeld("Down");
				}

				return false;
			}

			return true;
		}

		return false;
	}

	public void Activate()
	{
		active = true;
		menu.gameObject.SetActive(value: true);
		resultText.text = "";
	}

	public void Deactivate()
	{
		active = false;
		menu.gameObject.SetActive(value: false);
	}

	public void DeleteSave()
	{
		int value = SaveSystem.LoadProgressData("tutorialProgress", 0);
		string value2 = SaveSystem.LoadProgressData("version", "0");
		SaveSystem.DeleteProgress();
		SaveSystem.DeleteCampaign(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"));
		SaveSystem.SaveProgressData("tutorialProgress", value);
		SaveSystem.SaveProgressData("version", value2);
		resultText.text = "Save Data Deleted!";
	}

	public void ResetTutorial()
	{
		SaveSystem.DeleteProgressData("tutorialProgress");
		resultText.text = "Tutorial Reset!";
	}

	public void RunCommand(string command)
	{
		new Routine(Console.HandleCommand(command));
		resultText.text = command;
	}
}
