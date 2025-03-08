#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Lumin", fileName = "Lumin")]
public class StatusEffectLumin : StatusEffectData
{
	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if ((bool)apply.effectData && apply.count > 0 && apply.effectData.type == type && apply.target != target)
		{
			ActionQueue.Stack(new ActionSequence(Remove())
			{
				note = "Remove Lumin from [" + target.name + "]"
			}, fixedPosition: true);
		}

		return false;
	}

	public override bool RunBeginEvent()
	{
		target.effectFactor += 1f;
		return false;
	}

	public override bool RunEndEvent()
	{
		target.effectFactor -= 1f;
		return false;
	}
}
