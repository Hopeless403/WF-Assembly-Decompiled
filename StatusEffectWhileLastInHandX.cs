#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/While Last In Hand X", fileName = "While Last In Hand X")]
public class StatusEffectWhileLastInHandX : StatusEffectWhileActiveX
{
	public bool isInHand;

	public override void Init()
	{
		base.Init();
		base.OnEntityDestroyed += EntityDestroyed;
	}

	public override bool CanActivate()
	{
		if (isInHand && target.owner.handContainer.FirstOrDefault((Entity a) => a.alive) == target)
		{
			return true;
		}

		return false;
	}

	public override bool RunBeginEvent()
	{
		isInHand = target.InHand();
		return base.RunBeginEvent();
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (target == entity)
		{
			isInHand = target.InHand();
		}

		return base.RunEnableEvent(entity);
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		return target.enabled;
	}

	public override IEnumerator CardMove(Entity entity)
	{
		if (target == entity)
		{
			isInHand = target.InHand();
		}

		if (active)
		{
			if ((target == entity || entity.containers.Contains(target.owner.handContainer)) && !CanActivate())
			{
				yield return Deactivate();
			}
		}
		else if (CanActivate())
		{
			yield return Activate();
		}
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (!active && entity != target)
		{
			return EntityInHand(entity);
		}

		return false;
	}

	public IEnumerator EntityDestroyed(Entity entity, DeathType deathType)
	{
		if (CanActivate())
		{
			yield return Activate();
		}
	}

	public bool EntityInHand(Entity entity)
	{
		if (entity.containers.Length == 0)
		{
			return entity.preContainers.Contains(target.owner.handContainer);
		}

		return entity.containers.Contains(target.owner.handContainer);
	}
}
