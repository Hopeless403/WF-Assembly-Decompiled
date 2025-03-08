#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Double All X When Destroyed", fileName = "Double All X When Destroyed")]
public class StatusEffectDoubleAllXWhenDestroyed : StatusEffectData
{
	[SerializeField]
	public StatusEffectData effectToDouble;

	public override void Init()
	{
		base.OnEntityDestroyed += DestroyCheck;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		return entity == target;
	}

	public IEnumerator DestroyCheck(Entity entity, DeathType deathType)
	{
		List<Entity> allCards = Battle.GetAllCards();
		foreach (Entity item in allCards)
		{
			if (item.enabled)
			{
				StatusEffectData statusEffectData = item.FindStatus(effectToDouble.type);
				if (statusEffectData != null)
				{
					Hit hit = new Hit(target, item, 0);
					hit.AddStatusEffect(effectToDouble, statusEffectData.count);
					yield return hit.Process();
				}
			}
		}
	}
}
