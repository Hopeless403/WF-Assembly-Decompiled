#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public static class Lengthdir
{
	public static float X(float len, float radians)
	{
		return Mathf.Cos(radians) * len;
	}

	public static float Y(float len, float radians)
	{
		return Mathf.Sin(radians) * len;
	}

	public static Vector2 ToVector2(float len, float radians)
	{
		return new Vector2(X(len, radians), Y(len, radians));
	}
}
