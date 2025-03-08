#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetModeRandom", menuName = "Target Modes/Random")]
public class TargetModeRandom : TargetMode
{
	public override bool TargetRow => true;

	public override bool Random => true;

	public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		if ((bool)targetContainer)
		{
			if (targetContainer.Count > 0)
			{
				AddPotentialTargets(entity, hashSet, targetContainer);
			}
		}
		else if ((bool)target)
		{
			switch (target.containers.Length)
			{
				case 1:
				{
					CardContainer collection = target.containers[0];
					AddPotentialTargets(entity, hashSet, collection);
					break;
				}
				case 2:
				{
					int[] rowIndices = References.Battle.GetRowIndices(entity);
					int[] rowIndices2 = References.Battle.GetRowIndices(target);
				foreach (int item in rowIndices.Intersect(rowIndices2))
				{
					AddPotentialTargets(entity, hashSet, item);
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
				AddPotentialTargets(entity, hashSet, rowIndex);
			}

			if (hashSet.Count == 0)
			{
				int rowCount = Battle.instance.rowCount;
				for (int j = 0; j < rowCount; j++)
				{
					if (!rowIndices3.Contains(j))
					{
						AddPotentialTargets(entity, hashSet, j);
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

	public override Entity[] GetTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		Entity[] potentialTargets = GetPotentialTargets(entity, target, targetContainer);
		if (potentialTargets == null)
		{
			return null;
		}

		HashSet<Entity> hashSet = new HashSet<Entity>();
		if (entity.containers.Length == 1)
		{
			hashSet.Add(potentialTargets.RandomItem());
		}
		else if (entity.containers.Length > 1)
		{
			Dictionary<CardContainer, List<Entity>> dictionary = new Dictionary<CardContainer, List<Entity>>();
			CardContainer[] oppositeRows = References.Battle.GetOppositeRows(entity.containers);
			foreach (CardContainer key in oppositeRows)
			{
				dictionary.Add(key, new List<Entity>());
			}

			Entity[] array = potentialTargets;
			foreach (Entity entity2 in array)
			{
				oppositeRows = entity2.containers;
				foreach (CardContainer key2 in oppositeRows)
				{
					if (dictionary.TryGetValue(key2, out var value))
					{
						value.Add(entity2);
					}
				}
			}

			foreach (List<Entity> value2 in dictionary.Values)
			{
				if (value2.Count > 0)
				{
					hashSet.Add(value2.RandomItem());
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

	public override CardSlot[] GetTargetSlots(CardSlotLane row)
	{
		return new CardSlot[1] { row.slots.RandomItem() };
	}

	public static void AddPotentialTargets(Entity entity, HashSet<Entity> targets, IEnumerable<Entity> collection)
	{
		foreach (Entity item in collection)
		{
			if (entity.CanPlayOn(item, ignoreRowCheck: true))
			{
				targets.Add(item);
			}
		}
	}

	public static void AddPotentialTargets(Entity entity, HashSet<Entity> targets, int rowIndex)
	{
		List<Entity> enemiesInRow = entity.GetEnemiesInRow(rowIndex);
		AddPotentialTargets(entity, targets, enemiesInRow);
	}
}
