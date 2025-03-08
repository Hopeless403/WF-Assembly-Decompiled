#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Bonus Damage Equal To Cards", fileName = "Bonus Damage Equal To Cards")]
public class StatusEffectBonusDamageEqualToCards : StatusEffectData
{
	[SerializeField]
	public string cardName = "Dart";

	[SerializeField]
	public bool inHand = true;

	[SerializeField]
	public bool onBoard;

	[SerializeField]
	public bool includeSelf = true;

	[SerializeField]
	public bool ping = true;

	public int currentAmount;

	public override void Init()
	{
		base.PreCardPlayed += Gain;
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target && currentAmount == 0)
		{
			return CanTrigger();
		}

		return false;
	}

	public IEnumerator Gain(Entity entity, Entity[] targets)
	{
		int num = Count();
		if (num > 0)
		{
			currentAmount = num;
			target.tempDamage += currentAmount;
			target.PromptUpdate();
			if (ping)
			{
				target.curveAnimator?.Ping();
				yield return Sequences.Wait(0.5f);
			}
		}
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (currentAmount > 0 && (bool)target.owner && (bool)target.owner.entity && entity == target.owner.entity)
		{
			target.tempDamage -= currentAmount;
			currentAmount = 0;
			target.PromptUpdate();
		}

		return false;
	}

	public int Count()
	{
		return 0 + (inHand ? CountInHand() : 0) + (onBoard ? CountOnBoard() : 0);
	}

	public int CountInHand()
	{
		int num = 0;
		CardContainer handContainer = target.owner.handContainer;
		if ((bool)handContainer)
		{
			num += handContainer.Where((Entity entity) => entity.data.name == cardName).Count((Entity entity) => includeSelf || entity != target);
		}

		return num;
	}

	public int CountOnBoard()
	{
		return (from entity in Battle.GetAllUnits()
			where entity.data.name == cardName
			select entity).Count((Entity entity) => includeSelf || entity != target);
	}
}
