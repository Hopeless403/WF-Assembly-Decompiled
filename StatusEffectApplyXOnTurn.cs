#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X On Turn", fileName = "Apply X On Turn")]
public class StatusEffectApplyXOnTurn : StatusEffectApplyX
{
	[SerializeField]
	public bool trueOnTurn;

	public bool turnTaken;

	public override void Init()
	{
		base.OnCardPlayed += CheckCardPlay;
		base.OnTurn += CheckTurn;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!turnTaken && target.enabled && entity == target && Battle.IsOnBoard(target))
		{
			if (trueOnTurn)
			{
				turnTaken = true;
				return false;
			}

			return true;
		}

		return false;
	}

	public IEnumerator CheckCardPlay(Entity entity, Entity[] targets)
	{
		return Run(GetTargets());
	}

	public override bool RunTurnEvent(Entity entity)
	{
		if (trueOnTurn && turnTaken && entity == target)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator CheckTurn(Entity entity)
	{
		yield return Run(GetTargets());
		turnTaken = false;
	}
}
