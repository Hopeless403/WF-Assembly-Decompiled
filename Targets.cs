#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;

public static class Targets
{
	[Flags]
	public enum Flag
	{
		None = 0,
		Self = 1,
		Hand = 2,
		EnemyHand = 4,
		Allies = 8,
		AlliesInRow = 0x10,
		FrontAlly = 0x20,
		BackAlly = 0x40,
		AllyInFrontOf = 0x80,
		AllyBehind = 0x100,
		Enemies = 0x200,
		EnemiesInRow = 0x400,
		FrontEnemy = 0x800,
		Attacker = 0x1000,
		Target = 0x2000,
		RandomAlly = 0x4000,
		RandomEnemy = 0x8000,
		RandomUnit = 0x10000
	}

	public static bool CheckFlag(Flag flags, Flag target)
	{
		return (flags & target) != 0;
	}

	public static List<Entity> Get(Entity self, Flag canTarget, StatusEffectData effectToApply, TargetConstraint[] applyConstraints, Hit hit = null)
	{
		List<Entity> list = new List<Entity>();
		if (CheckFlag(canTarget, Flag.Self))
		{
			list.Add(self);
		}

		CheckFlag(canTarget, Flag.Hand);
		CheckFlag(canTarget, Flag.EnemyHand);
		if (CheckFlag(canTarget, Flag.Allies))
		{
			list.AddRange(self.GetAllAllies());
		}
		else if (CheckFlag(canTarget, Flag.AlliesInRow))
		{
			list.AddRange(self.GetAlliesInRow());
		}
		else
		{
			if (CheckFlag(canTarget, Flag.FrontAlly))
			{
				CardContainer[] containers = self.containers;
				for (int i = 0; i < containers.Length; i++)
				{
					foreach (Entity item3 in containers[i])
					{
						if ((bool)item3)
						{
							list.Add(item3);
							break;
						}
					}
				}
			}

			if (CheckFlag(canTarget, Flag.BackAlly))
			{
				CardContainer[] containers = self.containers;
				foreach (CardContainer cardContainer in containers)
				{
					for (int num = cardContainer.Count - 1; num >= 0; num--)
					{
						Entity entity = cardContainer[num];
						if ((bool)entity)
						{
							list.Add(entity);
							break;
						}
					}
				}
			}

			if (CheckFlag(canTarget, Flag.AllyInFrontOf))
			{
				foreach (CardContainer actualContainer in self.actualContainers)
				{
					Entity entity2 = null;
					if (actualContainer is CardSlot item && actualContainer.Group is CardSlotLane cardSlotLane)
					{
						for (int num2 = cardSlotLane.slots.IndexOf(item) - 1; num2 >= 0; num2--)
						{
							entity2 = cardSlotLane.slots[num2].GetTop();
							if ((bool)entity2)
							{
								break;
							}
						}
					}

					if ((bool)entity2)
					{
						list.Add(entity2);
						break;
					}
				}
			}

			if (CheckFlag(canTarget, Flag.AllyBehind))
			{
				foreach (CardContainer actualContainer2 in self.actualContainers)
				{
					Entity entity3 = null;
					if (actualContainer2 is CardSlot item2 && actualContainer2.Group is CardSlotLane cardSlotLane2)
					{
						for (int j = cardSlotLane2.slots.IndexOf(item2) + 1; j < cardSlotLane2.slots.Count; j++)
						{
							entity3 = cardSlotLane2.slots[j].GetTop();
							if ((bool)entity3)
							{
								break;
							}
						}
					}

					if ((bool)entity3)
					{
						list.Add(entity3);
						break;
					}
				}
			}
		}

		if (CheckFlag(canTarget, Flag.Enemies))
		{
			list.AddRange(self.GetAllEnemies());
		}
		else if (CheckFlag(canTarget, Flag.EnemiesInRow))
		{
			int[] rowIndices = Battle.instance.GetRowIndices(self);
			foreach (int rowIndex in rowIndices)
			{
				List<Entity> enemiesInRow = self.GetEnemiesInRow(rowIndex);
				if (enemiesInRow != null && enemiesInRow.Count > 0)
				{
					list.AddRange(enemiesInRow);
				}
			}
		}

		else if (CheckFlag(canTarget, Flag.FrontEnemy))
		{
			int[] rowIndices = Battle.instance.GetRowIndices(self);
			foreach (int rowIndex2 in rowIndices)
			{
				List<Entity> enemiesInRow2 = self.GetEnemiesInRow(rowIndex2);
				if (enemiesInRow2 != null && enemiesInRow2.Count > 0)
				{
					list.Add(enemiesInRow2[0]);
				}
			}
		}

		if (CheckFlag(canTarget, Flag.Attacker))
		{
			if (hit == null)
			{
				hit = self.lastHit;
			}

			if ((bool)hit?.attacker)
			{
				Entity entity4 = hit.attacker;
				if (!effectToApply.CanPlayOn(entity4))
				{
					entity4 = (effectToApply.CanPlayOn(entity4.owner.entity) ? entity4.owner.entity : null);
				}

				if ((bool)entity4)
				{
					list.Add(entity4);
				}
			}
		}

		if (CheckFlag(canTarget, Flag.Target))
		{
			if (hit == null)
			{
				hit = self.lastHit;
			}

			if ((bool)hit?.target)
			{
				list.Add(hit.target);
			}
		}

		if (CheckFlag(canTarget, Flag.RandomUnit))
		{
			List<Entity> cardsOnBoard = Battle.GetCardsOnBoard(self.owner);
			cardsOnBoard.AddRange(Battle.GetCardsOnBoard(Battle.GetOpponent(self.owner)));
			cardsOnBoard.Remove(self);
			RemoveIneligible(cardsOnBoard, effectToApply, applyConstraints);
			if (cardsOnBoard.Count > 0)
			{
				list.Add(cardsOnBoard.RandomItem());
			}
		}

		if (CheckFlag(canTarget, Flag.RandomAlly))
		{
			List<Entity> cardsOnBoard2 = Battle.GetCardsOnBoard(self.owner);
			cardsOnBoard2.Remove(self);
			RemoveIneligible(cardsOnBoard2, effectToApply, applyConstraints);
			if (cardsOnBoard2.Count > 0)
			{
				list.Add(cardsOnBoard2.RandomItem());
			}
		}

		if (CheckFlag(canTarget, Flag.RandomEnemy))
		{
			List<Entity> cardsOnBoard3 = Battle.GetCardsOnBoard(Battle.GetOpponent(self.owner));
			RemoveIneligible(cardsOnBoard3, effectToApply, applyConstraints);
			if (cardsOnBoard3.Count > 0)
			{
				list.Add(cardsOnBoard3.RandomItem());
			}
		}

		return list;
	}

	public static void RemoveIneligible(IList<Entity> list, StatusEffectData effectToApply, TargetConstraint[] applyConstraints)
	{
		for (int num = list.Count - 1; num >= 0; num--)
		{
			Entity target = list[num];
			if (!effectToApply.CanPlayOn(target))
			{
				list.RemoveAt(num);
			}
			else
			{
				for (int i = 0; i < applyConstraints.Length; i++)
				{
					if (!applyConstraints[i].Check(target))
					{
						list.RemoveAt(num);
						break;
					}
				}
			}
		}
	}
}
