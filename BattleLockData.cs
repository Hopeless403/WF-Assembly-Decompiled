#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Lock Data", fileName = "Battle Lock Data")]
public class BattleLockData : ScriptableObject
{
	public string battleName;

	public ChallengeData linkToChallenge;

	public bool IsLocked()
	{
		bool flag = !(SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>()).Contains(linkToChallenge.name);
		if (flag)
		{
			Debug.Log("Battle [" + battleName + "] is locked! Requires [" + linkToChallenge.name + "]");
		}

		return flag;
	}
}
