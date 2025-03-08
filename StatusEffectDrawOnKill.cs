#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Draw On Kill", fileName = "Draw On Kill")]
public class StatusEffectDrawOnKill : StatusEffectData
{
	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (entity.lastHit != null && entity.lastHit.attacker == target)
		{
			ActionQueue.Stack(new ActionDraw(target.owner, GetAmount()));
		}

		return false;
	}
}
