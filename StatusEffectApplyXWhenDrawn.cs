#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Drawn", fileName = "Apply X When Drawn")]
public class StatusEffectApplyXWhenDrawn : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnEnable += CheckEnable;
		base.OnCardMove += CheckCardMove;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (entity == target)
		{
			return target.InHand();
		}

		return false;
	}

	public IEnumerator CheckEnable(Entity entity)
	{
		return Run(GetTargets());
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (target.enabled && entity == target)
		{
			return target.InHand();
		}

		return false;
	}

	public IEnumerator CheckCardMove(Entity entity)
	{
		return Run(GetTargets());
	}
}
