#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;

[Serializable]
public class CardUpgradeSaveData : ILoadable<CardUpgradeData>
{
	public string name;

	public CardUpgradeSaveData()
	{
	}

	public CardUpgradeSaveData(string name)
	{
		this.name = name;
	}

	public CardUpgradeData Load()
	{
		return AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", name)?.Clone();
	}
}
