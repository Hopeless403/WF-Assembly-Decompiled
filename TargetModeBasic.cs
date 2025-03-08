#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetModeBasic", menuName = "Target Modes/Basic")]
public class TargetModeBasic : TargetMode
{
	public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		if ((bool)target)
		{
			hashSet.Add(target);
		}
		else
		{
			int[] rowIndices = Battle.instance.GetRowIndices(entity);
			if (rowIndices.Length != 0)
			{
				int[] array = rowIndices;
				foreach (int rowIndex in array)
				{
					AddTargets(entity, hashSet, rowIndex);
				}

				if (hashSet.Count == 0)
				{
					int rowCount = Battle.instance.rowCount;
					for (int j = 0; j < rowCount; j++)
					{
						if (!rowIndices.Contains(j))
						{
							AddTargets(entity, hashSet, j);
						}
					}
				}
			}
		}

		if (hashSet.Count <= 0)
		{
			return null;
		}

		return hashSet.ToArray();
	}

	public void AddTargets(Entity entity, HashSet<Entity> targets, int rowIndex)
	{
		List<Entity> enemiesInRow = entity.GetEnemiesInRow(rowIndex);
		Entity entity2 = null;
		foreach (Entity item in enemiesInRow)
		{
			if ((bool)item && item.enabled && item.alive && item.canBeHit)
			{
				entity2 = item;
				break;
			}
		}

		if ((bool)entity2)
		{
			targets.Add(entity2);
			return;
		}

		entity2 = GetEnemyCharacter(entity);
		if ((bool)entity2)
		{
			targets.Add(entity2);
		}
	}
}
