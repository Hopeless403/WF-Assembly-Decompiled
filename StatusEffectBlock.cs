#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Block", fileName = "Block")]
public class StatusEffectBlock : StatusEffectData
{
	public override void Init()
	{
		base.OnHit += Check;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target && hit.Offensive && hit.canBeNullified)
		{
			return hit.damage > 0;
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		SfxSystem.OneShot("event:/sfx/status_icon/block_decrease");
		hit.damageBlocked = hit.damage;
		hit.damage = 0;
		count--;
		if (count <= 0)
		{
			yield return Remove();
		}

		target.PromptUpdate();
	}
}
