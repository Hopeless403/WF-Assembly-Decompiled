#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationSystem : GameSystem
{
	[Serializable]
	public struct Animation
	{
		public string name;

		public Curve[] curves;
	}

	[Serializable]
	public struct Curve
	{
		public AnimationCurve curve;

		public Vector3 moveAmount;

		public Vector3 rotateAmount;

		public Vector2 durationRange;
	}

	public class RunCurve
	{
		public Curve curve;

		public float time;

		public float duration;

		public float value;

		public bool IsDone => time >= duration;

		public RunCurve(Curve curve)
		{
			this.curve = curve;
			duration = curve.durationRange.PettyRandom();
		}

		public void Update(float delta)
		{
			time += delta;
			value = curve.curve.Evaluate(time / duration);
		}

		public Vector3 GetPosOffset()
		{
			return curve.moveAmount * value;
		}

		public Vector3 GetRotOffset()
		{
			return curve.rotateAmount * value;
		}
	}

	public Transform target;

	public Animation[] animations;

	public Dictionary<string, Animation> lookup;

	public List<RunCurve> running = new List<RunCurve>();

	public void OnEnable()
	{
		lookup = new Dictionary<string, Animation>();
		Animation[] array = animations;
		for (int i = 0; i < array.Length; i++)
		{
			Animation value = array[i];
			lookup[value.name] = value;
		}

		running.Clear();
		Events.OnCameraAnimation += Run;
	}

	public void OnDisable()
	{
		Events.OnCameraAnimation -= Run;
	}

	public void Update()
	{
		int count = running.Count;
		if (count <= 0)
		{
			return;
		}

		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		float deltaTime = Time.deltaTime;
		for (int num = count - 1; num >= 0; num--)
		{
			RunCurve runCurve = running[num];
			runCurve.Update(deltaTime);
			zero += runCurve.GetPosOffset();
			zero2 += runCurve.GetRotOffset();
			if (runCurve.IsDone)
			{
				running.RemoveAt(num);
			}
		}

		target.localPosition = zero;
		target.localEulerAngles = zero2;
	}

	public void Run(string name)
	{
		if (lookup.ContainsKey(name))
		{
			Run(lookup[name]);
		}
	}

	public void Run(Animation animation)
	{
		Curve[] curves = animation.curves;
		foreach (Curve curve in curves)
		{
			running.Add(new RunCurve(curve));
		}
	}
}
