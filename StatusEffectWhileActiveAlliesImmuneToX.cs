#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/While Active Allies Immune To X", fileName = "While Active Allies Immune To X")]
public class StatusEffectWhileActiveAlliesImmuneToX : StatusEffectData
{
	public List<Entity> affected = new List<Entity>();

	public bool pingDone;

	[SerializeField]
	public StatusEffectData immunityEffect;

	public override void Init()
	{
		base.OnEnable += Enable;
		base.OnDisable += Disable;
		base.OnCardMove += CardMove;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		return entity == target;
	}

	public IEnumerator Enable(Entity entity)
	{
		if (!pingDone)
		{
			target.curveAnimator?.Ping();
			pingDone = true;
		}

		List<Entity> allies = target.GetAllies();
		foreach (Entity item in allies)
		{
			yield return Affect(item);
		}
	}

	public override bool RunDisableEvent(Entity entity)
	{
		return entity == target;
	}

	public IEnumerator Disable(Entity entity)
	{
		foreach (Entity item in affected)
		{
			yield return UnAffect(item);
		}

		affected.Clear();
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (target.enabled && entity != target && entity.owner == target.owner)
		{
			return Battle.IsOnBoard(entity);
		}

		return false;
	}

	public IEnumerator CardMove(Entity entity)
	{
		yield return Affect(entity);
	}

	public IEnumerator Affect(Entity entity)
	{
		if (!affected.Contains(entity))
		{
			yield return StatusEffectSystem.Apply(entity, target, immunityEffect, 1, temporary: true);
			entity.PromptUpdate();
			affected.Add(entity);
		}
	}

	public IEnumerator UnAffect(Entity entity)
	{
		if (!affected.Contains(entity))
		{
			yield break;
		}

		for (int i = entity.statusEffects.Count - 1; i >= 0; i--)
		{
			StatusEffectData statusEffectData = entity.statusEffects[i];
			if ((bool)statusEffectData && statusEffectData.name == immunityEffect.name)
			{
				yield return statusEffectData.RemoveStacks(1, removeTemporary: true);
			}
		}
	}
}
