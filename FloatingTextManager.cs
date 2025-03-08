#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FloatingTextManager : MonoBehaviourSingleton<FloatingTextManager>
{
	[Serializable]
	public struct Animation
	{
		[Serializable]
		public struct Tween
		{
			public enum Property
			{
				Move,
				Scale,
				Rotate
			}

			public LeanTweenType ease;

			[ShowIf("IsAnimationCurve")]
			public AnimationCurve curve;

			public float duration;

			public float delay;

			public Property property;

			public bool relative;

			public Vector3 to;

			public bool hasFrom;

			[ShowIf("hasFrom")]
			public Vector3 from;

			public bool IsAnimationCurve => ease == LeanTweenType.animationCurve;

			public void Fire(GameObject target, float strength)
			{
				LTDescr lTDescr = null;
				switch (property)
				{
					case Property.Move:
					{
						Vector3 localPosition = target.transform.localPosition;
					if (hasFrom)
					{
						target.transform.localPosition = (relative ? (localPosition + from * strength) : from);
						}
	
						lTDescr = LeanTween.moveLocal(target, relative ? (localPosition + to * strength) : to, duration);
						break;
					}
					case Property.Rotate:
					{
						Vector3 localEulerAngles = target.transform.localEulerAngles;
					if (hasFrom)
					{
						target.transform.localEulerAngles = (relative ? (localEulerAngles + from * strength) : from);
						}
	
						lTDescr = LeanTween.rotateLocal(target, relative ? (localEulerAngles + to * strength) : to, duration);
						break;
					}
					case Property.Scale:
					{
						Vector3 localScale = target.transform.localScale;
					if (hasFrom)
					{
						target.transform.localScale = (relative ? (localScale + from * strength) : from);
						}
	
						lTDescr = LeanTween.scale(target, relative ? (localScale + to * strength) : to, duration);
						break;
					}
				}

				if (IsAnimationCurve)
				{
					lTDescr?.setEase(curve);
				}
				else
				{
					lTDescr?.setEase(ease);
				}

				if (delay > 0f)
				{
					lTDescr?.setDelay(delay);
				}
			}

			public float GetDuration()
			{
				return duration + delay;
			}
		}

		public string name;

		public Tween[] tweens;

		public void Fire(GameObject target, float strength)
		{
			Tween[] array = tweens;
			foreach (Tween tween in array)
			{
				tween.Fire(target, strength);
			}
		}

		public float GetDuration()
		{
			float num = 0f;
			Tween[] array = tweens;
			foreach (Tween tween in array)
			{
				num += tween.GetDuration();
			}

			return num;
		}
	}

	[Serializable]
	public struct FadeCurve
	{
		public string name;

		public AnimationCurve curve;
	}

	[SerializeField]
	public FloatingText prefab;

	[SerializeField]
	public Animation[] animations;

	[SerializeField]
	public FadeCurve[] fadeCurves;

	public Dictionary<string, Animation> animationDictionary;

	public Dictionary<string, FadeCurve> fadeCurveDictionary;

	public static Queue<FloatingText> pool = new Queue<FloatingText>();

	public void Start()
	{
		animationDictionary = new Dictionary<string, Animation>();
		Animation[] array = animations;
		for (int i = 0; i < array.Length; i++)
		{
			Animation value = array[i];
			animationDictionary[value.name] = value;
		}

		fadeCurveDictionary = new Dictionary<string, FadeCurve>();
		FadeCurve[] array2 = fadeCurves;
		for (int i = 0; i < array2.Length; i++)
		{
			FadeCurve value2 = array2[i];
			fadeCurveDictionary[value2.name] = value2;
		}
	}

	public static Animation GetAnimation(string name)
	{
		return MonoBehaviourSingleton<FloatingTextManager>.instance.animationDictionary[name];
	}

	public static FadeCurve GetFadeCurve(string name)
	{
		return MonoBehaviourSingleton<FloatingTextManager>.instance.fadeCurveDictionary[name];
	}

	public static FloatingText GetFromPool()
	{
		if (pool.Count > 0)
		{
			FloatingText floatingText = pool.Dequeue();
			floatingText.gameObject.SetActive(value: true);
			return floatingText;
		}

		return MonoBehaviourSingleton<FloatingTextManager>.instance.CreatePrefab();
	}

	public static void ReturnToPool(FloatingText item)
	{
		item.Reset();
		item.StopAllCoroutines();
		item.gameObject.SetActive(value: false);
		item.transform.SetParent(MonoBehaviourSingleton<FloatingTextManager>.instance.transform);
		pool.Enqueue(item);
	}

	public FloatingText CreatePrefab()
	{
		FloatingText floatingText = UnityEngine.Object.Instantiate(prefab, base.transform);
		floatingText.gameObject.SetActive(value: true);
		return floatingText;
	}
}
