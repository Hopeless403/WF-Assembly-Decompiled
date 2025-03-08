#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Build", fileName = "Build")]
public class StatusEffectBuild : StatusEffectData
{
	[SerializeField]
	public int requires = 3;

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (target == entity && Battle.IsOnBoard(entity.containers) && !Battle.IsOnBoard(entity.preContainers))
		{
			List<Entity> list = (from ally in entity.GetAllies()
				where ally.statusEffects.Exists((StatusEffectData a) => a.name == base.name)
				select ally).ToList();
			list.Add(entity);
			if (list.Count >= requires)
			{
				ActionQueue.Stack(new ActionCombine(list.ToArray()), fixedPosition: true);
			}
		}

		return false;
	}
}
