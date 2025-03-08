#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class NegativeCharmAfterBossModifierSystem : GameSystem
{
	[SerializeField]
	public GainNegativeCharmSequence gainNegativeCharmSequencePrefab;

	public void OnEnable()
	{
		Events.OnBattleWin += BattleWin;
	}

	public void OnDisable()
	{
		Events.OnBattleWin -= BattleWin;
	}

	public void BattleWin()
	{
		if (Campaign.FindCharacterNode(References.Player).type.isBoss)
		{
			ActionQueue.Stack(new ActionSequence(GainCharmRoutine()));
		}
	}

	public IEnumerator GainCharmRoutine()
	{
		GainNegativeCharmSequence sequence = Object.Instantiate(gainNegativeCharmSequencePrefab, References.Player.entity.display.transform);
		yield return sequence.Run();
		sequence.gameObject.Destroy();
	}
}
