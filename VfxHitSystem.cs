#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using FMODUnity;
using UnityEngine;

public class VfxHitSystem : GameSystem
{
	[Serializable]
	public class Profile
	{
		public GameObject effectPrefab;
	}

	[Serializable]
	public class WithStatusProfile
	{
		public string statusType;

		public GameObject effectPrefab;

		public EventReference sfxEvent;
	}

	[SerializeField]
	public AnimationCurve profileCurve;

	[SerializeField]
	public Profile[] profiles;

	[SerializeField]
	public WithStatusProfile[] withStatusProfiles;

	public Profile GetProfile(int power)
	{
		return profiles[Mathf.Clamp(Mathf.RoundToInt(profileCurve.Evaluate(power)), 0, profiles.Length - 1)];
	}

	public static int GetHitPower(Hit hit)
	{
		return hit.damage + hit.damageBlocked + hit.extraOffensiveness;
	}

	public void OnEnable()
	{
		Events.OnEntityHit += EntityHit;
	}

	public void OnDisable()
	{
		Events.OnEntityHit -= EntityHit;
	}

	public void EntityHit(Hit hit)
	{
		if (hit.Offensive && hit.doAnimation && hit.countsAsHit && hit.BasicHit && GetHitPower(hit) > 0 && (bool)hit.target)
		{
			TakeHit(hit);
		}
	}

	public void TakeHit(Hit hit)
	{
		Vector3 position = hit.target.transform.position;
		Vector3 vector = position;
		if ((bool)hit.attacker)
		{
			Vector3 normalized = (vector - hit.attacker.transform.position).normalized;
			vector += normalized * -1f;
			vector.z = hit.target.transform.position.z;
		}

		Profile profile = GetProfile(GetHitPower(hit));
		if ((bool)profile.effectPrefab)
		{
			CreateEffect(profile.effectPrefab, vector, hit.target.transform.lossyScale);
		}

		WithStatusProfile[] array = withStatusProfiles;
		foreach (WithStatusProfile withStatusProfile in array)
		{
			if ((bool)hit.target.FindStatus(withStatusProfile.statusType))
			{
				CreateEffect(withStatusProfile.effectPrefab, position, hit.target.transform.lossyScale);
				if (!withStatusProfile.sfxEvent.IsNull)
				{
					SfxSystem.OneShot(withStatusProfile.sfxEvent);
				}
			}
		}
	}

	public void CreateEffect(GameObject prefab, Vector3 position, Vector3 scale)
	{
		if ((bool)prefab)
		{
			UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, base.transform).transform.localScale = scale;
		}
	}
}
