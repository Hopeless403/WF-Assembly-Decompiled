#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public class LTDescrOptional
{
	public AnimationCurve animationCurve;

	public int initFrameCount;

	public Color color;

	public Transform toTrans { get; set; }

	public Vector3 point { get; set; }

	public Vector3 axis { get; set; }

	public float lastVal { get; set; }

	public Quaternion origRotation { get; set; }

	public LTBezierPath path { get; set; }

	public LTSpline spline { get; set; }

	public LTRect ltRect { get; set; }

	public Action<float> onUpdateFloat { get; set; }

	public Action<float, float> onUpdateFloatRatio { get; set; }

	public Action<float, object> onUpdateFloatObject { get; set; }

	public Action<Vector2> onUpdateVector2 { get; set; }

	public Action<Vector3> onUpdateVector3 { get; set; }

	public Action<Vector3, object> onUpdateVector3Object { get; set; }

	public Action<Color> onUpdateColor { get; set; }

	public Action<Color, object> onUpdateColorObject { get; set; }

	public Action onComplete { get; set; }

	public Action<object> onCompleteObject { get; set; }

	public object onCompleteParam { get; set; }

	public object onUpdateParam { get; set; }

	public Action onStart { get; set; }

	public void reset()
	{
		animationCurve = null;
		onUpdateFloat = null;
		onUpdateFloatRatio = null;
		onUpdateVector2 = null;
		onUpdateVector3 = null;
		onUpdateFloatObject = null;
		onUpdateVector3Object = null;
		onUpdateColor = null;
		onComplete = null;
		onCompleteObject = null;
		onCompleteParam = null;
		onStart = null;
		point = Vector3.zero;
		initFrameCount = 0;
	}

	public void callOnUpdate(float val, float ratioPassed)
	{
		if (onUpdateFloat != null)
		{
			onUpdateFloat(val);
		}

		if (onUpdateFloatRatio != null)
		{
			onUpdateFloatRatio(val, ratioPassed);
		}
		else if (onUpdateFloatObject != null)
		{
			onUpdateFloatObject(val, onUpdateParam);
		}

		else if (onUpdateVector3Object != null)
		{
			onUpdateVector3Object(LTDescr.newVect, onUpdateParam);
		}

		else if (onUpdateVector3 != null)
		{
			onUpdateVector3(LTDescr.newVect);
		}

		else if (onUpdateVector2 != null)
		{
			onUpdateVector2(new Vector2(LTDescr.newVect.x, LTDescr.newVect.y));
		}
	}
}
