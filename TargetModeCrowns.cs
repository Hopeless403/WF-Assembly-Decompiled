#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetModeCrowns", menuName = "Target Modes/Crowns")]
public class TargetModeCrowns : TargetMode
{
	public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		hashSet.AddRange(from e in entity.GetAllEnemies()
			where (bool)e && e.enabled && e.alive && e.canBeHit && e.data.HasCrown
			select e);
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
}
