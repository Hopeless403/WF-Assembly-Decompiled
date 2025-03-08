#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using NaughtyAttributes;
using UnityEngine;

public class CardHand : CardContainer
{
	[SerializeField]
	public float fanCircleRadius = 20f;

	[SerializeField]
	public Vector3 fanCircleStartPos = new Vector3(0f, -20f, 0f);

	public bool staticAngleAdd;

	[ShowIf("staticAngleAdd")]
	public float fanCircleAngleAdd = 5f;

	[SerializeField]
	[DisableIf("staticAngleAdd")]
	public AnimationCurve fanCircleAngleAddCurve;

	[SerializeField]
	public bool dynamicGap;

	[SerializeField]
	[EnableIf("dynamicGap")]
	public AnimationCurve dynamicGapCurve;

	public override void TweenChildPosition(Entity child)
	{
		child.DrawOrder = GetChildDrawOrder(child);
		base.TweenChildPosition(child);
	}

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		float num = 3f * cardScale;
		holder.sizeDelta = new Vector2(num * (float)size + GetGap(size).x * (float)(size - 1), 4.5f * cardScale);
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		int num = IndexOf(child);
		float radians = (GetAngle(num) + 90f) * (MathF.PI / 180f);
		Vector3 vector = GetGap(Count);
		return fanCircleStartPos + vector * ((float)(Count - 1) * 0.5f) - vector * num + (Vector3)Lengthdir.ToVector2(fanCircleRadius, radians);
	}

	public override Vector3 GetChildRotation(Entity child)
	{
		return new Vector3(0f, 0f, GetAngle(IndexOf(child)));
	}

	public Vector3 GetGap(int cardCount)
	{
		if (!dynamicGap || dynamicGapCurve.length <= 0)
		{
			return gap;
		}

		Keyframe keyframe = dynamicGapCurve[dynamicGapCurve.length - 1];
		float time = keyframe.time;
		float value = (((float)cardCount > time) ? keyframe.value : dynamicGapCurve.Evaluate(cardCount));
		return gap.WithX(value);
	}

	public float GetAngleAdd()
	{
		if (fanCircleAngleAddCurve.length <= 0 || staticAngleAdd)
		{
			return fanCircleAngleAdd;
		}

		Keyframe keyframe = fanCircleAngleAddCurve[fanCircleAngleAddCurve.length - 1];
		float time = keyframe.time;
		if (!((float)Count > time))
		{
			return fanCircleAngleAddCurve.Evaluate(Count);
		}

		return keyframe.value;
	}

	public float GetAngle(int childIndex)
	{
		float angleAdd = GetAngleAdd();
		return (0f - angleAdd) * (float)(Count - 1) * 0.5f + (float)childIndex * angleAdd;
	}
}
