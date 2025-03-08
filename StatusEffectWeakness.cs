#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Weakness", fileName = "Weakness")]
public class StatusEffectWeakness : StatusEffectData
{
	public override void Init()
	{
		base.OnHit += Hit;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.Offensive && count > 0)
		{
			return hit.target == target;
		}

		return false;
	}

	public IEnumerator Hit(Hit hit)
	{
		hit.damage += count;
		yield break;
	}
}
