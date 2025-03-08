#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Reactions/While Active Apply X To Each Card Played", fileName = "While Active Apply X To Each Card Played")]
public class StatusEffectWhileActiveApplyXToEachCardPlayed : StatusEffectReaction
{
	[SerializeField]
	public StatusEffectData effectToApply;

	public override void Init()
	{
		base.PreCardPlayed += Activate;
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (CheckEntity(entity) && CanTrigger())
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator Activate(Entity entity, Entity[] targets)
	{
		yield return Run(entity);
	}

	public IEnumerator Run(Entity entity)
	{
		Hit hit = new Hit(target, entity, 0);
		hit.AddStatusEffect(effectToApply, GetAmount());
		Routine.Clump clump = new Routine.Clump();
		clump.Add(hit.Process());
		target.curveAnimator?.Ping();
		clump.Add(Sequences.Wait(0.3f));
		yield return clump.WaitForEnd();
	}

	public bool CheckEntity(Entity entity)
	{
		if (entity.owner == target.owner && entity.owner != null && entity.data != null)
		{
			return entity.data.cardType.item;
		}

		return false;
	}
}
