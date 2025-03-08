#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class CharacterDisplay : EntityDisplay
{
	[SerializeField]
	public int team = 1;

	public GoldDisplay goldDisplay;

	[Space]
	public DeckDisplay deckDisplay;

	public HandOverlay handOverlay;

	public int currentEffectBonus;

	public float currentEffectFactor;

	public Character _character;

	public bool IsDeckpackOpen
	{
		get
		{
			if (!Deckpack.IsOpen)
			{
				return deckDisplay.companionLimitSequence.gameObject.activeSelf;
			}

			return true;
		}
	}

	public Character character
	{
		get
		{
			if (!_character)
			{
				_character = entity.GetComponent<Character>();
			}

			return _character;
		}
	}

	public static void FindAndAssign(Character character)
	{
		CharacterDisplay[] array = UnityEngine.Object.FindObjectsOfType<CharacterDisplay>();
		foreach (CharacterDisplay characterDisplay in array)
		{
			if (!characterDisplay.entity && characterDisplay.team == character.team)
			{
				characterDisplay.Assign(character);
				return;
			}
		}

		throw new Exception("Could not find CharacterDisplay for Character [" + character.name + "]");
	}

	public void Assign(Character character)
	{
		Debug.Log($"[{character.name}] assigned to {this}");
		entity = character.entity;
		deckDisplay?.SetOwner(character);
		handOverlay?.SetOwner(character);
		character.entity.display = this;
		character.entity.PromptUpdate();
	}

	public void UnAssign()
	{
		entity = null;
	}

	public override IEnumerator UpdateDisplay(bool doPingIcons = true)
	{
		ClearStatusIcons();
		if ((bool)goldDisplay && (bool)character)
		{
			goldDisplay.Set(character.data.inventory.gold.Value);
		}

		if (entity.effectBonus != currentEffectBonus || entity.effectFactor != currentEffectFactor)
		{
			currentEffectBonus = entity.effectBonus;
			currentEffectFactor = entity.effectFactor;
			yield return StatusEffectSystem.EffectBonusChangedEvent(entity);
		}
	}

	public void ToggleInventory()
	{
		if (Deckpack.IsOpen)
		{
			Deckpack.Close();
			deckDisplay.displaySequence.End();
		}
		else
		{
			Deckpack.Open();
			deckDisplay.displaySequence.Begin();
		}
	}

	public void OpenInventory()
	{
		if (!Deckpack.IsOpen)
		{
			ToggleInventory();
		}
	}

	public void CloseInventory()
	{
		if (Deckpack.IsOpen)
		{
			ToggleInventory();
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void ForceUpdateDisplay()
	{
		StartCoroutine(UpdateDisplay());
	}
}
