#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Destroyed", fileName = "Apply X When Destroyed")]
public class StatusEffectApplyXWhenDestroyed : StatusEffectApplyX
{
	[SerializeField]
	public bool sacrificed;

	[SerializeField]
	public bool consumed;

	public override void Init()
	{
		base.OnEntityDestroyed += CheckDestroy;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (entity == target)
		{
			return CheckDeathType(deathType);
		}

		return false;
	}

	public IEnumerator CheckDestroy(Entity entity, DeathType deathType)
	{
		if (entity.LastHitStillProcessing())
		{
			yield return entity.WaitForLastHitToFinishProcessing();
		}

		yield return Run(GetTargets(null, GetTargetContainers(), GetTargetActualContainers()));
	}

	public bool CheckDeathType(DeathType deathType)
	{
		if (consumed && deathType != DeathType.Consume)
		{
			return false;
		}

		if (sacrificed && !DeathSystem.KilledByOwnTeam(target))
		{
			return false;
		}

		return true;
	}
}
