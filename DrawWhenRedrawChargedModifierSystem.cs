#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DrawWhenRedrawChargedModifierSystem : GameSystem
{
	public const int drawAmount = 1;

	public RedrawBellSystem _redrawBellSystem;

	public bool drawOnTurnEnd;

	public RedrawBellSystem redrawBellSystem => _redrawBellSystem ?? (_redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>());

	public void OnEnable()
	{
		Events.OnBattleTurnStart += BattleTurnStart;
		Events.OnBattleTurnEnd += BattleTurnEnd;
	}

	public void OnDisable()
	{
		Events.OnBattleTurnStart -= BattleTurnStart;
		Events.OnBattleTurnEnd -= BattleTurnEnd;
	}

	public void BattleTurnStart(int turn)
	{
		if (redrawBellSystem.IsCharged && !Battle.instance.ended)
		{
			drawOnTurnEnd = true;
		}
	}

	public void BattleTurnEnd(int turn)
	{
		if (drawOnTurnEnd && !Battle.instance.ended)
		{
			drawOnTurnEnd = false;
			ActionQueue.Stack(new ActionDraw(References.Player));
		}
	}
}
