#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Destroy After Use", fileName = "Destroy After Use")]
public class StatusEffectDestroyAfterUse : StatusEffectData
{
	public bool subbed;

	public bool destroy;

	public void OnDestroy()
	{
		Unsub();
	}

	public void Sub()
	{
		Events.OnActionPerform += CheckAction;
		subbed = true;
	}

	public void Unsub()
	{
		if (subbed)
		{
			Events.OnActionPerform -= CheckAction;
			subbed = false;
		}
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!subbed && entity == target && !target.silenced)
		{
			Sub();
			if (target.uses.current <= 1)
			{
				destroy = true;
			}
		}

		return false;
	}

	public void CheckAction(PlayAction action)
	{
		if (action is ActionReduceUses actionReduceUses && actionReduceUses.entity == target)
		{
			Unsub();
			if (destroy)
			{
				target.alive = false;
				ActionQueue.Stack(new ActionConsume(target));
			}
		}
	}
}
