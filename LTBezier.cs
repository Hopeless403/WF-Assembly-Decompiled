#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class LTBezier
{
	public float length;

	public Vector3 a;

	public Vector3 aa;

	public Vector3 bb;

	public Vector3 cc;

	public float len;

	public float[] arcLengths;

	public LTBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float precision)
	{
		this.a = a;
		aa = -a + 3f * (b - c) + d;
		bb = 3f * (a + c) - 6f * b;
		cc = 3f * (b - a);
		len = 1f / precision;
		arcLengths = new float[(int)len + 1];
		arcLengths[0] = 0f;
		Vector3 vector = a;
		float num = 0f;
		for (int i = 1; (float)i <= len; i++)
		{
			Vector3 vector2 = bezierPoint((float)i * precision);
			num += (vector - vector2).magnitude;
			arcLengths[i] = num;
			vector = vector2;
		}

		length = num;
	}

	public float map(float u)
	{
		float num = u * arcLengths[(int)len];
		int num2 = 0;
		int num3 = (int)len;
		int num4 = 0;
		while (num2 < num3)
		{
			num4 = num2 + ((int)((float)(num3 - num2) / 2f) | 0);
			if (arcLengths[num4] < num)
			{
				num2 = num4 + 1;
			}
			else
			{
				num3 = num4;
			}
		}

		if (arcLengths[num4] > num)
		{
			num4--;
		}

		if (num4 < 0)
		{
			num4 = 0;
		}

		return ((float)num4 + (num - arcLengths[num4]) / (arcLengths[num4 + 1] - arcLengths[num4])) / len;
	}

	public Vector3 bezierPoint(float t)
	{
		return ((aa * t + bb) * t + cc) * t + a;
	}

	public Vector3 point(float t)
	{
		return bezierPoint(map(t));
	}
}
