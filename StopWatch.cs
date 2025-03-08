#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public static class StopWatch
{
	public static float? startTime;

	public static float? stopTime;

	public static int Milliseconds => GetMilliseconds();

	public static float Seconds => (float)Milliseconds / 1000f;

	public static float StopTime => stopTime ?? Time.realtimeSinceStartup;

	public static void Start()
	{
		startTime = Time.realtimeSinceStartup;
		stopTime = null;
	}

	public static int Stop()
	{
		stopTime = Time.realtimeSinceStartup;
		return Milliseconds;
	}

	public static int GetMilliseconds()
	{
		if (startTime.HasValue)
		{
			return Mathf.RoundToInt((StopTime - startTime.Value) * 1000f);
		}

		return 0;
	}
}
