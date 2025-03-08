#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Dead;

[Serializable]
public class ProgressSaveData
{
	public int nextSeed;

	public int tutorialProgress;

	public bool tutorialCharmDone;

	public bool tutorialInjuryDone;

	public CardSaveData[] finalBossEnemies;

	public static ProgressSaveData Default()
	{
		return new ProgressSaveData
		{
			nextSeed = Dead.Random.Seed()
		};
	}
}
