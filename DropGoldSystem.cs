#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DropGoldSystem : GameSystem
{
	[SerializeField]
	public float goldFactor = 0.02f;

	[SerializeField]
	public int goldPerUpgrade = 5;

	[SerializeField]
	public bool dropGoldOnFlee = true;

	public void OnEnable()
	{
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntityFlee += EntityFlee;
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntityFlee -= EntityFlee;
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if (deathType == DeathType.Normal)
		{
			TryDropGold(entity, "Kill");
		}
	}

	public void EntityFlee(Entity entity)
	{
		if (dropGoldOnFlee)
		{
			TryDropGold(entity, "Flee");
		}
	}

	public void TryDropGold(Entity entity, string source)
	{
		if (!entity.owner || entity.owner.team != References.Player.team)
		{
			int goldToDrop = GetGoldToDrop(entity);
			if (goldToDrop > 0)
			{
				Events.InvokeDropGold(goldToDrop, source, References.Player, entity.transform.position);
			}
		}
	}

	public int GetGoldToDrop(Entity entity)
	{
		int num = Mathf.RoundToInt((float)entity.data.value * goldFactor * References.PlayerData.enemyGoldFactor);
		int num2 = ((num > 0) ? Mathf.Max(0, num + Mathf.RoundToInt(entity.data.random3.z)) : 0);
		if (entity.data.upgrades != null)
		{
			num2 += entity.data.upgrades.Count * goldPerUpgrade;
		}

		return num2;
	}
}
