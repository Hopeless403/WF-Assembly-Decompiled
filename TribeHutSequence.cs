#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class TribeHutSequence : BuildingSequence
{
	[SerializeField]
	public TribeFlagDisplay[] flags;

	public GameMode _gameMode;

	public GameMode gameMode => _gameMode ?? (_gameMode = AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"));

	public override IEnumerator Sequence()
	{
		SetupFlags();
		yield return null;
	}

	public void SetupFlags()
	{
		int valueOrDefault = (1 + building.checkedUnlocks?.Count).GetValueOrDefault();
		int num = gameMode.classes.Length;
		for (int i = 0; i < flags.Length; i++)
		{
			if (i < num)
			{
				flags[i].SetAvailable();
				if (i < valueOrDefault)
				{
					flags[i].SetUnlocked();
				}
			}
		}
	}
}
