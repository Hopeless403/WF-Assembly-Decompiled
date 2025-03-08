#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X Every Turn", fileName = "Apply X Every Turn")]
public class StatusEffectApplyXEveryTurn : StatusEffectApplyX
{
	public enum Mode
	{
		AfterTurn,
		TurnStart
	}

	[SerializeField]
	public Mode mode;

	public override void Init()
	{
		base.OnTurn += CheckTurn;
		base.OnTurnStart += CheckTurnStart;
	}

	public override bool RunTurnEvent(Entity entity)
	{
		if (mode == Mode.AfterTurn && target.enabled && entity == target)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator CheckTurn(Entity entity)
	{
		return Run(GetTargets());
	}

	public override bool RunTurnStartEvent(Entity entity)
	{
		if (mode == Mode.TurnStart && target.enabled && entity == target)
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator CheckTurnStart(Entity entity)
	{
		return Run(GetTargets());
	}
}
