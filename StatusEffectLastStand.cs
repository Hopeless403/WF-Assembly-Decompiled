#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Misc/Last Stand", fileName = "Last Stand")]
public class StatusEffectLastStand : StatusEffectData
{
	public bool activated;

	public override bool RunPostHitEvent(Hit hit)
	{
		if (!activated && hit.target == target && target.hp.current <= 0 && hit.attacker.owner.team != target.owner.team && Battle.IsOnBoard(hit.attacker))
		{
			Activate(hit);
		}

		return false;
	}

	public void Activate(Hit hit)
	{
		LastStandSystem.target = this;
		LastStandSystem.subject = target;
		LastStandSystem.attacker = hit.attacker;
		LastStandSystem.previousPhase = References.Battle.phase;
		Disable();
		References.Battle.phase = Battle.Phase.LastStand;
	}

	public void Disable()
	{
		activated = true;
	}

	public void ReEnable()
	{
		target.hp.current = 1;
		target.PromptUpdate();
		activated = false;
		preventDeath = true;
	}
}
