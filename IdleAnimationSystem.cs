#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class IdleAnimationSystem : GameSystem
{
	public readonly List<Entity> current = new List<Entity>();

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
		current.Clear();
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
	}

	public void EntityHover(Entity entity)
	{
		if ((bool)entity.data.idleAnimationProfile && entity.display is Card card)
		{
			card.imageIdleAnimator.FadeIn();
			card.backgroundIdleAnimator.FadeIn();
			current.Add(entity);
		}
	}

	public void EntityUnHover(Entity entity)
	{
		if (current.Contains(entity) && entity.display is Card card)
		{
			card.imageIdleAnimator.FadeOut();
			card.backgroundIdleAnimator.FadeOut();
			current.Remove(entity);
		}
	}
}
