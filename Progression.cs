#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[Serializable]
public class Progression
{
	public float current;

	public float previousRequired;

	public float required = 1f;

	[SerializeField]
	public float randomness = 0.1f;

	[SerializeField]
	public float minProgressAdd = 2f;

	[SerializeField]
	public float maxProgressAdd = 4f;

	public Progression()
	{
	}

	public Progression(float startAmount, float startRequirement, float randomness)
	{
		current = startAmount;
		previousRequired = 0f;
		required = startRequirement;
		this.randomness = randomness;
	}

	public bool Add(float amount)
	{
		if (RequirementMet())
		{
			return false;
		}

		current += amount + UnityEngine.Random.Range(0f - randomness, randomness);
		return true;
	}

	public bool RequirementMet()
	{
		return current >= required;
	}

	public void SetNextRequirement()
	{
		previousRequired = required;
		required += Mathf.Clamp(required, minProgressAdd, maxProgressAdd);
	}

	public float ProgressToNextUnlock()
	{
		return (current - previousRequired) / (required - previousRequired);
	}
}
