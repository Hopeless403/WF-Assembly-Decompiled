#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using NaughtyAttributes;
using UnityEngine;

public class CameraRumbleSystem : GameSystem
{
	public Transform target;

	[HorizontalLine(2f, EColor.Gray)]
	public float strength = 1f;

	public Vector3 positionInfluence = new Vector3(1f, 1f, 1f);

	public Vector3 rotationInfluence = new Vector3(1f, 1f, 1f);

	public float t;

	public float d;

	[SerializeField]
	public AnimationCurve curve;

	public bool active;

	public float delay;

	public void OnEnable()
	{
		Events.OnScreenRumble += Rumble;
	}

	public void OnDisable()
	{
		Events.OnScreenRumble -= Rumble;
	}

	public void Update()
	{
		if (!active)
		{
			return;
		}

		if (delay > 0f)
		{
			delay -= Time.deltaTime;
			return;
		}

		t += Time.deltaTime;
		float num = ((t <= d) ? curve.Evaluate(t) : 0f) * strength * CameraShakerSystem.ShakeAmount;
		if (num > 0f)
		{
			target.localPosition = Vector3.Cross(positionInfluence, PettyRandom.Vector3()) * num;
			target.localEulerAngles = Vector3.Cross(rotationInfluence, PettyRandom.Vector3()) * num;
		}
		else
		{
			target.localPosition = Vector3.zero;
			target.localEulerAngles = Vector3.zero;
		}

		if (t > d)
		{
			active = false;
		}
	}

	public void Rumble(float startStrength, float endStrength, float delay, float fadeInTime, float holdTime, float fadeOutTime)
	{
		this.delay = delay;
		t = 0f;
		d = fadeInTime + holdTime + fadeOutTime;
		curve = new AnimationCurve(new Keyframe(0f, startStrength, 0f, 0f), new Keyframe(fadeInTime, endStrength, 0f, 0f), new Keyframe(fadeInTime + holdTime, endStrength, 0f, 0f), new Keyframe(d, 0f, 0f, 0f));
		active = true;
		target.localPosition = Vector3.zero;
		target.localEulerAngles = Vector3.zero;
	}
}
