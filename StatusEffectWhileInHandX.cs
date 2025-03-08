#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/While In Hand X", fileName = "While In Hand X")]
public class StatusEffectWhileInHandX : StatusEffectWhileActiveX
{
	public override bool CanActivate()
	{
		return target.InHand();
	}

	public override bool CheckActivateOnMove(CardContainer[] fromContainers, CardContainer[] toContainers)
	{
		if (toContainers.Contains(target.owner.handContainer))
		{
			return !fromContainers.Contains(target.owner.handContainer);
		}

		return false;
	}

	public override bool CheckDeactivateOnMove(CardContainer[] fromContainers, CardContainer[] toContainers)
	{
		if (!toContainers.Contains(target.owner.handContainer))
		{
			return fromContainers.Contains(target.owner.handContainer);
		}

		return false;
	}
}
