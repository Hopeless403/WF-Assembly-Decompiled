#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Snow", fileName = "Snow")]
public class StatusEffectSnow : StatusEffectData
{
	public enum CountDownType
	{
		OnCounter,
		OnTurnEnd
	}

	[SerializeField]
	public CountDownType countDownType;

	public bool primed;

	public override void Init()
	{
		base.OnHit += Hit;
		base.OnTurnEnd += CustomCountDown;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target)
		{
			return hit.counterReduction > 0;
		}

		return false;
	}

	public IEnumerator Hit(Hit hit)
	{
		while (hit.counterReduction > 0 && count > 0)
		{
			if (countDownType == CountDownType.OnCounter)
			{
				count--;
			}

			hit.counterReduction--;
		}

		if (count <= 0)
		{
			yield return Remove();
		}
	}

	public override bool RunTurnStartEvent(Entity entity)
	{
		if (!primed && entity == target && countDownType == CountDownType.OnTurnEnd && Battle.IsOnBoard(entity))
		{
			primed = true;
		}

		return false;
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target && primed)
		{
			return countDownType == CountDownType.OnTurnEnd;
		}

		return false;
	}

	public IEnumerator CustomCountDown(Entity entity)
	{
		int amount = 1;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(entity, amount);
		}
	}
}
