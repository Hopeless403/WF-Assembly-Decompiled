#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Discarded", fileName = "Apply X When Discarded")]
public class StatusEffectApplyXWhenDiscarded : StatusEffectApplyX
{
	public override void Init()
	{
		Events.OnActionQueued += ActionQueued;
	}

	public void ActionQueued(PlayAction action)
	{
		if (action is ActionMove actionMove && actionMove.entity == target && (bool)target.owner && actionMove.toContainers.Contains(target.owner.discardContainer))
		{
			ActionQueue.Insert(ActionQueue.IndexOf(action), new ActionSequence(Sequence()));
		}
	}

	public IEnumerator Sequence()
	{
		return Run(GetTargets());
	}
}
