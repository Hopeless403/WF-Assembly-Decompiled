#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public static class RandomInclusive
{
	public static int Range(int min, int max)
	{
		return Random.Range(min, max + 1);
	}

	public static float Range(float min, float max)
	{
		return Random.Range(min, max);
	}
}
