#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class SnowBear : ScriptableCardImage
{
	[SerializeField]
	public Transform main;

	[SerializeField]
	public Transform ball;

	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public AnimationCurve ballScaleCurve;

	[SerializeField]
	public AnimationCurve ballYCurve;

	[SerializeField]
	public AnimationCurve yCurve;

	[Header("Tween")]
	[SerializeField]
	public AnimationCurve tweenCurve;

	[SerializeField]
	public float tweenDur;

	public float tweenFrom;

	public float tweenTo;

	public float tweenT;

	public bool tween;

	public bool currentSet;

	public int current;

	public override void UpdateEvent()
	{
		if (!currentSet || current != entity.damage.current)
		{
			currentSet = true;
			current = entity.damage.current;
			Set();
		}
	}

	public void Set()
	{
		tweenFrom = tweenTo;
		tweenTo = curve.Evaluate(current);
		StartTween();
	}

	public void StartTween()
	{
		tween = true;
		tweenT = 0f;
	}

	public void Update()
	{
		if (tween)
		{
			tweenT += Time.deltaTime / tweenDur;
			float progress = tweenFrom + tweenCurve.Evaluate(tweenT) * (tweenTo - tweenFrom);
			UpdateValues(progress);
			if (tweenT > 1f)
			{
				tween = false;
			}
		}
	}

	public void UpdateValues(float progress)
	{
		float num = ballScaleCurve.Evaluate(progress);
		float value = ballYCurve.Evaluate(progress);
		float value2 = yCurve.Evaluate(progress);
		ball.localScale = new Vector3(num, num, 1f);
		ball.localPosition = ball.localPosition.WithY(value);
		main.localPosition = main.localPosition.WithY(value2);
	}
}
