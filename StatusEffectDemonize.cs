#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Demonize", fileName = "Demonize")]
public class StatusEffectDemonize : StatusEffectData
{
	public override void Init()
	{
		base.OnHit += DemonizeHit;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.Offensive && count > 0 && hit.damage > 0)
		{
			return hit.target == target;
		}

		return false;
	}

	public IEnumerator DemonizeHit(Hit hit)
	{
		hit.damage = Mathf.RoundToInt((float)hit.damage * 2f);
		ActionQueue.Stack(new ActionSequence(CountDown())
		{
			fixedPosition = true,
			note = "Count Down Demonize"
		});
		yield break;
	}

	public IEnumerator CountDown()
	{
		if ((bool)this && (bool)target && target.alive)
		{
			int amount = 1;
			Events.InvokeStatusEffectCountDown(this, ref amount);
			if (amount != 0)
			{
				yield return CountDown(target, amount);
			}
		}
	}
}
