#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X On Kill", fileName = "Apply X On Kill")]
public class StatusEffectApplyXOnKill : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnEntityDestroyed += CheckDestroy;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (entity.lastHit != null)
		{
			return entity.lastHit.attacker == target;
		}

		return false;
	}

	public IEnumerator CheckDestroy(Entity entity, DeathType deathType)
	{
		return Run(GetTargets(entity.lastHit));
	}
}
