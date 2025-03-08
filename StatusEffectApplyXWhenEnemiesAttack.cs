#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Enemies Attack", fileName = "Apply X When Enemies Attack")]
public class StatusEffectApplyXWhenEnemiesAttack : StatusEffectApplyX
{
	public Entity attacker;

	public override void Init()
	{
		base.PreAttack += HitCheck;
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (target.enabled && target.alive && entity.owner != target.owner && Battle.IsOnBoard(target))
		{
			attacker = entity;
		}

		return false;
	}

	public override bool RunPreAttackEvent(Hit hit)
	{
		if (attacker != null && hit.attacker == attacker && target.enabled && target.alive && hit.Offensive)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator HitCheck(Hit hit)
	{
		attacker = null;
		yield return Run(GetTargets(hit));
	}
}
