#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Draw When Deployed", fileName = "Draw When Deployed")]
public class StatusEffectDeployDraw : StatusEffectData
{
	public override bool RunCardMoveEvent(Entity entity)
	{
		CardContainer[] preContainers = entity.preContainers;
		if (entity == target && (preContainers.Contains(target.owner.handContainer) || preContainers.Contains(target.owner.reserveContainer)) && Battle.IsOnBoard(entity))
		{
			ActionQueue.Stack(new ActionDraw(target.owner, GetAmount()));
		}

		return false;
	}
}
