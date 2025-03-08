#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class CounterImminentDisplaySystem : GameSystem
{
	[SerializeField]
	public bool disableCardAnimationHover = true;

	[SerializeField]
	public bool disableIconAnimationHover = true;

	public List<Entity> currentImminent = new List<Entity>();

	public List<Entity> currentHover = new List<Entity>();

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
		Events.OnEntityDisplayUpdated += EntityCheck;
		Events.OnEntityEnabled += EntityCheck;
		Events.OnEntityDisabled += EntityDisabled;
		Events.OnInspect += EntityHover;
		Events.OnInspectEnd += EntityUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
		Events.OnEntityDisplayUpdated -= EntityCheck;
		Events.OnEntityEnabled -= EntityCheck;
		Events.OnEntityDisabled -= EntityDisabled;
		Events.OnInspect -= EntityHover;
		Events.OnInspectEnd -= EntityUnHover;
	}

	public void EntityHover(Entity entity)
	{
		if (!(entity != null))
		{
			return;
		}

		currentHover.Add(entity);
		if (currentImminent.Contains(entity))
		{
			if (disableCardAnimationHover)
			{
				SetCardAnimation(entity, enable: false);
			}

			if (disableIconAnimationHover)
			{
				SetCounterIconAnimation(entity, enable: false);
			}
		}
	}

	public void EntityUnHover(Entity entity)
	{
		if (!(entity != null))
		{
			return;
		}

		currentHover.Remove(entity);
		if (entity.enabled && currentImminent.Contains(entity))
		{
			if (disableCardAnimationHover)
			{
				SetCardAnimation(entity, enable: true);
			}

			if (disableIconAnimationHover)
			{
				SetCounterIconAnimation(entity, enable: true);
			}
		}
	}

	public void EntityDisabled(Entity entity)
	{
		if (currentImminent.Contains(entity))
		{
			currentImminent.Remove(entity);
			SetCardAnimation(entity, enable: false);
			SetCounterIconAnimation(entity, enable: false);
		}
	}

	public void SetCardAnimation(Entity entity, bool enable)
	{
		if (entity.imminentAnimation != null)
		{
			if (enable)
			{
				entity.imminentAnimation.FadeIn();
			}
			else
			{
				entity.imminentAnimation.FadeOut();
			}
		}
	}

	public void SetCounterIconAnimation(Entity entity, bool enable)
	{
		if (!(entity.display.counterIcon != null) || !(entity.display.counterIcon is StatusIconCounter statusIconCounter))
		{
			return;
		}

		CardIdleAnimation imminentAnimation = statusIconCounter.imminentAnimation;
		if (imminentAnimation != null)
		{
			if (enable)
			{
				imminentAnimation.FadeIn();
			}
			else
			{
				imminentAnimation.FadeOut();
			}
		}
	}

	public void EntityCheck(Entity entity)
	{
		if (!entity.enabled)
		{
			return;
		}

		if (!currentImminent.Contains(entity))
		{
			if (Imminent(entity))
			{
				currentImminent.Add(entity);
				SetCardAnimation(entity, enable: true);
				SetCounterIconAnimation(entity, enable: true);
			}
		}
		else if (!Imminent(entity))
		{
			currentImminent.Remove(entity);
			SetCardAnimation(entity, enable: false);
			SetCounterIconAnimation(entity, enable: false);
		}
	}

	public bool Imminent(Entity entity)
	{
		if (!entity.IsSnowed)
		{
			return entity.counter.current == 1;
		}

		return false;
	}
}
