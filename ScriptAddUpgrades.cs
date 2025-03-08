#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Upgrades", menuName = "Scripts/Add Upgrades")]
public class ScriptAddUpgrades : Script
{
	[SerializeField]
	public int count = 1;

	[SerializeField]
	public CardUpgradeData[] upgradePool;

	[SerializeField]
	[HideIf("addToRandom")]
	public bool addToFirstEligible = true;

	[SerializeField]
	[HideIf("addToFirstEligible")]
	public bool addToRandom;

	[SerializeField]
	public bool ofCardType;

	[SerializeField]
	[ShowIf("ofCardType")]
	public CardType[] eligibleCardTypes;

	public override IEnumerator Run()
	{
		for (int i = 0; i < count; i++)
		{
			AddUpgrade();
		}

		yield break;
	}

	public void AddUpgrade()
	{
		foreach (CardUpgradeData item in upgradePool.InRandomOrder())
		{
			if (TryAddUpgrade(item))
			{
				break;
			}
		}
	}

	public bool TryAddUpgrade(CardUpgradeData upgradeData)
	{
		bool result = false;
		if (addToFirstEligible)
		{
			foreach (CardData item in References.PlayerData.inventory.deck)
			{
				if (Eligible(item) && upgradeData.CanAssign(item))
				{
					upgradeData.Clone().Assign(item);
					result = true;
					break;
				}
			}
		}
		else if (addToRandom)
		{
			foreach (CardData item2 in References.PlayerData.inventory.deck.InRandomOrder())
			{
				if (Eligible(item2) && upgradeData.CanAssign(item2))
				{
					upgradeData.Clone().Assign(item2);
					result = true;
					break;
				}
			}
		}
		else
		{
			References.PlayerData.inventory.upgrades.Add(upgradeData.Clone());
			result = true;
		}

		return result;
	}

	public bool Eligible(CardData cardData)
	{
		if (!ofCardType)
		{
			return true;
		}

		return eligibleCardTypes.Contains(cardData.cardType);
	}
}
