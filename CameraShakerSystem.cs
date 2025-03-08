#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Dead;
using DeadExtensions;
using NaughtyAttributes;
using UnityEngine;

public class CameraShakerSystem : GameSystem
{
	public Transform target;

	[HorizontalLine(2f, EColor.Gray)]
	public float magnitudeMod = 1f;

	public float angularMod = 0.67f;

	public float accelerationMod = 300f;

	[HorizontalLine(2f, EColor.Gray)]
	[SerializeField]
	public AnimationCurve hitStrengthCurve;

	[HorizontalLine(2f, EColor.Gray)]
	[Range(0f, 1f)]
	public float frict = 0.2f;

	[Range(0f, 180f)]
	public float angleRand = 20f;

	[Range(0f, 1f)]
	public float angularFrict = 0.1f;

	[HorizontalLine(2f, EColor.Gray)]
	public Vector3 positionInfluence = new Vector3(0.2f, 0.2f, 0f);

	public Vector3 rotationInfluence = new Vector3(1f, 1f, 1f);

	public Vector2 pos;

	public Vector2 velocity;

	public float angle;

	public float angularSpeed;

	public static float ShakeAmount;

	public void OnEnable()
	{
		Events.OnEntityHit += Hit;
		Events.OnScreenShake += Shake;
		Events.OnSettingChanged += SettingChanged;
		ShakeAmount = Settings.Load("ScreenShake", 1f);
	}

	public void OnDisable()
	{
		Events.OnEntityHit -= Hit;
		Events.OnScreenShake -= Shake;
		Events.OnSettingChanged -= SettingChanged;
		Stop();
	}

	public static void SettingChanged(string key, object value)
	{
		if (key == "ScreenShake" && value is float)
		{
			float shakeAmount = (float)value;
			ShakeAmount = shakeAmount;
		}
	}

	public void Hit(Hit hit)
	{
		if (hit.Offensive && hit.screenShake > 0f)
		{
			float hitDirection = GetHitDirection(hit);
			int offensiveness = hit.GetOffensiveness();
			float magnitude = hit.screenShake * hitStrengthCurve.Evaluate(offensiveness);
			Shake(magnitude, hitDirection);
		}
	}

	[Button("Test Shake", EButtonEnableMode.Always)]
	public void Shake(float magnitude = 1f)
	{
		Shake(magnitude, DeadExtensions.PettyRandom.value * 360f);
	}

	public void Shake(float magnitude = 1f, float? direction = null)
	{
		float radians = ((direction ?? Dead.PettyRandom.Range(0f, 360f)) + Dead.PettyRandom.Range(0f - angleRand, angleRand)) * (MathF.PI / 180f);
		float len = velocity.magnitude + magnitude * magnitudeMod * ShakeAmount;
		velocity = Lengthdir.ToVector2(len, radians);
		angularSpeed += magnitude * ShakeAmount * angularMod.WithRandomSign();
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Stop()
	{
		velocity = Vector2.zero;
		pos = Vector2.zero;
		angle = 0f;
		angularSpeed = 0f;
		if (target != null)
		{
			target.localPosition = Vector3.zero;
			target.localEulerAngles = Vector3.zero;
		}
	}

	public void Update()
	{
		float magnitude = pos.magnitude;
		if (magnitude > 0.01f)
		{
			float radians = pos.Angle();
			Vector2 vector = Lengthdir.ToVector2(magnitude * accelerationMod, radians);
			velocity -= vector * Time.deltaTime;
		}

		velocity = Delta.Multiply(velocity, 1f - frict, Time.deltaTime);
		pos += velocity * Time.deltaTime;
		angularSpeed -= angle * accelerationMod * Time.deltaTime;
		angularSpeed = Delta.Multiply(angularSpeed, 1f - angularFrict, Time.deltaTime);
		angle += angularSpeed * Time.deltaTime;
		target.localPosition = Vector3.Scale(new Vector3(0f - pos.x, pos.y, angle), positionInfluence);
		target.localEulerAngles = Vector3.Scale(new Vector3(0f - pos.x, pos.y, angle), rotationInfluence);
	}

	public float GetHitDirection(Hit hit)
	{
		if (!hit.attacker)
		{
			return Dead.PettyRandom.Range(0f, 360f);
		}

		return hit.attacker.transform.position.AngleTo(hit.target.transform.position);
	}
}
