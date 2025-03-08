#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class Character : MonoBehaviour, ISaveable<CharacterSaveData>
{
	public PlayerData data;

	public string title;

	public int team = 1;

	public Entity entity;

	[HorizontalLine(2f, EColor.Gray)]
	public CardContainer drawContainer;

	public CardContainer handContainer;

	public CardContainer discardContainer;

	public CardContainer reserveContainer;

	public bool freeAction;

	public bool endTurn;

	public bool autoTriggerUnits = true;

	public void Assign(PlayerData data)
	{
		this.data = data;
	}

	public int GetCompanionCount()
	{
		return data.inventory.deck.FindAll((CardData a) => a.cardType.name == "Friendly").Count;
	}

	public void GainGold(int amount)
	{
		if ((bool)data?.inventory)
		{
			data.inventory.AddGold(amount);
			entity.PromptUpdate();
		}
	}

	public void SpendGold(int amount)
	{
		if ((bool)data?.inventory)
		{
			data.inventory.gold -= amount;
			entity.PromptUpdate();
			Events.InvokeSpendGold(amount);
		}
	}

	[Button("Gain 10 Gold", EButtonEnableMode.Always)]
	public void Gain10Gold()
	{
		GainGold(10);
	}

	[Button("Gain 100 Gold", EButtonEnableMode.Always)]
	public void Gain100Gold()
	{
		GainGold(100);
	}

	public CharacterSaveData Save()
	{
		return new CharacterSaveData(this);
	}

	public override bool Equals(object other)
	{
		if (other is Character character)
		{
			return team == character.team;
		}

		return false;
	}
}
