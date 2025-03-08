#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Shroom", fileName = "Shroom")]
public class StatusEffectShroom : StatusEffectData
{
	public bool subbed;

	public bool primed;

	public override void Init()
	{
		base.OnTurnEnd += DealDamage;
		Events.OnPostProcessUnits += Prime;
		subbed = true;
	}

	public void OnDestroy()
	{
		Unsub();
	}

	public void Unsub()
	{
		if (subbed)
		{
			Events.OnPostProcessUnits -= Prime;
			subbed = false;
		}
	}

	public void Prime(Character character)
	{
		primed = true;
		Unsub();
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (primed && target.enabled)
		{
			return entity == target;
		}

		return false;
	}

	public IEnumerator DealDamage(Entity entity)
	{
		Hit hit = new Hit(GetDamager(), target, count)
		{
			screenShake = 0.25f,
			damageType = "shroom"
		};
		yield return hit.Process();
		yield return Sequences.Wait(0.2f);
		int amount = 1;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(entity, amount);
		}
	}
}
