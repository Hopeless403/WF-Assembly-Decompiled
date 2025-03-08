#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Summoned", fileName = "Summoned")]
public class StatusEffectSummoned : StatusEffectData
{
	public bool triggered;

	public override void Init()
	{
		base.OnTurnEnd += DealDamage;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (entity == target && !target.silenced)
		{
			triggered = true;
		}

		return false;
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target && triggered)
		{
			if (target.silenced)
			{
				triggered = false;
			}

			if (CanTrigger())
			{
				return true;
			}
		}

		return false;
	}

	public IEnumerator DealDamage(Entity entity)
	{
		triggered = false;
		SfxSystem.OneShot("event:/sfx/status/shadeheart");
		Hit hit = new Hit(target, target, 1)
		{
			damageType = "summoned",
			countsAsHit = false,
			screenShake = 0f
		};
		yield return hit.Process();
		yield return Sequences.Wait(0.1f);
	}
}
