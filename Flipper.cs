#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Flipper : MonoBehaviourCacheTransform, IPoolable
{
	public enum State
	{
		None,
		FlipUp,
		FlipDown
	}

	public State state;

	[SerializeField]
	public Entity entity;

	[SerializeField]
	public CardHover hover;

	public Vector2 flipUpDurationRand = new Vector2(0.5f, 0.6f);

	public Vector2 flipDownDurationRand = new Vector2(0.5f, 0.6f);

	public AnimationCurve flipUpCurve;

	public AnimationCurve flipDownCurve;

	public Vector3 flipPositionOffset = new Vector3(0f, 1f, -1f);

	public AnimationCurve flipPositionCurve;

	public UnityEvent onFlipUp;

	public UnityEvent onFlipDown;

	public float preAngle;

	public float angle;

	[ReadOnly]
	public bool flipped;

	public float t = 1f;

	public float duration;

	public bool isCompleteFired = true;

	public void Update()
	{
		if (!(t <= duration))
		{
			return;
		}

		isCompleteFired = false;
		t += Time.deltaTime;
		float time = t / duration;
		switch (state)
		{
			case State.FlipUp:
				UpdateAngle((1f - flipUpCurve.Evaluate(time)) * 180f);
				base.transform.localPosition = flipPositionCurve.Evaluate(time) * flipPositionOffset;
				break;
			case State.FlipDown:
				UpdateAngle(flipDownCurve.Evaluate(time) * 180f);
				base.transform.localPosition = flipPositionCurve.Evaluate(time) * flipPositionOffset;
				break;
		}

		if (t > duration)
		{
			if (!isCompleteFired)
			{
				Events.InvokeEntityFlipComplete(entity);
				isCompleteFired = true;
			}

			state = State.None;
		}
	}

	public void UpdateAngle(float angle)
	{
		this.angle = angle;
		if (preAngle < 90f && angle >= 90f)
		{
			flipped = true;
			onFlipDown.Invoke();
		}

		if (preAngle > 90f && angle <= 90f)
		{
			flipped = false;
			onFlipUp.Invoke();
		}

		preAngle = angle;
		base.transform.localEulerAngles = base.transform.localEulerAngles.WithY(angle);
	}

	[Button(null, EButtonEnableMode.Always)]
	public void FlipUp(bool force = false)
	{
		if (force || flipped || state == State.FlipDown)
		{
			preAngle = angle;
			t = 0f;
			duration = flipUpDurationRand.PettyRandom();
			state = State.FlipUp;
			hover.SetHoverable(value: true);
			Events.InvokeEntityFlipUp(entity);
		}
	}

	public void FlipUpInstant()
	{
		base.transform.localEulerAngles = base.transform.localEulerAngles.WithY(0f);
		angle = 0f;
		preAngle = 0f;
		flipped = false;
		onFlipUp.Invoke();
		state = State.None;
		hover.SetHoverable(value: true);
	}

	[Button(null, EButtonEnableMode.Always)]
	public void FlipDown(bool force = false)
	{
		if (force || !flipped || state == State.FlipUp)
		{
			preAngle = angle;
			t = 0f;
			duration = flipDownDurationRand.PettyRandom();
			state = State.FlipDown;
			hover.SetHoverable(value: false);
			Events.InvokeEntityFlipDown(entity);
		}
	}

	public void FlipDownInstant()
	{
		base.transform.localEulerAngles = base.transform.localEulerAngles.WithY(180f);
		angle = 180f;
		preAngle = 180f;
		flipped = true;
		onFlipDown.Invoke();
		state = State.None;
		hover.SetHoverable(value: false);
	}

	public void OnGetFromPool()
	{
	}

	public void OnReturnToPool()
	{
		state = State.None;
		preAngle = 0f;
		angle = 0f;
		flipped = false;
		t = 1f;
		duration = 0f;
		isCompleteFired = true;
		base.transform.localRotation = Quaternion.identity;
		base.transform.localPosition = Vector3.zero;
	}
}
