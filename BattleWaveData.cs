#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class BattleWaveData
{
	[Serializable]
	public class Wave
	{
		public int counter;

		public ulong[] unitIds;

		public bool isBossWave;

		public bool spawned;

		public Wave()
		{
		}

		public Wave(BattleWaveManager.Wave wave)
		{
			counter = wave.counter;
			unitIds = (from a in wave.units
				where a
				select a.id).ToArray();
			isBossWave = wave.isBossWave;
			spawned = wave.spawned;
		}
	}

	public List<ulong> deployed;

	public int counter;

	public int counterMax;

	public int currentWave;

	public int overflowWaveIndex;

	public Wave[] waves;
}
