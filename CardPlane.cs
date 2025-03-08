#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class CardPlane : CardContainer
{
	public readonly Dictionary<Entity, Vector3> positions = new Dictionary<Entity, Vector3>();

	public override float CardScale => 1f;

	public override void Add(Entity entity)
	{
		base.Add(entity);
		StorePosition(entity);
	}

	public override void Insert(int index, Entity entity)
	{
		base.Insert(index, entity);
		StorePosition(entity);
	}

	public override void Remove(Entity entity)
	{
		base.Remove(entity);
		FreePosition(entity);
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		if (!positions.ContainsKey(child))
		{
			return Vector3.zero;
		}

		return positions[child];
	}

	public void StorePosition(Entity entity)
	{
		positions[entity] = entity.transform.localPosition;
	}

	public void FreePosition(Entity entity)
	{
		positions.Remove(entity);
	}
}
