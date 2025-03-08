#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Rewards/Random Charm", fileName = "Random Charm")]
public class BossRewardDataRandomCharm : BossRewardData
{
	[Serializable]
	public new class Data : BossRewardData.Data
	{
		public string upgradeName;

		public CardUpgradeData GetUpgrade()
		{
			return AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", upgradeName);
		}

		public override void Select()
		{
			CardUpgradeData cardUpgradeData = GetUpgrade().Clone();
			References.PlayerData.inventory.upgrades.Add(cardUpgradeData);
			Events.InvokeUpgradeGained(cardUpgradeData);
		}
	}

	[SerializeField]
	public int minTier = 1;

	public override BossRewardData.Data Pull()
	{
		string upgradeName = References.Player.GetComponent<CharacterRewards>().Pull<CardUpgradeData>(this, "Charms", 1, allowDuplicates: false, CheckTier)[0].name;
		return new Data
		{
			type = Type.Charm,
			upgradeName = upgradeName
		};
	}

	public bool CheckTier(DataFile dataFile)
	{
		if (dataFile is CardUpgradeData cardUpgradeData)
		{
			return cardUpgradeData.tier >= minTier;
		}

		return false;
	}
}
