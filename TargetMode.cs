#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TargetMode : ScriptableObject
{
	public virtual bool TargetRow => false;

	public virtual bool NeedsTarget => true;

	public virtual bool Random => false;

	public virtual Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		throw new NotImplementedException();
	}

	public virtual Entity[] GetTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		return GetPotentialTargets(entity, target, targetContainer);
	}

	public virtual Entity[] GetSubsequentTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		if ((bool)target)
		{
			hashSet.Add(target);
		}
		else if ((bool)targetContainer)
		{
			hashSet.AddRange(targetContainer);
		}

		return hashSet.ToArray();
	}

	public virtual CardSlot[] GetTargetSlots(CardSlotLane row)
	{
		return row.slots.Where((CardSlot a) => a.Empty).ToArray();
	}

	public virtual bool CanTarget(Entity entity)
	{
		return true;
	}

	public virtual Entity GetEnemyCharacter(Entity entity)
	{
		Entity result = null;
		Character character = ((entity.owner == Battle.instance.player) ? Battle.instance.enemy : Battle.instance.player);
		if ((bool)character && (bool)character.entity && character.entity.canBeHit)
		{
			result = character.entity;
		}

		return result;
	}

	public TargetMode()
	{
	}
}
