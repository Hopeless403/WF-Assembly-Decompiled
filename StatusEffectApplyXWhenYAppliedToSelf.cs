#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Y Applied To Self", fileName = "Apply X When Y Applied To Self")]
public class StatusEffectApplyXWhenYAppliedToSelf : StatusEffectApplyX
{
	public string whenAppliedType = "spice";

	public string[] whenAppliedTypes = new string[1] { "spice" };

	public override void Init()
	{
		base.PostApplyStatus += Check;
	}

	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if (target.enabled && apply.target == target && (bool)apply.effectData && apply.count > 0)
		{
			return whenAppliedTypes.Contains(apply.effectData.type);
		}

		return false;
	}

	public IEnumerator Check(StatusEffectApply apply)
	{
		return Run(GetTargets());
	}
}
