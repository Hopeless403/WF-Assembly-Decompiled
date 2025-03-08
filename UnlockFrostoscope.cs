#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class UnlockFrostoscope : MonoBehaviour
{
	public void Check(GameObject gameObject)
	{
		Building component = gameObject.GetComponent<Building>();
		if ((object)component != null)
		{
			Check(component);
		}
	}

	public void Check(Building building)
	{
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		UnlockData finished = building.type.finished;
		if (!MetaprogressionSystem.IsUnlocked(finished, unlockedList) && SaveSystem.LoadProgressData<CardSaveData[]>("finalBossDeck") != null)
		{
			unlockedList.Add(finished.name);
			SaveSystem.SaveProgressData("unlocked", unlockedList);
			MetaprogressionSystem.SetUnlocksReady(finished.name);
			building.CheckIfUnlocked();
		}
	}
}
