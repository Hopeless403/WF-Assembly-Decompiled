#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Y Applied To Ally", fileName = "Apply X When Y Applied To Ally")]
public class StatusEffectApplyXWhenYAppliedToAlly : StatusEffectApplyX
{
	public string whenAppliedType = "snow";

	public override void Init()
	{
		base.PostApplyStatus += Check;
	}

	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if (target.enabled && apply.target.owner == target.owner && apply.effectData != null && apply.count > 0)
		{
			return apply.effectData.type == whenAppliedType;
		}

		return false;
	}

	public IEnumerator Check(StatusEffectApply apply)
	{
		return Run(GetTargets());
	}
}
