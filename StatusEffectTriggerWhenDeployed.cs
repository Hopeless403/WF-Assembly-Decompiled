#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Trigger When Deployed", fileName = "Trigger When Deployed")]
public class StatusEffectTriggerWhenDeployed : StatusEffectData
{
	public bool isAlreadyOnBoard;

	public override object GetMidBattleData()
	{
		return Battle.IsOnBoard(target);
	}

	public override void RestoreMidBattleData(object data)
	{
		if (data is bool)
		{
			bool flag = (bool)data;
			isAlreadyOnBoard = flag && Battle.IsOnBoard(target);
		}
	}

	public override void Init()
	{
		base.OnEnable += Enable;
		base.OnCardMove += CardMove;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (isAlreadyOnBoard)
		{
			isAlreadyOnBoard = false;
			return false;
		}

		if (entity == target)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator Enable(Entity entity)
	{
		yield return Sequences.Wait(0.2f);
		yield return Activate();
	}

	public override bool RunCardMoveEvent(Entity entity)
	{
		if (target == entity && target.enabled && Battle.IsOnBoard(target))
		{
			return !Battle.IsOnBoard(entity.preContainers);
		}

		return false;
	}

	public IEnumerator CardMove(Entity entity)
	{
		yield return Sequences.Wait(0.2f);
		yield return Activate();
	}

	public IEnumerator Activate()
	{
		if (!target.silenced)
		{
			yield return Sequences.Wait(0.1f);
			target.curveAnimator?.Ping();
			yield return Sequences.Wait(0.5f);
			ActionQueue.Stack(new ActionTrigger(target, null), fixedPosition: true);
		}
	}
}
