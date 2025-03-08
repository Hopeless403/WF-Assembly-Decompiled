#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BossRewardSelectModifier : BossRewardSelect
{
	[SerializeField]
	public ImageSprite bellImage;

	[SerializeField]
	public ImageSprite dingerImage;

	public override void SetUp(BossRewardData.Data rewardData, GainBlessingSequence2 gainBlessingSequence)
	{
		base.SetUp(rewardData, gainBlessingSequence);
		if (rewardData is BossRewardDataModifier.Data data)
		{
			GameModifierData modifier = data.GetModifier();
			bellImage.SetSprite(modifier.bellSprite);
			dingerImage.SetSprite(modifier.dingerSprite);
			popUpName = modifier.name;
			title = modifier.titleKey.GetLocalizedString();
			body = modifier.descriptionKey.GetLocalizedString();
		}
	}
}
