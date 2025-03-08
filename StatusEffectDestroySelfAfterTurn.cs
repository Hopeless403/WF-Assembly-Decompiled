#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Destroy Self After Turn", fileName = "Destroy Self After Turn")]
public class StatusEffectDestroySelfAfterTurn : StatusEffectData
{
	public bool cardPlayed;

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!cardPlayed && entity == target && !target.silenced)
		{
			ActionQueue.Add(new ActionKill(entity));
			cardPlayed = true;
		}

		return false;
	}
}
