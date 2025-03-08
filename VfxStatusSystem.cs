#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class VfxStatusSystem : GameSystem
{
	[Serializable]
	public class Profile
	{
		public string type;

		public GameObject applyEffectPrefab;
	}

	[Serializable]
	public class DamageProfile
	{
		public string damageType;

		public GameObject damageEffectPrefab;
	}

	[SerializeField]
	public Profile[] profiles;

	[SerializeField]
	public DamageProfile[] damageProfiles;

	public Dictionary<string, Profile> profileLookup;

	public Dictionary<string, DamageProfile> damageProfileLookup;

	public void OnEnable()
	{
		Events.OnStatusEffectApplied += StatusApplied;
		Events.OnEntityHit += EntityHit;
		profileLookup = new Dictionary<string, Profile>();
		Profile[] array = profiles;
		foreach (Profile profile in array)
		{
			profileLookup[profile.type] = profile;
		}

		damageProfileLookup = new Dictionary<string, DamageProfile>();
		DamageProfile[] array2 = damageProfiles;
		foreach (DamageProfile damageProfile in array2)
		{
			damageProfileLookup[damageProfile.damageType] = damageProfile;
		}
	}

	public void OnDisable()
	{
		Events.OnStatusEffectApplied -= StatusApplied;
		Events.OnEntityHit -= EntityHit;
	}

	public void StatusApplied(StatusEffectApply apply)
	{
		if ((bool)apply?.effectData && apply.target.display.init && apply.target.startingEffectsApplied && !Transition.Running && profileLookup.ContainsKey(apply.effectData.type))
		{
			Profile profile = profileLookup[apply.effectData.type];
			if (profile != null && (bool)profile.applyEffectPrefab)
			{
				Transform transform = apply.target.transform;
				CreateEffect(profile.applyEffectPrefab, transform.position, transform.lossyScale);
			}
		}
	}

	public void EntityHit(Hit hit)
	{
		if (!hit.BasicHit && damageProfileLookup.ContainsKey(hit.damageType))
		{
			DamageProfile damageProfile = damageProfileLookup[hit.damageType];
			if (damageProfile != null && damageProfile.damageEffectPrefab != null)
			{
				CreateEffect(damageProfile.damageEffectPrefab, hit.target.transform.position, hit.target.transform.lossyScale);
			}
		}
	}

	public void CreateEffect(GameObject prefab, Vector3 position, Vector3 scale)
	{
		UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, base.transform).transform.localScale = scale;
	}
}
