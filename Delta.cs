#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public static class Delta
{
	public static readonly float targetFps = 60f;

	public static float Multiply(float current, float multiply, float delta)
	{
		return current * Mathf.Pow(multiply, delta * targetFps);
	}

	public static Vector2 Multiply(Vector2 current, float multiply, float delta)
	{
		return current * Mathf.Pow(multiply, delta * targetFps);
	}

	public static Vector2 Multiply(Vector2 current, Vector3 multiply, float delta)
	{
		float p = delta * targetFps;
		float x = current.x * Mathf.Pow(multiply.x, p);
		float y = current.y * Mathf.Pow(multiply.y, p);
		return new Vector2(x, y);
	}

	public static Vector3 Multiply(Vector3 current, float multiply, float delta)
	{
		return current * Mathf.Pow(multiply, delta * targetFps);
	}

	public static Vector3 Multiply(Vector3 current, Vector3 multiply, float delta)
	{
		float p = delta * targetFps;
		float x = current.x * Mathf.Pow(multiply.x, p);
		float y = current.y * Mathf.Pow(multiply.y, p);
		float z = current.z * Mathf.Pow(multiply.z, p);
		return new Vector3(x, y, z);
	}

	public static float Lerp(float current, float target, float amount, float delta)
	{
		return Mathf.Lerp(current, target, 1f - Mathf.Pow(1f - amount, delta * targetFps));
	}

	public static Vector2 Lerp(Vector2 current, Vector2 target, float amount, float delta)
	{
		return Vector2.Lerp(current, target, 1f - Mathf.Pow(1f - amount, delta * targetFps));
	}

	public static Vector3 Lerp(Vector3 current, Vector3 target, float amount, float delta)
	{
		return Vector3.Lerp(current, target, 1f - Mathf.Pow(1f - amount, delta * targetFps));
	}

	public static Quaternion Lerp(Quaternion current, Quaternion target, float amount, float delta)
	{
		return Quaternion.Lerp(current, target, 1f - Mathf.Pow(1f - amount, delta * targetFps));
	}

	public static Vector3 LerpAngle(Vector3 current, Vector3 target, float amount, float delta)
	{
		float t = 1f - Mathf.Pow(1f - amount, delta * targetFps);
		float x = Mathf.LerpAngle(current.x, target.x, t);
		float y = Mathf.LerpAngle(current.y, target.y, t);
		float z = Mathf.LerpAngle(current.z, target.z, t);
		return new Vector3(x, y, z);
	}
}
