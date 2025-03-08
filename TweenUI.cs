#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using NaughtyAttributes;
using UnityEngine;

public class TweenUI : MonoBehaviour
{
	public enum Property
	{
		Move,
		Rotate,
		Scale
	}

	[Serializable]
	public struct Tween
	{
		public Property property;

		public LeanTweenType ease;

		public AnimationCurve curve;

		public bool randomSigns;

		[Header("Duration")]
		public float duration;

		[Header("Delay")]
		public float delay;

		[Header("Values")]
		public Vector3 to;

		public bool hasFrom;

		public Vector3 from;

		public void Fire(GameObject target, bool ignoreTimeScale)
		{
			LTDescr lTDescr = null;
			switch (property)
			{
				case Property.Move:
				if (hasFrom)
				{
					target.transform.localPosition = GetFrom();
					}
	
					lTDescr = LeanTween.moveLocal(target, to, duration);
					break;
				case Property.Rotate:
				if (hasFrom)
				{
					target.transform.localEulerAngles = GetFrom();
					}
	
					lTDescr = LeanTween.rotateLocal(target, to, duration);
					break;
				case Property.Scale:
				if (hasFrom)
				{
					target.transform.localScale = GetFrom();
					}
	
					lTDescr = LeanTween.scale(target, to, duration);
					break;
			}

			if (ignoreTimeScale)
			{
				lTDescr.setIgnoreTimeScale(useUnScaledTime: true);
			}

			if (delay > 0f)
			{
				lTDescr?.setDelay(delay);
			}

			if (ease == LeanTweenType.animationCurve)
			{
				lTDescr?.setEase(curve);
			}
			else
			{
				lTDescr?.setEase(ease);
			}
		}

		public Vector3 GetFrom()
		{
			if (!randomSigns)
			{
				return from;
			}

			return from.With(from.x.WithRandomSign(), from.y.WithRandomSign(), from.z.WithRandomSign());
		}
	}

	[SerializeField]
	public GameObject target;

	[SerializeField]
	[HideIf("fireOnStart")]
	public bool fireOnEnable;

	[SerializeField]
	[HideIf("fireOnEnable")]
	public bool fireOnStart;

	[SerializeField]
	[HideIf("FireOnStart")]
	public bool enableOnFire;

	[SerializeField]
	public bool disableAfter;

	[SerializeField]
	public bool multi;

	[SerializeField]
	[HideIf("multi")]
	public Property property;

	[SerializeField]
	[HideIf("multi")]
	public LeanTweenType ease;

	[SerializeField]
	[HideIf("multi")]
	[ShowIf("IsAnimationCurve")]
	public AnimationCurve animationCurve;

	[SerializeField]
	[HideIf("multi")]
	public bool randomSigns;

	[Header("Duration")]
	[SerializeField]
	[HideIf("multi")]
	public bool randomDuration;

	[SerializeField]
	[HideIf("multiOrRandomDuration")]
	public float duration;

	[SerializeField]
	[HideIf("multi")]
	[ShowIf("randomDuration")]
	public Vector2 durationRange;

	[Header("Delay")]
	[SerializeField]
	[HideIf("multi")]
	public bool randomDelay;

	[SerializeField]
	[HideIf("multiOrRandomDelay")]
	public float delay;

	[SerializeField]
	[HideIf("multi")]
	[ShowIf("randomDelay")]
	public Vector2 delayRange;

	[Header("Values")]
	[SerializeField]
	[HideIf("multi")]
	public Vector3 to;

	[SerializeField]
	[HideIf("multi")]
	public bool hasFrom;

	[SerializeField]
	[HideIf("multi")]
	[ShowIf("hasFrom")]
	public Vector3 from;

	[SerializeField]
	[ShowIf("multi")]
	public Tween[] tweens;

	[SerializeField]
	public bool cancelOtherTweens = true;

	[SerializeField]
	public bool ignoreTimeScale;

	public bool multiOrRandomDuration
	{
		get
		{
			if (!multi)
			{
				return randomDuration;
			}

			return true;
		}
	}

	public bool multiOrRandomDelay
	{
		get
		{
			if (!multi)
			{
				return randomDelay;
			}

			return true;
		}
	}

	public bool IsAnimationCurve => ease == LeanTweenType.animationCurve;

	public bool FireOnStart
	{
		get
		{
			if (!fireOnEnable)
			{
				return fireOnStart;
			}

			return true;
		}
	}

	public void OnEnable()
	{
		if (fireOnEnable)
		{
			Fire();
		}
	}

	public void Start()
	{
		if (fireOnStart)
		{
			Fire();
		}
	}

	public void Fire()
	{
		if (enableOnFire && !FireOnStart)
		{
			target.gameObject.SetActive(value: true);
		}

		if (cancelOtherTweens)
		{
			LeanTween.cancel(target);
		}

		if (multi)
		{
			delay = 0f;
			duration = 0f;
			Tween[] array = tweens;
			for (int i = 0; i < array.Length; i++)
			{
				Tween tween = array[i];
				tween.Fire(target, ignoreTimeScale);
				delay = Mathf.Max(delay, tween.delay);
				duration = Mathf.Max(duration, tween.duration);
			}

			if (disableAfter)
			{
				LeanTween.value(target, 0f, 0f, delay + duration).setIgnoreTimeScale(ignoreTimeScale).setOnComplete((Action)delegate
				{
					target.gameObject.SetActive(value: false);
				});
			}

			return;
		}

		if (randomDuration)
		{
			duration = durationRange.PettyRandom();
		}

		if (randomDelay)
		{
			delay = delayRange.PettyRandom();
		}

		LTDescr lTDescr = null;
		switch (property)
		{
			case Property.Move:
			if (hasFrom)
			{
				target.transform.localPosition = GetFrom();
				}
	
				lTDescr = LeanTween.moveLocal(target, to, duration);
				break;
			case Property.Rotate:
			if (hasFrom)
			{
				target.transform.localEulerAngles = GetFrom();
				}
	
				lTDescr = LeanTween.rotateLocal(target, to, duration);
				break;
			case Property.Scale:
			if (hasFrom)
			{
				target.transform.localScale = GetFrom();
				}
	
				lTDescr = LeanTween.scale(target, to, duration);
				break;
		}

		if (lTDescr == null)
		{
			return;
		}

		if (ignoreTimeScale)
		{
			lTDescr.setIgnoreTimeScale(useUnScaledTime: true);
		}

		if (delay > 0f)
		{
			lTDescr.setDelay(delay);
		}

		if (IsAnimationCurve)
		{
			lTDescr.setEase(animationCurve);
		}
		else
		{
			lTDescr.setEase(ease);
		}

		if (disableAfter)
		{
			lTDescr.setOnComplete((Action)delegate
			{
				target.gameObject.SetActive(value: false);
			});
		}
	}

	public float GetDuration()
	{
		return delay + duration;
	}

	public Vector3 GetFrom()
	{
		if (!randomSigns)
		{
			return from;
		}

		return from.With(from.x.WithRandomSign(), from.y.WithRandomSign(), from.z.WithRandomSign());
	}
}
