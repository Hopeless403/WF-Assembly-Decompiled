#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class HardModeModifierDisplay : ModifierDisplay
{
	[SerializeField]
	public GameObject background;

	public GameModifierData[] modifiers;

	public List<string> active;

	public void Populate()
	{
		int num = SaveSystem.LoadProgressData("hardModeModifiersUnlocked", 0);
		background.SetActive(num > 0);
		active = SaveSystem.LoadProgressData<List<string>>("activeHardModeModifiers");
		if (active == null)
		{
			active = new List<string>();
		}

		for (int i = 0; i < num && i < modifiers.Length; i++)
		{
			GameModifierData modifier = modifiers[i];
			ModifierIcon modifierIcon = CreateIcon(modifier);
			if (active.Contains(modifier.name))
			{
				ModifierSystem.AddModifier(Campaign.Data, modifier);
			}
			else
			{
				modifierIcon.GetComponent<ModifierToggle>()?.Toggle();
			}

			modifierIcon.GetComponentInChildren<InputAction>()?.action.AddListener(delegate
			{
				Toggle(modifier);
			});
		}
	}

	public void Toggle(GameModifierData modifier)
	{
		if (active.Contains(modifier.name))
		{
			active.Remove(modifier.name);
			ModifierSystem.RemoveModifier(Campaign.Data, modifier);
		}
		else
		{
			active.Add(modifier.name);
			ModifierSystem.AddModifier(Campaign.Data, modifier);
		}

		SaveSystem.SaveProgressData("activeHardModeModifiers", active);
	}
}
