#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public static class AspectRatio
{
	public static Vector2 GetAspectRatio(int x, int y)
	{
		float num = (float)x / (float)y;
		int num2 = 0;
		do
		{
			num2++;
		}
		while (Math.Round(num * (float)num2, 2) != (double)Mathf.RoundToInt(num * (float)num2));
		return new Vector2((float)Math.Round(num * (float)num2, 2), num2);
	}

	public static Vector2 GetAspectRatio(Vector2 xy)
	{
		float num = xy.x / xy.y;
		int num2 = 0;
		do
		{
			num2++;
		}
		while (Math.Round(num * (float)num2, 2) != (double)Mathf.RoundToInt(num * (float)num2));
		return new Vector2((float)Math.Round(num * (float)num2, 2), num2);
	}

	public static Vector2 GetAspectRatio(int x, int y, bool debug)
	{
		float num = (float)x / (float)y;
		int num2 = 0;
		do
		{
			num2++;
		}
		while (Math.Round(num * (float)num2, 2) != (double)Mathf.RoundToInt(num * (float)num2));
		if (debug)
		{
			Debug.Log("Aspect ratio is " + num * (float)num2 + ":" + num2 + " (Resolution: " + x + "x" + y + ")");
		}

		return new Vector2((float)Math.Round(num * (float)num2, 2), num2);
	}

	public static Vector2 GetAspectRatio(Vector2 xy, bool debug)
	{
		float num = xy.x / xy.y;
		int num2 = 0;
		do
		{
			num2++;
		}
		while (Math.Round(num * (float)num2, 2) != (double)Mathf.RoundToInt(num * (float)num2));
		if (debug)
		{
			Debug.Log("Aspect ratio is " + num * (float)num2 + ":" + num2 + " (Resolution: " + xy.x + "x" + xy.y + ")");
		}

		return new Vector2((float)Math.Round(num * (float)num2, 2), num2);
	}
}
