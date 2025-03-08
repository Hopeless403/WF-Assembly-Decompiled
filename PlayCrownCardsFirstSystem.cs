#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class PlayCrownCardsFirstSystem : GameSystem
{
	public bool crownPhase;

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
		Events.OnCheckAction += CheckAction;
		Events.OnBattleTurnEnd += BattleTurnEnd;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
		Events.OnCheckAction -= CheckAction;
		Events.OnBattleTurnEnd -= BattleTurnEnd;
	}

	public void BattleStart()
	{
		crownPhase = true;
	}

	public void CheckAction(ref PlayAction action, ref bool allow)
	{
		if (!crownPhase)
		{
			return;
		}

		PlayAction playAction = action;
		if (!(playAction is ActionMove actionMove))
		{
			if (playAction is ActionTrigger actionTrigger && !actionTrigger.entity.data.HasCrown)
			{
				allow = false;
				if (NoTargetTextSystem.Exists())
				{
					StartCoroutine(NoTargetTextSystem.Run(actionTrigger.entity, NoTargetType.PlayCrownCardsFirst));
				}
			}
		}
		else if (!actionMove.entity.data.HasCrown && !Battle.IsOnBoard(actionMove.entity) && Battle.IsOnBoard(actionMove.toContainers))
		{
			allow = false;
			if (NoTargetTextSystem.Exists())
			{
				StartCoroutine(NoTargetTextSystem.Run(actionMove.entity, NoTargetType.PlayCrownCardsFirst));
			}
		}
	}

	public void BattleTurnEnd(int turnNumber)
	{
		crownPhase = false;
		base.enabled = false;
	}
}
