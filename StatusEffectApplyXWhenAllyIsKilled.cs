#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Ally Is Killed", fileName = "Apply X When Ally Is Killed")]
public class StatusEffectApplyXWhenAllyIsKilled : StatusEffectApplyX
{
	[SerializeField]
	public bool sacrificed;

	public override void Init()
	{
		base.OnEntityDestroyed += Check;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (target.enabled && target.alive && DeathSystem.CheckTeamIsAlly(entity, target) && Battle.IsOnBoard(target) && Battle.IsOnBoard(entity))
		{
			if (sacrificed)
			{
				return DeathSystem.KilledByOwnTeam(entity);
			}

			return true;
		}

		return false;
	}

	public IEnumerator Check(Entity entity, DeathType deathType)
	{
		if (entity.LastHitStillProcessing())
		{
			yield return entity.WaitForLastHitToFinishProcessing();
		}

		if ((bool)contextEqualAmount)
		{
			int amount = contextEqualAmount.Get(entity);
			yield return Run(GetTargets(null, null, null, new Entity[1] { entity }), amount);
		}
		else
		{
			yield return Run(GetTargets(null, null, null, new Entity[1] { entity }));
		}
	}
}
