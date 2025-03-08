#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Silence", fileName = "Silence")]
public class StatusEffectSilence : StatusEffectData
{
	public override void Init()
	{
		base.OnTurnEnd += CustomCountDown;
	}

	public override bool RunBeginEvent()
	{
		target.silenceCount++;
		target.PromptUpdate();
		return false;
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target)
		{
			return target.enabled;
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

	public override bool RunEndEvent()
	{
		target.silenceCount--;
		target.PromptUpdate();
		return false;
	}
}
