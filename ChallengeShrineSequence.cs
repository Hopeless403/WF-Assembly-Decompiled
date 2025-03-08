#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class ChallengeShrineSequence : BuildingSequenceWithUnlocks<ChallengeShrineSequence>
{
	public void Start()
	{
		_OnStart();
	}

	public void OnEnable()
	{
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		Dictionary<string, string> dictionary = MetaprogressionSystem.Get<Dictionary<string, string>>("charms");
		for (int i = 0; i < challengeStones.Length; i++)
		{
			ChallengeStone challengeStone = challengeStones[i];
			if (unlockedList.Contains(challengeStone.challenge.reward.name))
			{
				string text = dictionary[challengeStone.challenge.reward.name];
				CardUpgradeData upgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", text);
				challengeStone.Open(upgradeData);
				CardDiscoverSystem.CheckDiscoverCharm(text);
			}
		}
	}
}
