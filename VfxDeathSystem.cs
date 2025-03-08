#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class VfxDeathSystem : GameSystem
{
	[SerializeField]
	public GameObject sacrificeFX;

	public void OnEnable()
	{
		Events.OnEntityKilled += EntityKilled;
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if (DeathSystem.KilledByOwnTeam(entity) && (bool)sacrificeFX)
		{
			Transform transform = entity.transform;
			CreateEffect(sacrificeFX, transform.position, transform.lossyScale);
		}
	}

	public void CreateEffect(GameObject prefab, Vector3 position, Vector3 scale)
	{
		if ((bool)prefab)
		{
			Object.Instantiate(prefab, position, Quaternion.identity, base.transform).transform.localScale = scale;
		}
	}
}
