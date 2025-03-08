#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetModeRow", menuName = "Target Modes/Row")]
public class TargetModeRow : TargetMode
{
	public override bool TargetRow => true;

	public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		if ((bool)targetContainer)
		{
			AddEligible(entity, hashSet, targetContainer);
		}
		else if ((bool)target)
		{
			switch (target.containers.Length)
			{
				case 1:
				{
					CardContainer fromCollection = target.containers[0];
					AddEligible(entity, hashSet, fromCollection);
					break;
				}
				case 2:
				{
					int[] rowIndices = References.Battle.GetRowIndices(entity);
					int[] rowIndices2 = References.Battle.GetRowIndices(target);
				foreach (int item in rowIndices.Intersect(rowIndices2))
				{
					List<Entity> enemiesInRow = entity.GetEnemiesInRow(item);
					AddEligible(entity, hashSet, enemiesInRow);
					}
	
					break;
				}
			}
		}
		else
		{
			int[] rowIndices3 = Battle.instance.GetRowIndices(entity);
			int[] array = rowIndices3;
			foreach (int rowIndex in array)
			{
				List<Entity> enemiesInRow2 = entity.GetEnemiesInRow(rowIndex);
				AddEligible(entity, hashSet, enemiesInRow2);
			}

			if (hashSet.Count == 0)
			{
				int rowCount = Battle.instance.rowCount;
				for (int j = 0; j < rowCount; j++)
				{
					if (!rowIndices3.Contains(j))
					{
						List<Entity> enemiesInRow3 = entity.GetEnemiesInRow(j);
						AddEligible(entity, hashSet, enemiesInRow3);
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

	public override Entity[] GetSubsequentTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		return GetTargets(entity, target, targetContainer);
	}

	public static void AddEligible(Entity entity, ISet<Entity> targets, IEnumerable<Entity> fromCollection)
	{
		foreach (Entity item in fromCollection)
		{
			if (entity.CanPlayOn(item, ignoreRowCheck: true))
			{
				targets.Add(item);
			}
		}
	}
}
