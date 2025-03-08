#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Class", menuName = "Character/Class")]
public class ClassData : DataFile, ISaveable<ClassSaveData>
{
	public string id;

	public UnlockData requiresUnlock;

	public Inventory startingInventory;

	public CardData[] leaders;

	public Character characterPrefab;

	public RewardPool[] rewardPools;

	public EventReference selectSfxEvent;

	[ShowAssetPreview(64, 64)]
	public Sprite flag;

	public ClassSaveData Save()
	{
		return new ClassSaveData(base.name);
	}

	public override string ToString()
	{
		return base.name;
	}
}
