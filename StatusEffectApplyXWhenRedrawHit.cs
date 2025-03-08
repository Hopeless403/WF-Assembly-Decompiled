#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Redraw Hit", fileName = "Apply X When Redraw Hit")]
public class StatusEffectApplyXWhenRedrawHit : StatusEffectApplyX
{
	public override void Init()
	{
		Events.OnRedrawBellHit += RedrawBellHit;
	}

	public void OnDestroy()
	{
		Events.OnRedrawBellHit -= RedrawBellHit;
	}

	public void RedrawBellHit(RedrawBellSystem redrawBellSystem)
	{
		if (Battle.IsOnBoard(target) && CanTrigger())
		{
			ActionQueue.Stack(new ActionSequence(Run(GetTargets())), fixedPosition: true);
		}
	}
}
