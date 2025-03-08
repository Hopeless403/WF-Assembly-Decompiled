#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Ally Is Hit", fileName = "Apply X When Ally Is Hit")]
public class StatusEffectApplyXWhenAllyIsHit : StatusEffectApplyX
{
	[SerializeField]
	public bool includeSelf;

	public override void Init()
	{
		base.PostHit += Check;
	}

	public override bool RunPostHitEvent(Hit hit)
	{
		if (target.enabled && (includeSelf || hit.target != target) && hit.canRetaliate && hit.target.owner == target.owner && hit.Offensive && hit.BasicHit && Battle.IsOnBoard(target))
		{
			return Battle.IsOnBoard(hit.target);
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		return Run(GetTargets(hit));
	}
}
