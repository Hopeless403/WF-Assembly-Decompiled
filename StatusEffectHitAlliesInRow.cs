#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Hit Allies In Row", fileName = "Hit Allies In Row")]
public class StatusEffectHitAlliesInRow : StatusEffectData
{
	public override void Init()
	{
		base.PostAttack += CheckHit;
	}

	public override bool RunPostAttackEvent(Hit hit)
	{
		if (hit.attacker == target)
		{
			return hit.target != null;
		}

		return false;
	}

	public IEnumerator CheckHit(Hit hit)
	{
		List<Entity> alliesInRow = hit.target.GetAlliesInRow();
		foreach (Entity item in alliesInRow)
		{
			Hit hit2 = new Hit(target, item, GetAmount());
			yield return hit2.Process();
		}
	}
}
