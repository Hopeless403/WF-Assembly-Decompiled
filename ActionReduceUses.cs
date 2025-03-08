#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionReduceUses : PlayAction
{
	public readonly Entity entity;

	public ActionReduceUses(Entity entity)
	{
		this.entity = entity;
	}

	public override IEnumerator Run()
	{
		if ((bool)entity)
		{
			yield return CardReduceUses(entity);
		}
	}

	public static IEnumerator CardReduceUses(Entity entity)
	{
		if (entity.uses.max <= 0 || entity.uses.current <= 0)
		{
			yield break;
		}

		if (--entity.uses.current <= 0)
		{
			if (entity.alive)
			{
				yield return Sequences.CardDiscard(entity);
			}
		}
		else if (entity.alive)
		{
			entity.TweenToContainer();
		}
	}
}
