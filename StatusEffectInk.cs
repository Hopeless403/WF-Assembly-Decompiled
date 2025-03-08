#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Ink", fileName = "Ink")]
public class StatusEffectInk : StatusEffectData
{
	public bool primed;

	public override void Init()
	{
		base.OnCardPlayed += Check;
	}

	public override bool RunPreCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (!primed && target.enabled && entity == target)
		{
			primed = true;
		}

		return false;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (primed && target.enabled)
		{
			return entity == target;
		}

		return false;
	}

	public IEnumerator Check(Entity entity, Entity[] targets)
	{
		Hit hit = new Hit(target, target);
		hit.AddAttackerStatuses();
		ActionQueue.Stack(new ActionSequence(CountDown()), fixedPosition: true);
		yield return hit.Process();
	}

	public IEnumerator CountDown()
	{
		int amount = 1;
		Events.InvokeStatusEffectCountDown(this, ref amount);
		if (amount != 0)
		{
			yield return CountDown(target, amount);
		}
	}
}
