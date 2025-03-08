#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Dead;
using UnityEngine;

public class SplatterSystem : GameSystem
{
	public static SplatterSystem instance;

	[SerializeField]
	[Range(0f, 2f)]
	public float spread = 1f;

	[SerializeField]
	public Vector2 speedRange = new Vector2(1f, 10f);

	[SerializeField]
	public Vector2 upSpeedRange = new Vector2(-1f, 0f);

	[SerializeField]
	[Range(0f, 2f)]
	public float bloodAmount = 1f;

	[SerializeField]
	[Range(1f, 10f)]
	public int maxPerHit = 6;

	[SerializeField]
	public Gradient rainbow;

	[SerializeField]
	public int maxTweens = 200;

	public static int BloodType;

	public static float BloodAmount;

	public readonly List<float> tweens = new List<float>();

	public void OnEnable()
	{
		instance = this;
		Events.OnEntityHit += EntityHit;
		Events.OnSettingChanged += SettingChanged;
		BloodType = Settings.Load("Blood", 0);
		BloodAmount = Settings.Load("BloodAmount", 1f);
	}

	public void OnDisable()
	{
		Events.OnEntityHit -= EntityHit;
		Events.OnSettingChanged -= SettingChanged;
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		for (int num = tweens.Count - 1; num >= 0; num--)
		{
			tweens[num] -= deltaTime;
			if (tweens[num] < 0f)
			{
				tweens.RemoveAt(num);
				num--;
			}
		}
	}

	public static void SettingChanged(string key, object value)
	{
		if (!(key == "BloodAmount"))
		{
			if (key == "Blood" && value is int)
			{
				int bloodType = (int)value;
				BloodType = bloodType;
			}
		}
		else if (value is float)
		{
			float num = (float)value;
			BloodAmount = num;
		}
	}

	public void EntityHit(Hit hit)
	{
		if (!hit.Offensive || hit.nullified || !hit.BasicHit || !hit.countsAsHit || !hit.target)
		{
			return;
		}

		BloodProfile bloodProfile = hit.target.data.bloodProfile;
		if ((bool)bloodProfile)
		{
			int num = Mathf.RoundToInt((float)Mathf.Min(hit.damage, maxPerHit) * bloodAmount * bloodProfile.bleedFactor * BloodAmount);
			Vector3 vector = ((!hit.attacker || !hit.target) ? new Vector3(PettyRandom.Range(-1f, 1f), PettyRandom.Range(-1f, 1f), 0f).normalized : (hit.target.transform.position - hit.attacker.transform.position).normalized);
			for (int i = 0; i < num; i++)
			{
				SplatterParticle splatterParticle = Object.Instantiate(bloodProfile.splatterParticlePrefab);
				Vector3 vector2 = new Vector3(PettyRandom.Range(-1f, 1f), PettyRandom.Range(-1.5f, 1.5f), 0f);
				splatterParticle.transform.position = hit.target.transform.position + vector2;
				splatterParticle.color = GetBloodColour(hit.target);
				Vector3 v = vector * speedRange.PettyRandom() + PettyRandom.Vector3() * spread;
				splatterParticle.velocity = v.WithZ(upSpeedRange.PettyRandom());
				splatterParticle.SetSource(hit.target.GetComponentInChildren<SplatterSurface>());
			}
		}
	}

	public Color GetBloodColour(Entity entity)
	{
		if (BloodType == 1)
		{
			return rainbow.Evaluate(entity.data.random3.x.Mod(1f));
		}

		return (!entity.data.bloodProfile) ? Color.white : (entity.data.bloodProfile.variableColor ? entity.data.bloodProfile.colorRange.Evaluate(entity.data.random3.x.Mod(1f)) : entity.data.bloodProfile.color);
	}

	public static bool CheckStartTween(float time)
	{
		if (instance.tweens.Count < instance.maxTweens)
		{
			instance.tweens.Add(time);
			return true;
		}

		return false;
	}
}
