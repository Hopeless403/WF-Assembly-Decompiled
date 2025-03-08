#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CardUpgradeData", menuName = "Card Upgrade Data")]
public class CardUpgradeData : DataFile, ISaveable<CardUpgradeSaveData>
{
	public enum Type
	{
		None,
		Charm,
		Token,
		Crown
	}

	public int tier;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString textKey;

	[ShowAssetPreview(64, 64)]
	public Sprite image;

	public Type type;

	public CardData.StatusEffectStacks[] attackEffects;

	public CardData.StatusEffectStacks[] effects;

	public CardData.TraitStacks[] giveTraits;

	public CardScript[] scripts;

	public bool becomesTargetedCard;

	public bool canBeRemoved;

	public bool takeSlot = true;

	[Header("Constraints for applying this to a card")]
	[SerializeField]
	public TargetConstraint[] targetConstraints;

	[Header("Stat Changes")]
	public int damage;

	public int hp;

	public int counter;

	public int uses;

	public int effectBonus;

	[Header("Set Exact Stats")]
	public bool setDamage;

	public bool setHp;

	public bool setCounter;

	public bool setUses;

	public List<CardData.StatusEffectStacks> effectsAffected;

	public List<CardData.TraitStacks> traitsAffected;

	public List<CardData.StatusEffectStacks> attackEffectsApplied;

	public List<CardData.StatusEffectStacks> startWithEffectsApplied;

	public int damageChange;

	public int hpChange;

	public int counterChange;

	public int usesChange;

	public string title => titleKey.GetLocalizedString();

	public string text => textKey.GetLocalizedString();

	public void Assign(CardData cardData)
	{
		int num = cardData.damage;
		int num2 = cardData.hp;
		int num3 = cardData.counter;
		int num4 = cardData.uses;
		AdjustStats(cardData);
		RunScripts(cardData);
		damageChange = cardData.damage - num;
		hpChange = cardData.hp - num2;
		counterChange = cardData.counter - num3;
		usesChange = cardData.uses - num4;
		AdjustEffectBonus(cardData);
		GainEffects(cardData);
		cardData.upgrades.Add(this);
	}

	public void AdjustStats(CardData cardData)
	{
		if (becomesTargetedCard)
		{
			cardData.hasAttack = true;
			if (cardData.playType == Card.PlayType.None)
			{
				cardData.playType = Card.PlayType.Play;
			}

			cardData.needsTarget = true;
		}

		bool flag = cardData.counter > 0;
		cardData.damage = (setDamage ? damage : (cardData.damage + damage));
		cardData.hp = (setHp ? hp : (cardData.hp + hp));
		cardData.counter = (setCounter ? counter : Mathf.Max(flag ? 1 : 0, cardData.counter + counter));
		cardData.uses = (setUses ? uses : (cardData.uses + uses));
		cardData.damage = Mathf.Max(0, cardData.damage);
		cardData.hp = Mathf.Max(0, cardData.hp);
		cardData.counter = Mathf.Max(0, cardData.counter);
		cardData.uses = Mathf.Max(0, cardData.uses);
	}

	public void RunScripts(CardData cardData)
	{
		CardScript[] array = scripts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Run(cardData);
		}
	}

	public void AdjustEffectBonus(CardData cardData)
	{
		if (effectBonus == 0)
		{
			return;
		}

		effectsAffected = new List<CardData.StatusEffectStacks>();
		CardData.StatusEffectStacks[] array = cardData.attackEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in array)
		{
			if (statusEffectStacks.data.isStatus || statusEffectStacks.data.stackable)
			{
				statusEffectStacks.count = Mathf.Max(0, statusEffectStacks.count + effectBonus);
				effectsAffected.Add(statusEffectStacks);
			}
		}

		array = cardData.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks2 in array)
		{
			if (statusEffectStacks2.data.canBeBoosted)
			{
				statusEffectStacks2.count = Mathf.Max(0, statusEffectStacks2.count + effectBonus);
				effectsAffected.Add(statusEffectStacks2);
			}
		}

		traitsAffected = new List<CardData.TraitStacks>();
		foreach (CardData.TraitStacks trait in cardData.traits)
		{
			if (trait.data.keyword.canStack)
			{
				trait.count = Mathf.Max(0, trait.count + effectBonus);
				traitsAffected.Add(trait);
			}
		}
	}

	public void GainEffects(CardData cardData)
	{
		if (attackEffects.Length != 0)
		{
			attackEffectsApplied = new List<CardData.StatusEffectStacks>();
			List<CardData.StatusEffectStacks> list = cardData.attackEffects.ToList();
			for (int num = attackEffects.Length - 1; num >= 0; num--)
			{
				CardData.StatusEffectStacks e2 = attackEffects[num];
				CardData.StatusEffectStacks statusEffectStacks = list.Find((CardData.StatusEffectStacks a) => a.data == e2.data);
				if (statusEffectStacks != null)
				{
					statusEffectStacks.count += e2.count;
				}
				else
				{
					list.Add(new CardData.StatusEffectStacks(e2.data, e2.count));
				}

				attackEffectsApplied.Add(e2);
			}

			cardData.attackEffects = list.ToArray();
		}

		if (effects.Length != 0)
		{
			startWithEffectsApplied = new List<CardData.StatusEffectStacks>();
			List<CardData.StatusEffectStacks> list2 = cardData.startWithEffects.ToList();
			for (int num2 = effects.Length - 1; num2 >= 0; num2--)
			{
				CardData.StatusEffectStacks e = effects[num2];
				CardData.StatusEffectStacks statusEffectStacks2 = list2.Find((CardData.StatusEffectStacks a) => a.data == e.data);
				if (statusEffectStacks2 != null)
				{
					statusEffectStacks2.count += e.count;
				}
				else
				{
					list2.Add(new CardData.StatusEffectStacks(e.data, e.count));
				}

				startWithEffectsApplied.Add(e);
			}

			cardData.startWithEffects = list2.ToArray();
		}

		CardData.TraitStacks.Stack(ref cardData.traits, giveTraits);
	}

	public IEnumerator Assign(Entity entity)
	{
		Events.InvokeUpgradeAssign(entity, this);
		Assign(entity.data);
		yield return entity.ClearStatuses();
		if (entity.display is Card card)
		{
			yield return card.UpdateData();
		}
	}

	public void UnAssign(CardData assignedTo)
	{
		assignedTo.damage -= damageChange;
		assignedTo.hp -= hpChange;
		assignedTo.counter -= counterChange;
		assignedTo.uses -= usesChange;
		if (effectBonus != 0)
		{
			foreach (CardData.StatusEffectStacks item in effectsAffected)
			{
				item.count -= effectBonus;
			}

			effectsAffected = null;
			foreach (CardData.TraitStacks item2 in traitsAffected)
			{
				item2.count -= effectBonus;
			}

			traitsAffected = null;
		}

		if (attackEffectsApplied != null && attackEffectsApplied.Count > 0)
		{
			List<CardData.StatusEffectStacks> list = assignedTo.attackEffects.ToList();
			for (int num = attackEffectsApplied.Count - 1; num >= 0; num--)
			{
				CardData.StatusEffectStacks e2 = attackEffectsApplied[num];
				CardData.StatusEffectStacks statusEffectStacks = list.Find((CardData.StatusEffectStacks a) => a.data == e2.data);
				statusEffectStacks.count -= e2.count;
				if (statusEffectStacks.count <= 0)
				{
					list.Remove(e2);
				}
			}
		}

		if (startWithEffectsApplied != null && startWithEffectsApplied.Count > 0)
		{
			List<CardData.StatusEffectStacks> list2 = assignedTo.startWithEffects.ToList();
			for (int num2 = startWithEffectsApplied.Count - 1; num2 >= 0; num2--)
			{
				CardData.StatusEffectStacks e = startWithEffectsApplied[num2];
				CardData.StatusEffectStacks statusEffectStacks2 = list2.Find((CardData.StatusEffectStacks a) => a.data == e.data);
				statusEffectStacks2.count -= e.count;
				if (statusEffectStacks2.count <= 0)
				{
					list2.Remove(e);
				}
			}

			assignedTo.startWithEffects = list2.ToArray();
			startWithEffectsApplied = null;
		}

		assignedTo.upgrades.Remove(this);
	}

	public bool CanAssign(Entity card)
	{
		CardData data = card.data;
		if (damage != 0 && !data.hasAttack)
		{
			return false;
		}

		if (hp != 0 && !data.hasHealth)
		{
			return false;
		}

		if (counter != 0 && data.counter <= 0)
		{
			return false;
		}

		foreach (CardData.TraitStacks t2 in giveTraits.Where((CardData.TraitStacks t) => !t.data.keyword.canStack))
		{
			if (data.traits.Any((CardData.TraitStacks a) => a.data.name == t2.data.name))
			{
				return false;
			}
		}

		TargetConstraint[] array = targetConstraints;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Check(card))
			{
				return false;
			}
		}

		return CheckSlots(data);
	}

	public bool CanAssign(CardData cardData)
	{
		if (damage != 0 && !cardData.hasAttack)
		{
			return false;
		}

		if (hp != 0 && !cardData.hasHealth)
		{
			return false;
		}

		if (counter != 0 && cardData.counter <= 0)
		{
			return false;
		}

		foreach (CardData.TraitStacks t2 in giveTraits.Where((CardData.TraitStacks t) => !t.data.keyword.canStack))
		{
			if (cardData.traits.Any((CardData.TraitStacks a) => a.data.Equals(t2.data)))
			{
				return false;
			}
		}

		TargetConstraint[] array = targetConstraints;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Check(cardData))
			{
				return false;
			}
		}

		return CheckSlots(cardData);
	}

	public void Display(Entity entity)
	{
		if (type == Type.None || !(entity.display is Card card))
		{
			return;
		}

		switch (type)
		{
			case Type.Charm:
			if ((bool)card.charmHolder)
			{
				card.charmHolder.Create(this);
				card.charmHolder.SetPositions();
				}
	
				break;
			case Type.Token:
			if ((bool)card.tokenHolder)
			{
				card.tokenHolder.Create(this);
				card.tokenHolder.SetPositions();
				}
	
				break;
			case Type.Crown:
			if ((bool)card.crownHolder)
			{
				card.crownHolder.Create(this);
				card.crownHolder.SetPositions();
				}
	
				break;
		}
	}

	public CardUpgradeData Clone()
	{
		return this.InstantiateKeepName();
	}

	public CardUpgradeSaveData Save()
	{
		return new CardUpgradeSaveData(base.name);
	}

	public bool CheckSlots(CardData cardData)
	{
		if (!takeSlot)
		{
			return true;
		}

		switch (type)
		{
			case Type.Charm:
			{
				int count = cardData.upgrades.FindAll((CardUpgradeData a) => a.type == type && a.takeSlot).Count;
				int num = cardData.charmSlots;
			if (cardData.customData != null)
			{
				num += cardData.customData.Get("extraCharmSlots", 0);
				}
	
			if (count >= num)
			{
				return false;
				}
	
				break;
			}
			case Type.Token:
			if (cardData.upgrades.FindAll((CardUpgradeData a) => a.type == type && a.takeSlot).Count >= cardData.tokenSlots)
			{
				return false;
				}
	
				break;
			case Type.Crown:
			if (cardData.upgrades.FindAll((CardUpgradeData a) => a.type == type && a.takeSlot).Count >= cardData.crownSlots)
			{
				return false;
				}
	
				break;
		}

		return true;
	}
}
