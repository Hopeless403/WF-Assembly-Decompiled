#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Rewards/Crown", fileName = "Crown")]
public class BossRewardDataCrown : BossRewardData
{
	[Serializable]
	public new class Data : BossRewardData.Data
	{
		public string upgradeDataName;

		public CardUpgradeData GetUpgrade()
		{
			return AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", upgradeDataName.IsNullOrWhitespace() ? "Crown" : upgradeDataName);
		}

		public override void Select()
		{
			CardUpgradeData cardUpgradeData = GetUpgrade().Clone();
			References.PlayerData.inventory.upgrades.Add(cardUpgradeData);
			Events.InvokeUpgradeGained(cardUpgradeData);
		}
	}

	public override BossRewardData.Data Pull()
	{
		return new Data
		{
			type = Type.Crown
		};
	}
}
