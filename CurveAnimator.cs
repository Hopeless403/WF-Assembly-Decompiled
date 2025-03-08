#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public class CurveAnimator : MonoBehaviourCacheTransform, IPoolable
{
	public static readonly Vector3 rotationInfluence = new Vector3(5f, 7f, 2f);

	public static readonly float rotationDurationMod = 1.5f;

	public static readonly float moveAnimationDur = 0.667f;

	public float pingDuration = 0.667f;

	public Vector3 pingScale = Vector3.one * 1.25f;

	public Vector3 pingMove = Vector3.back;

	public bool active;

	public float Move(Vector3 offset, AnimationCurve curve, float rotationAmount = 1f, float duration = 0.667f)
	{
		CancelTween();
		if ((bool)base.gameObject)
		{
			active = true;
			LeanTween.moveLocal(base.gameObject, offset, duration).setEase(curve).setOnComplete((Action)delegate
			{
				active = false;
			});
			if (rotationAmount != 0f)
			{
				Vector3 offset2 = new Vector3(offset.y * rotationInfluence.x, (0f - offset.x) * rotationInfluence.y, (0f - offset.x) * rotationInfluence.z) * rotationAmount;
				Rotate(offset2, curve, duration * rotationDurationMod);
			}
		}

		return duration;
	}

	public float Rotate(Vector3 offset, AnimationCurve curve, float duration)
	{
		LeanTween.rotateLocal(base.gameObject, offset, duration).setEase(curve);
		return duration;
	}

	public float Scale(Vector3 offset, AnimationCurve curve, float duration)
	{
		CancelTween();
		LeanTween.scale(base.gameObject, offset, duration).setEase(curve);
		return duration;
	}

	public float Ping()
	{
		if ((bool)base.gameObject)
		{
			CancelTween();
			active = true;
			AnimationCurve ease = Curves.Get("Ping");
			LeanTween.moveLocal(base.gameObject, pingMove, pingDuration).setEase(ease).setOnComplete((Action)delegate
			{
				active = false;
			});
			LeanTween.scale(base.gameObject, pingScale, pingDuration).setEase(ease);
			Events.InvokeEntityPing(base.gameObject);
		}

		return pingDuration;
	}

	public void CancelTween()
	{
		if ((bool)base.gameObject)
		{
			LeanTween.cancel(base.gameObject);
			base.transform.localPosition = Vector3.zero;
			base.transform.localEulerAngles = Vector3.zero;
			base.transform.localScale = Vector3.one;
		}
	}

	public void OnGetFromPool()
	{
	}

	public void OnReturnToPool()
	{
		CancelTween();
		active = false;
	}
}
