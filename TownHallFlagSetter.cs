#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class TownHallFlagSetter : MonoBehaviour
{
	[SerializeField]
	public GameObject[] flags;

	public void SetupFlags()
	{
		GameMode gameMode = AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal");
		List<ClassData> lockedClasses = MetaprogressionSystem.GetLockedClasses();
		for (int i = 0; i < gameMode.classes.Length; i++)
		{
			ClassData item = gameMode.classes[i];
			bool active = !lockedClasses.Contains(item);
			if (flags.Length > i)
			{
				flags[i].SetActive(active);
			}
		}
	}
}
