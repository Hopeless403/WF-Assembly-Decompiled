#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Activate Sacrifice Effects", fileName = "Activate Sacrifice Effects")]
public class StatusEffectInstantActivateSacrificeEffects : StatusEffectInstant
{
	[SerializeField]
	public bool forOtherTeam;

	public override IEnumerator Process()
	{
		if (!DeathSystem.KilledByOwnTeam(target))
		{
			if (forOtherTeam)
			{
				DeathSystem.TreatAsTeam(target.data.id, Battle.GetOpponent(target.owner).team);
				target.lastHit.owner = Battle.GetOpponent(target.lastHit.owner);
			}
			else
			{
				MakeLastHitSacrifice();
			}

			TriggerSacrificeVFX();
		}

		yield return base.Process();
	}

	public void MakeLastHitSacrifice()
	{
		target.lastHit = new Hit(target, target, 1)
		{
			attacker = null
		};
	}

	public void TriggerSacrificeVFX()
	{
		Object.FindObjectOfType<VfxDeathSystem>()?.EntityKilled(target, DeathType.Normal);
	}
}
