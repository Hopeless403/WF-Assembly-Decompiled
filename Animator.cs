#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
	[Serializable]
	public struct Animation
	{
		public enum Property
		{
			Move,
			Rotate,
			Scale
		}

		public string name;

		public Property property;

		public AnimationCurve curve;

		public Vector2 duration;

		public Vector2 delay;

		public Vector3 to;

		public Vector3 from;

		public bool hasFrom;

		public bool loop;

		public void Play(GameObject target)
		{
			LeanTween.cancel(target);
			LTDescr lTDescr = null;
			switch (property)
			{
				case Property.Move:
				if (hasFrom)
				{
					target.transform.localPosition = from;
					}
	
					lTDescr = LeanTween.moveLocal(target, to, duration.PettyRandom());
					break;
				case Property.Rotate:
				if (hasFrom)
				{
					target.transform.localEulerAngles = from;
					}
	
					lTDescr = LeanTween.rotateLocal(target, to, duration.PettyRandom());
					break;
				case Property.Scale:
				if (hasFrom)
				{
					target.transform.localScale = from;
					}
	
					lTDescr = LeanTween.scale(target, to, duration.PettyRandom());
					break;
			}

			float num = delay.PettyRandom();
			if (num > 0f)
			{
				lTDescr?.setDelay(num);
			}

			lTDescr?.setEase(curve);
			if (loop)
			{
				lTDescr?.setLoopClamp();
			}
		}
	}

	[SerializeField]
	public Animation[] animations;

	public Dictionary<string, Animation> lookup;

	public void Awake()
	{
		lookup = new Dictionary<string, Animation>();
		Animation[] array = animations;
		for (int i = 0; i < array.Length; i++)
		{
			Animation value = array[i];
			lookup[value.name] = value;
		}
	}

	public void Play(string animationName)
	{
		if (lookup.ContainsKey(animationName))
		{
			lookup[animationName].Play(base.gameObject);
		}
	}

	public void Stop()
	{
		LeanTween.cancel(base.gameObject);
	}
}
