#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class SnowfallParticles : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	[OnValueChanged("Evaluate")]
	public float storminess;

	[SerializeField]
	public AnimationCurve updateCurve;

	[Header("References")]
	[SerializeField]
	public ParticleSystem backSnow;

	[SerializeField]
	public ParticleSystem midSnow;

	[SerializeField]
	public ParticleSystem frontSnow;

	[SerializeField]
	public ParticleSystem bloops;

	[SerializeField]
	public SpriteRenderer fade;

	[Header("Values To Adjust")]
	[SerializeField]
	public AnimationCurve angleRange;

	[SerializeField]
	public AnimationCurve backEmissionRange;

	[SerializeField]
	public AnimationCurve midEmissionRange;

	[SerializeField]
	public AnimationCurve frontEmissionRange;

	[SerializeField]
	public AnimationCurve bloopsEmissionRange;

	[SerializeField]
	public AnimationCurve fadeAlphaRange;

	[SerializeField]
	public AnimationCurve simulationSpeedRange;

	[SerializeField]
	public AnimationCurve backSnowGravityRange;

	[SerializeField]
	public AnimationCurve midSnowGravityRange;

	[SerializeField]
	public AnimationCurve frontSnowGravityRange;

	[SerializeField]
	public AnimationCurve bloopsGravityRange;

	[SerializeField]
	public AnimationCurve xRange;

	[SerializeField]
	public AnimationCurve backSnowXSpeedRange;

	[SerializeField]
	public AnimationCurve midSnowXSpeedRange;

	[SerializeField]
	public AnimationCurve frontSnowXSpeedRange;

	[SerializeField]
	public AnimationCurve bloopsXSpeedRange;

	public ParticleSystem.MainModule backSnowMain;

	public ParticleSystem.MainModule midSnowMain;

	public ParticleSystem.MainModule frontSnowMain;

	public ParticleSystem.MainModule bloopsMain;

	public ParticleSystem.EmissionModule backSnowEmission;

	public ParticleSystem.EmissionModule midSnowEmission;

	public ParticleSystem.EmissionModule frontSnowEmission;

	public ParticleSystem.EmissionModule bloopsEmission;

	public ParticleSystem.VelocityOverLifetimeModule backSnowVel;

	public ParticleSystem.VelocityOverLifetimeModule midSnowVel;

	public ParticleSystem.VelocityOverLifetimeModule frontSnowVel;

	public ParticleSystem.VelocityOverLifetimeModule bloopsVel;

	public float t = 1f;

	public float storminessFrom;

	public float storminessTo;

	public float d = 1f;

	public void Awake()
	{
		backSnowMain = backSnow.main;
		midSnowMain = midSnow.main;
		frontSnowMain = frontSnow.main;
		bloopsMain = bloops.main;
		backSnowEmission = backSnow.emission;
		midSnowEmission = midSnow.emission;
		frontSnowEmission = frontSnow.emission;
		bloopsEmission = bloops.emission;
		backSnowVel = backSnow.velocityOverLifetime;
		midSnowVel = midSnow.velocityOverLifetime;
		frontSnowVel = frontSnow.velocityOverLifetime;
		bloopsVel = bloops.velocityOverLifetime;
	}

	public void OnEnable()
	{
		Events.OnSetWeatherIntensity += SetStorminess;
		Evaluate();
	}

	public void OnDisable()
	{
		Events.OnSetWeatherIntensity -= SetStorminess;
	}

	public void SetStorminess(float amount, float duration)
	{
		storminessFrom = storminess;
		storminessTo = amount;
		t = 0f;
		d = duration;
	}

	public void Update()
	{
		if (t < 1f)
		{
			t += Time.deltaTime / d;
			float num = updateCurve.Evaluate(t);
			storminess = storminessFrom + (storminessTo - storminessFrom) * num;
			Evaluate();
		}
	}

	public void Evaluate()
	{
		base.transform.localEulerAngles = new Vector3(0f, 0f, angleRange.Evaluate(storminess));
		backSnowEmission.rateOverTime = backEmissionRange.Evaluate(storminess);
		midSnowEmission.rateOverTime = midEmissionRange.Evaluate(storminess);
		frontSnowEmission.rateOverTime = frontEmissionRange.Evaluate(storminess);
		bloopsEmission.rateOverTime = bloopsEmissionRange.Evaluate(storminess);
		fade.color = fade.color.With(-1f, -1f, -1f, fadeAlphaRange.Evaluate(storminess));
		float simulationSpeed = simulationSpeedRange.Evaluate(storminess);
		backSnowMain.simulationSpeed = simulationSpeed;
		midSnowMain.simulationSpeed = simulationSpeed;
		frontSnowMain.simulationSpeed = simulationSpeed;
		bloopsMain.simulationSpeed = simulationSpeed;
		float num = backSnowGravityRange.Evaluate(storminess);
		backSnowMain.gravityModifier = new ParticleSystem.MinMaxCurve(0f - num, num);
		float num2 = midSnowGravityRange.Evaluate(storminess);
		midSnowMain.gravityModifier = new ParticleSystem.MinMaxCurve(0f - num2, num2);
		float num3 = frontSnowGravityRange.Evaluate(storminess);
		frontSnowMain.gravityModifier = new ParticleSystem.MinMaxCurve(0f - num3, num3);
		float num4 = bloopsGravityRange.Evaluate(storminess);
		bloopsMain.gravityModifier = new ParticleSystem.MinMaxCurve(0f - num4, num4);
		base.transform.SetLocalX(xRange.Evaluate(storminess));
		backSnowVel.xMultiplier = backSnowXSpeedRange.Evaluate(storminess);
		midSnowVel.xMultiplier = midSnowXSpeedRange.Evaluate(storminess);
		frontSnowVel.xMultiplier = frontSnowXSpeedRange.Evaluate(storminess);
		bloopsVel.xMultiplier = bloopsXSpeedRange.Evaluate(storminess);
	}
}
