#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Vim", fileName = "Vim")]
public class StatusEffectVim : StatusEffectData
{
	public override void Init()
	{
		base.OnHit += Check;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target && hit.Offensive && hit.canBeNullified)
		{
			return hit.BasicHit;
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		hit.dodged = true;
		hit.countsAsHit = false;
		hit.damageBlocked = hit.damage;
		hit.damage = 0;
		if ((bool)hit.attacker && hit.attacker.canBeHit && hit.canRetaliate)
		{
			Hit hit2 = new Hit(target, hit.attacker, count)
			{
				canRetaliate = false,
				damageType = "vim"
			};
			yield return hit2.Process();
		}

		ActionQueue.Stack(new ActionSequence(CountDown()), fixedPosition: true);
	}

	public IEnumerator CountDown()
	{
		int amount = count;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(target, amount);
		}
	}
}
