#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BossRewardSelectCharm : BossRewardSelect
{
	[SerializeField]
	public ImageSprite charmImage;

	public override void SetUp(BossRewardData.Data rewardData, GainBlessingSequence2 gainBlessingSequence)
	{
		base.SetUp(rewardData, gainBlessingSequence);
		if (rewardData is BossRewardDataRandomCharm.Data data)
		{
			CardUpgradeData upgrade = data.GetUpgrade();
			charmImage.SetSprite(upgrade.image);
			popUpName = upgrade.name;
			title = upgrade.title;
			body = upgrade.text;
		}
	}
}
