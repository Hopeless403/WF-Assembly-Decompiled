#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;

public class EventRoutine : SceneRoutine
{
	public Character _player;

	public CampaignNode node;

	public Character player => _player ?? (_player = References.Player);

	public Dictionary<string, object> data => node.data;

	public virtual IEnumerator Populate()
	{
		return null;
	}

	public void CheckAddUpgrades(int cardIndex, CardData cardDataClone)
	{
		string key = $"upgrades{cardIndex}";
		if (!node.data.ContainsKey(key))
		{
			return;
		}

		string[] saveCollection = node.data.GetSaveCollection<string>(key);
		foreach (string assetName in saveCollection)
		{
			CardUpgradeData cardUpgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", assetName);
			if ((bool)cardUpgradeData)
			{
				cardUpgradeData.Clone().Assign(cardDataClone);
			}
		}
	}
}
