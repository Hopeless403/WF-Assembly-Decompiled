#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardLane : CardContainer
{
	[Range(-1f, 1f)]
	public int dir = -1;

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		float num = 3f * cardScale;
		holder.sizeDelta = new Vector2(num * (float)size + gap.x * (float)(size - 1), 4.5f * cardScale);
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		int num = IndexOf(child);
		float num2 = 3f * CardScale;
		_ = CardScale;
		float num3 = holder.sizeDelta.x * 0.5f;
		float num4 = (float)max * num2 * 0.5f + (float)(max - 1) * gap.x * 0.5f;
		float num5 = (float)(-max) * gap.y * 0.5f;
		float num6 = (float)(-max) * gap.z * 0.5f;
		bool num7 = num4 > num3;
		if (num7)
		{
			num4 = num3;
		}

		float num8 = num4 * 2f;
		num4 *= (float)dir;
		num4 += num2 * 0.5f * (float)(-dir);
		float num9 = gap.x;
		if (num7)
		{
			num9 = (num8 - num2 * (float)Count) / (float)Mathf.Max(1, Count - 1);
		}

		float num10 = (num2 + num9) * (float)(-dir);
		float x = num4 + num10 * (float)num;
		float y = num5 + gap.y * (float)num;
		float z = num6 + gap.z * (float)num;
		return new Vector3(x, y, z);
	}

	public void SetDirection(int dir)
	{
		this.dir = dir;
	}
}
