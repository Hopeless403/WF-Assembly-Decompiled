#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X On Card Played", fileName = "Apply X On Card Played")]
public class StatusEffectApplyXOnCardPlayed : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnCardPlayed += Check;
	}

	public static CardContainer[] GetWasInRows(Entity entity, IEnumerable<Entity> targets)
	{
		if (entity.data.playType == Card.PlayType.Play && entity.NeedsTarget)
		{
			HashSet<CardContainer> list = new HashSet<CardContainer>();
			foreach (Entity target in targets)
			{
				if (target.containers != null && target.containers.Length != 0)
				{
					list.AddRange(target.containers);
				}
				else
				{
					list.AddRange(target.preContainers);
				}
			}

			return list.ToArray();
		}

		return entity.containers;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (target.enabled)
		{
			return entity == target;
		}

		return false;
	}

	public IEnumerator Check(Entity entity, Entity[] targets)
	{
		return Run(GetTargets(null, GetWasInRows(entity, targets), null, targets));
	}
}
