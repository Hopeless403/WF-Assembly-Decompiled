#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;

[Serializable]
public class TraitSaveData : ILoadable<CardData.TraitStacks>
{
	public string name;

	public int count;

	public CardData.TraitStacks Load()
	{
		TraitData traitData = AddressableLoader.Get<TraitData>("TraitData", name);
		if (!traitData)
		{
			return null;
		}

		return new CardData.TraitStacks
		{
			data = traitData,
			count = count
		};
	}
}
