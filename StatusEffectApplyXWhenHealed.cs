#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Healed", fileName = "Apply X When Healed")]
public class StatusEffectApplyXWhenHealed : StatusEffectApplyX
{
	[SerializeField]
	public bool alsoWhenMaxHealthIncreased = true;

	public override void Init()
	{
		base.OnHit += Check;
		if (alsoWhenMaxHealthIncreased)
		{
			base.OnApplyStatus += CheckStatus;
		}
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target)
		{
			return hit.damage < 0;
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		return Run(GetTargets(hit), -hit.damage);
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (alsoWhenMaxHealthIncreased && apply.target == target)
		{
			return apply.effectData.type == "max health up";
		}

		return false;
	}

	public IEnumerator CheckStatus(StatusEffectApply apply)
	{
		return Run(GetTargets(null, null, null, new Entity[1] { apply.target }), apply.count);
	}
}
