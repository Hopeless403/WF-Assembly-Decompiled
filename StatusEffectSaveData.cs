#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;

[Serializable]
public class StatusEffectSaveData : ILoadable<CardData.StatusEffectStacks>
{
	public string name;

	public int count;

	public CardData.StatusEffectStacks Load()
	{
		StatusEffectData statusEffectData = AddressableLoader.Get<StatusEffectData>("StatusEffectData", name);
		if (!statusEffectData)
		{
			return null;
		}

		return new CardData.StatusEffectStacks
		{
			data = statusEffectData,
			count = count
		};
	}
}
