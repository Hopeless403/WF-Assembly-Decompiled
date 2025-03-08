#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Ally Is Healed", fileName = "Apply X When Ally Is Healed")]
public class StatusEffectApplyXWhenAllyHealed : StatusEffectApplyX
{
	public override void Init()
	{
		base.PostHit += Check;
		base.OnApplyStatus += CheckStatus;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (target.enabled && hit.target != target && hit.target.owner == target.owner && hit.damage < 0)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		return Run(GetTargets(hit), -hit.damage);
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (target.enabled && apply.target != target && apply.target.owner == target.owner && apply.effectData.type == "max health up" && Battle.IsOnBoard(target))
		{
			return Battle.IsOnBoard(apply.target);
		}

		return false;
	}

	public IEnumerator CheckStatus(StatusEffectApply apply)
	{
		return Run(GetTargets(null, null, null, new Entity[1] { apply.target }), apply.count);
	}
}
